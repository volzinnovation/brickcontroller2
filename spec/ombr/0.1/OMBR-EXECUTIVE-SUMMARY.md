<!-- SPDX-License-Identifier: MIT -->

# OMBR - Open Modular Brick Robotics

> Executive summary - open infrastructure for programmable brick automation

- **Version:** 0.1 proposal for public review
- **Date:** 13 July 2026
- **Audience:** Founding engineering, manufacturing, legal, education, and open-source partners

## The opportunity

Programmable brick systems have demonstrated the educational and engineering
value of modular controllers, motors, sensors, and construction elements, but
today's offerings remain fragmented. Brick-integrated products are generally
closed at the electronics, interface, or lifecycle level. Maker platforms can
be open and highly programmable but do not fit brick and studless-beam
constructions. The products reviewed by OMBR do not document the full
combination of Linux-class compute, Wi-Fi and Bluetooth, brick-compatible
mechanics, open controller and peripheral designs, complete electrical
contracts, and a synchronized CAD and simulation workflow.

The result is avoidable lock-in: undocumented connectors, unavailable parts,
vendor-specific applications, incomplete engineering data, and systems that
cannot be independently reproduced, repaired, extended, or simulated.

## Executive proposition

OMBR defines a vendor-neutral system for modular brick automation. "Robotics"
is historical shorthand: the same system covers robots, vehicles, conveyors,
cranes, plotters, test rigs, instruments, kinetic installations, and other
programmable machines. A creation moves through one continuous workflow:
design in CAD, validate mechanics and wiring, simulate, program, deploy,
operate, and synchronize measured state back to its digital twin.

The platform is local-first, has no mandatory cloud dependency, supports
ordinary programming languages, and lets multiple manufacturers and community
projects create interoperable components.

## One system, three synchronized layers

- **Physical ecosystem:** A Raspberry Pi Zero-class controller, protected
  native ports, power sources, cables, motors, position actuators, lights,
  sensors, adapters, brick-grid structures, gears, transmissions, and test
  fixtures.
- **Open control plane:** Self-describing capabilities, complete electronic
  interface contracts, deterministic safety supervision, local APIs, portable
  Python and .NET SDKs, a headless service and CLI, a VS Code Workbench, and
  interchangeable standalone clients.
- **Digital engineering:** Editable parametric CAD, assemblies, joints and
  transmissions, collision and load information, simulation models,
  calibration, telemetry, recording, replay, and an offline-first digital
  twin using the same identities and limits as physical hardware.

## Design principles

- **A whole ecosystem, not another hub:** The specification includes the
  controller, motors, sensors, lights, cables, mechanics, software, CAD,
  simulation, test evidence, repair, and conformance.
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

## Reference controller

The first controller combines a replaceable Raspberry Pi Zero 2 W for Linux,
Wi-Fi, Bluetooth, programming, and digital-twin services with a separate
real-time MCU, independent watchdog, and hardware output-disable path. Four
protected native ports support motors and smart peripherals. Linux provides
programmability and networking; the independent controller owns deadline
enforcement, fault handling, and rapid output neutralization.

## Implementation strategy

The shortest implementation path reuses documented commercial components
inside an original open carrier and enclosure. Current screen candidates
include RP2040 safety control, dedicated motor drivers and power protection,
CAN FD data transport, qualified storage, and a provisional commodity
connector. Every choice remains subject to bench, thermal, EMC, lifecycle,
security, and cost qualification.

OMBR does not plan production moulds. It will maintain a bounded,
source-traceable engineering catalog, use independently measured compatibility
geometry where lawful, and publish original parametric parts for accessible
processes such as FDM, resin printing, CNC, or documented service bureaus.

## Commercial reality

The controller-only affordability objective is **EUR 49.99 including German
VAT**, excluding the power supply, battery, charger, external cables, motors,
sensors, lights, and construction elements. Its status is deliberately
**target-unproven**.

The current pre-quote estimate for a complete four-port controller at 1,000
units is **EUR 39-57 COGS** before engineering, certification, legal review,
long-term support, fulfilment, and channel costs. A more realistic initial
direct-sale range is approximately **EUR 65-95 including VAT**. Reaching the
target requires higher volume, connector and BOM cost reduction, manufacturing
partners, measured yield and test time, and carefully amortized compliance
cost. Safety, test evidence, repairability, and open source are not eligible
cost cuts.

## Open and lawful by design

OMBR is independent and is not affiliated with, authorized by, or endorsed by
the LEGO Group. OMBR is a working technical identifier pending trademark
clearance. Compatibility work preserves only necessary functional geometry,
uses independent metrology, and creates original nonfunctional appearance. It
does not copy logos, ornamental housings, or restricted CAD catalogs.

Open licensing does not by itself establish patent freedom to operate. The
selected hub, connector, motor, sensor, gear, protocol, and software revisions
must pass territory-specific trademark, patent, design, licensing, product
classification, cybersecurity, and regulatory gates before commercial
release. Patent-aware contribution terms, provenance records, public decision
logs, and conflict disclosures are part of the platform rather than launch
afterthoughts.

## Delivery path

- **Phase 0 - prove the contracts:** Finalize schemas and governance; validate
  the Pi-to-safety-controller path, protected single-port bench hardware,
  connector candidates, simulator, and safety state machine.
- **Phase 1 - release Hub R0:** Publish an open four-port developer carrier,
  enclosure, firmware, fixtures, SDKs, VS Code integration, and quote-backed
  price book.
- **Phase 2 - establish the ecosystem:** Deliver open motor, position motor,
  RGBW light, distance and reflectance or force sensors, cables, and adapters
  with measured simulation models.
- **Phase 3 and 1.0 - close the physical-digital loop:** Release fit metrology,
  parametric mechanics and gears, the complete Workbench and live twin, then
  qualify a production connector, independent implementations, conformance
  tests, legal dispositions, and applicable product-compliance evidence.

## Partnership opportunity

OMBR is seeking sponsors for prototype hardware, metrology, EMC and thermal
testing, legal review, and open infrastructure; electronics partners for
power, motor, safety, connector, RF, and DFM/DFT engineering; manufacturing
partners for sourcing, pilot builds, test automation, and lifecycle planning;
mechanical contributors for measurements, original CAD, gears, and fixtures;
software contributors for schemas, SDKs, VS Code, .NET, simulation, digital
twins, and visual programming; and educators and makers to define representative
creations and reproducible acceptance tests.

## Executive decision

OMBR is technically achievable with available components, but commercial
credibility depends on disciplined validation, cost-down engineering, and
rights and compliance work. The immediate decision is whether to fund and
staff Phase 0 as a public, evidence-driven reference program. Success would
create a durable common layer for programmable brick automation: open enough
to inspect and reproduce, practical enough to build, and rigorous enough for
independent implementations to work together.

This summary is informative. The normative requirements, evidence status, and
current limitations are defined by the
[OMBR 0.1 specification](OMBR-SPEC.md),
[implementation catalog](IMPLEMENTATION-CATALOG.md), and
[legal/IP register](LEGAL-IP-REGISTER.md).
