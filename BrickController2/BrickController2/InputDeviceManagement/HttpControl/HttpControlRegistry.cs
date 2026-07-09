using BrickController2.InputDeviceManagement.Sensors;
using BrickController2.PlatformServices.InputDevice;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace BrickController2.InputDeviceManagement.HttpControl;

internal sealed class HttpControlRegistry
{
    private static readonly TimeSpan VirtualControllerStaleAfter = TimeSpan.FromSeconds(30);
    private static readonly Regex ControllerNumberRegex = new(@"(\d+)$", RegexOptions.Compiled);

    private readonly object _lockObject = new();
    private readonly Dictionary<string, ControllerState> _controllers = new(StringComparer.OrdinalIgnoreCase);

    public IReadOnlyList<HttpControllerResponse> GetControllers()
    {
        lock (_lockObject)
        {
            return _controllers.Values
                .OrderBy(x => x.Number)
                .ThenBy(x => x.Id, StringComparer.OrdinalIgnoreCase)
                .Select(ToResponse)
                .ToArray();
        }
    }

    public HttpControllerResponse? GetController(string controllerId)
    {
        lock (_lockObject)
        {
            return _controllers.TryGetValue(controllerId, out var controller)
                ? ToResponse(controller)
                : null;
        }
    }

    public IReadOnlyList<HttpCapabilityResponse>? GetCapabilities(string controllerId)
    {
        lock (_lockObject)
        {
            return _controllers.TryGetValue(controllerId, out var controller)
                ? controller.Capabilities.Values.OrderBy(x => x.Id, StringComparer.OrdinalIgnoreCase).Select(ToResponse).ToArray()
                : null;
        }
    }

    public HttpCapabilityResponse? GetCapability(string controllerId, string capabilityId)
    {
        lock (_lockObject)
        {
            return TryGetCapability(controllerId, capabilityId, out _, out var capability)
                ? ToResponse(capability)
                : null;
        }
    }

    public ControllerState UpsertVirtualController(string controllerId, string name, int number, IEnumerable<CapabilityDefinitionRequest> capabilities)
    {
        lock (_lockObject)
        {
            if (!_controllers.TryGetValue(controllerId, out var controller))
            {
                controller = new ControllerState(controllerId, name, number, HttpControlControllerSource.Http, writable: true);
                _controllers[controllerId] = controller;
            }

            controller.Name = name;
            controller.Number = number;
            controller.Source = HttpControlControllerSource.Http;
            controller.Status = HttpControlControllerStatus.Connected;
            controller.Writable = true;
            controller.LastSeenAt = DateTimeOffset.UtcNow;
            controller.Capabilities.Clear();

            foreach (var capability in capabilities)
            {
                var state = CreateCapability(capability, writable: true, observed: false);
                controller.Capabilities[state.Id] = state;
            }

            return controller;
        }
    }

    public void RemoveVirtualController(string controllerId)
    {
        lock (_lockObject)
        {
            if (_controllers.TryGetValue(controllerId, out var controller) &&
                controller.Source == HttpControlControllerSource.Http)
            {
                controller.Status = HttpControlControllerStatus.Disconnected;
                controller.Writable = false;
                controller.LastSeenAt = DateTimeOffset.UtcNow;
                foreach (var capability in controller.Capabilities.Values)
                {
                    capability.Writable = false;
                }
            }
        }
    }

    public void MarkInputDeviceConnected(IInputDevice inputDevice)
    {
        lock (_lockObject)
        {
            if (_controllers.TryGetValue(inputDevice.InputDeviceId, out var existing) &&
                existing.Source == HttpControlControllerSource.Http)
            {
                existing.Status = HttpControlControllerStatus.Connected;
                existing.LastSeenAt = DateTimeOffset.UtcNow;
                return;
            }

            var controller = GetOrCreateController(inputDevice.InputDeviceId, inputDevice.Name, inputDevice.InputDeviceNumber, GetSource(inputDevice));
            controller.Status = HttpControlControllerStatus.Connected;
            controller.LastSeenAt = DateTimeOffset.UtcNow;
        }
    }

    public void MarkInputDeviceDisconnected(IInputDevice inputDevice)
    {
        lock (_lockObject)
        {
            if (_controllers.TryGetValue(inputDevice.InputDeviceId, out var controller))
            {
                controller.Status = HttpControlControllerStatus.Disconnected;
                controller.LastSeenAt = DateTimeOffset.UtcNow;
            }
        }
    }

    public IReadOnlyList<HttpCapabilitySetResponse>? TrySetCapability(
        string controllerId,
        IReadOnlyDictionary<string, float> values,
        out Dictionary<(InputDeviceEventType EventType, string EventCode), float> events,
        out string? error)
    {
        lock (_lockObject)
        {
            events = [];
            error = null;

            if (!_controllers.TryGetValue(controllerId, out var controller))
            {
                error = $"Controller '{controllerId}' was not found.";
                return null;
            }

            if (!controller.Writable)
            {
                error = $"Controller '{controllerId}' is read-only.";
                return null;
            }

            var resolved = new List<(CapabilityState Capability, float Value)>();
            foreach (var value in values)
            {
                if (!TryGetCapability(controllerId, value.Key, out _, out var capability))
                {
                    error = $"Capability '{value.Key}' was not found on controller '{controllerId}'.";
                    return null;
                }

                if (!capability.Writable)
                {
                    error = $"Capability '{capability.Id}' is read-only.";
                    return null;
                }

                if (value.Value < capability.MinValue || value.Value > capability.MaxValue)
                {
                    error = $"Value {value.Value} is outside '{capability.Id}' range {capability.MinValue}..{capability.MaxValue}.";
                    return null;
                }

                resolved.Add((capability, value.Value));
            }

            var updatedAt = DateTimeOffset.UtcNow;
            controller.LastSeenAt = updatedAt;
            controller.Status = HttpControlControllerStatus.Connected;

            var responses = new List<HttpCapabilitySetResponse>();
            foreach (var (capability, value) in resolved)
            {
                capability.Value = value;
                capability.Observed = true;
                capability.UpdatedAt = updatedAt;
                events[(capability.EventType, capability.EventCode)] = value;
                responses.Add(new HttpCapabilitySetResponse(
                    controller.Id,
                    capability.Id,
                    capability.EventType,
                    capability.EventCode,
                    capability.Value,
                    updatedAt));
            }

            return responses;
        }
    }

    public void ObserveInputDeviceEvent(InputDeviceEventArgs args)
    {
        lock (_lockObject)
        {
            var controller = GetOrCreateController(args.InputDeviceId, args.InputDeviceId, ParseControllerNumber(args.InputDeviceId), HttpControlControllerSource.Unknown);
            controller.Status = HttpControlControllerStatus.Connected;
            controller.LastSeenAt = DateTimeOffset.UtcNow;

            foreach (var inputEvent in args.InputDeviceEvents)
            {
                var capabilityId = inputEvent.Key.EventCode;
                var capability = controller.Capabilities.Values.FirstOrDefault(x =>
                    x.EventType == inputEvent.Key.EventType &&
                    string.Equals(x.EventCode, inputEvent.Key.EventCode, StringComparison.OrdinalIgnoreCase));

                if (capability is null)
                {
                    capability = new CapabilityState(
                        capabilityId,
                        inputEvent.Key.EventType,
                        inputEvent.Key.EventCode,
                        inputEvent.Key.EventCode,
                        GetDefaultMin(inputEvent.Key.EventType),
                        GetDefaultMax(inputEvent.Key.EventType),
                        0,
                        writable: controller.Source == HttpControlControllerSource.Http,
                        observed: true);
                    controller.Capabilities[capability.Id] = capability;
                }

                capability.Value = inputEvent.Value;
                capability.Observed = true;
                capability.UpdatedAt = controller.LastSeenAt;
            }
        }
    }

    public static int ParseControllerNumber(string controllerId)
    {
        var match = ControllerNumberRegex.Match(controllerId);
        return match.Success && int.TryParse(match.Groups[1].Value, out var number) ? number : 0;
    }

    private static CapabilityState CreateCapability(CapabilityDefinitionRequest capability, bool writable, bool observed)
    {
        var id = string.IsNullOrWhiteSpace(capability.Id) ? capability.EventCode : capability.Id;
        var eventCode = string.IsNullOrWhiteSpace(capability.EventCode) ? id : capability.EventCode;
        if (string.IsNullOrWhiteSpace(id) || string.IsNullOrWhiteSpace(eventCode))
        {
            throw new InvalidOperationException("Capability id or eventCode is required.");
        }

        return new CapabilityState(
            id,
            capability.EventType,
            eventCode,
            string.IsNullOrWhiteSpace(capability.DisplayName) ? eventCode : capability.DisplayName,
            capability.MinValue ?? GetDefaultMin(capability.EventType),
            capability.MaxValue ?? GetDefaultMax(capability.EventType),
            capability.NeutralValue ?? 0,
            writable,
            observed);
    }

    private bool TryGetCapability(string controllerId, string capabilityId, out ControllerState controller, out CapabilityState capability)
    {
        controller = null!;
        capability = null!;
        return _controllers.TryGetValue(controllerId, out controller!) &&
            controller.Capabilities.TryGetValue(capabilityId, out capability!);
    }

    private ControllerState GetOrCreateController(string id, string name, int number, HttpControlControllerSource source)
    {
        if (!_controllers.TryGetValue(id, out var controller))
        {
            controller = new ControllerState(id, name, number, source, writable: source == HttpControlControllerSource.Http);
            _controllers[id] = controller;
        }

        if (controller.Source == HttpControlControllerSource.Unknown && source != HttpControlControllerSource.Unknown)
        {
            controller.Source = source;
            controller.Writable = source == HttpControlControllerSource.Http;
        }

        if (string.IsNullOrWhiteSpace(controller.Name) || controller.Name == controller.Id)
        {
            controller.Name = name;
        }

        if (controller.Number == 0)
        {
            controller.Number = number;
        }

        return controller;
    }

    private static HttpControllerResponse ToResponse(ControllerState controller)
    {
        var status = controller.Status;
        if (controller.Source == HttpControlControllerSource.Http &&
            status == HttpControlControllerStatus.Connected &&
            controller.LastSeenAt is DateTimeOffset lastSeenAt &&
            DateTimeOffset.UtcNow - lastSeenAt > VirtualControllerStaleAfter)
        {
            status = HttpControlControllerStatus.Stale;
        }

        return new HttpControllerResponse(
            controller.Id,
            controller.Name,
            controller.Number,
            controller.Source,
            status,
            Readable: true,
            controller.Writable,
            controller.LastSeenAt,
            controller.Capabilities.Values.OrderBy(x => x.Id, StringComparer.OrdinalIgnoreCase).Select(ToResponse).ToArray());
    }

    private static HttpCapabilityResponse ToResponse(CapabilityState capability)
        => new(
            capability.Id,
            capability.EventType,
            capability.EventCode,
            capability.DisplayName,
            capability.Value,
            capability.MinValue,
            capability.MaxValue,
            capability.NeutralValue,
            capability.Writable,
            capability.Observed,
            capability.UpdatedAt);

    private static HttpControlControllerSource GetSource(IInputDevice inputDevice)
    {
        var type = inputDevice.GetType();
        if (type == typeof(OrientationSensorController))
        {
            return HttpControlControllerSource.OrientationSensor;
        }

        if (type.Name.Contains("LegoRemote", StringComparison.OrdinalIgnoreCase))
        {
            return HttpControlControllerSource.LegoRemote;
        }

        if (type.Name.Contains("Gamepad", StringComparison.OrdinalIgnoreCase) ||
            type.Name.Contains("Controller", StringComparison.OrdinalIgnoreCase))
        {
            return HttpControlControllerSource.PhysicalGamepad;
        }

        return HttpControlControllerSource.Unknown;
    }

    private static float GetDefaultMin(InputDeviceEventType eventType)
        => eventType == InputDeviceEventType.Axis ? InputDevices.AXIS_MIN_VALUE : InputDevices.BUTTON_RELEASED;

    private static float GetDefaultMax(InputDeviceEventType eventType)
        => eventType == InputDeviceEventType.Axis ? InputDevices.AXIS_MAX_VALUE : InputDevices.BUTTON_PRESSED;

    internal sealed class ControllerState
    {
        public ControllerState(string id, string name, int number, HttpControlControllerSource source, bool writable)
        {
            Id = id;
            Name = name;
            Number = number;
            Source = source;
            Writable = writable;
            Status = HttpControlControllerStatus.Connected;
        }

        public string Id { get; }
        public string Name { get; set; }
        public int Number { get; set; }
        public HttpControlControllerSource Source { get; set; }
        public HttpControlControllerStatus Status { get; set; }
        public bool Writable { get; set; }
        public DateTimeOffset? LastSeenAt { get; set; }
        internal Dictionary<string, CapabilityState> Capabilities { get; } = new(StringComparer.OrdinalIgnoreCase);
    }

    internal sealed class CapabilityState
    {
        public CapabilityState(
            string id,
            InputDeviceEventType eventType,
            string eventCode,
            string displayName,
            float minValue,
            float maxValue,
            float neutralValue,
            bool writable,
            bool observed)
        {
            Id = id;
            EventType = eventType;
            EventCode = eventCode;
            DisplayName = displayName;
            MinValue = minValue;
            MaxValue = maxValue;
            NeutralValue = neutralValue;
            Writable = writable;
            Observed = observed;
            Value = neutralValue;
        }

        public string Id { get; }
        public InputDeviceEventType EventType { get; }
        public string EventCode { get; }
        public string DisplayName { get; }
        public float Value { get; set; }
        public float MinValue { get; }
        public float MaxValue { get; }
        public float NeutralValue { get; }
        public bool Writable { get; set; }
        public bool Observed { get; set; }
        public DateTimeOffset? UpdatedAt { get; set; }
    }
}
