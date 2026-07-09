using Autofac;
using BrickController2.CreationManagement;
using BrickController2.DeviceManagement;
using BrickController2.DeviceManagement.MouldKing;
using BrickController2.PlatformServices.BluetoothLE;
using BrickController2.Protocols;
using BrickController2.UI.Services.AppIdentifier;
using Microsoft.Maui.ApplicationModel;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace BrickController2.MacCatalyst.Diagnostics;

internal sealed class Mk38MotorSmokeTest
{
    private const string EnabledEnvironmentVariable = "BC2_MK38_MOTOR_TEST";
    private const string PowerEnvironmentVariable = "BC2_MK38_MOTOR_TEST_POWER";
    private const string DurationEnvironmentVariable = "BC2_MK38_MOTOR_TEST_SECONDS";
    private const string LogEnvironmentVariable = "BC2_MK38_MOTOR_TEST_LOG";
    private const float DefaultPower = 0.5f;
    private static readonly TimeSpan DefaultChannelDuration = TimeSpan.FromSeconds(1);
    private static readonly TimeSpan BluetoothStartupTimeout = TimeSpan.FromSeconds(8);
    private static readonly TimeSpan ConnectTimeout = TimeSpan.FromSeconds(10);

    private readonly IManualDeviceManager _manualDeviceManager;
    private readonly IBluetoothLEService _bluetoothLEService;
    private readonly IMKPlatformService _mkPlatformService;
    private readonly IAppIdentifierService _appIdentifierService;
    private readonly IComponentContext _componentContext;

    public Mk38MotorSmokeTest(
        IManualDeviceManager manualDeviceManager,
        IBluetoothLEService bluetoothLEService,
        IMKPlatformService mkPlatformService,
        IAppIdentifierService appIdentifierService,
        IComponentContext componentContext)
    {
        _manualDeviceManager = manualDeviceManager;
        _bluetoothLEService = bluetoothLEService;
        _mkPlatformService = mkPlatformService;
        _appIdentifierService = appIdentifierService;
        _componentContext = componentContext;
    }

    public static bool IsRequested
        => string.Equals(Environment.GetEnvironmentVariable(EnabledEnvironmentVariable), "1", StringComparison.OrdinalIgnoreCase) ||
           string.Equals(Environment.GetEnvironmentVariable(EnabledEnvironmentVariable), "true", StringComparison.OrdinalIgnoreCase);

    public async Task<int> RunAsync(CancellationToken token)
    {
        var logPath = Environment.GetEnvironmentVariable(LogEnvironmentVariable);
        if (string.IsNullOrWhiteSpace(logPath))
        {
            logPath = Path.Combine(Path.GetTempPath(), "brickcontroller2-mk38-motor-test.log");
        }

        logPath = Path.GetFullPath(logPath);
        var logDirectory = Path.GetDirectoryName(logPath);
        if (!string.IsNullOrEmpty(logDirectory))
        {
            Directory.CreateDirectory(logDirectory);
        }
        await using var logWriter = new StreamWriter(logPath, append: false) { AutoFlush = true };

        void Log(string message)
        {
            var line = $"{DateTimeOffset.Now:yyyy-MM-dd HH:mm:ss.fff zzz} {message}";
            Console.WriteLine(line);
            logWriter.WriteLine(line);
        }

        Log("BrickController2 MK 3.8 motor smoke test starting.");
        Log($"Log file: {logPath}");

        var power = ReadFloat(PowerEnvironmentVariable, DefaultPower);
        power = Math.Clamp(power, -1f, 1f);
        var channelDuration = TimeSpan.FromSeconds(Math.Max(0.1, ReadDouble(DurationEnvironmentVariable, DefaultChannelDuration.TotalSeconds)));
        Log($"Requested output: {power.ToString("0.###", CultureInfo.InvariantCulture)} for {channelDuration.TotalSeconds:0.###} second(s) per channel.");

        Device? device = null;

        try
        {
            await CheckBluetoothAsync(Log, token).ConfigureAwait(false);
            await ProbeAdvertisingAsync(Log, token).ConfigureAwait(false);

            device = CreateMk38Device(Log);

            await ConnectDeviceAsync(device, Log, token).ConfigureAwait(false);
            await RunMotorSequenceAsync(device, power, channelDuration, Log, token).ConfigureAwait(false);

            Log("PASS: MK 3.8 motor smoke test completed.");
            return 0;
        }
        catch (OperationCanceledException)
        {
            Log("FAIL: MK 3.8 motor smoke test timed out or was cancelled.");
            return 2;
        }
        catch (Exception ex)
        {
            Log($"FAIL: {ex.GetType().Name}: {ex.Message}");
            Log(ex.ToString());
            return 1;
        }
        finally
        {
            if (device is not null)
            {
                await CleanupDeviceAsync(device, Log).ConfigureAwait(false);
            }
        }
    }

    private async Task CheckBluetoothAsync(Action<string> log, CancellationToken token)
    {
        log("Checking Bluetooth LE support.");
        if (!await _bluetoothLEService.IsBluetoothLESupportedAsync().ConfigureAwait(false))
        {
            throw new InvalidOperationException("Bluetooth LE is not supported by this Mac/App runtime.");
        }

        if (!await _bluetoothLEService.IsBluetoothLEAdvertisingSupportedAsync().ConfigureAwait(false))
        {
            throw new InvalidOperationException("Bluetooth LE advertising is not supported by this Mac/App runtime.");
        }

        var deadline = DateTimeOffset.UtcNow + BluetoothStartupTimeout;
        while (DateTimeOffset.UtcNow < deadline)
        {
            if (await _bluetoothLEService.IsBluetoothOnAsync().ConfigureAwait(false))
            {
                log("Bluetooth LE central manager reports PoweredOn.");
                return;
            }

            await Task.Delay(250, token).ConfigureAwait(false);
        }

        log("Bluetooth LE central manager did not report PoweredOn before the advertising probe. Continuing so the advertiser can report its exact state.");
    }

    private async Task ProbeAdvertisingAsync(Action<string> log, CancellationToken token)
    {
        log("Creating Bluetooth LE advertiser probe.");
        using var advertiser = _bluetoothLEService.CreateBluetoothLEAdvertiserDevice()
            ?? throw new InvalidOperationException("CreateBluetoothLEAdvertiserDevice returned null.");

        var payload = BuildMk38ConnectPayload();
        log($"Starting advertising probe with MK payload length {payload.Length}.");
        await advertiser.StartAdvertiseAsync(AdvertisingInterval.Min, TxPowerLevel.Max, MKProtocol.ManufacturerID, payload).ConfigureAwait(false);
        await Task.Delay(300, token).ConfigureAwait(false);
        await advertiser.StopAdvertiseAsync().ConfigureAwait(false);
        log("Advertising probe started and stopped successfully.");
    }

    private Device CreateMk38Device(Action<string> log)
    {
        var factoryData = GetMk38FactoryData();
        log($"Creating transient MK 3.8 manual device '{factoryData.Name}' with address '{factoryData.Address}'.");

        return _componentContext.ResolveOptionalKeyed<Device>(
                   factoryData.DeviceType,
                   new NamedParameter("name", factoryData.Name),
                   new NamedParameter("address", factoryData.Address),
                   new NamedParameter("deviceData", factoryData.DeviceData),
                   new NamedParameter("settings", factoryData.Settings))
               ?? throw new InvalidOperationException("MK 3.8 manual device could not be resolved from DI.");
    }

    private async Task ConnectDeviceAsync(Device device, Action<string> log, CancellationToken token)
    {
        using var timeout = CancellationTokenSource.CreateLinkedTokenSource(token);
        timeout.CancelAfter(ConnectTimeout);

        var disconnected = false;
        void OnDisconnected(Device disconnectedDevice)
        {
            disconnected = true;
            log($"ERROR: Device disconnected callback fired for '{disconnectedDevice.Name}' while test was running.");
        }

        log($"Connecting '{device.Name}' ({device.DeviceType}) using normal app path.");
        var result = await device.ConnectAsync(
            reconnect: false,
            onDeviceDisconnected: OnDisconnected,
            channelConfigurations: GetNormalMotorChannelConfigurations(device),
            startOutputProcessing: true,
            requestDeviceInformation: false,
            token: timeout.Token).ConfigureAwait(false);

        if (result != DeviceConnectionResult.Ok)
        {
            throw new InvalidOperationException($"Device.ConnectAsync returned {result}. Device state: {device.DeviceState}.");
        }

        if (disconnected || device.DeviceState != DeviceState.Connected)
        {
            throw new InvalidOperationException($"Device did not remain connected. Device state: {device.DeviceState}.");
        }

        log($"Connected. Device state: {device.DeviceState}.");
    }

    private static async Task RunMotorSequenceAsync(Device device, float power, TimeSpan channelDuration, Action<string> log, CancellationToken token)
    {
        for (var channel = 0; channel < device.NumberOfChannels; channel++)
        {
            token.ThrowIfCancellationRequested();

            log($"Channel {channel}: setting output to {power.ToString("0.###", CultureInfo.InvariantCulture)}.");
            device.SetOutput(channel, power);

            await Task.Delay(channelDuration, token).ConfigureAwait(false);

            log($"Channel {channel}: resetting output to 0.");
            device.SetOutput(channel, 0f);

            await Task.Delay(250, token).ConfigureAwait(false);
        }
    }

    private static async Task CleanupDeviceAsync(Device device, Action<string> log)
    {
        try
        {
            log("Resetting all outputs to 0.");
            for (var channel = 0; channel < device.NumberOfChannels; channel++)
            {
                device.SetOutput(channel, 0f);
            }
        }
        catch (Exception ex)
        {
            log($"WARN: Failed to reset one or more outputs: {ex.Message}");
        }

        try
        {
            log("Disconnecting device.");
            await device.DisconnectAsync().ConfigureAwait(false);
        }
        catch (Exception ex)
        {
            log($"WARN: Disconnect failed: {ex.Message}");
        }

    }

    private IDeviceFactoryData GetMk38FactoryData()
        => _manualDeviceManager.FactoryDataList.FirstOrDefault(x => x.DeviceType == DeviceType.MK3_8)
           ?? throw new InvalidOperationException("MK 3.8 manual device factory data is not registered.");

    private byte[] BuildMk38ConnectPayload()
    {
        byte[] telegram =
        [
            0xB1, 0x7B, 0xA7, 0x80, 0x80, 0x80, 0x4F, 0xC1
        ];

        var appId = _appIdentifierService.GetAppId(2).Span;
        telegram[1] = appId[0];
        telegram[2] = appId[1];

        if (!_mkPlatformService.TryGetRfPayload(telegram, out var payload))
        {
            throw new InvalidOperationException("MK platform service could not build the RF payload.");
        }

        return payload;
    }

    private static IEnumerable<ChannelConfiguration> GetNormalMotorChannelConfigurations(Device device)
        => Enumerable.Range(0, device.NumberOfChannels)
            .Select(channel => new ChannelConfiguration
            {
                Channel = channel,
                ChannelOutputType = ChannelOutputType.NormalMotor
            })
            .ToArray();

    private static float ReadFloat(string name, float fallback)
        => float.TryParse(Environment.GetEnvironmentVariable(name), NumberStyles.Float, CultureInfo.InvariantCulture, out var value)
            ? value
            : fallback;

    private static double ReadDouble(string name, double fallback)
        => double.TryParse(Environment.GetEnvironmentVariable(name), NumberStyles.Float, CultureInfo.InvariantCulture, out var value)
            ? value
            : fallback;
}

internal static class Mk38MotorSmokeTestLauncher
{
    public static void StartIfRequested(IServiceProvider services)
    {
        if (!Mk38MotorSmokeTest.IsRequested)
        {
            return;
        }

        MainThread.BeginInvokeOnMainThread(async () =>
        {
            await Task.Delay(1000).ConfigureAwait(true);
            var test = services.GetService(typeof(Mk38MotorSmokeTest)) as Mk38MotorSmokeTest;
            var exitCode = test is null ? 1 : await test.RunAsync(CancellationToken.None).ConfigureAwait(true);
            Environment.Exit(exitCode);
        });
    }
}
