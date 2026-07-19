#!/usr/bin/env bash
set -euo pipefail

usage() {
  cat <<'USAGE'
Usage: verify-app-bundle-dependencies.sh [--require-signature] [--expected-build NUMBER] APP_PATH

Checks every Mach-O file in a macOS app bundle and fails when a bundled
dependency is missing, a dynamic-library path uses unsafe parent traversal,
or the optional build/signature requirements are not met.
USAGE
}

require_signature=0
expected_build=""
app_path=""

while [[ $# -gt 0 ]]; do
  case "$1" in
    --require-signature)
      require_signature=1
      shift
      ;;
    --expected-build)
      expected_build="${2:-}"
      shift 2
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
      if [[ -n "$app_path" ]]; then
        echo "Only one app bundle may be checked at a time." >&2
        exit 2
      fi
      app_path="$1"
      shift
      ;;
  esac
done

if [[ -z "$app_path" || ! -d "$app_path" ]]; then
  echo "App bundle not found: ${app_path:-<missing>}" >&2
  exit 2
fi

info_plist="$app_path/Contents/Info.plist"
if [[ ! -f "$info_plist" ]]; then
  echo "Info.plist not found in app bundle: $app_path" >&2
  exit 2
fi

executable_name="$(/usr/libexec/PlistBuddy -c 'Print :CFBundleExecutable' "$info_plist")"
executable_dir="$app_path/Contents/MacOS"
main_executable="$executable_dir/$executable_name"
if [[ ! -f "$main_executable" ]]; then
  echo "Main executable not found: $main_executable" >&2
  exit 1
fi

if [[ -n "$expected_build" ]]; then
  actual_build="$(/usr/libexec/PlistBuddy -c 'Print :CFBundleVersion' "$info_plist")"
  if [[ "$actual_build" != "$expected_build" ]]; then
    echo "Unexpected CFBundleVersion: expected $expected_build, found $actual_build" >&2
    exit 1
  fi
fi

if [[ "$require_signature" == "1" ]]; then
  codesign --verify --deep --strict --verbose=2 "$app_path"
fi

errors=0
checked_binaries=0
checked_dependencies=0

report_error() {
  echo "ERROR: $*" >&2
  errors=$((errors + 1))
}

expand_path_variable() {
  local value="$1"
  local loader_dir="$2"
  case "$value" in
    @executable_path)
      printf '%s\n' "$executable_dir"
      ;;
    @executable_path/*)
      printf '%s/%s\n' "$executable_dir" "${value#@executable_path/}"
      ;;
    @loader_path)
      printf '%s\n' "$loader_dir"
      ;;
    @loader_path/*)
      printf '%s/%s\n' "$loader_dir" "${value#@loader_path/}"
      ;;
    /*)
      printf '%s\n' "$value"
      ;;
    *)
      return 1
      ;;
  esac
}

read_rpaths() {
  otool -l "$1" | awk '
    $1 == "cmd" && $2 == "LC_RPATH" { in_rpath = 1; next }
    in_rpath && $1 == "path" { print $2; in_rpath = 0 }
  '
}

verify_dependency() {
  local binary="$1"
  local dependency="$2"
  local loader_dir
  local resolved
  local candidate
  local found
  local rpath

  loader_dir="$(dirname "$binary")"
  checked_dependencies=$((checked_dependencies + 1))

  case "$dependency" in
    /System/*|/usr/lib/*)
      return 0
      ;;
  esac

  case "$dependency" in
    @executable_path/../../*|@loader_path/../../*)
      report_error "Unsafe dynamic library path in $binary: $dependency"
      return 0
      ;;
  esac

  case "$dependency" in
    @rpath/*)
      found=0
      while IFS= read -r rpath; do
        [[ -z "$rpath" ]] && continue
        if resolved="$(expand_path_variable "$rpath" "$loader_dir")"; then
          candidate="${resolved%/}/${dependency#@rpath/}"
          if [[ -e "$candidate" ]]; then
            found=1
            break
          fi
        fi
      done < <({ read_rpaths "$binary"; read_rpaths "$main_executable"; } | awk '!seen[$0]++')
      if [[ "$found" == "0" ]]; then
        report_error "Missing bundled library for $binary: $dependency"
      fi
      ;;
    @executable_path*|@loader_path*|/*)
      if resolved="$(expand_path_variable "$dependency" "$loader_dir")"; then
        if [[ ! -e "$resolved" ]]; then
          report_error "Missing bundled library for $binary: $dependency (resolved to $resolved)"
        elif [[ "$resolved" != "$app_path"/* ]]; then
          report_error "Non-system dependency escapes the app bundle in $binary: $dependency"
        fi
      else
        report_error "Unsupported dynamic library path in $binary: $dependency"
      fi
      ;;
    *)
      report_error "Relative dynamic library path in $binary: $dependency"
      ;;
  esac
}

while IFS= read -r -d '' candidate; do
  if ! file -b "$candidate" | grep -q 'Mach-O'; then
    continue
  fi

  checked_binaries=$((checked_binaries + 1))
  while IFS= read -r dependency; do
    [[ -z "$dependency" ]] && continue
    verify_dependency "$candidate" "$dependency"
  done < <(otool -L "$candidate" | tail -n +2 | sed -E 's/^[[:space:]]+([^[:space:]]+).*/\1/')
done < <(find "$app_path/Contents" -type f -print0)

if [[ "$checked_binaries" == "0" ]]; then
  echo "No Mach-O binaries found in app bundle: $app_path" >&2
  exit 1
fi

if [[ "$errors" != "0" ]]; then
  echo "Dependency verification failed with $errors error(s)." >&2
  exit 1
fi

echo "Verified $checked_dependencies dependencies across $checked_binaries Mach-O binaries in $app_path"
