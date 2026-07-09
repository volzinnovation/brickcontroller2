using BrickController2.UI.Services.Preferences;
using System;
using System.Security.Cryptography;

namespace BrickController2.InputDeviceManagement.HttpControl;

internal sealed class HttpControlSettingsService : IHttpControlSettingsService
{
    internal const string EnabledEnvironmentVariable = "BRICKCONTROLLER_HTTP_ENABLED";
    internal const string PortEnvironmentVariable = "BRICKCONTROLLER_HTTP_PORT";
    internal const string ListenModeEnvironmentVariable = "BRICKCONTROLLER_HTTP_LISTEN_MODE";
    internal const string TokenEnvironmentVariable = "BRICKCONTROLLER_HTTP_TOKEN";
    internal const string TokenEnvironmentVariableAlias = "BC2_HTTP_TOKEN";

    private const string Section = "HttpControl";
    private const string EnabledKey = "Enabled";
    private const string PortKey = "Port";
    private const string ListenModeKey = "ListenMode";
    private const string AccessTokenKey = "AccessToken";

    private readonly IPreferencesService _preferences;

    public HttpControlSettingsService(IPreferencesService preferences)
    {
        _preferences = preferences;
    }

    public HttpControlOptions Current
    {
        get
        {
            var enabled = _preferences.Get(EnabledKey, false, Section);
            var enabledOverride = GetEnvironmentBool(EnabledEnvironmentVariable);
            if (enabledOverride.HasValue)
            {
                enabled = enabledOverride.Value;
            }

            var port = _preferences.Get(PortKey, HttpControlOptions.DefaultPort, Section);
            var portOverride = GetEnvironmentInt(PortEnvironmentVariable);
            if (portOverride.HasValue)
            {
                port = portOverride.Value;
            }

            if (!IsValidPort(port))
            {
                port = HttpControlOptions.DefaultPort;
            }

            var listenMode = _preferences.Get(ListenModeKey, HttpControlListenMode.Loopback, Section);
            var listenModeOverride = GetEnvironmentListenMode(ListenModeEnvironmentVariable);
            if (listenModeOverride.HasValue)
            {
                listenMode = listenModeOverride.Value;
            }

            var accessToken = _preferences.Get(AccessTokenKey, string.Empty, Section);
            var tokenOverride = GetEnvironmentString(TokenEnvironmentVariable, TokenEnvironmentVariableAlias);
            if (!string.IsNullOrWhiteSpace(tokenOverride))
            {
                accessToken = tokenOverride;
            }

            return new HttpControlOptions(
                enabled,
                port,
                listenMode,
                accessToken);
        }
    }

    public void Save(HttpControlOptions options)
    {
        if (!IsValidPort(options.Port))
        {
            throw new ArgumentOutOfRangeException(nameof(options.Port), "Port must be between 1 and 65535.");
        }

        var accessToken = options.AccessToken;
        if (options.Enabled && string.IsNullOrWhiteSpace(accessToken))
        {
            accessToken = GenerateToken();
        }

        _preferences.Set(EnabledKey, options.Enabled, Section);
        _preferences.Set(PortKey, options.Port, Section);
        _preferences.Set(ListenModeKey, options.ListenMode, Section);
        _preferences.Set(AccessTokenKey, accessToken, Section);
    }

    public string EnsureAccessToken()
    {
        var current = Current;
        if (!string.IsNullOrWhiteSpace(current.AccessToken))
        {
            return current.AccessToken;
        }

        var accessToken = GenerateToken();
        Save(current with { AccessToken = accessToken });
        return accessToken;
    }

    public string RegenerateAccessToken()
    {
        var accessToken = GenerateToken();
        Save(Current with { AccessToken = accessToken });
        return accessToken;
    }

    private static bool IsValidPort(int port) => port is >= 1 and <= 65535;

    private static bool? GetEnvironmentBool(string name)
    {
        var value = GetEnvironmentString(name);
        if (string.IsNullOrWhiteSpace(value))
        {
            return null;
        }

        return value.Trim().ToLowerInvariant() switch
        {
            "1" or "true" or "yes" or "on" => true,
            "0" or "false" or "no" or "off" => false,
            _ => null
        };
    }

    private static int? GetEnvironmentInt(string name)
    {
        var value = GetEnvironmentString(name);
        return int.TryParse(value, out var result) ? result : null;
    }

    private static HttpControlListenMode? GetEnvironmentListenMode(string name)
    {
        var value = GetEnvironmentString(name);
        return Enum.TryParse<HttpControlListenMode>(value, ignoreCase: true, out var listenMode)
            ? listenMode
            : null;
    }

    private static string GetEnvironmentString(params string[] names)
    {
        foreach (var name in names)
        {
            var value = Environment.GetEnvironmentVariable(name);
            if (!string.IsNullOrWhiteSpace(value))
            {
                return value.Trim();
            }
        }

        return string.Empty;
    }

    private static string GenerateToken()
    {
        Span<byte> bytes = stackalloc byte[24];
        RandomNumberGenerator.Fill(bytes);
        return Convert.ToBase64String(bytes);
    }
}
