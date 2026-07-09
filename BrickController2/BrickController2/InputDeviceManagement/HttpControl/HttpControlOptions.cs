namespace BrickController2.InputDeviceManagement.HttpControl;

public sealed record HttpControlOptions(
    bool Enabled,
    int Port,
    HttpControlListenMode ListenMode,
    string AccessToken)
{
    public const int DefaultPort = 5081;
}
