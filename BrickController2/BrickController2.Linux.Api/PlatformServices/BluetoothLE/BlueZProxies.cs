using Tmds.DBus;

namespace BrickController2.Linux.Api.PlatformServices.BluetoothLE;

internal static class BlueZDbus
{
    public const string ServiceName = "org.bluez";
    public const string ObjectManagerPath = "/";
    public const string AdapterInterface = "org.bluez.Adapter1";
    public const string DeviceInterface = "org.bluez.Device1";
    public const string GattServiceInterface = "org.bluez.GattService1";
    public const string GattCharacteristicInterface = "org.bluez.GattCharacteristic1";
}

[DBusInterface("org.freedesktop.DBus.ObjectManager")]
internal interface IBlueZObjectManager : IDBusObject
{
    Task<IDictionary<ObjectPath, IDictionary<string, IDictionary<string, object>>>> GetManagedObjectsAsync();
}

[DBusInterface(BlueZDbus.AdapterInterface)]
internal interface IBlueZAdapter1 : IDBusObject
{
    Task StartDiscoveryAsync();

    Task StopDiscoveryAsync();

    Task SetDiscoveryFilterAsync(IDictionary<string, object> properties);

    Task<T> GetAsync<T>(string property);

    Task SetAsync(string property, object value);

    Task<IDisposable> WatchPropertiesAsync(Action<PropertyChanges> handler);
}

[DBusInterface(BlueZDbus.DeviceInterface)]
internal interface IBlueZDevice1 : IDBusObject
{
    Task ConnectAsync();

    Task DisconnectAsync();

    Task<T> GetAsync<T>(string property);

    Task SetAsync(string property, object value);

    Task<IDisposable> WatchPropertiesAsync(Action<PropertyChanges> handler);
}

[DBusInterface(BlueZDbus.GattServiceInterface)]
internal interface IBlueZGattService1 : IDBusObject
{
    Task<T> GetAsync<T>(string property);

    Task<IDisposable> WatchPropertiesAsync(Action<PropertyChanges> handler);
}

[DBusInterface(BlueZDbus.GattCharacteristicInterface)]
internal interface IBlueZGattCharacteristic1 : IDBusObject
{
    Task<byte[]> ReadValueAsync(IDictionary<string, object> options);

    Task WriteValueAsync(byte[] value, IDictionary<string, object> options);

    Task StartNotifyAsync();

    Task StopNotifyAsync();

    Task<T> GetAsync<T>(string property);

    Task<IDisposable> WatchPropertiesAsync(Action<PropertyChanges> handler);
}
