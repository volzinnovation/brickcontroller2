# Linux headless API host

`BrickController2.Linux.Api` is a headless Ubuntu/Linux host that exposes BrickController Bluetooth LE operations over HTTP. It uses BlueZ through D-Bus as a BLE central and is intended for machines where the normal UI is not available or where a separate process should drive Bluetooth devices.

The current API is GATT-level: scan, connect, enumerate services, read, write, and subscribe to characteristic notifications. Higher-level device/profile commands can be layered on this host by routing HTTP command models into the existing BrickController device managers.

## Build installer

On Ubuntu:

```bash
dotnet workload install maui-android
sudo apt install dpkg-dev

APP_DISPLAY_VERSION=3.4 bash build/installers/build-linux-api-deb.sh linux-x64
```

The installer is written to `artifacts/installers`.

Install it with:

```bash
sudo apt install ./artifacts/installers/BrickController2.Api_3.4_linux-x64.deb
```

The package installs:

- `/usr/lib/brickcontroller2-api/` for the self-contained app
- `/usr/bin/brickcontroller2-api` as a launcher
- `brickcontroller2-api.service` as a systemd service

The service binds to `http://127.0.0.1:5080` by default. Override `ASPNETCORE_URLS` in a systemd drop-in if the API should listen on another interface. Add authentication or a reverse proxy before exposing it outside localhost.

## Service commands

```bash
systemctl status brickcontroller2-api.service
journalctl -u brickcontroller2-api.service -f
sudo systemctl restart brickcontroller2-api.service
```

BlueZ must be running and the Bluetooth adapter must be powered:

```bash
systemctl status bluetooth.service
bluetoothctl show
```

## HTTP examples

Status:

```bash
curl http://127.0.0.1:5080/api/bluetooth/status
```

Scan for five seconds:

```bash
curl -X POST http://127.0.0.1:5080/api/bluetooth/scan \
  -H 'Content-Type: application/json' \
  -d '{"durationSeconds":5}'
```

Connect after a device has been discovered:

```bash
curl -X POST http://127.0.0.1:5080/api/bluetooth/devices/AA:BB:CC:DD:EE:FF/connect \
  -H 'Content-Type: application/json' \
  -d '{"autoConnect":false}'
```

Read a characteristic:

```bash
curl http://127.0.0.1:5080/api/bluetooth/devices/AA:BB:CC:DD:EE:FF/gatt/0000180f-0000-1000-8000-00805f9b34fb/00002a19-0000-1000-8000-00805f9b34fb/read
```

Write a characteristic with a hex payload:

```bash
curl -X POST http://127.0.0.1:5080/api/bluetooth/devices/AA:BB:CC:DD:EE:FF/gatt/SERVICE_UUID/CHARACTERISTIC_UUID/write \
  -H 'Content-Type: application/json' \
  -d '{"encoding":"hex","data":"01020304","withResponse":false}'
```

Enable notifications and read the latest buffered events:

```bash
curl -X POST http://127.0.0.1:5080/api/bluetooth/devices/AA:BB:CC:DD:EE:FF/gatt/SERVICE_UUID/CHARACTERISTIC_UUID/notifications \
  -H 'Content-Type: application/json' \
  -d '{"enable":true}'

curl http://127.0.0.1:5080/api/bluetooth/devices/AA:BB:CC:DD:EE:FF/notifications
```
