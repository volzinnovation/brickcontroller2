using BrickController2.PlatformServices.BluetoothLE;

namespace BrickController2.Linux.Api.PlatformServices.BluetoothLE;

internal sealed class BlueZGattCharacteristic : IGattCharacteristic
{
    public BlueZGattCharacteristic(IBlueZGattCharacteristic1 nativeCharacteristic, Guid uuid)
    {
        NativeCharacteristic = nativeCharacteristic;
        Uuid = uuid;
    }

    public Guid Uuid { get; }

    public IBlueZGattCharacteristic1 NativeCharacteristic { get; }
}
