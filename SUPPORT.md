# BrickController Support

This is the official support page for BrickController, including the BrickController 2026 app for macOS.

## Contact support

For private support, email [raphael.volz@hs-pforzheim.de](mailto:raphael.volz@hs-pforzheim.de?subject=BrickController%20Support). Support is available in English or German.

Please include:

- the BrickController version shown in the app;
- your Mac model and macOS version;
- the controller, remote, receiver, or hub model;
- what you expected to happen and what happened instead;
- the steps needed to reproduce the problem.

Screenshots and exact error messages are helpful. Do not send passwords, HTTP access tokens, or other private information.

## Help and public support

- Read the [English user guide](Help/en/index.md), [German user guide](Help/de/index.md), or [Hungarian user guide](Help/hu/index.md).
- Visit the [BrickController project on GitHub](https://github.com/volzinnovation/brickcontroller2).
- Search [existing support requests](https://github.com/volzinnovation/brickcontroller2/issues).
- [Open a new support request](https://github.com/volzinnovation/brickcontroller2/issues/new) for a question, reproducible bug, or feature request.

GitHub issues are public. Use email instead if your request contains personal or sensitive information.

## Frequently asked questions

### Why does BrickController ask for Bluetooth access?

Bluetooth is used to discover, connect to, and control compatible receivers, hubs, and remotes. The app asks only when you choose to add or scan for a Bluetooth device, or when you configure Bluetooth from the app's Settings page. macOS remembers the system permission. If access was denied, open **System Settings → Privacy & Security → Bluetooth** and enable BrickController.

### Can I use BrickController without Bluetooth?

Yes. You can open the app, manage creations and profiles, import or export configurations, and configure supported non-Bluetooth features without granting Bluetooth access. Bluetooth is required only for features that communicate with Bluetooth hardware.

### BrickController cannot find my receiver or remote

Check that Bluetooth is enabled on the Mac, BrickController is allowed under **System Settings → Privacy & Security → Bluetooth**, the hardware is powered on and in pairing or discovery mode, and another app is not already connected to it. Then open **Devices** in BrickController and scan again.

### Where can I find supported hardware?

The current list of supported receivers and controllers is maintained in the [project README](README.md#supported-receivers) and the detailed [controllers and equipment guide](docs/controllers-and-equipment.md).

### The app displayed an error or closed unexpectedly

Install the latest available version, restart the app, and repeat the action once. If the issue continues, contact support with the details listed above. For an update-related issue, mention whether the problem also occurs after removing the previous app version and installing a clean copy.

## Project and legal notice

BrickController is open-source software. Source code, releases, documentation, and the public issue tracker are available in the [BrickController GitHub repository](https://github.com/volzinnovation/brickcontroller2).

BrickController is an independent project and is not affiliated with or endorsed by the LEGO Group.
