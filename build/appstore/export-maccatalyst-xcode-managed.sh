#!/usr/bin/env bash
set -euo pipefail
export COPYFILE_DISABLE=1

ROOT_DIR="$(cd "$(dirname "$0")/../.." && pwd)"
PROJECT="$ROOT_DIR/BrickController2/BrickController2.MacCatalyst/BrickController2.MacCatalyst.csproj"
APP_INFO_PLIST="$ROOT_DIR/BrickController2/BrickController2.MacCatalyst/Info.plist"
CONFIGURATION="${CONFIGURATION:-Release}"
RID="${RID:-maccatalyst-arm64}"
ARTIFACTS_PATH="${ARTIFACTS_PATH:-$ROOT_DIR/artifacts/xcode-managed-export}"
TEAM_ID="${TEAM_ID:-F4CT29PZS5}"
APP_NAME="${APP_NAME:-BrickController}"
BUNDLE_ID="${BUNDLE_ID:-de.raphaelvolz.brickcontroller}"
EXECUTABLE_NAME="${EXECUTABLE_NAME:-BrickController2.MacCatalyst}"
VERIFY_APP_SCRIPT="$ROOT_DIR/build/macos/verify-app-bundle-dependencies.sh"
VERIFY_PKG_SCRIPT="$ROOT_DIR/build/macos/verify-installer-package.sh"
APP_VERSION="${APP_VERSION:-$(/usr/libexec/PlistBuddy -c 'Print :CFBundleShortVersionString' "$APP_INFO_PLIST")}"
APP_BUILD="${APP_BUILD:-$(/usr/libexec/PlistBuddy -c 'Print :CFBundleVersion' "$APP_INFO_PLIST")}"

usage() {
  cat <<USAGE
Usage: $0 [--upload]

Builds an ad-hoc signed Mac Catalyst .app, wraps it in a minimal .xcarchive,
and lets Xcode export it with automatic App Store Connect signing. This works
with Xcode cloud-managed Apple Distribution and installer certificates.

Options:
  --upload   Ask xcodebuild -exportArchive to upload to App Store Connect.

Environment:
  ARTIFACTS_PATH  Output root. Default: artifacts/xcode-managed-export
  TEAM_ID         Apple Developer team ID. Default: F4CT29PZS5
  BUNDLE_ID       Bundle ID. Default: de.raphaelvolz.brickcontroller
  RID             Runtime identifier. Default: maccatalyst-arm64
  APP_VERSION     CFBundleShortVersionString. Default: read from Info.plist
  APP_BUILD       CFBundleVersion. Default: read from Info.plist
USAGE
}

destination="export"
while [[ $# -gt 0 ]]; do
  case "$1" in
    --upload)
      destination="upload"
      shift
      ;;
    -h|--help)
      usage
      exit 0
      ;;
    *)
      echo "Unknown argument: $1" >&2
      usage >&2
      exit 2
      ;;
  esac
done

rm -rf \
  "$ARTIFACTS_PATH/bin/BrickController2.MacCatalyst" \
  "$ARTIFACTS_PATH/obj/BrickController2.MacCatalyst" \
  "$ARTIFACTS_PATH/$APP_NAME.xcarchive" \
  "$ARTIFACTS_PATH/export"

dotnet publish "$PROJECT" \
  -f net10.0-maccatalyst \
  -c "$CONFIGURATION" \
  --artifacts-path "$ARTIFACTS_PATH" \
  -p:RuntimeIdentifier="$RID" \
  -p:MtouchLink=SdkOnly \
  -p:CreatePackage=false \
  -p:CodesignKey=- \
  -p:CodesignEntitlements="$ROOT_DIR/BrickController2/BrickController2.MacCatalyst/Entitlements.plist"

app_path="$(find "$ARTIFACTS_PATH/bin/BrickController2.MacCatalyst" -type d -name '*.app' -print 2>/dev/null | while IFS= read -r file; do printf '%s\t%s\n' "$(stat -f %m "$file")" "$file"; done | sort -rn | head -1 | cut -f2-)"
if [[ -z "$app_path" ]]; then
  echo "No .app bundle found below $ARTIFACTS_PATH/bin/BrickController2.MacCatalyst" >&2
  exit 3
fi

archive_path="$ARTIFACTS_PATH/$APP_NAME.xcarchive"
export_options="$ARTIFACTS_PATH/ExportOptions.plist"
export_path="$ARTIFACTS_PATH/export"

mkdir -p "$archive_path/Products/Applications" "$archive_path/dSYMs"
ditto --noextattr --norsrc "$app_path" "$archive_path/Products/Applications/$APP_NAME.app"
archive_app="$archive_path/Products/Applications/$APP_NAME.app"
xattr -cr "$archive_app" 2>/dev/null || true
dot_clean -m "$archive_app" 2>/dev/null || true
find "$archive_app" -name '._*' -delete

"$VERIFY_APP_SCRIPT" --require-signature --expected-build "$APP_BUILD" "$archive_app"

app_executable="$archive_app/Contents/MacOS/$EXECUTABLE_NAME"
if command -v dsymutil >/dev/null 2>&1 && [[ -f "$app_executable" ]]; then
  dsymutil "$app_executable" -o "$archive_path/dSYMs/$APP_NAME.app.dSYM" || true
fi

info_plist="$archive_path/Info.plist"
rm -f "$info_plist"
plutil -create xml1 "$info_plist"
creation_date="$(date -u '+%Y-%m-%dT%H:%M:%SZ')"
plutil -insert CreationDate -date "$creation_date" "$info_plist"
/usr/libexec/PlistBuddy \
  -c 'Add :ArchiveVersion integer 2' \
  -c "Add :Name string $APP_NAME" \
  -c "Add :SchemeName string $APP_NAME" \
  -c 'Add :ApplicationProperties dict' \
  -c "Add :ApplicationProperties:ApplicationPath string Applications/$APP_NAME.app" \
  -c 'Add :ApplicationProperties:Architectures array' \
  -c 'Add :ApplicationProperties:Architectures:0 string arm64' \
  -c "Add :ApplicationProperties:CFBundleIdentifier string $BUNDLE_ID" \
  -c "Add :ApplicationProperties:CFBundleShortVersionString string $APP_VERSION" \
  -c "Add :ApplicationProperties:CFBundleVersion string $APP_BUILD" \
  -c 'Add :ApplicationProperties:SigningIdentity string -' \
  -c "Add :ApplicationProperties:Team string $TEAM_ID" \
  "$info_plist"

rm -f "$export_options"
plutil -create xml1 "$export_options"
/usr/libexec/PlistBuddy \
  -c "Add :destination string $destination" \
  -c 'Add :method string app-store-connect' \
  -c 'Add :signingStyle string automatic' \
  -c 'Add :stripSwiftSymbols bool true' \
  -c "Add :teamID string $TEAM_ID" \
  -c 'Add :uploadSymbols bool true' \
  "$export_options"

xcodebuild -exportArchive \
  -archivePath "$archive_path" \
  -exportPath "$export_path" \
  -exportOptionsPlist "$export_options" \
  -allowProvisioningUpdates

if [[ "$destination" == "export" ]]; then
  pkg="$(find "$export_path" -maxdepth 1 -type f -name '*.pkg' -print | head -1)"
  if [[ -n "$pkg" ]]; then
    "$VERIFY_PKG_SCRIPT" --expected-build "$APP_BUILD" "$pkg"
    echo "PKG: $pkg"
  fi
fi
