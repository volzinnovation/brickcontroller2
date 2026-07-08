using BrickController2.PlatformServices.BluetoothLE;
using Microsoft.Extensions.Logging;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace BrickController2.Linux.PlatformServices.BluetoothLE;

public class BluetoothLEService : IBluetoothLEService
{
    private readonly ILogger<BluetoothLEService> _logger;

    public BluetoothLEService(ILogger<BluetoothLEService> logger)
    {
        _logger = logger;
    }

    public Task<bool> IsBluetoothLESupportedAsync() => Task.FromResult(false);

    public Task<bool> IsBluetoothLEAdvertisingSupportedAsync() => Task.FromResult(false);

    public Task<bool> IsBluetoothOnAsync() => Task.FromResult(false);

    public Task<bool> ScanDevicesAsync(Action<ScanResult> scanCallback, CancellationToken token)
    {
        _logger.LogWarning("Bluetooth LE is not implemented for the Linux GTK4 desktop head.");
        return Task.FromResult(false);
    }

    public Task<IBluetoothLEDevice?> GetKnownDeviceAsync(string address)
    {
        _logger.LogWarning("Bluetooth LE device lookup is not implemented for the Linux GTK4 desktop head.");
        return Task.FromResult<IBluetoothLEDevice?>(null);
    }

    public IBluetoothLEAdvertiserDevice? CreateBluetoothLEAdvertiserDevice() => null;
}
