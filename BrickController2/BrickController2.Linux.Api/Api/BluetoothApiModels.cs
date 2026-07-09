namespace BrickController2.Linux.Api.Api;

public sealed record BluetoothStatusResponse(
    bool Supported,
    bool Powered,
    bool AdvertisingSupported);

public sealed record ScanRequest(
    int DurationSeconds = 5);

public sealed record ScanDeviceResponse(
    string Name,
    string Address,
    IReadOnlyDictionary<string, string> AdvertisementData);

public sealed record ConnectRequest(
    bool AutoConnect = false);

public sealed record DeviceSessionResponse(
    string Address,
    string State,
    IReadOnlyList<GattServiceResponse> Services);

public sealed record GattServiceResponse(
    string Uuid,
    IReadOnlyList<GattCharacteristicResponse> Characteristics);

public sealed record GattCharacteristicResponse(
    string Uuid);

public sealed record CharacteristicValueResponse(
    string Address,
    string ServiceUuid,
    string CharacteristicUuid,
    string Hex,
    string Base64);

public sealed record CharacteristicWriteRequest(
    string? Data,
    string Encoding = "hex",
    bool WithResponse = true);

public sealed record NotificationRequest(
    bool Enable = true);

public sealed record NotificationStateResponse(
    string Address,
    string ServiceUuid,
    string CharacteristicUuid,
    bool Enabled);

public sealed record NotificationEventResponse(
    DateTimeOffset Timestamp,
    string CharacteristicUuid,
    string Hex,
    string Base64);

public sealed record ErrorResponse(
    string Error);
