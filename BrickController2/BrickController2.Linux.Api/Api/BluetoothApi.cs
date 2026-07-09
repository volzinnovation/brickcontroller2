using BrickController2.Linux.Api.Services;
using BrickController2.PlatformServices.BluetoothLE;
using Microsoft.AspNetCore.Mvc;

namespace BrickController2.Linux.Api.Api;

internal static class BluetoothApi
{
    public static IEndpointRouteBuilder MapBluetoothApi(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/api/bluetooth")
            .WithTags("Bluetooth");

        group.MapGet("/status", GetStatusAsync)
            .WithName("GetBluetoothStatus");

        group.MapPost("/scan", ScanAsync)
            .WithName("ScanBluetoothDevices");

        group.MapPost("/devices/{address}/connect", ConnectAsync)
            .WithName("ConnectBluetoothDevice");

        group.MapPost("/devices/{address}/disconnect", DisconnectAsync)
            .WithName("DisconnectBluetoothDevice");

        group.MapGet("/devices/{address}/services", GetServicesAsync)
            .WithName("GetBluetoothDeviceServices");

        group.MapGet("/devices/{address}/gatt/{serviceUuid}/{characteristicUuid}/read", ReadCharacteristicAsync)
            .WithName("ReadBluetoothCharacteristic");

        group.MapPost("/devices/{address}/gatt/{serviceUuid}/{characteristicUuid}/write", WriteCharacteristicAsync)
            .WithName("WriteBluetoothCharacteristic");

        group.MapPost("/devices/{address}/gatt/{serviceUuid}/{characteristicUuid}/notifications", SetNotificationsAsync)
            .WithName("SetBluetoothCharacteristicNotifications");

        group.MapGet("/devices/{address}/notifications", GetNotifications)
            .WithName("GetBluetoothDeviceNotifications");

        return app;
    }

    private static async Task<IResult> GetStatusAsync(IBluetoothLEService bluetooth)
    {
        var supported = await bluetooth.IsBluetoothLESupportedAsync().ConfigureAwait(false);
        var powered = supported && await bluetooth.IsBluetoothOnAsync().ConfigureAwait(false);
        var advertisingSupported = supported && await bluetooth.IsBluetoothLEAdvertisingSupportedAsync().ConfigureAwait(false);

        return Results.Ok(new BluetoothStatusResponse(supported, powered, advertisingSupported));
    }

    private static async Task<IResult> ScanAsync(
        [FromBody] ScanRequest? request,
        BluetoothDeviceSessionRegistry registry,
        CancellationToken token)
    {
        var durationSeconds = Math.Clamp(request?.DurationSeconds ?? 5, 1, 60);
        var devices = await registry.ScanAsync(TimeSpan.FromSeconds(durationSeconds), token).ConfigureAwait(false);
        return Results.Ok(devices);
    }

    private static async Task<IResult> ConnectAsync(
        string address,
        [FromBody] ConnectRequest? request,
        BluetoothDeviceSessionRegistry registry,
        CancellationToken token)
    {
        var response = await registry.ConnectAsync(address, request?.AutoConnect ?? false, token).ConfigureAwait(false);
        return response is null
            ? Results.NotFound(new ErrorResponse($"Bluetooth device '{address}' is not known to BlueZ. Scan first, then connect."))
            : Results.Ok(response);
    }

    private static async Task<IResult> DisconnectAsync(
        string address,
        BluetoothDeviceSessionRegistry registry,
        CancellationToken token)
    {
        var disconnected = await registry.DisconnectAsync(address, token).ConfigureAwait(false);
        return disconnected
            ? Results.NoContent()
            : Results.NotFound(new ErrorResponse($"Bluetooth device session '{address}' was not found."));
    }

    private static IResult GetServicesAsync(string address, BluetoothDeviceSessionRegistry registry)
    {
        var session = registry.GetSession(address);
        return session is null
            ? Results.NotFound(new ErrorResponse($"Bluetooth device session '{address}' is not connected."))
            : Results.Ok(session);
    }

    private static async Task<IResult> ReadCharacteristicAsync(
        string address,
        string serviceUuid,
        string characteristicUuid,
        BluetoothDeviceSessionRegistry registry,
        CancellationToken token)
    {
        var response = await registry.ReadAsync(address, serviceUuid, characteristicUuid, token).ConfigureAwait(false);
        return response is null
            ? Results.NotFound(new ErrorResponse("Connected device, service, or characteristic was not found."))
            : Results.Ok(response);
    }

    private static async Task<IResult> WriteCharacteristicAsync(
        string address,
        string serviceUuid,
        string characteristicUuid,
        [FromBody] CharacteristicWriteRequest request,
        BluetoothDeviceSessionRegistry registry,
        CancellationToken token)
    {
        if (!BluetoothPayloadEncoding.TryDecode(request.Data, request.Encoding, out var data, out var error))
        {
            return Results.BadRequest(new ErrorResponse(error));
        }

        var written = await registry.WriteAsync(
                address,
                serviceUuid,
                characteristicUuid,
                data,
                request.WithResponse,
                token)
            .ConfigureAwait(false);

        return written
            ? Results.NoContent()
            : Results.NotFound(new ErrorResponse("Connected device, service, or characteristic was not found, or BlueZ rejected the write."));
    }

    private static async Task<IResult> SetNotificationsAsync(
        string address,
        string serviceUuid,
        string characteristicUuid,
        [FromBody] NotificationRequest request,
        BluetoothDeviceSessionRegistry registry,
        CancellationToken token)
    {
        var enabled = await registry.SetNotificationsAsync(address, serviceUuid, characteristicUuid, request.Enable, token)
            .ConfigureAwait(false);

        return enabled is null
            ? Results.NotFound(new ErrorResponse("Connected device, service, or characteristic was not found, or BlueZ rejected the notification change."))
            : Results.Ok(new NotificationStateResponse(address, serviceUuid, characteristicUuid, enabled.Value));
    }

    private static IResult GetNotifications(string address, BluetoothDeviceSessionRegistry registry)
    {
        var notifications = registry.GetNotifications(address);
        return notifications is null
            ? Results.NotFound(new ErrorResponse($"Bluetooth device session '{address}' is not connected."))
            : Results.Ok(notifications);
    }
}
