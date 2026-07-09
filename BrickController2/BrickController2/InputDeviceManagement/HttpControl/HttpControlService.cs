using Autofac;
using BrickController2.PlatformServices.InputDevice;
using BrickController2.PlatformServices.InputDeviceService;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading;
using System.Threading.Tasks;

namespace BrickController2.InputDeviceManagement.HttpControl;

internal sealed class HttpControlService :
    InputDeviceServiceBase<HttpInputDevice>,
    IInputDeviceService<HttpInputDevice>,
    IHttpControlService,
    IDisposable
{
    private static readonly JsonSerializerOptions JsonOptions = new()
    {
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
        DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,
        Converters = { new JsonStringEnumConverter(JsonNamingPolicy.CamelCase) }
    };

    private readonly IInputDeviceManagerService _inputDeviceManagerService;
    private readonly IHttpControlSettingsService _settingsService;
    private readonly HttpControlRegistry _registry = new();
    private readonly Dictionary<string, HttpInputDevice> _virtualDevices = new(StringComparer.OrdinalIgnoreCase);

    private readonly object _listenerLock = new();
    private HttpListener? _listener;
    private CancellationTokenSource? _listenerTokenSource;
    private Task? _listenerTask;
    private HttpControlRuntimeStatus _runtimeStatus = HttpControlRuntimeStatus.Stopped;
    private string? _errorMessage;
    private IReadOnlyList<string> _reachableUrls = [];
    private bool _inputEventsSubscribed;
    private bool _disposed;

    public HttpControlService(
        IInputDeviceManagerService inputDeviceManagerService,
        IHttpControlSettingsService settingsService,
        ILogger<HttpControlService> logger)
        : base(inputDeviceManagerService, logger)
    {
        _inputDeviceManagerService = inputDeviceManagerService;
        _settingsService = settingsService;
        _inputDeviceManagerService.InputDevicesChangedEvent += InputDevicesChangedEventHandler;

        if (Options.Enabled)
        {
            StartHttpListener(Options);
        }
    }

    public event EventHandler? StatusChanged;

    public bool IsSupported => HttpListener.IsSupported;

    public bool IsEnabled
    {
        get => Options.Enabled;
        set => ApplyOptions(Options with { Enabled = value });
    }

    public HttpControlOptions Options => _settingsService.Current;

    public HttpControlRuntimeStatus RuntimeStatus => _runtimeStatus;

    public string? ErrorMessage => _errorMessage;

    public IReadOnlyList<string> ReachableUrls => _reachableUrls;

    public override void Initialize()
    {
        foreach (var inputDevice in _inputDeviceManagerService.GetInputDevices())
        {
            _registry.MarkInputDeviceConnected(inputDevice);
        }

        foreach (var device in _virtualDevices.Values)
        {
            if (!TryGetInputDevice(x => x.InputDeviceId == device.InputDeviceId, out _))
            {
                AddInputDevice(device);
            }
        }
    }

    public override void Stop()
    {
        while (TryRemoveInputDevice(out _))
        {
        }
    }

    public void ApplyOptions(HttpControlOptions options)
    {
        _settingsService.Save(options);
        var current = Options;

        if (current.Enabled)
        {
            StartHttpListener(current);
        }
        else
        {
            StopHttpListener();
        }

        RaiseStatusChanged();
    }

    public string RegenerateAccessToken()
    {
        var token = _settingsService.RegenerateAccessToken();
        RaiseStatusChanged();
        return token;
    }

    public void Dispose()
    {
        if (_disposed)
        {
            return;
        }

        _disposed = true;
        StopHttpListener();
        _inputDeviceManagerService.InputDevicesChangedEvent -= InputDevicesChangedEventHandler;
    }

    private void StartHttpListener(HttpControlOptions options)
    {
        lock (_listenerLock)
        {
            StopHttpListenerCore(unsubscribeInputEvents: false);
            EnsureInputEventSubscription();

            _settingsService.EnsureAccessToken();

            if (!IsSupported)
            {
                _runtimeStatus = HttpControlRuntimeStatus.Unsupported;
                _errorMessage = "HttpListener is not supported on this platform.";
                _reachableUrls = [];
                return;
            }

            try
            {
                _listener = new HttpListener();
                foreach (var prefix in GetListenerPrefixes(options))
                {
                    _listener.Prefixes.Add(prefix);
                }

                _listener.Start();
                _listenerTokenSource = new CancellationTokenSource();
                _listenerTask = Task.Run(() => ListenAsync(_listener, _listenerTokenSource.Token));
                _reachableUrls = GetReachableUrls(options);
                _runtimeStatus = HttpControlRuntimeStatus.Running;
                _errorMessage = null;
            }
            catch (Exception ex) when (ex is HttpListenerException or InvalidOperationException or SocketException)
            {
                StopHttpListenerCore(unsubscribeInputEvents: false);
                _runtimeStatus = HttpControlRuntimeStatus.Error;
                _errorMessage = ex.Message;
                _reachableUrls = GetReachableUrls(options);
                _logger.LogError(ex, "Failed to start HTTP control listener on port {port}.", options.Port);
            }
        }

        RaiseStatusChanged();
    }

    private void StopHttpListener()
    {
        lock (_listenerLock)
        {
            StopHttpListenerCore(unsubscribeInputEvents: true);
            _runtimeStatus = HttpControlRuntimeStatus.Stopped;
            _errorMessage = null;
            _reachableUrls = [];
        }

        RaiseStatusChanged();
    }

    private void StopHttpListenerCore(bool unsubscribeInputEvents)
    {
        _listenerTokenSource?.Cancel();

        try
        {
            _listener?.Stop();
            _listener?.Close();
        }
        catch (ObjectDisposedException)
        {
        }

        _listener = null;
        _listenerTokenSource?.Dispose();
        _listenerTokenSource = null;
        _listenerTask = null;

        if (unsubscribeInputEvents)
        {
            RemoveInputEventSubscription();
        }
    }

    private async Task ListenAsync(HttpListener listener, CancellationToken token)
    {
        while (!token.IsCancellationRequested)
        {
            HttpListenerContext context;
            try
            {
                context = await listener.GetContextAsync().ConfigureAwait(false);
            }
            catch (Exception ex) when (token.IsCancellationRequested ||
                                      ex is HttpListenerException or ObjectDisposedException or InvalidOperationException)
            {
                break;
            }

            _ = Task.Run(() => ProcessRequestAsync(context), token);
        }
    }

    private async Task ProcessRequestAsync(HttpListenerContext context)
    {
        try
        {
            var method = context.Request.HttpMethod;
            var path = context.Request.Url?.AbsolutePath ?? "/";
            var segments = path.Trim('/').Split('/', StringSplitOptions.RemoveEmptyEntries)
                .Select(Uri.UnescapeDataString)
                .ToArray();

            if (segments.Length == 0)
            {
                await WriteJsonAsync(context, HttpStatusCode.OK, new
                {
                    service = "BrickController2 HTTP Control API",
                    status = "/api/control/status",
                    controllers = "/api/controllers"
                }).ConfigureAwait(false);
                return;
            }

            if (!IsStatusRoute(method, segments) && !IsAuthorized(context))
            {
                await WriteJsonAsync(context, HttpStatusCode.Unauthorized, new HttpErrorResponse("Missing or invalid HTTP control token.")).ConfigureAwait(false);
                return;
            }

            if (segments is ["api", "control", "status"] && method == "GET")
            {
                await WriteJsonAsync(context, HttpStatusCode.OK, GetStatusResponse()).ConfigureAwait(false);
            }
            else if (segments is ["api", "control", "settings"] && method == "GET")
            {
                await WriteJsonAsync(context, HttpStatusCode.OK, GetSettingsResponse()).ConfigureAwait(false);
            }
            else if (segments is ["api", "control", "token", "regenerate"] && method == "POST")
            {
                var token = RegenerateAccessToken();
                await WriteJsonAsync(context, HttpStatusCode.OK, new { accessToken = token }).ConfigureAwait(false);
            }
            else if (segments is ["api", "controllers"] && method == "GET")
            {
                await WriteJsonAsync(context, HttpStatusCode.OK, _registry.GetControllers()).ConfigureAwait(false);
            }
            else if (segments is ["api", "controllers", "virtual"] && method == "POST")
            {
                await RegisterVirtualControllerAsync(context).ConfigureAwait(false);
            }
            else if (segments is ["api", "controllers", "virtual", var virtualControllerId] && method == "DELETE")
            {
                await DeleteVirtualControllerAsync(context, virtualControllerId).ConfigureAwait(false);
            }
            else if (segments.Length == 3 && segments[0] == "api" && segments[1] == "controllers" && method == "GET")
            {
                await GetControllerAsync(context, segments[2]).ConfigureAwait(false);
            }
            else if (segments.Length == 4 &&
                     segments[0] == "api" &&
                     segments[1] == "controllers" &&
                     segments[3] == "capabilities" &&
                     method == "GET")
            {
                await GetCapabilitiesAsync(context, segments[2]).ConfigureAwait(false);
            }
            else if (segments.Length == 4 &&
                     segments[0] == "api" &&
                     segments[1] == "controllers" &&
                     segments[3] == "capabilities:batchSet" &&
                     method == "POST")
            {
                await BatchSetCapabilitiesAsync(context, segments[2]).ConfigureAwait(false);
            }
            else if (segments.Length == 5 &&
                     segments[0] == "api" &&
                     segments[1] == "controllers" &&
                     segments[3] == "capabilities" &&
                     method == "GET")
            {
                await GetCapabilityAsync(context, segments[2], segments[4]).ConfigureAwait(false);
            }
            else if (segments.Length == 5 &&
                     segments[0] == "api" &&
                     segments[1] == "controllers" &&
                     segments[3] == "capabilities" &&
                     method == "PUT")
            {
                await SetCapabilityAsync(context, segments[2], segments[4]).ConfigureAwait(false);
            }
            else if (segments.Length == 6 &&
                     segments[0] == "api" &&
                     segments[1] == "controllers" &&
                     segments[3] == "capabilities" &&
                     segments[5] == "press" &&
                     method == "POST")
            {
                await PressCapabilityAsync(context, segments[2], segments[4]).ConfigureAwait(false);
            }
            else
            {
                await WriteJsonAsync(context, HttpStatusCode.NotFound, new HttpErrorResponse("HTTP control endpoint was not found.")).ConfigureAwait(false);
            }
        }
        catch (JsonException ex)
        {
            await WriteJsonAsync(context, HttpStatusCode.BadRequest, new HttpErrorResponse(ex.Message)).ConfigureAwait(false);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "HTTP control request failed.");
            await WriteJsonAsync(context, HttpStatusCode.InternalServerError, new HttpErrorResponse(ex.Message)).ConfigureAwait(false);
        }
    }

    private async Task RegisterVirtualControllerAsync(HttpListenerContext context)
    {
        var request = await ReadJsonAsync<VirtualControllerRequest>(context).ConfigureAwait(false) ?? new VirtualControllerRequest();
        var controllerId = string.IsNullOrWhiteSpace(request.Id)
            ? GetNextVirtualControllerId()
            : request.Id.Trim();
        var name = string.IsNullOrWhiteSpace(request.Name) ? controllerId : request.Name.Trim();
        var number = HttpControlRegistry.ParseControllerNumber(controllerId);
        if (number == 0)
        {
            number = GetNextVirtualControllerNumber();
        }

        var capabilities = request.Capabilities is { Count: > 0 }
            ? request.Capabilities
            : GetDefaultCapabilities();

        HttpControllerResponse response;
        lock (_virtualDevices)
        {
            if (_virtualDevices.ContainsKey(controllerId))
            {
                TryRemoveInputDevice(x => x.InputDeviceId == controllerId, out _);
            }

            _registry.UpsertVirtualController(controllerId, name, number, capabilities);
            var device = new HttpInputDevice(InputDeviceEventService, controllerId, name, number);
            _virtualDevices[controllerId] = device;

            if (CanProcessEvents)
            {
                AddInputDevice(device);
            }

            response = _registry.GetController(controllerId)!;
        }

        await WriteJsonAsync(context, HttpStatusCode.OK, response).ConfigureAwait(false);
    }

    private async Task DeleteVirtualControllerAsync(HttpListenerContext context, string controllerId)
    {
        lock (_virtualDevices)
        {
            _virtualDevices.Remove(controllerId);
            TryRemoveInputDevice(x => x.InputDeviceId == controllerId, out _);
            _registry.RemoveVirtualController(controllerId);
        }

        await WriteNoContentAsync(context).ConfigureAwait(false);
    }

    private async Task GetControllerAsync(HttpListenerContext context, string controllerId)
    {
        var controller = _registry.GetController(controllerId);
        if (controller is null)
        {
            await WriteJsonAsync(context, HttpStatusCode.NotFound, new HttpErrorResponse($"Controller '{controllerId}' was not found.")).ConfigureAwait(false);
            return;
        }

        await WriteJsonAsync(context, HttpStatusCode.OK, controller).ConfigureAwait(false);
    }

    private async Task GetCapabilitiesAsync(HttpListenerContext context, string controllerId)
    {
        var capabilities = _registry.GetCapabilities(controllerId);
        if (capabilities is null)
        {
            await WriteJsonAsync(context, HttpStatusCode.NotFound, new HttpErrorResponse($"Controller '{controllerId}' was not found.")).ConfigureAwait(false);
            return;
        }

        await WriteJsonAsync(context, HttpStatusCode.OK, capabilities).ConfigureAwait(false);
    }

    private async Task GetCapabilityAsync(HttpListenerContext context, string controllerId, string capabilityId)
    {
        var capability = _registry.GetCapability(controllerId, capabilityId);
        if (capability is null)
        {
            await WriteJsonAsync(context, HttpStatusCode.NotFound, new HttpErrorResponse($"Capability '{capabilityId}' was not found.")).ConfigureAwait(false);
            return;
        }

        await WriteJsonAsync(context, HttpStatusCode.OK, capability).ConfigureAwait(false);
    }

    private async Task SetCapabilityAsync(HttpListenerContext context, string controllerId, string capabilityId)
    {
        var request = await ReadJsonAsync<CapabilitySetRequest>(context).ConfigureAwait(false);
        if (request is null)
        {
            await WriteJsonAsync(context, HttpStatusCode.BadRequest, new HttpErrorResponse("Request body is required.")).ConfigureAwait(false);
            return;
        }

        await SetCapabilitiesAsync(context, controllerId, new Dictionary<string, float> { [capabilityId] = request.Value }, singleResponse: true)
            .ConfigureAwait(false);
    }

    private async Task BatchSetCapabilitiesAsync(HttpListenerContext context, string controllerId)
    {
        var request = await ReadJsonAsync<CapabilityBatchSetRequest>(context).ConfigureAwait(false);
        if (request?.Values is not { Count: > 0 })
        {
            await WriteJsonAsync(context, HttpStatusCode.BadRequest, new HttpErrorResponse("At least one capability value is required.")).ConfigureAwait(false);
            return;
        }

        await SetCapabilitiesAsync(context, controllerId, request.Values, singleResponse: false).ConfigureAwait(false);
    }

    private async Task PressCapabilityAsync(HttpListenerContext context, string controllerId, string capabilityId)
    {
        var capability = _registry.GetCapability(controllerId, capabilityId);
        if (capability is null)
        {
            await WriteJsonAsync(context, HttpStatusCode.NotFound, new HttpErrorResponse($"Capability '{capabilityId}' was not found.")).ConfigureAwait(false);
            return;
        }

        if (capability.EventType != InputDeviceEventType.Button)
        {
            await WriteJsonAsync(context, HttpStatusCode.BadRequest, new HttpErrorResponse("Only button capabilities can be pressed.")).ConfigureAwait(false);
            return;
        }

        var request = await ReadJsonAsync<CapabilityPressRequest>(context).ConfigureAwait(false) ?? new CapabilityPressRequest();
        var durationMs = Math.Clamp(request.DurationMs, 0, 10_000);

        if (!await SetCapabilitiesAsync(context, controllerId, new Dictionary<string, float> { [capabilityId] = InputDevices.BUTTON_PRESSED }, singleResponse: false, writeResponse: false)
                .ConfigureAwait(false))
        {
            return;
        }
        if (durationMs > 0)
        {
            await Task.Delay(durationMs).ConfigureAwait(false);
        }

        await SetCapabilitiesAsync(context, controllerId, new Dictionary<string, float> { [capabilityId] = InputDevices.BUTTON_RELEASED }, singleResponse: true)
            .ConfigureAwait(false);
    }

    private async Task<bool> SetCapabilitiesAsync(
        HttpListenerContext context,
        string controllerId,
        IReadOnlyDictionary<string, float> values,
        bool singleResponse,
        bool writeResponse = true)
    {
        if (!_virtualDevices.TryGetValue(controllerId, out var device))
        {
            await WriteJsonAsync(context, HttpStatusCode.Conflict, new HttpErrorResponse($"Controller '{controllerId}' is not a writable HTTP virtual controller.")).ConfigureAwait(false);
            return false;
        }

        var responses = _registry.TrySetCapability(controllerId, values, out var events, out var error);
        if (responses is null)
        {
            await WriteJsonAsync(context, HttpStatusCode.BadRequest, new HttpErrorResponse(error ?? "Capability value could not be set.")).ConfigureAwait(false);
            return false;
        }

        device.Publish(events);

        if (!writeResponse)
        {
            return true;
        }

        await WriteJsonAsync(context, HttpStatusCode.OK, singleResponse ? responses[0] : responses).ConfigureAwait(false);
        return true;
    }

    private HttpControlStatusResponse GetStatusResponse()
    {
        var options = Options;
        return new HttpControlStatusResponse(
            "BrickController2 HTTP Control API",
            "1",
            options.Enabled,
            RuntimeStatus,
            options.Port,
            options.ListenMode,
            IsAuthRequired(options),
            ReachableUrls,
            ErrorMessage);
    }

    private HttpControlSettingsResponse GetSettingsResponse()
    {
        var options = Options;
        return new HttpControlSettingsResponse(
            options.Enabled,
            options.Port,
            options.ListenMode,
            IsAuthRequired(options),
            !string.IsNullOrWhiteSpace(options.AccessToken),
            GetTokenPreview(options.AccessToken));
    }

    private bool IsAuthorized(HttpListenerContext context)
    {
        var options = Options;
        if (!IsAuthRequired(options))
        {
            return true;
        }

        var token = context.Request.Headers["X-BrickController-Token"];
        var authorization = context.Request.Headers["Authorization"];
        if (string.IsNullOrWhiteSpace(token) &&
            authorization?.StartsWith("Bearer ", StringComparison.OrdinalIgnoreCase) == true)
        {
            token = authorization["Bearer ".Length..].Trim();
        }

        return FixedTimeEquals(token, options.AccessToken);
    }

    private static bool IsStatusRoute(string method, string[] segments)
        => method == "GET" && segments is ["api", "control", "status"];

    private static bool IsAuthRequired(HttpControlOptions options)
        => !string.IsNullOrWhiteSpace(options.AccessToken);

    private static bool FixedTimeEquals(string? left, string? right)
    {
        if (string.IsNullOrEmpty(left) || string.IsNullOrEmpty(right))
        {
            return false;
        }

        var leftBytes = Encoding.UTF8.GetBytes(left);
        var rightBytes = Encoding.UTF8.GetBytes(right);
        return leftBytes.Length == rightBytes.Length &&
            CryptographicOperations.FixedTimeEquals(leftBytes, rightBytes);
    }

    private static string? GetTokenPreview(string token)
        => string.IsNullOrWhiteSpace(token)
            ? null
            : token.Length <= 6 ? token : $"...{token[^6..]}";

    private async Task<T?> ReadJsonAsync<T>(HttpListenerContext context)
    {
        if (!context.Request.HasEntityBody)
        {
            return default;
        }

        return await JsonSerializer.DeserializeAsync<T>(context.Request.InputStream, JsonOptions).ConfigureAwait(false);
    }

    private static async Task WriteJsonAsync(HttpListenerContext context, HttpStatusCode statusCode, object value)
    {
        var response = context.Response;
        response.StatusCode = (int)statusCode;
        response.ContentType = "application/json; charset=utf-8";
        var json = JsonSerializer.Serialize(value, JsonOptions);
        var bytes = Encoding.UTF8.GetBytes(json);
        response.ContentLength64 = bytes.Length;
        await response.OutputStream.WriteAsync(bytes).ConfigureAwait(false);
        response.Close();
    }

    private static Task WriteNoContentAsync(HttpListenerContext context)
    {
        context.Response.StatusCode = (int)HttpStatusCode.NoContent;
        context.Response.Close();
        return Task.CompletedTask;
    }

    private void EnsureInputEventSubscription()
    {
        if (_inputEventsSubscribed)
        {
            return;
        }

        _inputDeviceManagerService.InputDeviceEvent += InputDeviceEventHandler;
        _inputEventsSubscribed = true;
    }

    private void RemoveInputEventSubscription()
    {
        if (!_inputEventsSubscribed)
        {
            return;
        }

        _inputDeviceManagerService.InputDeviceEvent -= InputDeviceEventHandler;
        _inputEventsSubscribed = false;
    }

    private void InputDeviceEventHandler(object? sender, InputDeviceEventArgs e)
    {
        _registry.ObserveInputDeviceEvent(e);
    }

    private void InputDevicesChangedEventHandler(object? sender, InputDeviceChangedEventArgs e)
    {
        foreach (var inputDevice in e.Items)
        {
            if (e.Action == NotifyInputDevicesChangedAction.Connected)
            {
                _registry.MarkInputDeviceConnected(inputDevice);
            }
            else
            {
                _registry.MarkInputDeviceDisconnected(inputDevice);
            }
        }
    }

    private int GetNextVirtualControllerNumber()
    {
        var usedNumbers = _registry.GetControllers()
            .Select(x => x.Number)
            .ToHashSet();
        var number = 1;
        while (usedNumbers.Contains(number))
        {
            number++;
        }

        return number;
    }

    private string GetNextVirtualControllerId()
    {
        var number = GetNextVirtualControllerNumber();
        return InputDevices.GetControllerIdFromNumber(number);
    }

    private static IReadOnlyList<string> GetListenerPrefixes(HttpControlOptions options)
        => options.ListenMode == HttpControlListenMode.LocalNetwork
            ? [$"http://*:{options.Port}/"]
            : [$"http://127.0.0.1:{options.Port}/"];

    private static IReadOnlyList<string> GetReachableUrls(HttpControlOptions options)
    {
        var urls = new List<string> { $"http://127.0.0.1:{options.Port}" };
        if (options.ListenMode == HttpControlListenMode.LocalNetwork)
        {
            urls.AddRange(GetLocalIPAddresses().Select(address => $"http://{address}:{options.Port}"));
        }

        return urls.Distinct(StringComparer.OrdinalIgnoreCase).ToArray();
    }

    private static IEnumerable<string> GetLocalIPAddresses()
    {
        foreach (var networkInterface in NetworkInterface.GetAllNetworkInterfaces())
        {
            if (networkInterface.OperationalStatus != OperationalStatus.Up ||
                networkInterface.NetworkInterfaceType == NetworkInterfaceType.Loopback)
            {
                continue;
            }

            foreach (var address in networkInterface.GetIPProperties().UnicastAddresses)
            {
                if (address.Address.AddressFamily == AddressFamily.InterNetwork &&
                    !IPAddress.IsLoopback(address.Address) &&
                    !address.Address.ToString().StartsWith("169.254.", StringComparison.Ordinal))
                {
                    yield return address.Address.ToString();
                }
            }
        }
    }

    private static List<CapabilityDefinitionRequest> GetDefaultCapabilities() =>
    [
        Button("buttonA", "Button_A", "Button A"),
        Button("buttonB", "Button_B", "Button B"),
        Button("buttonX", "Button_X", "Button X"),
        Button("buttonY", "Button_Y", "Button Y"),
        Button("leftShoulder", "LeftShoulder", "Left shoulder"),
        Button("rightShoulder", "RightShoulder", "Right shoulder"),
        Axis("leftTrigger", "LeftTrigger", "Left trigger", 0, 1, 0),
        Axis("rightTrigger", "RightTrigger", "Right trigger", 0, 1, 0),
        Axis("dPadX", "DPad_X", "D-pad X"),
        Axis("dPadY", "DPad_Y", "D-pad Y"),
        Axis("leftThumbStickX", "LeftThumbStick_X", "Left stick X"),
        Axis("leftThumbStickY", "LeftThumbStick_Y", "Left stick Y"),
        Axis("rightThumbStickX", "RightThumbStick_X", "Right stick X"),
        Axis("rightThumbStickY", "RightThumbStick_Y", "Right stick Y")
    ];

    private static CapabilityDefinitionRequest Button(string id, string eventCode, string displayName)
        => new()
        {
            Id = id,
            EventType = InputDeviceEventType.Button,
            EventCode = eventCode,
            DisplayName = displayName,
            MinValue = 0,
            MaxValue = 1,
            NeutralValue = 0
        };

    private static CapabilityDefinitionRequest Axis(
        string id,
        string eventCode,
        string displayName,
        float minValue = -1,
        float maxValue = 1,
        float neutralValue = 0)
        => new()
        {
            Id = id,
            EventType = InputDeviceEventType.Axis,
            EventCode = eventCode,
            DisplayName = displayName,
            MinValue = minValue,
            MaxValue = maxValue,
            NeutralValue = neutralValue
        };

    private void RaiseStatusChanged()
    {
        StatusChanged?.Invoke(this, EventArgs.Empty);
    }
}
