namespace BrickController2.InputDeviceManagement.HttpControl;

public enum HttpControlListenMode
{
    Loopback,
    LocalNetwork
}

public enum HttpControlRuntimeStatus
{
    Stopped,
    Running,
    Error,
    Unsupported
}

internal enum HttpControlControllerStatus
{
    Connected,
    Disconnected,
    Stale,
    Unsupported,
    Error
}

internal enum HttpControlControllerSource
{
    PhysicalGamepad,
    OrientationSensor,
    LegoRemote,
    Http,
    Unknown
}
