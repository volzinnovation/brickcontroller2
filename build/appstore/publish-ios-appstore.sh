#!/usr/bin/env bash
set -euo pipefail

ROOT_DIR="$(cd "$(dirname "$0")/../.." && pwd)"
PROJECT="$ROOT_DIR/BrickController2/BrickController2.iOS/BrickController2.iOS.csproj"
CONFIGURATION="${CONFIGURATION:-Release}"
RID="${RID:-ios-arm64}"
ARTIFACTS_PATH="${ARTIFACTS_PATH:-$ROOT_DIR/artifacts/appstore-publish}"

usage() {
  cat <<USAGE
Usage: IOS_CODESIGN_KEY="Apple Distribution: ..." IOS_CODESIGN_PROVISION="Profile Name" $0 [--upload] [extra dotnet publish args...]

Required for signing:
  IOS_CODESIGN_KEY         Apple Distribution certificate name from Keychain.
  IOS_CODESIGN_PROVISION   iOS App Store provisioning profile name.

Optional upload authentication:
  ASC_API_KEY_ID           App Store Connect API key id.
  ASC_API_ISSUER_ID        App Store Connect issuer id.
  API_PRIVATE_KEYS_DIR     Directory containing AuthKey_<key id>.p8.
  ARTIFACTS_PATH           Build output root. Default: artifacts/appstore-publish

The iOS MAUI workload must be installed before this script can build.
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

if ! dotnet workload list | grep -q '^maui-ios[[:space:]]'; then
  echo "Missing .NET workload: maui-ios. Install it with: sudo dotnet workload install maui-ios" >&2
  exit 2
fi
if [[ -z "${IOS_CODESIGN_KEY:-}" || -z "${IOS_CODESIGN_PROVISION:-}" ]]; then
  echo "IOS_CODESIGN_KEY and IOS_CODESIGN_PROVISION are required." >&2
  usage >&2
  exit 2
fi

publish_args=(
  "$PROJECT"
  -f net10.0-ios
  -c "$CONFIGURATION"
  --artifacts-path "$ARTIFACTS_PATH"
  -p:RuntimeIdentifier="$RID"
  -p:ArchiveOnBuild=true
  -p:CodesignKey="$IOS_CODESIGN_KEY"
  -p:CodesignProvision="$IOS_CODESIGN_PROVISION"
)
if [[ "$extra_arg_count" -gt 0 ]]; then
  publish_args+=("${extra_args[@]}")
fi

dotnet publish "${publish_args[@]}"

ipa="$(find "$ARTIFACTS_PATH" -type f -name '*.ipa' -print 2>/dev/null | while IFS= read -r file; do printf '%s\t%s\n' "$(stat -f %m "$file")" "$file"; done | sort -rn | head -1 | cut -f2-)"
if [[ -z "$ipa" ]]; then
  echo "No IPA found below $ARTIFACTS_PATH" >&2
  exit 3
fi
echo "IPA: $ipa"

if [[ "$upload" == "1" ]]; then
  if [[ -z "${ASC_API_KEY_ID:-}" || -z "${ASC_API_ISSUER_ID:-}" ]]; then
    echo "ASC_API_KEY_ID and ASC_API_ISSUER_ID are required for upload." >&2
    exit 2
  fi
  xcrun altool --upload-package "$ipa" \
    --api-key "$ASC_API_KEY_ID" \
    --api-issuer "$ASC_API_ISSUER_ID" \
    --show-progress
fi
