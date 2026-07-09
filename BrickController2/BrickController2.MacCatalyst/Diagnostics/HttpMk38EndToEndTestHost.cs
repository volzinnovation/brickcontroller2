using BrickController2.BusinessLogic;
using BrickController2.CreationManagement;
using BrickController2.DeviceManagement;
using BrickController2.DeviceManagement.MouldKing;
using BrickController2.PlatformServices.BluetoothLE;
using BrickController2.PlatformServices.InputDevice;
using BrickController2.PlatformServices.InputDeviceService;
using BrickController2.Protocols;
using BrickController2.UI.Services.AppIdentifier;
using Microsoft.Maui.ApplicationModel;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace BrickController2.MacCatalyst.Diagnostics;

internal sealed class HttpMk38EndToEndTestHost
{
    private const string EnabledEnvironmentVariable = "BC2_HTTP_MK38_E2E_TEST";
    private const string LogEnvironmentVariable = "BC2_HTTP_MK38_E2E_TEST_LOG";
    private const string ControllerId = "HTTP MK3.8 Test";
    private const string CreationName = "HTTP MK3.8 End-to-End Test";
    private const string ProfileName = "HTTP MK3.8 End-to-End Test";
    private const string ReadyMarker = "BC2_HTTP_MK38_E2E_READY";
    private const string FailureMarker = "BC2_HTTP_MK38_E2E_FAIL";
    private static readonly TimeSpan BluetoothStartupTimeout = TimeSpan.FromSeconds(8);
    private static readonly TimeSpan ConnectTimeout = TimeSpan.FromSeconds(10);

    private readonly ICreationManager _creationManager;
    private readonly IDeviceManager _deviceManager;
    private readonly IManualDeviceManager _manualDeviceManager;
    private readonly IBluetoothLEService _bluetoothLEService;
    private readonly IMKPlatformService _mkPlatformService;
    private readonly IAppIdentifierService _appIdentifierService;
    private readonly IInputDeviceEventService _inputDeviceEventService;
    private readonly IPlayLogic _playLogic;
    private bool _inputEventsSubscribed;

    public HttpMk38EndToEndTestHost(
        ICreationManager creationManager,
        IDeviceManager deviceManager,
        IManualDeviceManager manualDeviceManager,
        IBluetoothLEService bluetoothLEService,
        IMKPlatformService mkPlatformService,
        IAppIdentifierService appIdentifierService,
        IInputDeviceEventService inputDeviceEventService,
        IPlayLogic playLogic)
    {
        _creationManager = creationManager;
        _deviceManager = deviceManager;
        _manualDeviceManager = manualDeviceManager;
        _bluetoothLEService = bluetoothLEService;
        _mkPlatformService = mkPlatformService;
        _appIdentifierService = appIdentifierService;
        _inputDeviceEventService = inputDeviceEventService;
        _playLogic = playLogic;
    }

    public static bool IsRequested
        => string.Equals(Environment.GetEnvironmentVariable(EnabledEnvironmentVariable), "1", StringComparison.OrdinalIgnoreCase) ||
           string.Equals(Environment.GetEnvironmentVariable(EnabledEnvironmentVariable), "true", StringComparison.OrdinalIgnoreCase);

    public async Task<int> StartAsync(CancellationToken token)
    {
        var logPath = Environment.GetEnvironmentVariable(LogEnvironmentVariable);
        StreamWriter? logWriter = null;

        if (!string.IsNullOrWhiteSpace(logPath))
        {
            logPath = Path.GetFullPath(logPath);
            var logDirectory = Path.GetDirectoryName(logPath);
            if (!string.IsNullOrEmpty(logDirectory))
            {
                Directory.CreateDirectory(logDirectory);
            }

            logWriter = new StreamWriter(logPath, append: false) { AutoFlush = true };
        }

        void Log(string message)
        {
            var line = $"{DateTimeOffset.Now:yyyy-MM-dd HH:mm:ss.fff zzz} {message}";
            Console.WriteLine(line);
            logWriter?.WriteLine(line);
        }

        try
        {
            Log("BrickController2 HTTP MK 3.8 end-to-end test host starting.");
            Log($"Controller ID: {ControllerId}");
            Log($"Creation/profile: {CreationName}");

            await CheckBluetoothAsync(Log, token).ConfigureAwait(false);
            await ProbeAdvertisingAsync(Log, token).ConfigureAwait(false);

            var device = await EnsureMk38DeviceAsync(Log).ConfigureAwait(false);
            var profile = await EnsureProfileAsync(device, Log).ConfigureAwait(false);

            _playLogic.ActiveProfile = profile;
            EnsureInputEventSubscription();
            await ConnectDeviceAsync(device, Log, token).ConfigureAwait(false);
            _playLogic.StartPlay();

            Log($"Profile '{ProfileName}' is active and maps {ControllerId} capabilities MK38_Channel_0..4 to {device.Id} channels 0..4.");
            Log(ReadyMarker);
            return 0;
        }
        catch (OperationCanceledException)
        {
            Log($"{FailureMarker}: setup timed out or was cancelled.");
            return 2;
        }
        catch (Exception ex)
        {
            Log($"{FailureMarker}: {ex.GetType().Name}: {ex.Message}");
            Log(ex.ToString());
            return 1;
        }
        finally
        {
            logWriter?.Dispose();
        }
    }

    private async Task CheckBluetoothAsync(Action<string> log, CancellationToken token)
    {
        log("Checking Bluetooth LE support.");
        if (!await _bluetoothLEService.IsBluetoothLESupportedAsync().ConfigureAwait(false))
        {
            throw new InvalidOperationException("Bluetooth LE is not supported by this Mac/App runtime.");
        }

        if (!await _bluetoothLEService.IsBluetoothLEAdvertisingSupportedAsync().ConfigureAwait(false))
        {
            throw new InvalidOperationException("Bluetooth LE advertising is not supported by this Mac/App runtime.");
        }

        var deadline = DateTimeOffset.UtcNow + BluetoothStartupTimeout;
        while (DateTimeOffset.UtcNow < deadline)
        {
            if (await _bluetoothLEService.IsBluetoothOnAsync().ConfigureAwait(false))
            {
                log("Bluetooth LE central manager reports PoweredOn.");
                return;
            }

            await Task.Delay(250, token).ConfigureAwait(false);
        }

        log("Bluetooth LE central manager did not report PoweredOn before the advertising probe. Continuing so the advertiser can report its exact state.");
    }

    private async Task ProbeAdvertisingAsync(Action<string> log, CancellationToken token)
    {
        log("Creating Bluetooth LE advertiser probe.");
        using var advertiser = _bluetoothLEService.CreateBluetoothLEAdvertiserDevice()
            ?? throw new InvalidOperationException("CreateBluetoothLEAdvertiserDevice returned null.");

        var payload = BuildMk38ConnectPayload();
        log($"Starting advertising probe with MK payload length {payload.Length}.");
        await advertiser.StartAdvertiseAsync(AdvertisingInterval.Min, TxPowerLevel.Max, MKProtocol.ManufacturerID, payload).ConfigureAwait(false);
        await Task.Delay(300, token).ConfigureAwait(false);
        await advertiser.StopAdvertiseAsync().ConfigureAwait(false);
        log("Advertising probe started and stopped successfully.");
    }

    private async Task<Device> EnsureMk38DeviceAsync(Action<string> log)
    {
        await _deviceManager.LoadDevicesAsync().ConfigureAwait(false);

        var factoryData = GetMk38FactoryData();
        var device = _deviceManager.Devices.FirstOrDefault(x => x.DeviceType == DeviceType.MK3_8 && x.Address == factoryData.Address);
        if (device is not null)
        {
            log($"Using existing MK 3.8 manual device '{device.Name}' ({device.Id}).");
            return device;
        }

        log($"Creating MK 3.8 manual device '{factoryData.Name}' with address '{factoryData.Address}'.");
        await _deviceManager.CreateDeviceAsync(factoryData.DeviceType, factoryData.Name, factoryData.Address, factoryData.DeviceData).ConfigureAwait(false);

        return _deviceManager.Devices.FirstOrDefault(x => x.DeviceType == DeviceType.MK3_8 && x.Address == factoryData.Address)
            ?? throw new InvalidOperationException("MK 3.8 manual device was created but is not available through DeviceManager.");
    }

    private async Task<ControllerProfile> EnsureProfileAsync(Device device, Action<string> log)
    {
        await _creationManager.LoadCreationsAndSequencesAsync().ConfigureAwait(false);

        var creation = _creationManager.Creations.FirstOrDefault(x => x.Name == CreationName);
        if (creation is null)
        {
            log($"Creating test creation '{CreationName}'.");
            creation = await _creationManager.AddCreationAsync(CreationName).ConfigureAwait(false);
        }

        var existingProfile = creation.ControllerProfiles.FirstOrDefault(x => x.Name == ProfileName);
        if (existingProfile is not null)
        {
            log($"Replacing existing test profile '{ProfileName}'.");
            await _creationManager.DeleteControllerProfileAsync(existingProfile).ConfigureAwait(false);
        }

        var profile = await _creationManager.AddControllerProfileAsync(creation, ProfileName).ConfigureAwait(false);

        for (var channel = 0; channel < device.NumberOfChannels; channel++)
        {
            var controllerEvent = await _creationManager.AddOrGetControllerEventAsync(
                profile,
                ControllerId,
                InputDeviceEventType.Axis,
                $"MK38_Channel_{channel}").ConfigureAwait(false);

            await _creationManager.AddOrUpdateControllerActionAsync(
                controllerEvent,
                device.Id,
                channel,
                isInvert: false,
                ControllerButtonType.Normal,
                ControllerAxisType.Normal,
                ControllerAxisCharacteristic.Linear,
                maxOutputPercent: 100,
                axisActiveZonePercent: 100,
                axisDeadZonePercent: 0,
                ChannelOutputType.NormalMotor,
                maxServoAngle: 0,
                servoBaseAngle: 0,
                stepperAngle: 0,
                sequenceName: string.Empty).ConfigureAwait(false);
        }

        log($"Created profile '{ProfileName}' with {device.NumberOfChannels} HTTP axis mappings.");
        return profile;
    }

    private async Task ConnectDeviceAsync(Device device, Action<string> log, CancellationToken token)
    {
        using var timeout = CancellationTokenSource.CreateLinkedTokenSource(token);
        timeout.CancelAfter(ConnectTimeout);

        void OnDisconnected(Device disconnectedDevice)
        {
            log($"ERROR: Device disconnected callback fired for '{disconnectedDevice.Name}' while HTTP end-to-end test host was running.");
        }

        log($"Connecting '{device.Name}' ({device.DeviceType}) using normal app path.");
        var result = await device.ConnectAsync(
            reconnect: false,
            onDeviceDisconnected: OnDisconnected,
            channelConfigurations: GetNormalMotorChannelConfigurations(device),
            startOutputProcessing: true,
            requestDeviceInformation: false,
            token: timeout.Token).ConfigureAwait(false);

        if (result != DeviceConnectionResult.Ok)
        {
            throw new InvalidOperationException($"Device.ConnectAsync returned {result}. Device state: {device.DeviceState}.");
        }

        if (device.DeviceState != DeviceState.Connected)
        {
            throw new InvalidOperationException($"Device did not remain connected. Device state: {device.DeviceState}.");
        }

        log($"Connected. Device state: {device.DeviceState}.");
    }

    private void EnsureInputEventSubscription()
    {
        if (_inputEventsSubscribed)
        {
            return;
        }

        _inputDeviceEventService.InputDeviceEvent += InputDeviceEventHandler;
        _inputEventsSubscribed = true;
    }

    private void InputDeviceEventHandler(object? sender, InputDeviceEventArgs e)
    {
        _playLogic.ProcessGameControllerEvent(e);
    }

    private IDeviceFactoryData GetMk38FactoryData()
        => _manualDeviceManager.FactoryDataList.FirstOrDefault(x => x.DeviceType == DeviceType.MK3_8)
           ?? throw new InvalidOperationException("MK 3.8 manual device factory data is not registered.");

    private byte[] BuildMk38ConnectPayload()
    {
        byte[] telegram =
        [
            0xB1, 0x7B, 0xA7, 0x80, 0x80, 0x80, 0x4F, 0xC1
        ];

        var appId = _appIdentifierService.GetAppId(2).Span;
        telegram[1] = appId[0];
        telegram[2] = appId[1];

        if (!_mkPlatformService.TryGetRfPayload(telegram, out var payload))
        {
            throw new InvalidOperationException("MK platform service could not build the RF payload.");
        }

        return payload;
    }

    private static IEnumerable<ChannelConfiguration> GetNormalMotorChannelConfigurations(Device device)
        => Enumerable.Range(0, device.NumberOfChannels)
            .Select(channel => new ChannelConfiguration
            {
                Channel = channel,
                ChannelOutputType = ChannelOutputType.NormalMotor
            })
            .ToArray();
}

internal static class HttpMk38EndToEndTestHostLauncher
{
    public static void StartIfRequested(IServiceProvider services)
    {
        if (!HttpMk38EndToEndTestHost.IsRequested)
        {
            return;
        }

        MainThread.BeginInvokeOnMainThread(async () =>
        {
            await Task.Delay(1000).ConfigureAwait(true);
            var host = services.GetService(typeof(HttpMk38EndToEndTestHost)) as HttpMk38EndToEndTestHost;
            var exitCode = host is null ? 1 : await host.StartAsync(CancellationToken.None).ConfigureAwait(true);
            if (exitCode != 0)
            {
                Environment.Exit(exitCode);
            }
        });
    }
}
