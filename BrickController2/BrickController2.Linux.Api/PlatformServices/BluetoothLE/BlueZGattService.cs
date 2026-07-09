using BrickController2.PlatformServices.BluetoothLE;

namespace BrickController2.Linux.Api.PlatformServices.BluetoothLE;

internal sealed class BlueZGattService : IGattService
{
    public BlueZGattService(Guid uuid, IReadOnlyList<BlueZGattCharacteristic> characteristics)
    {
        Uuid = uuid;
        Characteristics = characteristics;
    }

    public Guid Uuid { get; }

    public IEnumerable<IGattCharacteristic> Characteristics { get; }
}
