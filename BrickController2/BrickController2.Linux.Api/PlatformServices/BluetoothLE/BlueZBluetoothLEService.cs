using BrickController2.Linux.Api.Services;
using BrickController2.PlatformServices.BluetoothLE;
using BrickController2.Protocols;
using Microsoft.Extensions.Logging;
using System.Buffers.Binary;
using System.Text;
using Tmds.DBus;

namespace BrickController2.Linux.Api.PlatformServices.BluetoothLE;

internal sealed class BlueZBluetoothLEService : IBluetoothLEService, IDisposable
{
    private readonly ILogger<BlueZBluetoothLEService> _logger;
    private readonly ILoggerFactory _loggerFactory;
    private readonly Connection _connection = new(Address.System);
    private readonly SemaphoreSlim _connectionLock = new(1, 1);
    private bool _isConnected;
    private ObjectPath? _defaultAdapterPath;

    public BlueZBluetoothLEService(ILogger<BlueZBluetoothLEService> logger, ILoggerFactory loggerFactory)
    {
        _logger = logger;
        _loggerFactory = loggerFactory;
    }

    public async Task<bool> IsBluetoothLESupportedAsync()
    {
        var adapter = await GetDefaultAdapterAsync(CancellationToken.None).ConfigureAwait(false);
        return adapter is not null;
    }

    public Task<bool> IsBluetoothLEAdvertisingSupportedAsync() => Task.FromResult(false);

    public async Task<bool> IsBluetoothOnAsync()
    {
        var adapter = await GetDefaultAdapterAsync(CancellationToken.None).ConfigureAwait(false);
        if (adapter is null)
        {
            return false;
        }

        try
        {
            return await adapter.Value.Proxy.GetAsync<bool>("Powered").ConfigureAwait(false);
        }
        catch (Exception ex)
        {
            _logger.LogDebug(ex, "BlueZ adapter state could not be read.");
            return false;
        }
    }

    public async Task<bool> ScanDevicesAsync(Action<ScanResult> scanCallback, CancellationToken token)
    {
        var adapter = await GetDefaultAdapterAsync(token).ConfigureAwait(false);
        if (adapter is null || !await adapter.Value.Proxy.GetAsync<bool>("Powered").WaitAsync(token).ConfigureAwait(false))
        {
            return false;
        }

        try
        {
            await adapter.Value.Proxy.SetDiscoveryFilterAsync(new Dictionary<string, object>
            {
                ["Transport"] = "le"
            }).WaitAsync(token).ConfigureAwait(false);

            await adapter.Value.Proxy.StartDiscoveryAsync().WaitAsync(token).ConfigureAwait(false);

            while (true)
            {
                await PublishKnownDevicesAsync(scanCallback, token).ConfigureAwait(false);
                await Task.Delay(TimeSpan.FromMilliseconds(500), token).ConfigureAwait(false);
            }
        }
        catch (OperationCanceledException) when (token.IsCancellationRequested)
        {
            return true;
        }
        catch (Exception ex)
        {
            _logger.LogWarning(ex, "BlueZ LE scan failed.");
            return false;
        }
        finally
        {
            try
            {
                await adapter.Value.Proxy.StopDiscoveryAsync().ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                _logger.LogDebug(ex, "BlueZ LE scan stop failed.");
            }
        }
    }

    public async Task<IBluetoothLEDevice?> GetKnownDeviceAsync(string address)
    {
        var normalizedAddress = NormalizeAddress(address);

        try
        {
            await EnsureConnectedAsync(CancellationToken.None).ConfigureAwait(false);
            var managedObjects = await GetManagedObjectsAsync(CancellationToken.None).ConfigureAwait(false);

            foreach (var (path, interfaces) in managedObjects)
            {
                if (!interfaces.TryGetValue(BlueZDbus.DeviceInterface, out var properties))
                {
                    continue;
                }

                if (!string.Equals(GetString(properties, "Address"), normalizedAddress, StringComparison.OrdinalIgnoreCase))
                {
                    continue;
                }

                var deviceProxy = _connection.CreateProxy<IBlueZDevice1>(BlueZDbus.ServiceName, path);
                return new BlueZBluetoothLEDevice(
                    _connection,
                    deviceProxy,
                    path,
                    normalizedAddress,
                    _loggerFactory.CreateLogger<BlueZBluetoothLEDevice>());
            }
        }
        catch (Exception ex)
        {
            _logger.LogWarning(ex, "BlueZ failed to enumerate known devices.");
        }

        return null;
    }

    public IBluetoothLEAdvertiserDevice? CreateBluetoothLEAdvertiserDevice() => null;

    public void Dispose()
    {
        _connectionLock.Dispose();
        _connection.Dispose();
    }

    private async Task<BlueZAdapter?> GetDefaultAdapterAsync(CancellationToken token)
    {
        try
        {
            await EnsureConnectedAsync(token).ConfigureAwait(false);

            if (_defaultAdapterPath is not null)
            {
                return new BlueZAdapter(_defaultAdapterPath.Value, _connection.CreateProxy<IBlueZAdapter1>(BlueZDbus.ServiceName, _defaultAdapterPath.Value));
            }

            var managedObjects = await GetManagedObjectsAsync(token).ConfigureAwait(false);
            foreach (var (path, interfaces) in managedObjects)
            {
                if (interfaces.ContainsKey(BlueZDbus.AdapterInterface))
                {
                    _defaultAdapterPath = path;
                    return new BlueZAdapter(path, _connection.CreateProxy<IBlueZAdapter1>(BlueZDbus.ServiceName, path));
                }
            }
        }
        catch (Exception ex) when (ex is DBusException or ConnectException or InvalidOperationException)
        {
            _logger.LogWarning(ex, "BlueZ adapter discovery failed. Is bluetoothd running and accessible?");
        }

        return null;
    }

    private async Task EnsureConnectedAsync(CancellationToken token)
    {
        if (_isConnected)
        {
            return;
        }

        await _connectionLock.WaitAsync(token).ConfigureAwait(false);
        try
        {
            if (_isConnected)
            {
                return;
            }

            await _connection.ConnectAsync().WaitAsync(token).ConfigureAwait(false);
            _isConnected = true;
        }
        finally
        {
            _connectionLock.Release();
        }
    }

    private async Task<IDictionary<ObjectPath, IDictionary<string, IDictionary<string, object>>>> GetManagedObjectsAsync(CancellationToken token)
    {
        var objectManager = _connection.CreateProxy<IBlueZObjectManager>(BlueZDbus.ServiceName, BlueZDbus.ObjectManagerPath);
        return await objectManager.GetManagedObjectsAsync().WaitAsync(token).ConfigureAwait(false);
    }

    private async Task PublishKnownDevicesAsync(Action<ScanResult> scanCallback, CancellationToken token)
    {
        var managedObjects = await GetManagedObjectsAsync(token).ConfigureAwait(false);
        foreach (var (_, interfaces) in managedObjects)
        {
            if (interfaces.TryGetValue(BlueZDbus.DeviceInterface, out var properties))
            {
                var scanResult = CreateScanResult(properties);
                if (!string.IsNullOrWhiteSpace(scanResult.DeviceAddress))
                {
                    scanCallback(scanResult);
                }
            }
        }
    }

    private static ScanResult CreateScanResult(IDictionary<string, object> properties)
    {
        var address = NormalizeAddress(GetString(properties, "Address") ?? string.Empty);
        var name = GetString(properties, "Name") ?? GetString(properties, "Alias") ?? string.Empty;
        var advertisementData = new Dictionary<byte, byte[]>();

        if (!string.IsNullOrWhiteSpace(name))
        {
            advertisementData[BluetoothLowEnergy.ADTYPE_LOCAL_NAME_COMPLETE] = Encoding.UTF8.GetBytes(name);
        }

        var manufacturerData = GetManufacturerData(properties);
        if (manufacturerData.Length > 0)
        {
            advertisementData[BluetoothLowEnergy.ADTYPE_MANUFACTURER_SPECIFIC] = manufacturerData;
        }

        var serviceUuids = GetServiceUuids(properties);
        if (serviceUuids.Count > 0)
        {
            advertisementData[BluetoothLowEnergy.ADTYPE_COMPLETE_SERVICE_128BIT] = BluetoothUuid.ToBluetoothAdvertisementBytes(serviceUuids);
        }

        return new ScanResult(name, address, advertisementData);
    }

    private static string? GetString(IDictionary<string, object> properties, string name)
        => properties.TryGetValue(name, out var value) ? value as string : null;

    private static byte[] GetManufacturerData(IDictionary<string, object> properties)
    {
        if (!properties.TryGetValue("ManufacturerData", out var value))
        {
            return [];
        }

        return EnumerateManufacturerData(value)
            .SelectMany(entry =>
            {
                var result = new byte[entry.Data.Length + sizeof(ushort)];
                BinaryPrimitives.WriteUInt16LittleEndian(result, entry.CompanyId);
                entry.Data.CopyTo(result.AsSpan(sizeof(ushort)));
                return result;
            })
            .ToArray();
    }

    private static IEnumerable<(ushort CompanyId, byte[] Data)> EnumerateManufacturerData(object value)
    {
        if (value is IEnumerable<KeyValuePair<ushort, byte[]>> byteEntries)
        {
            foreach (var entry in byteEntries)
            {
                yield return (entry.Key, entry.Value);
            }

            yield break;
        }

        if (value is IEnumerable<KeyValuePair<ushort, object>> objectEntries)
        {
            foreach (var entry in objectEntries)
            {
                yield return (entry.Key, ToByteArray(entry.Value));
            }
        }
    }

    private static IReadOnlyList<Guid> GetServiceUuids(IDictionary<string, object> properties)
    {
        if (!properties.TryGetValue("UUIDs", out var value) || value is not IEnumerable<string> uuidTexts)
        {
            return [];
        }

        return uuidTexts
            .Select(uuidText => BluetoothUuid.TryParse(uuidText, out var uuid) ? uuid : (Guid?)null)
            .Where(uuid => uuid.HasValue)
            .Select(uuid => uuid!.Value)
            .ToArray();
    }

    private static byte[] ToByteArray(object? value)
        => value switch
        {
            null => [],
            byte[] bytes => bytes,
            IEnumerable<byte> bytes => bytes.ToArray(),
            _ => []
        };

    private static string NormalizeAddress(string address) => address.Trim().ToUpperInvariant();

    private readonly record struct BlueZAdapter(ObjectPath Path, IBlueZAdapter1 Proxy);
}
