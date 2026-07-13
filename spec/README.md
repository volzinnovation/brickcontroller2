<!-- SPDX-License-Identifier: MIT -->

# Specifications

## HOMBRE — Holistic Open Modular Brick Robotics Ecosystem

[Two-page HOMBRE executive summary](ombr/0.1/HOMBRE-EXECUTIVE-SUMMARY.md)
([A4 PDF](../output/pdf/HOMBRE-Executive-Summary.pdf))

[HOMBRE education curriculum draft](../docs/education/HOMBRE-EDUCATION-CURRICULUM-DRAFT.md)

[Brick arm and assembly-farm engineering challenge](../docs/use-cases/HOMBRE-BRICK-ARM-AND-ASSEMBLY-FARM-CHALLENGE.md)

HOMBRE is the umbrella project and ecosystem. Its normative technical
foundation, the [Open Modular Brick Robotics (OMBR) 0.1.0 draft
specification](ombr/0.1/OMBR-SPEC.md), defines a capability-based Mini,
Raspberry Pi Zero-class Midi, and accelerated AI/vision Maxi,
brick-compatible robotics and automation system for robots, vehicles,
stationary machines, instruments, and programmable mechanisms. OMBR remains
the stable identifier family for profiles, schemas, APIs, project packages,
tests, and conformance claims. The system includes:

- Wi-Fi and BLE;
- MQTT-based coordination of independently safe Creations in Collectives,
  workcells, and assembly farms;
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
- Git-friendly versioned Creation folders, deterministic releases, and an
  optional open portal for publishing, verifying, mirroring, and forking the
  complete CAD/software/instruction package;
- synchronized CAD, simulation, and runtime digital twins using the same App
  API and logical capabilities as the physical controller;
- reproducible AI/vision model packages, camera/calibration contracts, and
  accelerator-neutral inference with explicit resource and fallback evidence;
- automated-assembly recipes, task/resource leases, inspection, work-product
  provenance, and a five-axis arm plus gripper challenge profile; and
- authentic multidisciplinary engineering education using the same open
  requirements, Git, CAD, interface, program, simulation, test, evidence, and
  release practices as the implementation ecosystem.

The OMBR draft is accompanied by a
[HOMBRE implementation catalog](ombr/0.1/IMPLEMENTATION-CATALOG.md) that maps every
functional family to candidate open designs, open-source software, or honestly
identified COTS parts and includes a quantity-tier controller cost model. Its
[legal, IP, and market-access register](ombr/0.1/LEGAL-IP-REGISTER.md) defines
the patent/FTO, modular-design, trademark, CAD/data-license, protocol,
contributor-patent, product-classification, and regulatory gates. Neither file
claims that unresolved candidates are qualified or legally cleared.

The [`OMBR-AFFORDABLE-HUB-MIDI-1`](ombr/0.1/OMBR-SPEC.md#17-implementation-catalog-and-cost-model)
controller-only affordability goal is EUR 49.99 including German VAT. It is
explicitly `target-unproven`, applies only to the exact Midi base SKU and
excludes delivery and named external accessories: the current pre-quote
four-port model estimates EUR 39–57 unit COGS at 1000 units and roughly EUR
65–95 initial direct retail unless design, volume, and supply economics improve.

HOMBRE is a whole programmable brick-automation ecosystem, not only a robot,
controller brick, or application. BrickController2 contributes proven concepts
and transitional compatibility, but is not the canonical HOMBRE Workbench or
an OMBR storage model, API, or runtime.

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
