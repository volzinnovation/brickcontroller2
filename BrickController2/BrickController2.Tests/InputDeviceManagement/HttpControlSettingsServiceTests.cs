using BrickController2.InputDeviceManagement.HttpControl;
using BrickController2.UI.Services.Preferences;
using FluentAssertions;
using System;
using System.Collections.Generic;
using Xunit;

namespace BrickController2.Tests.InputDeviceManagement;

public class HttpControlSettingsServiceTests
{
    [Fact]
    public void Current_UsesEnvironmentOverrides()
    {
        var preferences = new InMemoryPreferencesService();
        var service = new HttpControlSettingsService(preferences);

        WithEnvironment(
            [
                (HttpControlSettingsService.EnabledEnvironmentVariable, "true"),
                (HttpControlSettingsService.PortEnvironmentVariable, "5099"),
                (HttpControlSettingsService.ListenModeEnvironmentVariable, "LocalNetwork"),
                (HttpControlSettingsService.TokenEnvironmentVariable, "test-token")
            ],
            () =>
            {
                var options = service.Current;

                options.Enabled.Should().BeTrue();
                options.Port.Should().Be(5099);
                options.ListenMode.Should().Be(HttpControlListenMode.LocalNetwork);
                options.AccessToken.Should().Be("test-token");
            });
    }

    [Fact]
    public void Current_UsesTokenAlias()
    {
        var service = new HttpControlSettingsService(new InMemoryPreferencesService());

        WithEnvironment(
            [(HttpControlSettingsService.TokenEnvironmentVariableAlias, "alias-token")],
            () => service.Current.AccessToken.Should().Be("alias-token"));
    }

    private static void WithEnvironment(IReadOnlyList<(string Name, string Value)> variables, Action action)
    {
        var previousValues = new Dictionary<string, string?>();
        foreach (var (name, value) in variables)
        {
            previousValues[name] = Environment.GetEnvironmentVariable(name);
            Environment.SetEnvironmentVariable(name, value);
        }

        try
        {
            action();
        }
        finally
        {
            foreach (var (name, value) in previousValues)
            {
                Environment.SetEnvironmentVariable(name, value);
            }
        }
    }

    private sealed class InMemoryPreferencesService : IPreferencesService
    {
        private readonly Dictionary<(string Key, string? Section), object> _values = new();

        public bool ContainsKey(string key, string? section = null)
            => _values.ContainsKey((key, section));

        public T Get<T>(string key, string? section = null) where T : notnull
            => _values.TryGetValue((key, section), out var value) ? (T)value : default!;

        public T Get<T>(string key, T defaultValue, string? section = null) where T : notnull
            => _values.TryGetValue((key, section), out var value) ? (T)value : defaultValue;

        public void Set<T>(string key, T value, string? section = null) where T : notnull
            => _values[(key, section)] = value;
    }
}
