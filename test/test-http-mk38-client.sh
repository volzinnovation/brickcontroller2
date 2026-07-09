#!/usr/bin/env bash
set -euo pipefail

ROOT_DIR="$(cd "$(dirname "${BASH_SOURCE[0]}")/.." && pwd)"
PROJECT="$ROOT_DIR/BrickController2/BrickController2.MacCatalyst/BrickController2.MacCatalyst.csproj"
CLIENT="$ROOT_DIR/example/http_mk38_motor_test_client.py"
CONFIGURATION="${CONFIGURATION:-Debug}"
TEST_TOKEN="${BRICKCONTROLLER_TOKEN:-test-token}"
HTTP_PORT="${BRICKCONTROLLER_HTTP_PORT:-5081}"
BASE_URL="${BRICKCONTROLLER_BASE_URL:-http://127.0.0.1:$HTTP_PORT}"
APP_LOG="${BC2_HTTP_TEST_APP_LOG:-/tmp/brickcontroller2-http-test-app.log}"
READY_MARKER="BC2_HTTP_MK38_E2E_READY"
FAILURE_MARKER="BC2_HTTP_MK38_E2E_FAIL"

if [[ -z "${RID:-}" ]]; then
  case "$(uname -m)" in
    arm64) RID="maccatalyst-arm64" ;;
    x86_64) RID="maccatalyst-x64" ;;
    *) echo "Unsupported Mac architecture: $(uname -m)" >&2; exit 64 ;;
  esac
fi

OTHER_INSTANCES="$(ps -axo pid=,command= | awk '/BrickController2[.]MacCatalyst/ && !/awk/ { print }' || true)"
if [[ -n "$OTHER_INSTANCES" ]]; then
  echo "Another BrickController2 MacCatalyst process is already running." >&2
  echo "Close it before this test so the script controls the HTTP token and port." >&2
  echo "$OTHER_INSTANCES" >&2
  exit 66
fi

dotnet build "$PROJECT" \
  -c "$CONFIGURATION" \
  -f net10.0-maccatalyst \
  -r "$RID" \
  -p:CodesignKey=-

APP_BIN="$(find "$ROOT_DIR/BrickController2/BrickController2.MacCatalyst/bin/$CONFIGURATION/net10.0-maccatalyst/$RID" \
  -path "*/BrickController 2.app/Contents/MacOS/BrickController2.MacCatalyst" \
  -type f \
  -perm -111 \
  | sort \
  | tail -n 1)"

if [[ -z "$APP_BIN" ]]; then
  echo "Could not find built BrickController2.MacCatalyst app binary." >&2
  exit 65
fi

rm -f "$APP_LOG"

export BRICKCONTROLLER_HTTP_ENABLED=1
export BRICKCONTROLLER_HTTP_TOKEN="$TEST_TOKEN"
export BRICKCONTROLLER_HTTP_PORT="$HTTP_PORT"
export BRICKCONTROLLER_HTTP_LISTEN_MODE="${BRICKCONTROLLER_HTTP_LISTEN_MODE:-Loopback}"
export BC2_HTTP_MK38_E2E_TEST=1

echo "Starting BrickController2 with HTTP token '$TEST_TOKEN' at $BASE_URL"
"$APP_BIN" >"$APP_LOG" 2>&1 &
APP_PID=$!

cleanup() {
  if kill -0 "$APP_PID" 2>/dev/null; then
    kill "$APP_PID" 2>/dev/null || true
    wait "$APP_PID" 2>/dev/null || true
  fi
}
trap cleanup EXIT

for _ in {1..80}; do
  if curl -fsS "$BASE_URL/api/control/status" >/dev/null 2>&1; then
    break
  fi

  if ! kill -0 "$APP_PID" 2>/dev/null; then
    echo "BrickController2 exited before HTTP control became ready." >&2
    cat "$APP_LOG" >&2 || true
    exit 1
  fi

  sleep 0.25
done

if ! curl -fsS "$BASE_URL/api/control/status" >/dev/null 2>&1; then
  echo "HTTP control did not become ready at $BASE_URL." >&2
  cat "$APP_LOG" >&2 || true
  exit 1
fi

echo "HTTP control is ready. Waiting for MK 3.8 profile/device readiness."
for _ in {1..160}; do
  if [[ -f "$APP_LOG" ]] && grep -q "$READY_MARKER" "$APP_LOG"; then
    break
  fi

  if [[ -f "$APP_LOG" ]] && grep -q "$FAILURE_MARKER" "$APP_LOG"; then
    echo "MK 3.8 end-to-end setup failed." >&2
    cat "$APP_LOG" >&2 || true
    exit 1
  fi

  if ! kill -0 "$APP_PID" 2>/dev/null; then
    echo "BrickController2 exited before MK 3.8 end-to-end setup became ready." >&2
    cat "$APP_LOG" >&2 || true
    exit 1
  fi

  sleep 0.25
done

if ! [[ -f "$APP_LOG" ]] || ! grep -q "$READY_MARKER" "$APP_LOG"; then
  echo "MK 3.8 end-to-end setup did not become ready." >&2
  cat "$APP_LOG" >&2 || true
  exit 1
fi

echo "MK 3.8 profile/device setup is ready. Running HTTP example client."
BRICKCONTROLLER_TOKEN="$TEST_TOKEN" "$CLIENT" --base-url "$BASE_URL" "$@"
