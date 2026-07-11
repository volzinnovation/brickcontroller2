#!/usr/bin/env bash
set -euo pipefail

PROFILE_DIR="${PROFILE_DIR:-}"
API_PRIVATE_KEYS_DIR="${API_PRIVATE_KEYS_DIR:-$HOME/.appstoreconnect/private_keys}"
EXPECTED_BUNDLE_ID="${EXPECTED_BUNDLE_ID:-de.raphaelvolz.brickcontroller}"
ASC_API_KEY_ID="${ASC_API_KEY_ID:-${ASC_KEY_ID:-${APP_STORE_CONNECT_API_KEY_ID:-}}}"
ASC_API_ISSUER_ID="${ASC_API_ISSUER_ID:-${ASC_ISSUER_ID:-${APP_STORE_CONNECT_API_ISSUER_ID:-}}}"
missing=0

check() {
  local label="$1"
  local command="$2"

  if eval "$command" >/dev/null 2>&1; then
    printf 'OK: %s\n' "$label"
  else
    printf 'MISSING: %s\n' "$label"
    missing=1
  fi
}

check "Apple Distribution signing identity in Keychain" \
  "security find-identity -v -p codesigning | grep -q 'Apple Distribution:'"

check "3rd Party Mac Developer Installer certificate in Keychain" \
  "security find-identity -v | grep -q '3rd Party Mac Developer Installer:'"

if [[ -n "$PROFILE_DIR" ]]; then
  profile_dirs=("$PROFILE_DIR")
else
  profile_dirs=(
    "$HOME/Library/MobileDevice/Provisioning Profiles"
    "$HOME/Library/Developer/Xcode/UserData/Provisioning Profiles"
  )
fi

profile_check=""
for dir in "${profile_dirs[@]}"; do
  profile_check+="find '$dir' -maxdepth 1 -type f \\( -name '*.mobileprovision' -o -name '*.provisionprofile' \\) 2>/dev/null; "
done

check "iOS or Mac App Store provisioning profile installed" \
  "{ $profile_check } | grep -q ."

check "Provisioning profile for $EXPECTED_BUNDLE_ID installed" \
  "tmpdir=\$(mktemp -d); trap 'rm -rf \"\$tmpdir\"' EXIT; { $profile_check } | while IFS= read -r profile; do plist=\"\$tmpdir/profile.plist\"; security cms -D -i \"\$profile\" >\"\$plist\" 2>/dev/null || continue; /usr/libexec/PlistBuddy -c 'Print Entitlements:application-identifier' \"\$plist\" 2>/dev/null || true; /usr/libexec/PlistBuddy -c 'Print Entitlements:com.apple.application-identifier' \"\$plist\" 2>/dev/null || true; done | grep -q \"\\.$EXPECTED_BUNDLE_ID$\""

if [[ -n "${ASC_API_KEY_ID:-}" ]]; then
  check "App Store Connect API private key AuthKey_${ASC_API_KEY_ID}.p8" \
    "test -f '$API_PRIVATE_KEYS_DIR/AuthKey_${ASC_API_KEY_ID}.p8'"
else
  check "App Store Connect API private key AuthKey_<key id>.p8" \
    "find '$API_PRIVATE_KEYS_DIR' -maxdepth 1 -type f -name 'AuthKey_*.p8' | grep -q ."
fi

if [[ -n "${ASC_API_KEY_ID:-}" ]]; then
  printf 'OK: ASC_API_KEY_ID is set\n'
else
  printf 'MISSING: ASC_API_KEY_ID environment variable\n'
  missing=1
fi

if [[ -n "${ASC_API_ISSUER_ID:-}" ]]; then
  printf 'OK: ASC_API_ISSUER_ID is set\n'
else
  printf 'MISSING: ASC_API_ISSUER_ID environment variable\n'
  missing=1
fi

if [[ "$missing" == "0" ]]; then
  printf 'App Store distribution prerequisites are installed.\n'
else
  printf 'App Store distribution prerequisites are incomplete.\n' >&2
  exit 1
fi
