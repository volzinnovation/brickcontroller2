using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using CoreBluetooth;
using CoreFoundation;
using Foundation;
using BrickController2.PlatformServices.BluetoothLE;

using static BrickController2.Protocols.BluetoothLowEnergy;

namespace BrickController2.iOS.PlatformServices.BluetoothLE
{
    public class BluetoothLEService : CBCentralManagerDelegate, IBluetoothLEService
    {
        private CBCentralManager? _centralManager;
        private TaskCompletionSource<CBManagerState>? _initialStateCompletionSource;
        private readonly IDictionary<CBPeripheral, BluetoothLEDevice> _peripheralMap = new Dictionary<CBPeripheral, BluetoothLEDevice>();
        private readonly object _lock = new();

        private Action<ScanResult>? _scanCallback;

        public Task<bool> IsBluetoothLESupportedAsync() => Task.FromResult(true);
        public Task<bool> IsBluetoothLEAdvertisingSupportedAsync() => Task.FromResult(true);
        public async Task<bool> IsBluetoothOnAsync()
        {
            var centralManager = await GetCentralManagerAsync();
            return centralManager.State == CBManagerState.PoweredOn;
        }

        public async Task<bool> ScanDevicesAsync(Action<ScanResult> scanCallback, CancellationToken token)
        {
            var centralManager = await GetCentralManagerAsync();
            if (!await IsBluetoothLESupportedAsync() || centralManager.State != CBManagerState.PoweredOn || centralManager.IsScanning)
            {
                return false;
            }

            var tcs = new TaskCompletionSource<bool>(TaskCreationOptions.RunContinuationsAsynchronously);

            using (token.Register(() =>
            {
                lock (_lock)
                {
                    centralManager.StopScan();
                    _scanCallback = null;
                    tcs.TrySetResult(true);
                }
            }))
            {
                _scanCallback = scanCallback;
                centralManager.ScanForPeripherals(Array.Empty<CBUUID>(), new PeripheralScanningOptions { AllowDuplicatesKey = true });

                return await tcs.Task;
            }
        }

        public async Task<IBluetoothLEDevice?> GetKnownDeviceAsync(string address)
        {
            var centralManager = await GetCentralManagerAsync();
            if (centralManager.State != CBManagerState.PoweredOn)
            {
                return default;
            }

            var peripheral = centralManager.RetrievePeripheralsWithIdentifiers(new NSUuid(address)).FirstOrDefault();
            if (peripheral is null)
            {
                return default;
            }

            var device = new BluetoothLEDevice(centralManager, peripheral);
            _peripheralMap[peripheral] = device;

            return device;
        }

        public override void UpdatedState(CBCentralManager central)
        {
            _initialStateCompletionSource?.TrySetResult(central.State);
        }

        public override void DiscoveredPeripheral(CBCentralManager central, CBPeripheral peripheral, NSDictionary advertisementData, NSNumber RSSI)
        {
            lock(_lock)
            {
                if (peripheral is null || peripheral.Identifier is null)
                {
                    return;
                }

                var processedAdvertisementData = ProcessAdvertisementData(advertisementData);
                _scanCallback?.Invoke(new ScanResult(peripheral.Name, peripheral.Identifier.ToString(), processedAdvertisementData));
            }
        }

        public override void ConnectedPeripheral(CBCentralManager central, CBPeripheral peripheral)
        {
            var device = _peripheralMap[peripheral];
            device.OnDeviceConnected();
        }

        public override void DisconnectedPeripheral(CBCentralManager central, CBPeripheral peripheral, NSError? error)
        {
            var device = _peripheralMap[peripheral];
            device.OnDeviceDisconnected();
        }

        public override void FailedToConnectPeripheral(CBCentralManager central, CBPeripheral peripheral, NSError? error)
        {
            var device = _peripheralMap[peripheral];
            device.OnDeviceDisconnected();
        }

        private Dictionary<byte, byte[]> ProcessAdvertisementData(NSDictionary advertisementData)
        {
            var result = new Dictionary<byte, byte[]>();

            var manufacturerData = GetDataForKey(advertisementData, CBAdvertisement.DataManufacturerDataKey);
            if (manufacturerData is not null)
            {
                result[ADTYPE_MANUFACTURER_SPECIFIC] = manufacturerData;
            }

            var completeDeviceName = GetDataForKey(advertisementData, CBAdvertisement.DataLocalNameKey);
            if (completeDeviceName is not null)
            {
                result[ADTYPE_LOCAL_NAME_COMPLETE] = completeDeviceName;
            }

            var serviceUuid = GetServiceUuidForKey(advertisementData, CBAdvertisement.DataServiceUUIDsKey);
            if (serviceUuid is not null)
            {
                // set it as incomplete service UUID (even though it might be the complete list)
                result[ADTYPE_INCOMPLETE_SERVICE_128BIT] = serviceUuid;
            }

            // TODO: add the rest of the advertisementdata...

            return result;
        }

        private byte[]? GetDataForKey(NSDictionary advertisementData, NSString key)
        {
            if (advertisementData == null || !advertisementData.ContainsKey(key))
            {
                return null;
            }

            var rawObject = advertisementData[key];
            if (rawObject is NSData dataObject)
            {
                return dataObject.ToArray();
            }
            else if (rawObject is NSString stringObject)
            {
                return Encoding.ASCII.GetBytes(stringObject.ToString());
            }

            return null;
        }

        private static byte[]? GetServiceUuidForKey(NSDictionary advertisementData, NSString key)
        {
            if (advertisementData != null &&
                advertisementData.TryGetValue(key, out var rawObject) &&
                rawObject is NSArray arrayObject)
            {
                // find first available 128-bit UUID
                for (nuint i = 0; i < arrayObject.Count; i++)
                {
                    var cbuuid = arrayObject.GetItem<CBUUID>(i);
                    if (cbuuid?.Data?.Length == 16)
                    {
                        // Service UUID's are read backwards (little endian) according to specs
                        var serviceUUid = cbuuid.Data.ToArray();
                        Array.Reverse(serviceUUid);
                        return serviceUUid;
                    }
                }
            }
            return null;
        }

        public IBluetoothLEAdvertiserDevice? CreateBluetoothLEAdvertiserDevice()
        {
            return new BluetoothLEAdvertiserDevice();
        }

        private async Task<CBCentralManager> GetCentralManagerAsync()
        {
            CBCentralManager centralManager;
            Task<CBManagerState>? initialStateTask;

            lock (_lock)
            {
                if (_centralManager is null)
                {
                    _initialStateCompletionSource = new TaskCompletionSource<CBManagerState>(TaskCreationOptions.RunContinuationsAsynchronously);
#pragma warning disable CA1422 // Validate platform compatibility
                    _centralManager = new CBCentralManager(this, DispatchQueue.MainQueue);
#pragma warning restore CA1422 // Validate platform compatibility
                }

                centralManager = _centralManager;
                initialStateTask = centralManager.State is CBManagerState.Unknown or CBManagerState.Resetting
                    ? _initialStateCompletionSource?.Task
                    : null;
            }

            if (initialStateTask is not null)
            {
                await initialStateTask;
            }

            return centralManager;
        }
    }
}
