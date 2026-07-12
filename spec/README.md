<!-- SPDX-License-Identifier: MIT -->

# Specifications

## Open Modular Brick Robotics

[OMBR 0.1.0 draft](ombr/0.1/OMBR-SPEC.md) proposes an open, Raspberry Pi
Zero-based, brick-compatible robotics ecosystem with:

- Wi-Fi and Bluetooth;
- a separate real-time safety controller;
- complete machine-readable electronic interface contracts for every component
  boundary, including exact connectors/pinouts, protocol, source/sink roles,
  operating and absolute voltage, full current envelopes, sequencing,
  grounding, protection, cable constraints, evidence, and fault behavior;
- an openly reproducible hub, battery/power path, motors and other actuators,
  lights, sensors, cables, adapters, fixtures, and recovery tools;
- editable brick-compatible enclosure, component, cable-routing, and
  attachment-frame CAD;
- transport-neutral capabilities and control profiles;
- portable Python/.NET and future language runtimes, a headless CLI/service,
  an open Visual Studio Code Workbench, and an independent .NET client path;
- and synchronized CAD, simulation, and runtime digital twins.

This is a whole programmable brick-robotics ecosystem specification, not a proposal for
only a controller brick or application. BrickController2 contributes proven
concepts and transitional compatibility, but is not the canonical OMBR UI,
storage model, API, or runtime.

The draft includes a
[machine-readable project, program, and twin syntax schema](ombr/0.1/schema/ombr-project.schema.json)
and an
[illustrative syntax-rover manifest](ombr/0.1/examples/syntax-rover.ombr.json).

Version 0.1 is a proposal for public review, not a production hardware or
safety certification. No OMBR conformance profile is claimable against it.
The native link remains experimental until the production connector,
transport/topology, sender authentication or port isolation, and exact
framing/timing/version rules pass their public tests.

These draft files are currently licensed under the repository's MIT license.
The specification's future patent-aware governance and license remain an
explicit pre-0.2 decision.
