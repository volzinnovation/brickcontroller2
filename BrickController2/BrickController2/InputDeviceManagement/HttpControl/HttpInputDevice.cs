using BrickController2.PlatformServices.InputDevice;
using BrickController2.PlatformServices.InputDeviceService;
using System.Collections.Generic;

namespace BrickController2.InputDeviceManagement.HttpControl;

internal sealed class HttpInputDevice : InputDeviceBase<HttpInputDeviceState>
{
    public HttpInputDevice(
        IInputDeviceEventServiceInternal inputDeviceEventService,
        string inputDeviceId,
        string name,
        int inputDeviceNumber)
        : base(inputDeviceEventService, new HttpInputDeviceState(inputDeviceId))
    {
        InputDeviceId = inputDeviceId;
        Name = name;
        InputDeviceNumber = inputDeviceNumber;
    }

    public void Publish(IDictionary<(InputDeviceEventType EventType, string EventCode), float> events)
    {
        RaiseEvent(events);
    }
}

internal sealed record HttpInputDeviceState(string InputDeviceId);
