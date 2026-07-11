#!/usr/bin/env bash
set -euo pipefail

ROOT_DIR="$(cd "$(dirname "$0")/../.." && pwd)"
PROJECT="$ROOT_DIR/BrickController2/BrickController2.MacCatalyst/BrickController2.MacCatalyst.csproj"
CONFIGURATION="${CONFIGURATION:-Release}"
RID="${RID:-maccatalyst-arm64}"
ARTIFACTS_PATH="${ARTIFACTS_PATH:-$ROOT_DIR/artifacts/appstore-publish}"

usage() {
  cat <<USAGE
Usage: MAC_CODESIGN_KEY="Apple Distribution: ..." MAC_CODESIGN_PROVISION="Profile Name" MAC_PACKAGE_SIGNING_KEY="3rd Party Mac Developer Installer: ..." $0 [--upload] [extra dotnet publish args...]

Required for signing:
  MAC_CODESIGN_KEY          Apple Distribution certificate name from Keychain.
  MAC_CODESIGN_PROVISION    Mac App Store provisioning profile name.
  MAC_PACKAGE_SIGNING_KEY   Mac installer certificate name from Keychain.

Optional upload authentication:
  ASC_API_KEY_ID            App Store Connect API key id.
  ASC_API_ISSUER_ID         App Store Connect issuer id.
  API_PRIVATE_KEYS_DIR      Directory containing AuthKey_<key id>.p8.
  ARTIFACTS_PATH            Build output root. Default: artifacts/appstore-publish
USAGE
}

upload=0
extra_args=()
extra_arg_count=0
while [[ $# -gt 0 ]]; do
  case "$1" in
    --upload)
      upload=1
      shift
      ;;
    -h|--help)
      usage
      exit 0
      ;;
    *)
      extra_args+=("$1")
      extra_arg_count=$((extra_arg_count + 1))
      shift
      ;;
  esac
done

if [[ -z "${MAC_CODESIGN_KEY:-}" || -z "${MAC_CODESIGN_PROVISION:-}" || -z "${MAC_PACKAGE_SIGNING_KEY:-}" ]]; then
  echo "MAC_CODESIGN_KEY, MAC_CODESIGN_PROVISION, and MAC_PACKAGE_SIGNING_KEY are required." >&2
  usage >&2
  exit 2
fi

publish_args=(
  "$PROJECT"
  -f net10.0-maccatalyst
  -c "$CONFIGURATION"
  --artifacts-path "$ARTIFACTS_PATH"
  -p:RuntimeIdentifier="$RID"
  -p:MtouchLink=SdkOnly
  -p:CreatePackage=true
  -p:EnableCodeSigning=true
  -p:EnablePackageSigning=true
  -p:CodesignKey="$MAC_CODESIGN_KEY"
  -p:CodesignProvision="$MAC_CODESIGN_PROVISION"
  -p:CodesignEntitlements="$ROOT_DIR/BrickController2/BrickController2.MacCatalyst/Entitlements.plist"
  -p:PackageSigningKey="$MAC_PACKAGE_SIGNING_KEY"
)
if [[ "$extra_arg_count" -gt 0 ]]; then
  publish_args+=("${extra_args[@]}")
fi

dotnet publish "${publish_args[@]}"

pkg="$(find "$ARTIFACTS_PATH" -type f -name '*.pkg' -print 2>/dev/null | while IFS= read -r file; do printf '%s\t%s\n' "$(stat -f %m "$file")" "$file"; done | sort -rn | head -1 | cut -f2-)"
if [[ -z "$pkg" ]]; then
  echo "No PKG found below $ARTIFACTS_PATH" >&2
  exit 3
fi
echo "PKG: $pkg"

if [[ "$upload" == "1" ]]; then
  if [[ -z "${ASC_API_KEY_ID:-}" || -z "${ASC_API_ISSUER_ID:-}" ]]; then
    echo "ASC_API_KEY_ID and ASC_API_ISSUER_ID are required for upload." >&2
    exit 2
  fi
  xcrun altool --upload-package "$pkg" \
    --api-key "$ASC_API_KEY_ID" \
    --api-issuer "$ASC_API_ISSUER_ID" \
    --show-progress
fi
