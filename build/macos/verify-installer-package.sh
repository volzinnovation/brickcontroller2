#!/usr/bin/env bash
set -euo pipefail

usage() {
  cat <<'USAGE'
Usage: verify-installer-package.sh [--require-system-sqlite] [--expected-build NUMBER] PKG_PATH

Expands a macOS installer package and verifies every contained app bundle,
including code signatures and all bundled Mach-O dependencies.
USAGE
}

script_dir="$(cd "$(dirname "${BASH_SOURCE[0]}")" && pwd)"
expected_build=""
require_system_sqlite=0
pkg_path=""

while [[ $# -gt 0 ]]; do
  case "$1" in
    --expected-build)
      expected_build="${2:-}"
      shift 2
      ;;
    --require-system-sqlite)
      require_system_sqlite=1
      shift
      ;;
    -h|--help)
      usage
      exit 0
      ;;
    -* )
      echo "Unknown option: $1" >&2
      usage >&2
      exit 2
      ;;
    *)
      pkg_path="$1"
      shift
      ;;
  esac
done

if [[ -z "$pkg_path" || ! -f "$pkg_path" ]]; then
  echo "Installer package not found: ${pkg_path:-<missing>}" >&2
  exit 2
fi

work_dir="$(mktemp -d "${TMPDIR:-/tmp}/brickcontroller-pkg.XXXXXX")"
expanded_dir="$work_dir/expanded"
cleanup() {
  chmod -R u+w "$work_dir" 2>/dev/null || true
  rm -rf "$work_dir"
}
trap cleanup EXIT

pkgutil --expand-full "$pkg_path" "$expanded_dir"

app_count=0
while IFS= read -r -d '' app_path; do
  app_count=$((app_count + 1))
  args=(--require-signature)
  if [[ "$require_system_sqlite" == "1" ]]; then
    args+=(--require-system-sqlite)
  fi
  if [[ -n "$expected_build" ]]; then
    args+=(--expected-build "$expected_build")
  fi
  "$script_dir/verify-app-bundle-dependencies.sh" "${args[@]}" "$app_path"
done < <(find "$expanded_dir" -type d -name '*.app' -print0)

if [[ "$app_count" == "0" ]]; then
  echo "No app bundle found in installer package: $pkg_path" >&2
  exit 1
fi

echo "Verified $app_count app bundle(s) in $pkg_path"
