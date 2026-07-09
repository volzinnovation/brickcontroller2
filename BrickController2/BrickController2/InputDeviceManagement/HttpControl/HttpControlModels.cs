using BrickController2.PlatformServices.InputDevice;
using System;
using System.Collections.Generic;

namespace BrickController2.InputDeviceManagement.HttpControl;

internal sealed class VirtualControllerRequest
{
    public string? Id { get; set; }
    public string? Name { get; set; }
    public List<CapabilityDefinitionRequest>? Capabilities { get; set; }
}

internal sealed class CapabilityDefinitionRequest
{
    public string? Id { get; set; }
    public InputDeviceEventType EventType { get; set; }
    public string? EventCode { get; set; }
    public string? DisplayName { get; set; }
    public float? MinValue { get; set; }
    public float? MaxValue { get; set; }
    public float? NeutralValue { get; set; }
}

internal sealed class CapabilitySetRequest
{
    public float Value { get; set; }
}

internal sealed class CapabilityBatchSetRequest
{
    public Dictionary<string, float>? Values { get; set; }
}

internal sealed class CapabilityPressRequest
{
    public int DurationMs { get; set; } = 75;
}

internal sealed record HttpControlStatusResponse(
    string Service,
    string Version,
    bool Enabled,
    HttpControlRuntimeStatus Status,
    int Port,
    HttpControlListenMode ListenMode,
    bool AuthRequired,
    IReadOnlyList<string> Urls,
    string? Error);

internal sealed record HttpControlSettingsResponse(
    bool Enabled,
    int Port,
    HttpControlListenMode ListenMode,
    bool AuthRequired,
    bool AccessTokenSet,
    string? AccessTokenPreview);

internal sealed record HttpControllerResponse(
    string Id,
    string Name,
    int Number,
    HttpControlControllerSource Source,
    HttpControlControllerStatus Status,
    bool Readable,
    bool Writable,
    DateTimeOffset? LastSeenAt,
    IReadOnlyList<HttpCapabilityResponse> Capabilities);

internal sealed record HttpCapabilityResponse(
    string Id,
    InputDeviceEventType EventType,
    string EventCode,
    string DisplayName,
    float Value,
    float MinValue,
    float MaxValue,
    float NeutralValue,
    bool Writable,
    bool Observed,
    DateTimeOffset? UpdatedAt);

internal sealed record HttpCapabilitySetResponse(
    string ControllerId,
    string CapabilityId,
    InputDeviceEventType EventType,
    string EventCode,
    float Value,
    DateTimeOffset UpdatedAt);

internal sealed record HttpErrorResponse(string Error);
