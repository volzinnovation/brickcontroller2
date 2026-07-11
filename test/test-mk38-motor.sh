#!/usr/bin/env bash
set -euo pipefail

ROOT_DIR="$(cd "$(dirname "${BASH_SOURCE[0]}")/.." && pwd)"
PROJECT="$ROOT_DIR/BrickController2/BrickController2.MacCatalyst/BrickController2.MacCatalyst.csproj"
CONFIGURATION="${CONFIGURATION:-Debug}"

if [[ -z "${RID:-}" ]]; then
  case "$(uname -m)" in
    arm64) RID="maccatalyst-arm64" ;;
    x86_64) RID="maccatalyst-x64" ;;
    *) echo "Unsupported Mac architecture: $(uname -m)" >&2; exit 64 ;;
  esac
fi

export BC2_MK38_MOTOR_TEST=1
export BC2_MK38_MOTOR_TEST_POWER="${BC2_MK38_MOTOR_TEST_POWER:-0.5}"
export BC2_MK38_MOTOR_TEST_SECONDS="${BC2_MK38_MOTOR_TEST_SECONDS:-1}"
export BC2_MK38_MOTOR_TEST_LOG="${BC2_MK38_MOTOR_TEST_LOG:-/tmp/brickcontroller2-mk38-motor-test.log}"

OTHER_INSTANCES="$(ps -axo pid=,command= | awk '/BrickController2[.]MacCatalyst/ && !/awk/ { print }' || true)"
if [[ -n "$OTHER_INSTANCES" ]]; then
  echo "Another BrickController2 MacCatalyst process is already running." >&2
  echo "Close it before this test. MK 3.8 control is BLE-advertisement based, so another instance can override this test." >&2
  echo "$OTHER_INSTANCES" >&2
  exit 66
fi

dotnet build "$PROJECT" \
  -c "$CONFIGURATION" \
  -f net10.0-maccatalyst \
  -r "$RID" \
  -p:CodesignKey=-

APP_BIN="$(find "$ROOT_DIR/BrickController2/BrickController2.MacCatalyst/bin/$CONFIGURATION/net10.0-maccatalyst/$RID" \
  -path "*/BrickController.app/Contents/MacOS/BrickController2.MacCatalyst" \
  -type f \
  -perm -111 \
  | sort \
  | tail -n 1)"

if [[ -z "$APP_BIN" ]]; then
  echo "Could not find built BrickController2.MacCatalyst app binary." >&2
  exit 65
fi

echo "Running MK 3.8 motor smoke test from: $APP_BIN"
echo "Log: $BC2_MK38_MOTOR_TEST_LOG"
set +e
"$APP_BIN"
STATUS=$?
set -e

echo
echo "===== MK 3.8 motor smoke test log ====="
if [[ -f "$BC2_MK38_MOTOR_TEST_LOG" ]]; then
  cat "$BC2_MK38_MOTOR_TEST_LOG"
else
  echo "No log file was written at $BC2_MK38_MOTOR_TEST_LOG" >&2
fi
echo "===== exit code: $STATUS ====="
exit "$STATUS"
