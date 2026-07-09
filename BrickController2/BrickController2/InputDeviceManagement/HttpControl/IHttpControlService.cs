using System;
using System.Collections.Generic;

namespace BrickController2.InputDeviceManagement.HttpControl;

public interface IHttpControlService
{
    event EventHandler? StatusChanged;

    bool IsSupported { get; }

    HttpControlOptions Options { get; }

    HttpControlRuntimeStatus RuntimeStatus { get; }

    string? ErrorMessage { get; }

    IReadOnlyList<string> ReachableUrls { get; }

    void ApplyOptions(HttpControlOptions options);

    string RegenerateAccessToken();
}
