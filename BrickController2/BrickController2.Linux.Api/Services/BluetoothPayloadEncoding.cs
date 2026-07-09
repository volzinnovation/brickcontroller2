namespace BrickController2.Linux.Api.Services;

internal static class BluetoothPayloadEncoding
{
    public static bool TryDecode(string? value, string? encoding, out byte[] data, out string error)
    {
        data = [];
        error = string.Empty;

        if (string.IsNullOrWhiteSpace(value))
        {
            error = "Payload data is required.";
            return false;
        }

        encoding = string.IsNullOrWhiteSpace(encoding) ? "hex" : encoding.Trim().ToLowerInvariant();

        return encoding switch
        {
            "hex" => TryDecodeHex(value, out data, out error),
            "base64" => TryDecodeBase64(value, out data, out error),
            _ => Fail($"Unsupported payload encoding '{encoding}'. Use 'hex' or 'base64'.", out data, out error)
        };
    }

    public static string ToHex(byte[] data) => Convert.ToHexString(data).ToLowerInvariant();

    private static bool TryDecodeHex(string value, out byte[] data, out string error)
    {
        var normalized = value
            .Replace("0x", string.Empty, StringComparison.OrdinalIgnoreCase)
            .Replace(":", string.Empty, StringComparison.Ordinal)
            .Replace("-", string.Empty, StringComparison.Ordinal)
            .Replace("_", string.Empty, StringComparison.Ordinal)
            .Replace(" ", string.Empty, StringComparison.Ordinal)
            .Replace("\t", string.Empty, StringComparison.Ordinal)
            .Replace("\r", string.Empty, StringComparison.Ordinal)
            .Replace("\n", string.Empty, StringComparison.Ordinal);

        if (normalized.Length == 0)
        {
            return Fail("Hex payload is empty.", out data, out error);
        }

        if (normalized.Length % 2 != 0)
        {
            return Fail("Hex payload must have an even number of digits.", out data, out error);
        }

        try
        {
            data = Convert.FromHexString(normalized);
            error = string.Empty;
            return true;
        }
        catch (FormatException)
        {
            return Fail("Hex payload contains non-hex characters.", out data, out error);
        }
    }

    private static bool TryDecodeBase64(string value, out byte[] data, out string error)
    {
        try
        {
            data = Convert.FromBase64String(value);
            error = string.Empty;
            return true;
        }
        catch (FormatException)
        {
            return Fail("Base64 payload is invalid.", out data, out error);
        }
    }

    private static bool Fail(string message, out byte[] data, out string error)
    {
        data = [];
        error = message;
        return false;
    }
}
