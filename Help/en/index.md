# BrickController 2 Help

BrickController 2 lets you control supported brick receivers with a compatible gamepad, LEGO remote, motion input, or the optional HTTP control interface.

## Getting started

1. Turn on the receiver or hub.
2. Connect a supported controller to your Mac.
3. Open BrickController 2 and create or select a creation.
4. Add the receiver and map controller inputs to outputs.
5. Test the mappings at low power before operating the model normally.

## Bluetooth permissions

The first time BrickController 2 scans for receivers, macOS may request Bluetooth permission. Allow access in **System Settings > Privacy & Security > Bluetooth**. If a receiver does not appear, verify that it is powered on, nearby, and not connected to another device.

## Controller troubleshooting

- Confirm that macOS recognizes the gamepad.
- Disconnect and reconnect the controller before restarting the app.
- Check the selected profile and input mappings.
- Start with low output values when testing a new profile.

## HTTP control

The Mac version can expose an optional HTTP control API for integrations and test clients. Configuration and examples are documented in the repository.

## Source code and issue reporting

Project source, technical documentation, releases, and issue reporting:

https://github.com/volzinnovation/brickcontroller2/

## Credits

Original author: István Murvai

Maintainer of the Mac version: Prof. Dr. Raphael Volz (Pforzheim University)

## License information

BrickController 2 is distributed under the MIT License. The license permits use, modification, redistribution, publication, and commercial distribution, provided that the applicable copyright and permission notice are retained.

The application uses third-party components under permissive licenses, including MIT, Apache 2.0, and public-domain-style terms. Their copyright notices, license texts, and project links must be retained in the distributed application and accompanying notices.
