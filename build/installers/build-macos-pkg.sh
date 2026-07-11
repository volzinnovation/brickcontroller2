#!/usr/bin/env bash
set -euo pipefail

SCRIPT_DIR="$(cd "$(dirname "${BASH_SOURCE[0]}")" && pwd)"
REPO_ROOT="$(cd "${SCRIPT_DIR}/../.." && pwd)"

RID="${1:-maccatalyst-arm64}"
CONFIGURATION="${CONFIGURATION:-Release}"
VERSION="${APP_DISPLAY_VERSION:-0.0.0}"
PROJECT="BrickController2/BrickController2.MacCatalyst/BrickController2.MacCatalyst.csproj"
OUTPUT_DIR="artifacts/installers"
STAGING_DIR="artifacts/macos-pkgroot/${RID}"

cd "${REPO_ROOT}"

if [[ -n "${XCODE_DEVELOPER_DIR:-}" ]]; then
  export DEVELOPER_DIR="${XCODE_DEVELOPER_DIR}"
fi

dotnet publish "${PROJECT}" \
  -c "${CONFIGURATION}" \
  -f net10.0-maccatalyst \
  -r "${RID}" \
  --self-contained true \
  -p:ApplicationDisplayVersion="${VERSION}" \
  -p:CodesignKey="${CODESIGN_KEY:--}"

PUBLISH_DIR="BrickController2/BrickController2.MacCatalyst/bin/${CONFIGURATION}/net10.0-maccatalyst/${RID}/publish"
mkdir -p "${OUTPUT_DIR}"

PUBLISHED_PKG="$(find "${PUBLISH_DIR}" -maxdepth 1 -type f -name '*.pkg' | head -n 1)"
if [[ -n "${PUBLISHED_PKG}" ]]; then
  PKG_PATH="${OUTPUT_DIR}/BrickController_${VERSION}_${RID}.pkg"
  cp "${PUBLISHED_PKG}" "${PKG_PATH}"
  echo "${PKG_PATH}"
  exit 0
fi

APP_PATH="$(find "${PUBLISH_DIR}" -maxdepth 1 -type d -name '*.app' | head -n 1)"
if [[ -z "${APP_PATH}" ]]; then
  echo "No .pkg package or .app bundle found in ${PUBLISH_DIR}" >&2
  exit 1
fi

rm -rf "${STAGING_DIR}"
mkdir -p "${STAGING_DIR}/Applications"
cp -R "${APP_PATH}" "${STAGING_DIR}/Applications/BrickController2.app"

PKG_PATH="${OUTPUT_DIR}/BrickController_${VERSION}_${RID}.pkg"
productbuild \
  --root "${STAGING_DIR}" \
  --identifier "de.raphaelvolz.brickcontroller" \
  --version "${VERSION}" \
  --install-location "/" \
  "${PKG_PATH}"

echo "${PKG_PATH}"
