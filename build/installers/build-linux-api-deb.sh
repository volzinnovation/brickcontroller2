#!/usr/bin/env bash
set -euo pipefail

SCRIPT_DIR="$(cd "$(dirname "${BASH_SOURCE[0]}")" && pwd)"
REPO_ROOT="$(cd "${SCRIPT_DIR}/../.." && pwd)"

RID="${1:-linux-x64}"
CONFIGURATION="${CONFIGURATION:-Release}"
VERSION="${APP_DISPLAY_VERSION:-0.0.0}"
DEB_VERSION="${VERSION#v}"
PROJECT="BrickController2/BrickController2.Linux.Api/BrickController2.Linux.Api.csproj"
PUBLISH_DIR="artifacts/publish/brickcontroller-api-${RID}"
PACKAGE_ROOT="artifacts/package/brickcontroller-api-${RID}"
OUTPUT_DIR="artifacts/installers"
PACKAGE_NAME="brickcontroller-api"

if ! command -v dpkg-deb >/dev/null 2>&1; then
  echo "dpkg-deb is required to build the Debian package. Run this script on Ubuntu or install dpkg tooling first." >&2
  exit 127
fi

case "${RID}" in
  linux-x64)
    DEB_ARCH="amd64"
    ;;
  linux-arm64)
    DEB_ARCH="arm64"
    ;;
  *)
    echo "Unsupported runtime identifier: ${RID}" >&2
    exit 1
    ;;
esac

cd "${REPO_ROOT}"

dotnet publish "${PROJECT}" \
  -c "${CONFIGURATION}" \
  -r "${RID}" \
  --self-contained true \
  -p:Version="${DEB_VERSION}" \
  -p:PublishSingleFile=false \
  -o "${PUBLISH_DIR}"

rm -rf "${PACKAGE_ROOT}"
mkdir -p \
  "${PACKAGE_ROOT}/DEBIAN" \
  "${PACKAGE_ROOT}/usr/bin" \
  "${PACKAGE_ROOT}/usr/lib/brickcontroller-api" \
  "${PACKAGE_ROOT}/lib/systemd/system"

cp -a "${PUBLISH_DIR}/." "${PACKAGE_ROOT}/usr/lib/brickcontroller-api/"

cat > "${PACKAGE_ROOT}/usr/bin/brickcontroller-api" <<'EOF'
#!/usr/bin/env bash
export ASPNETCORE_URLS="${ASPNETCORE_URLS:-http://127.0.0.1:5080}"
exec /usr/lib/brickcontroller-api/BrickController2.Linux.Api "$@"
EOF

cat > "${PACKAGE_ROOT}/lib/systemd/system/brickcontroller-api.service" <<'EOF'
[Unit]
Description=BrickController headless Bluetooth API
Documentation=https://github.com/volzinnovation/brickcontroller2/
After=dbus.service bluetooth.service
Requires=bluetooth.service

[Service]
Type=simple
Environment=ASPNETCORE_URLS=http://127.0.0.1:5080
ExecStart=/usr/bin/brickcontroller-api
Restart=on-failure
RestartSec=3

[Install]
WantedBy=multi-user.target
EOF

cat > "${PACKAGE_ROOT}/DEBIAN/control" <<EOF
Package: ${PACKAGE_NAME}
Version: ${DEB_VERSION}
Section: utils
Priority: optional
Architecture: ${DEB_ARCH}
Maintainer: BrickController Contributors <noreply@example.com>
Depends: bluez, dbus
Description: Headless BrickController Bluetooth API host
 Runs a localhost HTTP API that translates requests into BlueZ Bluetooth LE central operations.
EOF

cat > "${PACKAGE_ROOT}/DEBIAN/postinst" <<'EOF'
#!/usr/bin/env bash
set -e

if command -v systemctl >/dev/null 2>&1; then
  systemctl daemon-reload || true
  systemctl enable --now brickcontroller-api.service || true
fi
EOF

cat > "${PACKAGE_ROOT}/DEBIAN/prerm" <<'EOF'
#!/usr/bin/env bash
set -e

if command -v systemctl >/dev/null 2>&1; then
  systemctl stop brickcontroller-api.service || true
  systemctl disable brickcontroller-api.service || true
fi
EOF

cat > "${PACKAGE_ROOT}/DEBIAN/postrm" <<'EOF'
#!/usr/bin/env bash
set -e

if command -v systemctl >/dev/null 2>&1; then
  systemctl daemon-reload || true
fi
EOF

chmod 0755 \
  "${PACKAGE_ROOT}/usr/bin/brickcontroller-api" \
  "${PACKAGE_ROOT}/DEBIAN/postinst" \
  "${PACKAGE_ROOT}/DEBIAN/prerm" \
  "${PACKAGE_ROOT}/DEBIAN/postrm"

mkdir -p "${OUTPUT_DIR}"
TARGET="${OUTPUT_DIR}/BrickController.Api_${DEB_VERSION}_${RID}.deb"
dpkg-deb --build --root-owner-group "${PACKAGE_ROOT}" "${TARGET}"
echo "${TARGET}"
