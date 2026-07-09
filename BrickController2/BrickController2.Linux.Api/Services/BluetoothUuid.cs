using BrickController2.Protocols;

namespace BrickController2.Linux.Api.Services;

internal static class BluetoothUuid
{
    private const string BluetoothBaseUuidSuffix = "-0000-1000-8000-00805f9b34fb";

    public static bool TryParse(string? value, out Guid uuid)
    {
        uuid = default;

        if (string.IsNullOrWhiteSpace(value))
        {
            return false;
        }

        value = value.Trim();

        if (Guid.TryParse(value, out uuid))
        {
            return true;
        }

        if (value.Length == 4 && IsHex(value))
        {
            return Guid.TryParse($"0000{value}{BluetoothBaseUuidSuffix}", out uuid);
        }

        if (value.Length == 8 && IsHex(value))
        {
            return Guid.TryParse($"{value}{BluetoothBaseUuidSuffix}", out uuid);
        }

        return false;
    }

    public static string Normalize(Guid uuid) => uuid.ToString("D").ToLowerInvariant();

    public static byte[] ToBluetoothAdvertisementBytes(IEnumerable<Guid> uuids)
        => uuids.SelectMany(uuid => uuid.To128BitByteArray()).ToArray();

    private static bool IsHex(string value)
        => value.All(c => c is >= '0' and <= '9' or >= 'a' and <= 'f' or >= 'A' and <= 'F');
}
