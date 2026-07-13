<!-- SPDX-License-Identifier: MIT -->

# Specifications

## Open Modular Brick Robotics

[Two-page executive summary](ombr/0.1/OMBR-EXECUTIVE-SUMMARY.md)
([A4 PDF](../output/pdf/OMBR-Executive-Summary.pdf))

[OMBR 0.1.0 draft](ombr/0.1/OMBR-SPEC.md) proposes an open, Raspberry Pi
Zero-based, brick-compatible robotics and automation ecosystem for robots,
vehicles, stationary machines, instruments, and programmable mechanisms with:

- Wi-Fi and Bluetooth;
- a separate real-time safety controller;
- complete machine-readable electronic interface contracts for every component
  boundary, including exact connectors/pinouts, protocol, source/sink roles,
  operating and absolute voltage, full current envelopes, sequencing,
  grounding, protection, cable constraints, evidence, and fault behavior;
- an openly reproducible hub, battery/power path, motors and other actuators,
  lights, sensors, cables, adapters, fixtures, and recovery tools;
- editable brick-compatible enclosure, component, gear and transmission,
  cable-routing, and attachment-frame CAD plus a provenance-aware mechanical
  reference library;
- transport-neutral capabilities and control profiles;
- portable Python/.NET and future language runtimes, a headless CLI/service,
  an open Visual Studio Code Workbench, and an independent .NET client path;
- and synchronized CAD, simulation, and runtime digital twins.

The draft is accompanied by an
[implementation catalog](ombr/0.1/IMPLEMENTATION-CATALOG.md) that maps every
functional family to candidate open designs, open-source software, or honestly
identified COTS parts and includes a quantity-tier controller cost model. Its
[legal, IP, and market-access register](ombr/0.1/LEGAL-IP-REGISTER.md) defines
the patent/FTO, modular-design, trademark, CAD/data-license, protocol,
contributor-patent, product-classification, and regulatory gates. Neither file
claims that unresolved candidates are qualified or legally cleared.

The controller-only affordability goal is EUR 49.99 including German VAT, but
it is explicitly `target-unproven`: the current pre-quote four-port model
estimates EUR 39–57 unit COGS at 1000 units before several excluded commercial
costs and indicates roughly EUR 65–95 initial direct retail unless the design,
volume, and supply economics improve.

This is a whole programmable brick-automation ecosystem specification, not a
proposal for only a robot, controller brick, or application. BrickController2
contributes proven concepts and transitional compatibility, but is not the
canonical OMBR UI, storage model, API, or runtime.

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
