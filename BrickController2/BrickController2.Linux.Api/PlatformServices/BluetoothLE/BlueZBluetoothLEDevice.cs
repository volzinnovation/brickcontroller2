using BrickController2.Linux.Api.Services;
using BrickController2.PlatformServices.BluetoothLE;
using Microsoft.Extensions.Logging;
using Tmds.DBus;

namespace BrickController2.Linux.Api.PlatformServices.BluetoothLE;

internal sealed class BlueZBluetoothLEDevice : IBluetoothLEDevice
{
    private static readonly IDictionary<string, object> EmptyOptions = new Dictionary<string, object>();
    private static readonly IDictionary<string, object> WriteWithResponseOptions = new Dictionary<string, object>
    {
        ["type"] = "request"
    };
    private static readonly IDictionary<string, object> WriteWithoutResponseOptions = new Dictionary<string, object>
    {
        ["type"] = "command"
    };

    private readonly Connection _connection;
    private readonly IBlueZDevice1 _device;
    private readonly ObjectPath _devicePath;
    private readonly ILogger<BlueZBluetoothLEDevice> _logger;
    private readonly SemaphoreSlim _sync = new(1, 1);
    private readonly Dictionary<BlueZGattCharacteristic, IDisposable> _notificationWatches = [];
    private readonly TimeSpan _serviceResolutionTimeout = TimeSpan.FromSeconds(15);

    private Action<Guid, byte[]?>? _onCharacteristicChanged;
    private Action<IBluetoothLEDevice>? _onDeviceDisconnected;
    private IReadOnlyList<BlueZGattService> _services = [];
    private IDisposable? _deviceWatch;

    public BlueZBluetoothLEDevice(
        Connection connection,
        IBlueZDevice1 device,
        ObjectPath devicePath,
        string address,
        ILogger<BlueZBluetoothLEDevice> logger)
    {
        _connection = connection;
        _device = device;
        _devicePath = devicePath;
        Address = address;
        _logger = logger;
    }

    public string Address { get; }

    public BluetoothLEDeviceState State { get; private set; } = BluetoothLEDeviceState.Disconnected;

    public async Task<IEnumerable<IGattService>?> ConnectAndDiscoverServicesAsync(
        bool autoConnect,
        Action<Guid, byte[]?> onCharacteristicChanged,
        Action<IBluetoothLEDevice> onDeviceDisconnected,
        CancellationToken token)
    {
        await _sync.WaitAsync(token).ConfigureAwait(false);
        try
        {
            if (State == BluetoothLEDeviceState.Connected && _services.Count > 0)
            {
                return _services;
            }

            if (State != BluetoothLEDeviceState.Disconnected)
            {
                return null;
            }

            _onCharacteristicChanged = onCharacteristicChanged;
            _onDeviceDisconnected = onDeviceDisconnected;
            await WatchDevicePropertiesAsync(token).ConfigureAwait(false);

            State = BluetoothLEDeviceState.Connecting;
            await _device.ConnectAsync().WaitAsync(token).ConfigureAwait(false);

            State = BluetoothLEDeviceState.Discovering;
            await WaitForServicesResolvedAsync(token).ConfigureAwait(false);

            _services = await DiscoverServicesAsync(token).ConfigureAwait(false);
            State = BluetoothLEDeviceState.Connected;

            return _services;
        }
        catch (OperationCanceledException)
        {
            await DisconnectNativeAsync().ConfigureAwait(false);
            return null;
        }
        catch (Exception ex)
        {
            _logger.LogWarning(ex, "BlueZ failed to connect to BLE device {Address}.", Address);
            await DisconnectNativeAsync().ConfigureAwait(false);
            return null;
        }
        finally
        {
            _sync.Release();
        }
    }

    public async Task DisconnectAsync()
    {
        await _sync.WaitAsync().ConfigureAwait(false);
        try
        {
            await DisconnectNativeAsync().ConfigureAwait(false);
        }
        finally
        {
            _sync.Release();
        }
    }

    public async Task<bool> EnableNotificationAsync(IGattCharacteristic characteristic, CancellationToken token)
    {
        if (characteristic is not BlueZGattCharacteristic blueZCharacteristic)
        {
            return false;
        }

        await _sync.WaitAsync(token).ConfigureAwait(false);
        try
        {
            if (State != BluetoothLEDeviceState.Connected)
            {
                return false;
            }

            if (!_notificationWatches.ContainsKey(blueZCharacteristic))
            {
                var watch = await blueZCharacteristic.NativeCharacteristic.WatchPropertiesAsync(OnCharacteristicPropertyChanged)
                    .WaitAsync(token)
                    .ConfigureAwait(false);
                _notificationWatches[blueZCharacteristic] = watch;
            }

            await blueZCharacteristic.NativeCharacteristic.StartNotifyAsync().WaitAsync(token).ConfigureAwait(false);
            return true;
        }
        catch (Exception ex) when (ex is not OperationCanceledException)
        {
            _logger.LogWarning(ex, "BlueZ failed to enable notifications for {CharacteristicUuid} on {Address}.", characteristic.Uuid, Address);
            return false;
        }
        finally
        {
            _sync.Release();
        }

        void OnCharacteristicPropertyChanged(PropertyChanges changes)
        {
            if (changes.Changed.Any(change => change.Key == "Value"))
            {
                _onCharacteristicChanged?.Invoke(blueZCharacteristic.Uuid, changes.Get<byte[]>("Value"));
            }
        }
    }

    public async Task<bool> DisableNotificationAsync(IGattCharacteristic characteristic, CancellationToken token)
    {
        if (characteristic is not BlueZGattCharacteristic blueZCharacteristic)
        {
            return false;
        }

        await _sync.WaitAsync(token).ConfigureAwait(false);
        try
        {
            if (_notificationWatches.Remove(blueZCharacteristic, out var watch))
            {
                watch.Dispose();
            }

            await blueZCharacteristic.NativeCharacteristic.StopNotifyAsync().WaitAsync(token).ConfigureAwait(false);
            return true;
        }
        catch (Exception ex) when (ex is not OperationCanceledException)
        {
            _logger.LogWarning(ex, "BlueZ failed to disable notifications for {CharacteristicUuid} on {Address}.", characteristic.Uuid, Address);
            return false;
        }
        finally
        {
            _sync.Release();
        }
    }

    public async Task<byte[]?> ReadAsync(IGattCharacteristic characteristic, CancellationToken token)
    {
        if (characteristic is not BlueZGattCharacteristic blueZCharacteristic || State != BluetoothLEDeviceState.Connected)
        {
            return null;
        }

        try
        {
            return await blueZCharacteristic.NativeCharacteristic.ReadValueAsync(EmptyOptions)
                .WaitAsync(token)
                .ConfigureAwait(false);
        }
        catch (Exception ex) when (ex is not OperationCanceledException)
        {
            _logger.LogWarning(ex, "BlueZ failed to read {CharacteristicUuid} on {Address}.", characteristic.Uuid, Address);
            return null;
        }
    }

    public Task<bool> WriteAsync(IGattCharacteristic characteristic, byte[] data, CancellationToken token)
        => WriteAsync(characteristic, data, WriteWithResponseOptions, token);

    public Task<bool> WriteNoResponseAsync(IGattCharacteristic characteristic, byte[] data, CancellationToken token)
        => WriteAsync(characteristic, data, WriteWithoutResponseOptions, token);

    private async Task<bool> WriteAsync(
        IGattCharacteristic characteristic,
        byte[] data,
        IDictionary<string, object> options,
        CancellationToken token)
    {
        if (characteristic is not BlueZGattCharacteristic blueZCharacteristic || State != BluetoothLEDeviceState.Connected)
        {
            return false;
        }

        try
        {
            await blueZCharacteristic.NativeCharacteristic.WriteValueAsync(data, options)
                .WaitAsync(token)
                .ConfigureAwait(false);
            return true;
        }
        catch (Exception ex) when (ex is not OperationCanceledException)
        {
            _logger.LogWarning(ex, "BlueZ failed to write {CharacteristicUuid} on {Address}.", characteristic.Uuid, Address);
            return false;
        }
    }

    private async Task WatchDevicePropertiesAsync(CancellationToken token)
    {
        _deviceWatch ??= await _device.WatchPropertiesAsync(changes =>
        {
            if (changes.Changed.Any(change => change.Key == "Connected") && changes.GetStruct<bool>("Connected") == false)
            {
                State = BluetoothLEDeviceState.Disconnected;
                _onDeviceDisconnected?.Invoke(this);
            }
        }).WaitAsync(token).ConfigureAwait(false);
    }

    private async Task WaitForServicesResolvedAsync(CancellationToken token)
    {
        var deadline = DateTimeOffset.UtcNow + _serviceResolutionTimeout;

        while (DateTimeOffset.UtcNow < deadline)
        {
            if (await TryGetServicesResolvedAsync(token).ConfigureAwait(false))
            {
                return;
            }

            await Task.Delay(TimeSpan.FromMilliseconds(250), token).ConfigureAwait(false);
        }

        _logger.LogDebug("BlueZ did not report ServicesResolved for {Address} within {Timeout}; trying discovery anyway.", Address, _serviceResolutionTimeout);
    }

    private async Task<bool> TryGetServicesResolvedAsync(CancellationToken token)
    {
        try
        {
            return await _device.GetAsync<bool>("ServicesResolved").WaitAsync(token).ConfigureAwait(false);
        }
        catch (DBusException ex)
        {
            _logger.LogDebug(ex, "BlueZ could not read ServicesResolved for {Address}.", Address);
            return false;
        }
    }

    private async Task<IReadOnlyList<BlueZGattService>> DiscoverServicesAsync(CancellationToken token)
    {
        var services = new List<BlueZGattService>();
        var objectManager = _connection.CreateProxy<IBlueZObjectManager>(BlueZDbus.ServiceName, BlueZDbus.ObjectManagerPath);
        var managedObjects = await objectManager.GetManagedObjectsAsync().WaitAsync(token).ConfigureAwait(false);

        foreach (var (servicePath, interfaces) in managedObjects.OrderBy(item => item.Key.ToString()))
        {
            if (!servicePath.ToString().StartsWith(_devicePath.ToString(), StringComparison.Ordinal) ||
                !interfaces.TryGetValue(BlueZDbus.GattServiceInterface, out var serviceProperties))
            {
                continue;
            }

            if (!BluetoothUuid.TryParse(GetString(serviceProperties, "UUID"), out var serviceUuid))
            {
                continue;
            }

            var characteristics = GetCharacteristicsForService(managedObjects, servicePath, token);
            services.Add(new BlueZGattService(serviceUuid, await characteristics.ConfigureAwait(false)));
        }

        return services;
    }

    private async Task<IReadOnlyList<BlueZGattCharacteristic>> GetCharacteristicsForService(
        IDictionary<ObjectPath, IDictionary<string, IDictionary<string, object>>> managedObjects,
        ObjectPath servicePath,
        CancellationToken token)
    {
        var characteristics = new List<BlueZGattCharacteristic>();

        foreach (var (characteristicPath, interfaces) in managedObjects.OrderBy(item => item.Key.ToString()))
        {
            if (!interfaces.TryGetValue(BlueZDbus.GattCharacteristicInterface, out var characteristicProperties))
            {
                continue;
            }

            if (!IsCharacteristicOfService(characteristicPath, characteristicProperties, servicePath))
            {
                continue;
            }

            if (!BluetoothUuid.TryParse(GetString(characteristicProperties, "UUID"), out var characteristicUuid))
            {
                continue;
            }

            var proxy = _connection.CreateProxy<IBlueZGattCharacteristic1>(BlueZDbus.ServiceName, characteristicPath);
            _ = await proxy.GetAsync<string[]>("Flags").WaitAsync(token).ConfigureAwait(false);
            characteristics.Add(new BlueZGattCharacteristic(proxy, characteristicUuid));
        }

        return characteristics;
    }

    private static bool IsCharacteristicOfService(
        ObjectPath characteristicPath,
        IDictionary<string, object> characteristicProperties,
        ObjectPath servicePath)
    {
        if (characteristicProperties.TryGetValue("Service", out var serviceValue) &&
            serviceValue is ObjectPath characteristicServicePath)
        {
            return characteristicServicePath == servicePath;
        }

        return characteristicPath.ToString().StartsWith(servicePath.ToString(), StringComparison.Ordinal);
    }

    private async Task DisconnectNativeAsync()
    {
        State = BluetoothLEDeviceState.Disconnecting;

        foreach (var watch in _notificationWatches.Values)
        {
            watch.Dispose();
        }
        _notificationWatches.Clear();

        _deviceWatch?.Dispose();
        _deviceWatch = null;

        try
        {
            await _device.DisconnectAsync().ConfigureAwait(false);
        }
        catch (Exception ex)
        {
            _logger.LogDebug(ex, "BlueZ disconnect for {Address} did not complete cleanly.", Address);
        }

        _services = [];
        _onCharacteristicChanged = null;
        _onDeviceDisconnected = null;
        State = BluetoothLEDeviceState.Disconnected;
    }

    private static string? GetString(IDictionary<string, object> properties, string name)
        => properties.TryGetValue(name, out var value) ? value as string : null;
}
