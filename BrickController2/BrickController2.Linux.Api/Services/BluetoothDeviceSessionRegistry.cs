using BrickController2.Linux.Api.Api;
using BrickController2.PlatformServices.BluetoothLE;
using Microsoft.Extensions.Logging;
using System.Collections.Concurrent;

namespace BrickController2.Linux.Api.Services;

internal sealed class BluetoothDeviceSessionRegistry
{
    private const int MaxNotificationEvents = 100;

    private readonly IBluetoothLEService _bluetooth;
    private readonly ILogger<BluetoothDeviceSessionRegistry> _logger;
    private readonly ConcurrentDictionary<string, BluetoothDeviceSession> _sessions = new(StringComparer.OrdinalIgnoreCase);

    public BluetoothDeviceSessionRegistry(IBluetoothLEService bluetooth, ILogger<BluetoothDeviceSessionRegistry> logger)
    {
        _bluetooth = bluetooth;
        _logger = logger;
    }

    public async Task<IReadOnlyList<ScanDeviceResponse>> ScanAsync(TimeSpan duration, CancellationToken token)
    {
        var results = new ConcurrentDictionary<string, ScanDeviceResponse>(StringComparer.OrdinalIgnoreCase);

        using var scanCts = CancellationTokenSource.CreateLinkedTokenSource(token);
        scanCts.CancelAfter(duration);

        await _bluetooth.ScanDevicesAsync(scanResult =>
        {
            if (string.IsNullOrWhiteSpace(scanResult.DeviceAddress))
            {
                return;
            }

            var response = ToScanDeviceResponse(scanResult);
            results.AddOrUpdate(response.Address, response, (_, _) => response);
        }, scanCts.Token).ConfigureAwait(false);

        return results.Values
            .OrderBy(device => device.Name)
            .ThenBy(device => device.Address)
            .ToArray();
    }

    public async Task<DeviceSessionResponse?> ConnectAsync(string address, bool autoConnect, CancellationToken token)
    {
        var normalizedAddress = NormalizeAddress(address);
        var session = _sessions.GetOrAdd(normalizedAddress, static key => new BluetoothDeviceSession(key));

        await session.Sync.WaitAsync(token).ConfigureAwait(false);
        try
        {
            if (session.Device?.State == BluetoothLEDeviceState.Connected && session.Services.Count > 0)
            {
                return session.ToResponse();
            }

            var device = await _bluetooth.GetKnownDeviceAsync(normalizedAddress).ConfigureAwait(false);
            if (device is null)
            {
                _sessions.TryRemove(normalizedAddress, out _);
                return null;
            }

            session.Device = device;
            var services = await device.ConnectAndDiscoverServicesAsync(
                    autoConnect,
                    (characteristicUuid, value) => session.AddNotification(characteristicUuid, value),
                    _ => session.MarkDisconnected(),
                    token)
                .ConfigureAwait(false);

            if (services is null)
            {
                _sessions.TryRemove(normalizedAddress, out _);
                return null;
            }

            session.Services = services.ToArray();
            return session.ToResponse();
        }
        finally
        {
            session.Sync.Release();
        }
    }

    public async Task<bool> DisconnectAsync(string address, CancellationToken token)
    {
        var normalizedAddress = NormalizeAddress(address);
        if (!_sessions.TryGetValue(normalizedAddress, out var session))
        {
            return false;
        }

        await session.Sync.WaitAsync(token).ConfigureAwait(false);
        try
        {
            if (session.Device is not null)
            {
                await session.Device.DisconnectAsync().ConfigureAwait(false);
            }

            _sessions.TryRemove(normalizedAddress, out _);
            return true;
        }
        finally
        {
            session.Sync.Release();
        }
    }

    public DeviceSessionResponse? GetSession(string address)
        => _sessions.TryGetValue(NormalizeAddress(address), out var session) ? session.ToResponse() : null;

    public async Task<CharacteristicValueResponse?> ReadAsync(
        string address,
        string serviceUuid,
        string characteristicUuid,
        CancellationToken token)
    {
        var characteristicContext = TryGetCharacteristic(address, serviceUuid, characteristicUuid);
        if (characteristicContext is null)
        {
            return null;
        }

        var (session, service, characteristic) = characteristicContext.Value;
        var data = await session.Device!.ReadAsync(characteristic, token).ConfigureAwait(false);
        return data is null
            ? null
            : ToCharacteristicValueResponse(session.Address, service.Uuid, characteristic.Uuid, data);
    }

    public async Task<bool> WriteAsync(
        string address,
        string serviceUuid,
        string characteristicUuid,
        byte[] data,
        bool withResponse,
        CancellationToken token)
    {
        var characteristicContext = TryGetCharacteristic(address, serviceUuid, characteristicUuid);
        if (characteristicContext is null)
        {
            return false;
        }

        var (session, _, characteristic) = characteristicContext.Value;
        return withResponse
            ? await session.Device!.WriteAsync(characteristic, data, token).ConfigureAwait(false)
            : await session.Device!.WriteNoResponseAsync(characteristic, data, token).ConfigureAwait(false);
    }

    public async Task<bool?> SetNotificationsAsync(
        string address,
        string serviceUuid,
        string characteristicUuid,
        bool enable,
        CancellationToken token)
    {
        var characteristicContext = TryGetCharacteristic(address, serviceUuid, characteristicUuid);
        if (characteristicContext is null)
        {
            return null;
        }

        var (session, _, characteristic) = characteristicContext.Value;
        var success = enable
            ? await session.Device!.EnableNotificationAsync(characteristic, token).ConfigureAwait(false)
            : await session.Device!.DisableNotificationAsync(characteristic, token).ConfigureAwait(false);

        return success ? enable : null;
    }

    public IReadOnlyList<NotificationEventResponse>? GetNotifications(string address)
        => _sessions.TryGetValue(NormalizeAddress(address), out var session)
            ? session.GetNotifications()
            : null;

    private (BluetoothDeviceSession Session, IGattService Service, IGattCharacteristic Characteristic)? TryGetCharacteristic(
        string address,
        string serviceUuid,
        string characteristicUuid)
    {
        if (!BluetoothUuid.TryParse(serviceUuid, out var serviceGuid) ||
            !BluetoothUuid.TryParse(characteristicUuid, out var characteristicGuid))
        {
            return null;
        }

        if (!_sessions.TryGetValue(NormalizeAddress(address), out var session) ||
            session.Device?.State != BluetoothLEDeviceState.Connected)
        {
            return null;
        }

        var service = session.Services.FirstOrDefault(item => item.Uuid == serviceGuid);
        var characteristic = service?.Characteristics.FirstOrDefault(item => item.Uuid == characteristicGuid);
        return service is null || characteristic is null
            ? null
            : (session, service, characteristic);
    }

    private static ScanDeviceResponse ToScanDeviceResponse(ScanResult scanResult)
        => new(
            scanResult.DeviceName,
            NormalizeAddress(scanResult.DeviceAddress),
            scanResult.AdvertismentData.ToDictionary(
                item => $"0x{item.Key:X2}",
                item => BluetoothPayloadEncoding.ToHex(item.Value)));

    private static CharacteristicValueResponse ToCharacteristicValueResponse(
        string address,
        Guid serviceUuid,
        Guid characteristicUuid,
        byte[] data)
        => new(
            address,
            BluetoothUuid.Normalize(serviceUuid),
            BluetoothUuid.Normalize(characteristicUuid),
            BluetoothPayloadEncoding.ToHex(data),
            Convert.ToBase64String(data));

    private static string NormalizeAddress(string address) => address.Trim().ToUpperInvariant();

    private sealed class BluetoothDeviceSession
    {
        private readonly ConcurrentQueue<NotificationEventResponse> _notifications = new();

        public BluetoothDeviceSession(string address)
        {
            Address = address;
        }

        public string Address { get; }

        public SemaphoreSlim Sync { get; } = new(1, 1);

        public IBluetoothLEDevice? Device { get; set; }

        public IReadOnlyList<IGattService> Services { get; set; } = [];

        public void AddNotification(Guid characteristicUuid, byte[]? value)
        {
            value ??= [];
            _notifications.Enqueue(new NotificationEventResponse(
                DateTimeOffset.UtcNow,
                BluetoothUuid.Normalize(characteristicUuid),
                BluetoothPayloadEncoding.ToHex(value),
                Convert.ToBase64String(value)));

            while (_notifications.Count > MaxNotificationEvents)
            {
                _notifications.TryDequeue(out _);
            }
        }

        public void MarkDisconnected()
        {
            Services = [];
        }

        public IReadOnlyList<NotificationEventResponse> GetNotifications()
            => _notifications.ToArray();

        public DeviceSessionResponse ToResponse()
            => new(
                Address,
                Device?.State.ToString() ?? BluetoothLEDeviceState.Disconnected.ToString(),
                Services
                    .Select(service => new GattServiceResponse(
                        BluetoothUuid.Normalize(service.Uuid),
                        service.Characteristics
                            .Select(characteristic => new GattCharacteristicResponse(BluetoothUuid.Normalize(characteristic.Uuid)))
                            .OrderBy(characteristic => characteristic.Uuid)
                            .ToArray()))
                    .OrderBy(service => service.Uuid)
                    .ToArray());
    }
}
