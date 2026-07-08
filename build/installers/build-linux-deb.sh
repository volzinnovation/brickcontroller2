#!/usr/bin/env bash
set -euo pipefail

SCRIPT_DIR="$(cd "$(dirname "${BASH_SOURCE[0]}")" && pwd)"
REPO_ROOT="$(cd "${SCRIPT_DIR}/../.." && pwd)"

RID="${1:-linux-x64}"
CONFIGURATION="${CONFIGURATION:-Release}"
VERSION="${APP_DISPLAY_VERSION:-0.0.0}"
PROJECT="BrickController2/BrickController2.Linux/BrickController2.Linux.csproj"
OUTPUT_DIR="artifacts/installers"

cd "${REPO_ROOT}"

dotnet publish "${PROJECT}" \
  -c "${CONFIGURATION}" \
  -r "${RID}" \
  --self-contained true \
  -p:ApplicationDisplayVersion="${VERSION}" \
  -p:Version="${VERSION}" \
  -p:CreateDeb=true

mkdir -p "${OUTPUT_DIR}"
DEB_PATH="$(find "BrickController2/BrickController2.Linux/bin/Deb" -name '*.deb' -type f | head -n 1)"
if [[ -z "${DEB_PATH}" ]]; then
  echo "No .deb package found for ${RID}" >&2
  exit 1
fi

TARGET="${OUTPUT_DIR}/BrickController2_${VERSION}_${RID}.deb"
cp "${DEB_PATH}" "${TARGET}"
echo "${TARGET}"
