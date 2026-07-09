#!/usr/bin/env python3
"""
Example BrickController2 HTTP-control client.

This creates a virtual HTTP controller with five axis capabilities, then sets
each capability to 50% for one second and resets it to 0. The HTTP API exposes
controller input events. It does not directly connect to output devices. To move
a physical Mould King MK 3.8 controller, BrickController2 must be running a
creation/player profile that maps these events to the MK 3.8 device channels:

  Controller ID: HTTP MK3.8 Test
  Capabilities: mk38Channel0 .. mk38Channel4

The script uses only Python's standard library.
"""

from __future__ import annotations

import argparse
import json
import os
import sys
import time
import urllib.error
import urllib.parse
import urllib.request
from dataclasses import dataclass
from typing import Any


DEFAULT_BASE_URL = "http://127.0.0.1:5081"
DEFAULT_CONTROLLER_ID = "HTTP MK3.8 Test"
DEFAULT_POWER = 0.5
DEFAULT_SECONDS = 1.0


@dataclass(frozen=True)
class HttpResponse:
    status: int
    body: Any
    raw_body: str


class BrickControllerClient:
    def __init__(self, base_url: str, token: str | None) -> None:
        self.base_url = base_url.rstrip("/")
        self.token = token

    def request(
        self,
        method: str,
        path: str,
        body: dict[str, Any] | None = None,
        auth: bool = True,
    ) -> HttpResponse:
        headers = {"Accept": "application/json"}
        data = None
        if body is not None:
            data = json.dumps(body).encode("utf-8")
            headers["Content-Type"] = "application/json"
        if auth and self.token:
            headers["X-BrickController-Token"] = self.token

        request = urllib.request.Request(
            self.base_url + path,
            data=data,
            headers=headers,
            method=method,
        )

        try:
            with urllib.request.urlopen(request, timeout=10) as response:
                raw_body = response.read().decode("utf-8", errors="replace")
                return HttpResponse(response.status, parse_json(raw_body), raw_body)
        except urllib.error.HTTPError as error:
            raw_body = error.read().decode("utf-8", errors="replace")
            return HttpResponse(error.code, parse_json(raw_body), raw_body)

    def get_status(self) -> HttpResponse:
        return self.request("GET", "/api/control/status", auth=False)

    def get_settings(self) -> HttpResponse:
        return self.request("GET", "/api/control/settings")

    def list_controllers(self, auth: bool = True) -> HttpResponse:
        return self.request("GET", "/api/controllers", auth=auth)

    def create_virtual_controller(self, controller_id: str) -> HttpResponse:
        return self.request(
            "POST",
            "/api/controllers/virtual",
            {
                "id": controller_id,
                "name": controller_id,
                "capabilities": [
                    {
                        "id": f"mk38Channel{channel}",
                        "eventType": "axis",
                        "eventCode": f"MK38_Channel_{channel}",
                        "displayName": f"MK 3.8 channel {channel}",
                        "minValue": -1,
                        "maxValue": 1,
                        "neutralValue": 0,
                    }
                    for channel in range(5)
                ],
            },
        )

    def get_controller(self, controller_id: str) -> HttpResponse:
        return self.request("GET", f"/api/controllers/{quote(controller_id)}")

    def get_capability(self, controller_id: str, capability_id: str) -> HttpResponse:
        return self.request(
            "GET",
            f"/api/controllers/{quote(controller_id)}/capabilities/{quote(capability_id)}",
        )

    def set_capability(self, controller_id: str, capability_id: str, value: float) -> HttpResponse:
        return self.request(
            "PUT",
            f"/api/controllers/{quote(controller_id)}/capabilities/{quote(capability_id)}",
            {"value": value},
        )

    def delete_virtual_controller(self, controller_id: str) -> HttpResponse:
        return self.request("DELETE", f"/api/controllers/virtual/{quote(controller_id)}")


def parse_json(raw_body: str) -> Any:
    if not raw_body:
        return None
    try:
        return json.loads(raw_body)
    except json.JSONDecodeError:
        return raw_body


def quote(value: str) -> str:
    return urllib.parse.quote(value, safe="")


def require_status(response: HttpResponse, expected: int | tuple[int, ...], label: str) -> None:
    expected_values = expected if isinstance(expected, tuple) else (expected,)
    if response.status not in expected_values:
        raise RuntimeError(
            f"{label}: HTTP {response.status}, expected {expected_values}. Body: {response.raw_body}"
        )
    print(f"PASS {label}: HTTP {response.status}")


def run_http_smoke(client: BrickControllerClient, controller_id: str) -> None:
    require_status(client.get_status(), 200, "service status")

    unauthenticated = client.list_controllers(auth=False)
    if unauthenticated.status == 401:
        print("PASS protected controllers route rejects missing token: HTTP 401")
    elif unauthenticated.status == 200:
        print("INFO protected controllers route accepted request; auth is disabled")
    else:
        raise RuntimeError(
            f"unexpected unauthenticated controllers response: HTTP {unauthenticated.status} {unauthenticated.raw_body}"
        )

    require_status(client.get_settings(), 200, "authenticated settings")
    require_status(client.list_controllers(), 200, "authenticated controllers list")
    require_status(client.create_virtual_controller(controller_id), 200, "create virtual controller")
    require_status(client.get_controller(controller_id), 200, "read virtual controller")


def run_motor_sequence(
    client: BrickControllerClient,
    controller_id: str,
    power: float,
    seconds: float,
) -> None:
    for channel in range(5):
        capability_id = f"mk38Channel{channel}"

        require_status(
            client.get_capability(controller_id, capability_id),
            200,
            f"read capability {capability_id}",
        )

        print(f"Channel {channel}: set {power:.3f}")
        require_status(
            client.set_capability(controller_id, capability_id, power),
            200,
            f"set {capability_id} to {power:.3f}",
        )

        time.sleep(seconds)

        print(f"Channel {channel}: reset 0")
        require_status(
            client.set_capability(controller_id, capability_id, 0.0),
            200,
            f"reset {capability_id}",
        )

        time.sleep(0.25)


def reset_all_channels(client: BrickControllerClient, controller_id: str) -> None:
    for channel in range(5):
        capability_id = f"mk38Channel{channel}"
        response = client.set_capability(controller_id, capability_id, 0.0)
        if response.status not in (200, 400, 404, 409):
            print(
                f"WARN reset {capability_id}: HTTP {response.status} {response.raw_body}",
                file=sys.stderr,
            )


def build_parser() -> argparse.ArgumentParser:
    parser = argparse.ArgumentParser(
        description="Exercise BrickController2 HTTP control API and drive five MK 3.8 test channels.",
    )
    parser.add_argument("--base-url", default=os.environ.get("BRICKCONTROLLER_BASE_URL", DEFAULT_BASE_URL))
    parser.add_argument(
        "--token",
        default=os.environ.get("BRICKCONTROLLER_TOKEN") or os.environ.get("BC2_HTTP_TOKEN"),
        help="HTTP control access token. Can also use BRICKCONTROLLER_TOKEN or BC2_HTTP_TOKEN.",
    )
    parser.add_argument("--controller-id", default=DEFAULT_CONTROLLER_ID)
    parser.add_argument("--power", type=float, default=DEFAULT_POWER)
    parser.add_argument("--seconds", type=float, default=DEFAULT_SECONDS)
    parser.add_argument(
        "--keep-controller",
        action="store_true",
        help="Leave the virtual controller registered after the test.",
    )
    parser.add_argument(
        "--setup-only",
        action="store_true",
        help="Create the virtual controller and stop before setting channel values.",
    )
    return parser


def main() -> int:
    args = build_parser().parse_args()
    if not args.token:
        print(
            "Missing HTTP token. Pass --token or set BRICKCONTROLLER_TOKEN.",
            file=sys.stderr,
        )
        return 64

    power = max(-1.0, min(1.0, args.power))
    seconds = max(0.1, args.seconds)
    client = BrickControllerClient(args.base_url, args.token)

    try:
        print(
            "INFO this is an HTTP virtual-controller input sequence. "
            "Physical motors move only when BrickController2 maps this controller "
            "to MK 3.8 outputs and a player is active."
        )
        run_http_smoke(client, args.controller_id)

        if args.setup_only:
            print("Setup complete. Map the listed capabilities in BrickController2, then rerun without --setup-only.")
            return 0

        run_motor_sequence(client, args.controller_id, power, seconds)
        print("PASS HTTP virtual-controller sequence completed.")
        return 0
    except Exception as error:
        print(f"FAIL {error}", file=sys.stderr)
        return 1
    finally:
        if not args.keep_controller:
            reset_all_channels(client, args.controller_id)
            response = client.delete_virtual_controller(args.controller_id)
            if response.status not in (200, 204, 404):
                print(
                    f"WARN delete virtual controller: HTTP {response.status} {response.raw_body}",
                    file=sys.stderr,
                )


if __name__ == "__main__":
    raise SystemExit(main())
