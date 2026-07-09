# Desktop installers

BrickController 2 has two desktop heads in addition to the existing Windows build:

- `BrickController2.MacCatalyst` targets macOS 15+ through .NET MAUI Mac Catalyst and reuses the iOS CoreBluetooth/GameController platform services.
- `BrickController2.Linux` targets Ubuntu/Linux through the experimental .NET MAUI GTK4 backend from `dotnet/maui-labs`.
- `BrickController2.Linux.Api` targets Ubuntu/Linux as a headless HTTP API host backed by BlueZ BLE central operations.

## macOS

Build an unsigned `.pkg` installer on macOS:

```bash
dotnet workload install maui-maccatalyst
APP_DISPLAY_VERSION=3.4 bash build/installers/build-macos-pkg.sh maccatalyst-arm64
APP_DISPLAY_VERSION=3.4 bash build/installers/build-macos-pkg.sh maccatalyst-x64
```

The installers are written to `artifacts/installers`.

Set `CODESIGN_KEY` before running the script to use a real signing identity instead of ad-hoc signing. Release builds should also be notarized before public distribution.

The installed .NET MacCatalyst workload normally expects the selected Xcode version to match exactly. For the .NET 10.0.301 workload set, Xcode 26.6 can report as incompatible with `Microsoft.MacCatalyst.Sdk.net10.0_26.5` even though the underlying Apple SDK is unchanged from Xcode 26.5. The Mac Catalyst project disables the strict Xcode version check by default through `ValidateXcodeVersion=false`.

If you prefer to keep validation enabled, run the package build with Xcode 26.5 selected:

```bash
XCODE_DEVELOPER_DIR=/Applications/Xcode_26.5.app/Contents/Developer \
  APP_DISPLAY_VERSION=3.4 \
  bash build/installers/build-macos-pkg.sh maccatalyst-arm64
```

## Ubuntu/Linux

The Linux desktop app uses the GTK4 backend that Microsoft documents as experimental and not officially supported. The UI, database, preferences, localization, and file import/export are wired through the Linux head. Bluetooth LE and game controller input are still unsupported in this UI head.

For Bluetooth on Linux, use the headless API host. It exposes scan/connect/GATT read/write/notification operations over HTTP and routes them through BlueZ on D-Bus. Details and HTTP examples are in [linux-headless-api.md](linux-headless-api.md).

Install prerequisites and build a `.deb` installer on Ubuntu:

```bash
dotnet workload install maui-android

sudo apt install libgtk-4-dev libwebkitgtk-6.0-dev \
  gobject-introspection libgirepository1.0-dev \
  gir1.2-gtk-4.0 gir1.2-webkit-6.0 pkg-config dpkg-dev

APP_DISPLAY_VERSION=3.4 bash build/installers/build-linux-deb.sh linux-x64
```

The installer is written to `artifacts/installers`.

Build the headless API `.deb` installer on Ubuntu:

```bash
dotnet workload install maui-android
sudo apt install dpkg-dev

APP_DISPLAY_VERSION=3.4 bash build/installers/build-linux-api-deb.sh linux-x64
```
