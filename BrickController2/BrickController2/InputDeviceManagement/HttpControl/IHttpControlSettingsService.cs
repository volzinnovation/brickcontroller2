namespace BrickController2.InputDeviceManagement.HttpControl;

internal interface IHttpControlSettingsService
{
    HttpControlOptions Current { get; }

    void Save(HttpControlOptions options);

    string EnsureAccessToken();

    string RegenerateAccessToken();
}
