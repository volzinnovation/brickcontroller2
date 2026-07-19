#!/usr/bin/env bash
set -euo pipefail

if [[ "$(uname -s)" != "Darwin" ]]; then
  echo "Skipping macOS bundle dependency tests on $(uname -s)."
  exit 0
fi

root_dir="$(cd "$(dirname "${BASH_SOURCE[0]}")/.." && pwd)"
verifier="$root_dir/build/macos/verify-app-bundle-dependencies.sh"
fixture_root="$(mktemp -d "${TMPDIR:-/tmp}/brickcontroller-dependency-test.XXXXXX")"
app_path="$fixture_root/Fixture.app"
contents="$app_path/Contents"
executable="$contents/MacOS/Fixture"
dylib="$contents/MonoBundle/libfixture.dylib"

cleanup() {
  chmod -R u+w "$fixture_root" 2>/dev/null || true
  rm -rf "$fixture_root"
}
trap cleanup EXIT

mkdir -p "$contents/MacOS" "$contents/MonoBundle"

cat > "$fixture_root/library.c" <<'SOURCE'
int fixture_answer(void) { return 42; }
SOURCE

cat > "$fixture_root/main.c" <<'SOURCE'
extern int fixture_answer(void);
int main(void) { return fixture_answer() == 42 ? 0 : 1; }
SOURCE

cat > "$contents/Info.plist" <<'PLIST'
<?xml version="1.0" encoding="UTF-8"?>
<!DOCTYPE plist PUBLIC "-//Apple//DTD PLIST 1.0//EN" "http://www.apple.com/DTDs/PropertyList-1.0.dtd">
<plist version="1.0">
<dict>
  <key>CFBundleExecutable</key>
  <string>Fixture</string>
  <key>CFBundleIdentifier</key>
  <string>test.brickcontroller.dependencies</string>
  <key>CFBundleVersion</key>
  <string>999</string>
</dict>
</plist>
PLIST

clang -dynamiclib "$fixture_root/library.c" \
  -install_name '@rpath/libfixture.dylib' \
  -o "$dylib"
clang "$fixture_root/main.c" \
  -L"$contents/MonoBundle" \
  -lfixture \
  -Wl,-rpath,@executable_path/../MonoBundle \
  -o "$executable"

"$verifier" --expected-build 999 "$app_path" >/dev/null

mv "$dylib" "$fixture_root/libfixture.missing"
if "$verifier" "$app_path" >"$fixture_root/missing.out" 2>&1; then
  echo "Verifier accepted an app with a missing dynamic library." >&2
  exit 1
fi
grep -q 'Missing bundled library' "$fixture_root/missing.out"
mv "$fixture_root/libfixture.missing" "$dylib"

install_name_tool \
  -change '@rpath/libfixture.dylib' \
  '@executable_path/../../Contents/MonoBundle/libfixture.dylib' \
  "$executable"
if "$verifier" "$app_path" >"$fixture_root/unsafe.out" 2>&1; then
  echo "Verifier accepted an unsafe parent-traversing dynamic library path." >&2
  exit 1
fi
grep -q 'Unsafe dynamic library path' "$fixture_root/unsafe.out"

echo "macOS app bundle dependency verifier tests passed."
