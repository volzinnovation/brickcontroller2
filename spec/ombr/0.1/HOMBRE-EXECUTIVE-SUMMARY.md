<!-- SPDX-License-Identifier: MIT -->

# HOMBRE

> Holistic Open Modular Brick Robotics Ecosystem

- **Version:** 0.1 proposal for public review
- **Date:** 13 July 2026
- **Audience:** Founding engineering, manufacturing, legal, education, and open-source partners

## The opportunity

Programmable brick systems have demonstrated the educational and engineering
value of modular controllers, motors, sensors, and construction elements, but
today's offerings remain fragmented. Brick-integrated products are generally
closed at the electronics, interface, or lifecycle level. Maker platforms can
be open and highly programmable but do not fit brick and studless-beam
constructions. The products reviewed for HOMBRE do not document the full
combination of Linux-class compute, Wi-Fi and Bluetooth Low Energy (BLE),
brick-compatible mechanics, open controller and peripheral designs, complete electrical
contracts, and a synchronized CAD and simulation workflow.

The result is avoidable lock-in: undocumented connectors, unavailable parts,
vendor-specific applications, incomplete engineering data, and systems that
cannot be independently reproduced, repaired, extended, or simulated.

## Executive proposition

HOMBRE is the umbrella project and ecosystem for modular brick automation.
Its Open Modular Brick Robotics (OMBR) Specification defines the vendor-neutral
technical system and stable `OMBR-*` identifier family. "Robotics" is
historical shorthand: the same system covers robots, vehicles, conveyors,
cranes, plotters, test rigs, instruments, kinetic installations, and other
programmable machines. A creation moves through one continuous workflow:
design in CAD, validate mechanics and wiring, simulate, program, deploy,
operate, and synchronize measured state back to its digital twin.
Its ordinary source files are versioned in Git and pack into an immutable
release that can be verified, mirrored, and forked through an optional open
Creation Portal without making that portal a runtime dependency.
Versioned Collectives coordinate several Creations through MQTT 5 while every
machine retains local safety authority. This enables workcells and, as a
long-term challenge, a farm that can assemble a brick robotic arm from
prepared parts with inspection, traceability, and recoverable human steps.

The platform is local-first, has no mandatory cloud dependency, supports
ordinary programming languages, and lets multiple manufacturers and community
projects create interoperable components.

## One system, three synchronized layers

- **Physical ecosystem:** Capability-based Mini, Midi, and Maxi controllers,
  protected native ports, power sources, cables, motors, position actuators,
  lights, sensors, adapters, brick-grid structures, gears, transmissions, and
  test fixtures. Screen references are BBC micro:bit V2 plus a Wi-Fi companion,
  Raspberry Pi Zero 2 W, and a Jetson Orin Nano-class AI/vision controller.
- **Open control plane:** Self-describing capabilities, complete electronic
  interface contracts, deterministic safety supervision, local APIs, portable
  Python and .NET programs over the same physical/simulation App API, a
  headless service and CLI, a VS Code Workbench, and interchangeable standalone
  clients.
- **Digital engineering:** Editable parametric CAD, assemblies, joints and
  transmissions, collision and load information, simulation models,
  calibration, telemetry, recording, replay, and an offline-first digital
  twin using the same identities and limits as physical hardware.

## Design principles

- **A whole ecosystem, not another hub:** HOMBRE integrates physical and
  digital components, evidence, repair, conformance, and authentic engineering
  education through the same open artifacts and practices.
- **Complete interfaces:** Every electrical boundary declares its connector,
  pinout, protocol, voltage limits, minimum, continuous, peak, inrush, and
  fault currents, grounding, sequencing, cable constraints, protection, fault
  behavior, and evidence.
- **Honest openness:** Project-controlled PCB, firmware, CAD, fixtures,
  protocols, and tests are open. Proprietary silicon and commodity parts are
  identified as documented, replaceable COTS dependencies, not mislabeled as
  open hardware.
- **Physical and digital parity:** The same capability model drives a real
  motor or sensor and its simulated counterpart. The twin is an engineering
  record, not a disconnected visualization.

## Where it applies

The initial audience spans makers, education, research, model engineering,
product prototyping, laboratory automation, and non-safety-related stationary
machines. Version 0.1 is not a safety-critical, road-going, medical, aviation,
or production-machine controller.

## Controller family

Mini targets constrained embedded control, Midi is the initial affordable
Linux hub around Raspberry Pi Zero 2 W, and Maxi adds local AI and vision.
They preserve capability and App API meanings through target-specific
artifacts. Every complete hub adds Wi-Fi, BLE, protected ports, and an
independent safety/output-disable path; application compute, AI, remote
services, and MQTT never own emergency or inner motion control.

## Implementation strategy

The shortest implementation path combines documented commercial components
with original open carriers, enclosures, safety control, protected motor and
power electronics, CAN FD, storage, and connectors. Every choice requires
bench, thermal, EMC, lifecycle, security, and cost qualification. HOMBRE does
not plan production moulds; it will maintain a source-traceable catalog and
publish lawful original parametric parts for accessible fabrication.

## Commercial reality

The Midi controller-only affordability objective is **EUR 49.99 including
German VAT**, excluding delivery, the power supply, battery, charger, external
cables, motors, sensors, lights, gamepad, and construction elements. Mini,
Maxi, cameras, robotic arms, and assembly-farm infrastructure require separate
cost records. The Midi target remains deliberately **target-unproven**.

At 1,000 units, the current pre-quote estimate is **EUR 39-57 COGS** before
engineering, certification, legal review, support, fulfilment, and channel
costs; an initial direct-sale range of **EUR 65-95 including VAT** is more
realistic. The target requires volume, BOM reduction, manufacturing partners,
measured yield/test time, and compliance cost control. Safety, evidence,
repairability, and open source are not eligible cost cuts.

## Open and lawful by design

HOMBRE is independent and is not affiliated with, authorized by, or endorsed
by the LEGO Group. HOMBRE names the ecosystem; OMBR names its specification
and identifiers. Compatibility work preserves necessary functional geometry,
uses independent metrology and original appearance, and does not copy logos,
ornamental housings, or restricted CAD catalogs.

Open licensing is not patent freedom to operate. Selected hardware, software,
geometry, names, and datasets must pass territorial rights, product,
cybersecurity, and regulatory gates. Provenance, patent-aware contribution
terms, public decisions, and conflict disclosures are required from the start.

## Delivery path

- **Phase 0 - contracts:** Freeze schemas and governance; prove the compute-to-safety
  path, one protected port, App API, Git release, Portal, and simulator.
- **Phase 1 - Hub R0:** Publish the open Midi carrier, enclosure, firmware,
  fixtures, SDKs, Workbench integration, and quote-backed price book.
- **Phase 2 - ecosystem:** Deliver motors, lights, sensors, cables, adapters,
  and measured simulation models.
- **Phase 3 - physical-digital loop:** Release metrology, mechanics, Workbench,
  twin, production link, conformance, legal, and product evidence.
- **Phase 4 - collective automation:** Prove tier portability, an MQTT workcell,
  a brick arm measured side by side against a dated physical SO-101 reference,
  and at least ten consecutive attempted named-arm products before farm claims.

## Partnership opportunity

HOMBRE seeks sponsors and partners for prototypes, metrology, EMC/thermal and
legal work; electronics, RF, safety, and DFM/DFT; sourcing, pilot builds and
test automation; original mechanics and fixtures; schemas, SDKs, VS Code,
.NET, simulation, twins and visual programming; and representative education
Creations with reproducible acceptance tests.

## Executive decision

HOMBRE is technically achievable, but commercial credibility depends on
validation, cost-down engineering, rights, and compliance. The immediate
decision is whether to fund Phase 0 as a public evidence-driven reference
program, creating a common brick-automation layer that independent teams can
inspect, reproduce, build, and implement.

This summary is informative. The normative requirements, evidence status, and
current limitations are defined by the
[OMBR 0.1 Specification](OMBR-SPEC.md),
[implementation catalog](IMPLEMENTATION-CATALOG.md), and
[legal/IP register](LEGAL-IP-REGISTER.md). The concrete benchmark and farm
challenge is defined in the
[brick-arm and assembly-farm challenge](../../../docs/use-cases/HOMBRE-BRICK-ARM-AND-ASSEMBLY-FARM-CHALLENGE.md).
