<!-- SPDX-License-Identifier: MIT -->

# Open Modular Brick Robotics Specification

Version: 0.1.0-draft

Date: 13 July 2026

Status: Proposal for public review

Short name: OMBR

OMBR is a working technical identifier, not a final product name or
certification mark.

“Robotics” in the working name is historical shorthand. OMBR's technical
scope is modular brick automation: mobile and stationary robots, vehicles,
machines, instruments, kinetic installations, and other programmable
mechanisms use the same open component, mechanical, control, and twin
contracts.

Current draft license: MIT under the repository's [LICENSE.txt](../../../LICENSE.txt).
The patent-aware specification license proposed in section 6.3 is a future
governance decision and does not silently relicense this draft.

## Abstract

This document specifies an open, brick-compatible robotics and automation
ecosystem built around Raspberry Pi Zero-class compute. It covers:

- a Wi-Fi- and Bluetooth-capable controller hub;
- a separate real-time safety controller;
- an open native peripheral interface;
- openly documented motors, lights, sensors, cables, and adapters;
- a brick-grid mechanical interface, open reference-part system, gears and
  power-transmission semantics, and editable component CAD;
- a transport-neutral capability and control model;
- an editor-independent programming, deployment, debugging, and operator
  toolchain;
- an offline-first digital twin linked to CAD and simulation; and
- a maintained implementation catalog that maps requirements to honestly
  graded open designs and documented commercial components;
- an auditable cost model with an aggressive EUR 49.99 base-hub target; and
- licensing, legal/IP, product-regulatory, governance, security, safety, and
  conformance requirements.

The design deliberately separates the open project-controlled hardware from
the Raspberry Pi module and other third-party components. A Raspberry Pi Zero
2 W is a documented and replaceable commercial component, but its silicon,
PCB, and firmware are not completely open hardware. OMBR therefore does not
claim that every atom or upstream dependency is open. It requires every
project-controlled carrier, peripheral, enclosure, protocol, tool, and
manufacturing artifact to be open and reproducible.

This proposal is informed by BrickController2's proven control model:
Creations contain profiles, inputs expose capabilities, profiles bind inputs
to device actions, actuator values can be normalized, and timelines can be
looped or interpolated. OMBR generalizes those ideas into a versioned hardware,
network, programming, CAD, and simulation contract. BrickController2 is an
interim integration vehicle and one possible client, not the required OMBR
development environment or system boundary.

## 1. Design principles

These principles govern every normative profile, implementation decision, and
conformance claim that follows. Where a lower-level requirement appears
ambiguous, it is interpreted consistently with these principles rather than as
an exception to them.

| ID | Principle |
| --- | --- |
| PRIN-001 | Local ownership: the owner can build, repair, reflash, operate, and recover the system without a vendor service. |
| PRIN-002 | Honest openness: third-party closed components are identified rather than hidden behind a whole-product open-hardware claim. |
| PRIN-003 | Safety below Linux: loss, crash, or compromise of Linux cannot prevent a hardware-enforced safe stop. |
| PRIN-004 | One capability model: physical hardware, simulated hardware, BLE, Wi-Fi, and local programs use the same meanings and units. |
| PRIN-005 | CAD is source: editable parametric files and machine-readable interfaces are primary; meshes are derivatives. |
| PRIN-006 | Offline first: internet connectivity extends the system but is never required for core operation. |
| PRIN-007 | Replaceable interfaces: compute, battery, cables, port boards, and wear components are serviceable. |
| PRIN-008 | Measured compatibility: mechanical, electrical, and behavioral claims are backed by fixtures and test reports. |
| PRIN-009 | Safe extensibility: unknown components and fields fail predictably without blocking forward-compatible readers. |
| PRIN-010 | Reproducible evidence: firmware, CAD, simulation, and tests identify exact source and artifact digests. |
| PRIN-011 | Open core before UI: every substantive editor or application workflow is available through public schemas, a documented API, SDK, or headless CLI. |
| PRIN-012 | Tool portability: no canonical project meaning, credential, build step, device identity, or twin state exists only in an editor workspace, private database, hosted service, or binary UI state. |
| PRIN-013 | Complete electronic boundaries: every exposed, cabled, charging, programming, debug, and replaceable-subassembly interface has one versioned, machine-readable contract; connector appearance never substitutes for it. |
| PRIN-014 | Rated operation is distinct from survival: recommended operating, transient, and absolute-maximum values are separately named, and an absolute maximum is never presented as a usable setpoint. |
| PRIN-015 | Current is an envelope, not one headline number: minimum functional or source-load current, sleep, quiescent, idle, nominal, continuous, peak, inrush, current-limit, short-circuit, leakage, and backfeed behavior are distinguished with conditions and duration. |
| PRIN-016 | Compatibility is the safe intersection of source, sink, connector, contact, cable, protocol, environment, and protection limits; the weakest applicable limit wins. |
| PRIN-017 | Energy is staged and contained: high-power rails default off, power direction is explicit, stored and regenerative energy has a discharge path, and one interface fault cannot silently energize or destabilize another domain. |
| PRIN-018 | Returns are part of the circuit: every signal and rail declares its return, grounding, shield, common-mode, isolation, and backfeed model rather than relying on an unlabeled global ground. |
| PRIN-019 | Electrical claims are conditional and measurable: ratings identify temperature, airflow, enclosure, cable, duty cycle, supply impedance, firmware configuration, sample count, uncertainty, and evidence. |
| PRIN-020 | Service access is not a secret interface: test pads, boot straps, programming headers, recovery ports, and factory fixtures that affect owner repair or firmware replacement are documented and safely disabled or authorized in normal use. |
| PRIN-021 | Creation generality: component, control, safety, CAD, and twin semantics MUST describe robots, vehicles, stationary machines, and other automation without assuming locomotion, a chassis, or one robot-shaped root. |
| PRIN-022 | Mechanical authority is explicit: catalog identity, visualization mesh, connection metadata, measured functional geometry, and open-replacement fabrication source are different evidence levels and MUST NOT be substituted for one another. |
| PRIN-023 | Affordable means a complete, dated SKU and channel model: required parts, tax, margin, test, compliance, support, warranty, and exclusions are visible rather than hidden behind a bare-BOM headline. |
| PRIN-024 | Open licensing and technical conformance do not grant third-party patent, design, copyright, database, trademark, or regulatory clearance; each release carries a scoped rights and market-access record. |

## 2. Status and interpretation

Version 0.1 is a design baseline, not a production-ready electrical or toy
safety certification. No version-1 OMBR profile conformance or certification
claim may be made against this draft. Implementations may say only “implements
OMBR 0.1.0-draft” and must identify every unimplemented or experimental area.

The key words MUST, MUST NOT, REQUIRED, SHALL, SHALL NOT, SHOULD, SHOULD NOT,
RECOMMENDED, NOT RECOMMENDED, MAY, and OPTIONAL are to be interpreted as
described by [BCP 14](https://www.rfc-editor.org/rfc/rfc8174) when they appear
in all capitals.

Stable requirement rows such as OPEN-001 and HUB-004 are the future
conformance baseline. Unnumbered BCP 14 statements remain draft requirements
addressable by section and opening phrase; they must receive stable IDs before
their profile becomes claimable. Informative explanations, examples,
component candidates, and roadmaps do not create conformance requirements.

## 3. Vision and scope

OMBR should make a physical brick creation and its digital representation two
views of the same system, whether that creation is a robot, vehicle,
stationary machine, instrument, or installation:

1. design the assembly from brick and project-authored parts;
2. validate fit, wiring, mass, joints, and capabilities;
3. program behavior in ordinary languages with portable SDKs;
4. simulate the same actuator and sensor contract used by the hardware;
5. deploy the creation, programs, and control profiles to the hub;
6. operate locally through a gamepad, editor, standalone application, CLI, or
   on-device program; and
7. synchronize calibrated and observed state back to the digital twin.

The ecosystem should feel like a modular maker platform: stable interfaces,
self-describing peripherals, source files beside every product, multiple
programming levels, and no required vendor cloud. It should additionally fit
brick and studless beam constructions and preserve the repeatable geometry
needed for CAD.

### 3.1 Primary use cases

- remote-controlled ground, rail, water, and other vehicles with proportional
  propulsion, steering, braking, and auxiliary functions;
- autonomous or teleoperated robots running Python, .NET, C/C++, ROS, or
  visual programs;
- stationary machines such as conveyors, sorters, lifts, cranes, plotters,
  machine tools, test rigs, and packaging or material-handling demonstrators;
- programmable mechanisms, kinetic art, model infrastructure, laboratory and
  classroom automation, and instrumented physical experiments;
- one workspace for source, device discovery, build, deploy, debug, remote
  control, CAD-linked twin inspection, simulation, and trace comparison;
- classroom and maker projects that work without an internet connection;
- reusable motor, light, and sensor modules from multiple manufacturers;
- live telemetry, calibration, recording, and replay;
- CAD-first assembly, collision checking, and build instructions;
- software-in-the-loop and hardware-in-the-loop simulation; and
- adapters for selected legacy brick-motor ecosystems.

### 3.2 Non-goals

- cloning a proprietary product housing, logo, or ornamental design;
- claiming affiliation with or certification by the LEGO Group;
- making Linux the sole real-time motor or safety controller;
- requiring a hosted account, telemetry service, or public cloud;
- promising compatibility with every mechanically similar legacy connector;
- treating a rendered mesh as editable mechanical source;
- making version 0.1 suitable for safety-critical, road-going, aviation,
  medical, production-industrial, or other regulated control; or
- claiming a commercial product is certified merely because its Pi module is.

## 4. Terms and object model

| Term | Meaning |
| --- | --- |
| Hub | A networked controller that hosts compute, storage, control orchestration, port management, and local APIs. |
| Safety controller | The real-time MCU that owns watchdogs, output enables, power limits, and deterministic stop behavior. |
| Port | A physical connection point with independently described electrical, data, and safety limits. |
| Peripheral | A port-connected module. It may be an actuator, sensor, light, adapter, or port expander. |
| Component model | A versioned design shared by every manufactured instance of a component. |
| Component instance | A physical or simulated occurrence with owner-resettable identity, revision, and calibration. |
| Capability | A typed property, action, or event with units, bounds, access rules, and quality metadata. |
| Creation | A user-owned assembly graph, control configuration, asset set, and twin configuration for a robot, vehicle, stationary machine, instrument, installation, or other automated mechanism. |
| Mechanical reference catalog | A versioned description of an external or OMBR-native part source, its authority, license, coverage, snapshot, identifiers, and limitations. |
| Mechanical mating profile | A versioned functional geometry and behavior contract for a connection or transmission interface such as a stud, pin, axle, hole, gear mesh, rack, joint, wheel, or track. |
| Implementation candidate | An exact open design, open-source package, or documented COTS component evaluated against named OMBR requirements; inclusion is not endorsement or qualification. |
| Offer observation | Dated evidence that a particular seller offered a particular SKU, condition, quantity, region, tax/delivery basis, and lead time at a stated price. It is not a property of the underlying design. |
| Landed COGS | The recurring cost of a tested, packaged unit ready for the stated sales channel, including procurement, conversion, assembly, yield, test, inbound logistics, duty, and required amortized items defined by the cost profile. |
| Control source | A gamepad, UI, program, sensor, timeline, or network client that produces input capabilities. |
| Binding | A versioned rule mapping input capability values to actions or desired properties. |
| Digital twin | Linked design, as-built, calibrated, simulated, and runtime representations of a creation. |
| Safe state | The documented output condition entered after timeout, fault, stop, boot, shutdown, or power loss. |
| Native source | The preferred editable representation used to modify an artifact. |
| Derived artifact | A generated exchange, visualization, manufacturing, or deployment output. |
| Program descriptor | A portable declaration of source, runtime, entry point, permissions, dependencies, resources, lifecycle, and execution target. |
| Developer service | A headless, user-owned process that exposes discovery, pairing, validation, build, deployment, simulation, debugging, twin, and control operations to tools. |
| Tool client | A Visual Studio Code extension, standalone application, CLI, CI job, notebook, or other consumer of the public developer and hub APIs. |
| Execution target | The declared place where a program runs: hub, workstation, simulator, or an explicitly supported peripheral runtime. |
| Interface contract | The immutable, versioned connector, contact, pinout, protocol, electrical, timing, environmental, protection, and evidence definition shared by compatible endpoints. |
| Operating range | Values over which all declared functions and accuracy are guaranteed under the stated conditions. It is narrower than transient-survival and absolute-maximum limits. |
| Absolute maximum | A non-operating stress boundary whose exceedance may cause permanent damage; operation at or near it is not implied. |
| Current envelope | The role- and condition-specific minimum, consumption, capacity, transient, protection, fault, leakage, and backfeed currents for one rail at one interface. |

## 5. System architecture

~~~mermaid
flowchart LR
    subgraph Tools["Open developer toolchain"]
        VSC["Visual Studio Code extension"]
        APP["Cross-platform .NET application"]
        CLI["ombr CLI / CI / automation"]
        DEV["Developer service (ombrd) + shared SDK"]
        VSC --> DEV
        APP --> DEV
        CLI --> DEV
    end

    subgraph Design["Source, design, and simulation"]
        CODE["Python / .NET / other program source"]
        CAD["Parametric CAD and brick assembly"]
        PKG["OMBR project package"]
        SIM["Pinned simulator / optional ROS"]
        CODE --> PKG
        CAD --> PKG
        PKG <--> SIM
    end

    subgraph Hub["OMBR hub"]
        API["BLE discovery + HTTPS + MQTT"]
        RUNTIME["Sandboxed user-program runtime"]
        ORCH["Creation, profiles, timelines"]
        TWIN["Twin store and event log"]
        PI["Raspberry Pi Zero 2 W"]
        MCU["Real-time safety MCU"]
        POWER["Protected power and port switches"]
        PI --> API
        PI --> RUNTIME
        PI --> ORCH
        PI --> TWIN
        RUNTIME --> API
        ORCH --> MCU
        MCU --> POWER
    end

    subgraph Modules["Open peripherals"]
        MOTOR["Motor / position actuator"]
        LIGHT["RGBW / light module"]
        SENSOR["Distance / color / IMU / force sensor"]
        ADAPTER["Legacy-port adapter"]
    end

    DEV <--> PKG
    DEV <--> SIM
    DEV <--> API
    API <--> TWIN
    POWER <--> MOTOR
    POWER <--> LIGHT
    POWER <--> SENSOR
    POWER <--> ADAPTER
    MOTOR --> TWIN
    LIGHT --> TWIN
    SENSOR --> TWIN
~~~

The public project model, APIs, SDK, and CLI are the product boundary. Visual
Studio Code is the primary reference workbench; a standalone .NET client and
other tools are peers over the same contracts. The reference developer
service owns long-lived workstation device sessions and keeps native USB,
Bluetooth, credentials, simulation processes, and build tools out of editor
webviews. It is independently installable and is not required when another
client implements the public APIs directly.

The Pi owns networking, storage, sandboxed user programs, profiles, API
translation, package deployment, and twin synchronization. The safety MCU owns output
enable, watchdogs, hard deadlines, current and temperature faults, and the
ability to remove motor power without Linux cooperation. Each active
peripheral adds its own local watchdog and protection.

### 5.1 Planned conformance profiles

Conformance will be modular after the relevant profile is frozen. No profile
in this table is claimable in version 0.1.

| Profile | Principal requirement groups | Additional evidence | Draft blocker |
| --- | --- | --- | --- |
| OMBR-HUB-1 | OPEN, HUB, SAFE, ELEC, applicable PWR, NET, API, SEC, SW | Hub, selected power-source, radio, safety, and recovery rows in section 16.4 | Fail-safe stop circuit, BLE/API contract, stable power profile, and applicable product classification |
| OMBR-POWER-1 | OPEN, ELEC, PWR, applicable SAFE and SEC | Source, battery/charging when present, protection, energy, thermal, service, and lifecycle rows | Frozen chemistry/source classes, fixtures, limits, regulatory plan, and fault-energy evidence |
| OMBR-LINK-1 | OPEN, ELEC, LINK, PER, TIME, SEC | Wire, electrical, cable, fault, and interoperability rows | Production connector, physical transport/topology, sender authentication or isolation, framing, timing, and version negotiation |
| OMBR-MECH-1 | OPEN, MECH, PKG, TWIN | Mechanical and manufacturing rows | Metrology dataset, normative geometry, gauges, force windows, lifecycle, and conditioning |
| OMBR-MOTOR-1 | OPEN, ELEC, PER, MOTOR, TIME | Motor, electrical, thermal, lifetime, and HIL rows | Open-loop and feedback subprofile fixtures and pass limits |
| OMBR-LIGHT-1 | OPEN, ELEC, PER, LIGHT | Optical, electrical, thermal, and effects rows | Calibration and pass limits |
| OMBR-SENSOR-1 | OPEN, ELEC, PER, SENSOR | Calibration, uncertainty, timing, environment, and simulation rows | Sensor-class fixtures and pass limits |
| OMBR-TWIN-1 | OPEN, PKG, TWIN, SEC | Schema, archive, identity, replacement, and round-trip rows | Split design/deployment/runtime schemas and golden package |
| OMBR-SIM-1 | OPEN, PKG, TWIN, SIM | Pinned-run and parity rows | Mandatory fidelity tiers, metrics, and maximum error bounds |
| OMBR-ADAPTER-1 | OPEN, ELEC, ADAPTER, LINK, PER and applicable role profile | Compatibility, isolation, misconfiguration, translation, and legacy-device matrix | Endpoint-specific fixtures, rights review, safe power translation, and independently measured combinations |
| OMBR-CONTROL-1 | OPEN, INPUT, CTRL, TIME, and applicable ELEC, API, and SEC | Control-source descriptors, calibration, mapping, loss, lease, feedback, and input fixtures | Frozen input descriptors, handheld/generic-HID profile, latency limits, and two-source mapping vectors |
| OMBR-WORKBENCH-1 | OPEN, DEV, API, PKG, TWIN, SIM, SEC, SW | Golden project through headless CLI plus VS Code and an independent client | Versioned developer API/CLI, client capability matrix, golden workflows, and offline packages |
| OMBR-RUNTIME-1 | OPEN, PROG, API, TIME, SEC, SW | Reproducible build, permission, deploy, lifecycle, failure, log, and debug rows | Runtime descriptors, sandbox profile, SDK contracts, and Python/.NET golden programs |
| OMBR-ECOSYSTEM-1 | Every applicable profile plus ECO, CAT, IP, REG, GOV, and BENCH | Complete public source release, implementation catalog, rights/market-access register, governance evidence, system integration matrix, and benchmark catalog | All constituent profile blockers plus independently reproduced end-to-end starter system |

The requirement-group column is orientation for this draft, not a computable
claim. GOV-013 requires every stable profile to replace it with an exact,
machine-readable claimant/applicability/evidence matrix.

P0 in section 8.8 is an engineering prototype only. OMBR-LINK-1 is blocked
by the connector, transport/topology, authenticated-session or isolation
choice, and exact framing/timing/version rules—not by the connector alone.
`OMBR-AFFORDABLE-HUB-1` is a dated market/price claim governed by COST rather
than a timeless technical conformance profile. Passing OMBR-HUB-1 does not
establish the price claim, and missing the price target does not permit an
implementation to weaken HUB, SAFE, ELEC, OPEN, IP, or REG requirements.

### 5.2 Whole-ecosystem completeness

OMBR specifies a complete programmable brick-automation ecosystem, not only
a robot, hub, editor, or connector. Its scope spans the controller and battery;
ports, plugs, and cables; sensors; lights and motion actuators; brick-grid
structural parts, connectors, gears, transmissions, mounting, and source CAD;
firmware and user-program runtimes; developer and operator tools; the digital
twin and simulator; manufacturing, calibration, test, repair, and teaching
material; and the governance needed for independent implementations. A
Creation may move through its environment or remain fixed; neither locomotion
nor a single chassis is required.

~~~mermaid
flowchart TB
    RELEASE["Content-addressed open ecosystem release"]

    subgraph Physical["Physical product family"]
        INPUT["Handheld / gamepad / accessible control source"]
        POWER["Battery, charger, or supervised bench source"]
        HUB["Pi hub + safety MCU + protected ports"]
        CABLE["Open connectors, cables, extenders, adapters"]
        ACT["Open-loop and feedback motion actuators"]
        LIGHT["White, RGBW, and addressable lights"]
        SENSOR["Distance, color, touch/force, IMU, other sensors"]
        MECH["Bricks, beams, pins, axles, gears, joints, shells, cable routes"]
        INPUT -.-> HUB
        POWER --> HUB
        HUB --> CABLE
        CABLE --> ACT
        CABLE --> LIGHT
        CABLE --> SENSOR
        MECH --- HUB
        MECH --- ACT
        MECH --- LIGHT
        MECH --- SENSOR
    end

    subgraph Software["Open software and digital engineering"]
        FW["Bootloaders, firmware, hub services, program runtimes"]
        TOOLS["SDK + developer service + CLI + VS Code/.NET clients"]
        TWIN2["CAD + part library + digital twin + simulator"]
        FW <--> TOOLS
        TOOLS <--> TWIN2
    end

    subgraph Lifecycle["Reproducible lifecycle"]
        MAKE["BOMs, manufacturing, fixtures, calibration, benchmarks"]
        CARE["Docs, teaching, repair, spares, recovery, end of life"]
        GOV2["Open governance, licensing, registries, security, archives"]
    end

    RELEASE --> HUB
    RELEASE --> FW
    RELEASE --> MAKE
    RELEASE --> CARE
    RELEASE --> GOV2
~~~

| ID | Requirement |
| --- | --- |
| ECO-001 | A complete ecosystem release MUST identify the exact hub, power, native link, cable, mechanical, motor or actuator, light, sensor, runtime, developer-tool, twin, and simulation profiles it implements, plus the exact implementation-catalog snapshot, price-book/cost status, rights/market-access register, documentation artifacts, and conformance-suite versions it contains. |
| ECO-002 | The reference starter system MUST include a buildable hub with IMU, protected power source or bench-power profile, at least two qualified cable lengths, one open-loop motor path, one feedback motion actuator, one controllable light, distance or proximity, color or reflectance, and touch or force sensors, an open handheld control source or documented generic-HID gamepad profile, brick-compatible mounting source, hub runtime, CLI/SDK, Workbench, simulator, and end-to-end example Creation. |
| ECO-003 | Constituent components MUST remain independently replaceable and implementable. An aggregate ecosystem claim MUST NOT hide a failed, proprietary, experimental, or untested constituent profile. |
| ECO-004 | One content-addressed ecosystem release manifest MUST tie together compatible hardware, firmware, schemas, SDKs, tools, CAD, calibration, simulation models, test suites, documentation, and known limitations. |
| ECO-005 | A complete reference workflow MUST build, validate, simulate, deploy, operate, recover, and export without a vendor account, proprietary cloud, private registry, or editor marketplace. |
| ECO-006 | The project MUST publish a versioned compatibility matrix covering model and revision combinations, cables, power profiles, firmware, APIs, SDKs, tools, CAD formats, and simulator versions; an untested combination MUST be marked unknown rather than compatible. |
| ECO-007 | Every released component family MUST publish lifecycle state, repair and replacement path, known errata, supported revisions, end-of-life notice, and a migration or independently implementable substitute where feasible. |
| ECO-008 | Reference tutorials, classroom material, example programs, build instructions, and safety guidance MUST be openly licensed, exportable, printable, and usable offline. |
| ECO-009 | Ecosystem documentation and reference clients MUST include keyboard operation, non-color-only state cues, machine-readable text alternatives, localization-ready strings, and declared accessibility limitations. |
| ECO-010 | System integration evidence MUST exercise real hub, cable, actuator, light, sensor, program, Workbench, and twin/simulation paths together; isolated component tests alone are insufficient for OMBR-ECOSYSTEM-1. |
| ECO-011 | Before a stable ecosystem profile, at least one independently built physical peripheral and one independent CLI or application client MUST complete the public end-to-end test without project-private information. |
| ECO-012 | Compatibility with third-party brick elements MUST be factual, narrowly scoped, measured, and legally distinct from project ownership, endorsement, or certification. |

## 6. Openness and release contract

### 6.1 Scope

| ID | Requirement |
| --- | --- |
| OPEN-001 | Every project-controlled hardware design MUST be released in the preferred editable form for modification. |
| OPEN-002 | PCB releases MUST include native schematics and layout, symbols and footprints, BOM with manufacturer part numbers, approved alternates, Gerbers, drills, pick-and-place, assembly drawings, and production-test files. |
| OPEN-003 | Mechanical releases MUST include native parametric CAD, STEP exchange, printable 3MF, dimensioned drawings, datums, tolerances, material assumptions, and test gauges. STL MAY be included but MUST NOT be the only source. |
| OPEN-004 | Firmware and software releases MUST include source, build instructions, dependency locks, reproducible-build metadata, SBOM, protocol tests, and recovery instructions. |
| OPEN-005 | Motor releases MUST include motor, winding, gearing, encoder, output geometry, torque-speed-current, backlash, thermal, and lifetime characterization or explicitly identify a third-party motor cartridge. |
| OPEN-006 | Cable releases MUST include connector drawings and part numbers, contact and tooling data, conductor gauge, insulation, twist or shielding, strain relief, pinout, continuity tests, and rated current assumptions. |
| OPEN-007 | Sensor and light releases MUST include calibration procedure, raw characterization data, uncertainty or tolerance, environmental limits, and test fixtures. |
| OPEN-008 | Each release MUST identify every third-party or COTS boundary and preserve upstream license and attribution metadata. |
| OPEN-009 | Manufacturing outputs MUST be generated from or traceable to the released editable source at the same revision. |
| OPEN-010 | No required artifact MAY be restricted by a no-commercial-use or no-derivatives term. |
| OPEN-011 | Required project-controlled builds MUST use publicly obtainable, redistributable build tools and documented command-line procedures. Any unavoidable proprietary programming, manufacturing, or test-tool dependency MUST be identified as a non-open boundary with an open replacement plan. |
| OPEN-012 | A release MUST pin the source revision, build environment, dependencies, inputs, parameters, and expected digests, and MUST publish independent rebuild results or explicitly mark each non-reproducible artifact and its cause. |
| OPEN-013 | Programming adapters, bootstrapping tools, factory fixtures, test firmware, calibration rigs, gauges, and pass/fail procedures needed to reproduce a project-controlled product MUST be released with the product source. |
| OPEN-014 | Release artifacts MUST include a machine-readable SBOM, signed checksums, and verifiable build provenance. Trust MUST remain owner-configurable; a project signing service MUST NOT become the only key an owner can authorize. |
| OPEN-015 | Schemas, SDKs, tool packages, firmware, CAD dependencies, part metadata, and documentation MUST support content-addressed offline mirroring and verification. Disappearance of the primary website MUST NOT make an existing release unbuildable. |
| OPEN-016 | A proprietary application store, editor marketplace, hosted package registry, or vendor account MUST NOT be the sole distribution path for any required tool or artifact. |
| OPEN-017 | Hardware releases MUST include exploded views, fastener and tool lists, non-destructive disassembly, replaceable wear-item and battery instructions, diagnostic procedures, spare-part source, and end-of-life material guidance. |
| OPEN-018 | Normative documentation MUST be available in an open, diffable text source plus stable rendered output; diagrams and datasets MUST include accessible descriptions and machine-readable source. |
| OPEN-019 | Public interfaces MUST be independently implementable without copying reference implementation code, accepting click-through terms, obtaining a trademark license, or reverse-engineering undisclosed behavior. |
| OPEN-020 | Stable project-controlled reference hardware SHOULD obtain and publish an OSHWA certification identifier when eligible. OSHWA certification is evidence of openness, not a substitute for OMBR safety, interoperability, or performance conformance. |
| OPEN-021 | Before implementing a project-controlled subsystem, the project MUST publish an open prior-art and reuse ledger covering maintained open hardware/software candidates, exact source and revision, artifact-level licenses, patent or contributor terms, build status, maintenance, security, reusable scope, incompatibilities, and adopt/adapt/reject rationale. |
| OPEN-022 | Adopted or forked upstream source MUST retain history and attribution where available, record the exact delta, preserve upstream licenses and notices, track security and releases, and document whether generally useful changes were offered upstream. |
| OPEN-023 | A maintained, license-compatible open component SHOULD be reused or extended when it meets safety, performance, lifecycle, and architecture needs. A new implementation requires a recorded reason; novelty alone is not one. |
| OPEN-024 | The reuse ledger MUST distinguish open source, open hardware, open specification, source-available, redistributable binary, public documentation, and proprietary COTS. Public access or a Git repository alone MUST NOT be labeled open. |
| OPEN-025 | Before freezing a connector, attachment geometry, adapter protocol, dataset, product name, logo, or commercial reference design, the project MUST record qualified territorial review of relevant patents, registered designs or trade dress, trademarks, copyright and database licenses, clean-room evidence, and required mitigations. Measurement or clean-room work MUST NOT be presented as legal clearance by itself. |

These requirements follow the
[Open Source Hardware Definition](https://oshwa.org/definition/) and
[OSHWA sharing best practices](https://oshwa.org/resources/sharing-best-practices/),
which distinguish editable design source from manufacturing exports.
The optional certification target in OPEN-020 follows the
[OSHWA Certification Requirements](https://certification.oshwa.org/requirements.html).

### 6.2 Raspberry Pi boundary

The reference hub uses a Raspberry Pi Zero 2 W as a replaceable COTS module.
Its official brief documents a 65 by 30 mm board, a quad-core 1 GHz
Cortex-A53, 512 MB RAM, 2.4 GHz Wi-Fi, Bluetooth 4.2/BLE, USB OTG, and a
40-pin GPIO footprint. See the
[product brief](https://datasheets.raspberrypi.com/rpizero2/raspberry-pi-zero-2-w-product-brief.pdf).

The official portal publishes a reduced schematic and mechanical drawing, not
complete editable PCB and manufacturing source. A conforming release MUST
therefore say:

> The OMBR carrier, enclosure, peripherals, protocols, firmware, and tools are
> open source hardware/software as individually licensed. Raspberry Pi Zero
> 2 W and identified semiconductor components are replaceable third-party
> dependencies and are not claimed as project-authored open hardware.

The compute boundary MUST be documented well enough to permit a future
alternative Linux module without changing peripheral or twin semantics.

### 6.3 Target licensing policy

The existing BrickController2 code remains under its current MIT license.
Relicensing it requires agreement from the relevant rights holders.

For new OMBR repositories and artifacts, the project SHOULD adopt:

| Artifact | Recommended license |
| --- | --- |
| Specification text and governance | Community Specification License 1.0 |
| New schemas, validators, tooling core/CLI, generated SDKs, Visual Studio Code extension, alternative .NET client, simulators, conformance tools, and reference software | Apache-2.0 |
| PCB, cable, enclosure, motor, light, and sensor design source | CERN-OHL-W-2.0 |
| Tutorials, build manuals, and non-normative diagrams | CC BY 4.0 |
| Deliberately unencumbered examples and generated fixtures | CC0-1.0 where appropriate |

The choice of
[CERN-OHL-W-2.0](https://ohwr.org/licences/) permits reciprocal improvement
of covered design source while allowing separately licensed external material
through documented interfaces. Specification governance SHOULD include
royalty-free contributor patent commitments. Every artifact MUST carry
machine-readable SPDX metadata; binary CAD SHOULD use REUSE sidecar metadata.

The initial release-engineering target is byte-for-byte reproducibility under
the [Reproducible Builds definition](https://reproducible-builds.org/docs/definition/),
[SLSA 1.2](https://slsa.dev/spec/v1.2/) build provenance, and an
[SPDX 3.0](https://spdx.dev/use/specifications/) SBOM. A release records the
exact standard versions and claimed level; merely emitting an attestation does
not prove that its contents or builder are trustworthy.

### 6.4 Open prior art and upstream-first engineering

The initial reuse ledger investigates these precedents without preselecting
them as dependencies:

| Area | Open precedent to evaluate | Reusable idea |
| --- | --- | --- |
| Brick firmware and programming | [Pybricks](https://pybricks.com/learn/intro/story-mission/) | Open firmware, Python APIs, multi-hub semantics, and offline-capable programming workflow |
| Linux brick runtime | [ev3dev](https://www.ev3dev.org/) | Debian-based hub environment, motor/sensor driver model, and BrickPi ecosystem precedent |
| Visual education tooling | [Open Roberta Lab](https://www.open-roberta.org/about/) | Open visual programming, education workflows, multi-robot abstraction, and export requirements |
| Pi peripheral split | Raspberry Pi Build HAT firmware and protocol | Pi plus real-time MCU architecture and documented low-level link |
| Electronics and mechanics | M5Stack hardware files, OSHWA-certified projects, KiCad, FreeCAD, and OpenSCAD ecosystems | Modular source releases, editable EDA/CAD, fixtures, and community manufacturing |
| Brick CAD | LDraw, LeoCAD, and compatible open tooling | Part identity, assembly exchange, instructions, and attribution models |
| Robotics and simulation | ROS 2, Gazebo, SDFormat, and URDF | Device-independent messaging, frames, simulation assets, plugins, and test scenarios |
| Developer integration | Code - OSS, LSP, DAP, JSON Schema, and open CLI conventions | Replaceable editor UI, diagnostics, tasks, debugging, and automation |
| Supply-chain metadata | REUSE, SPDX, SLSA, and Reproducible Builds | Machine-readable licensing, SBOM, provenance, and independent verification |

Each ledger entry records a specific repository/release and evidence. A project
name in this table does not establish compatible licensing, current
maintenance, safety, reproducibility, or fitness for OMBR.

### 6.5 Legal, intellectual-property, and market-access gates

OMBR defines technical and documentation requirements. It does not grant
rights in third-party patents, designs, copyrights, databases, or trademarks,
does not certify freedom to operate, and is not legal advice. Conformance does
not imply affiliation with or endorsement by any third party. Implementers are
responsible for market-, product-, revision-, use-, and date-specific clearance
and for applicable safety and regulatory compliance.

The public [legal/IP register](LEGAL-IP-REGISTER.md) is a screening and
governance template, not a legal opinion. Privileged counsel work may remain
confidential while the public register records scope, evidence IDs, decision,
mitigation, date, owner, and residual risk. Public patent, trademark, and
design databases help find risks but cannot establish that no relevant right
exists or that a particular act is permitted.

| ID | Requirement |
| --- | --- |
| IP-001 | Every reference product and material revision MUST have a rights register scoped by exact design/firmware/data revision, intended acts (`make`, `use`, `sell`, `offer`, `import`, `distribute`, or `modify`), launch window, target countries, sales channel, age/use classification, and reviewed features. A project-wide or permanent “cleared” flag is prohibited. |
| IP-002 | Rights gates MUST run at concept, architecture freeze, design freeze, prelaunch, and every material change. Outcomes MUST use `no-issue-identified-not-clearance`, `monitor`, `redesign`, `seek-license`, `obtain-counsel-opinion`, or `stop`; unresolved high-impact items block the affected release or feature. |
| IP-003 | Patent/FTO screening MUST decompose at least the controller architecture, power and motor drive, port/connector/contact system, cable, motor and sensor interfaces, brick/beam mating geometry, gear/transmission features, protocols, firmware behavior, digital-twin synchronization, and CAD/simulation workflow into searchable features. It MUST cover keywords, IPC/CPC classes, applicants/inventors, patent families, published applications and grants, claims, priority/expiry, current official territorial status, and a recorded disposition. Qualified counsel review is REQUIRED before commercial market launch. |
| IP-004 | Patent records MUST retain family and jurisdiction, applicant/assignee, priority/publication/grant dates, relevant independent claims, claim-chart evidence ID, legal-status source and date, fees/expiry/opposition information where relevant, reviewer, mitigation, and next review. An expired family in one country MUST NOT imply freedom in another country or under another family member. |
| IP-005 | The OMBR name, logo, certification marks, domains, package namespaces, product names, and presentation MUST receive word/device-mark clearance for the relevant markets and classes before public commercial use. Third-party marks including LEGO, TECHNIC, and MINDSTORMS MUST NOT be incorporated into those identifiers or used as badges, logos, stylized decoration, or source indicators. |
| IP-006 | Third-party marks MAY appear only as minimally necessary plain-text factual references in compatibility, procurement, benchmark, or historical records after review. The claim MUST identify the exact product/interface and revision, separate mechanical, electrical, protocol, and behavioral compatibility, cite tests, attribute the owner, and state non-affiliation nearby. A disclaimer does not cure confusing or otherwise improper use. |
| IP-007 | Every exterior and compatibility-critical geometry MUST receive target-market registered/unregistered design and trade-dress review. The engineering file MUST map each retained interface feature to its objective function, alternatives considered, independent measurement evidence, and CAD history; shells, colors, ornament, cavities, and nonfunctional geometry MUST be independently designed. “Must fit,” modularity, repair, or technical function MUST NOT be treated as automatic clearance. |
| IP-008 | LEGO/BrickLink sites, instructions, catalog images, Studio assets, and other restricted sources MUST NOT be scraped, bulk mirrored, extracted, or redistributed as the OMBR library. Sparse factual aliases MAY be recorded with source and date. Release assets MUST use project-authored renders/photos/metrology/CAD or individually licensed material with machine-readable attribution and redistribution rights. |
| IP-009 | Every imported CAD, LDraw, data, image, document, software, and model artifact MUST record exact source URL/revision/hash/date, author or holder, per-artifact license/terms, attribution, modifications, use basis, redistribution/cache status, and limitations. CI MUST block required release assets with unknown, noncommercial, no-derivatives, field-of-use, revocable/tool-only, or otherwise incompatible terms. |
| IP-010 | Protocol interoperability research MUST record lawful access, authorized acts, information unavailable from the rights holder, exact interoperability purpose, necessity and minimization, jurisdiction, reviewer separation where used, and independently authored outputs. It MUST NOT publish extracted firmware/source, comments, keys, protected assets, or security bypasses. Repair or interoperability exceptions MUST NOT be assumed across territories. |
| IP-011 | Open-source and open-hardware licenses MUST be reviewed separately from third-party FTO. The artifact manifest MUST preserve holders, notices, patent clauses, source obligations, reciprocal scope, generated-artifact status, and dependency compatibility. A supplier's public datasheet or SDK does not license its silicon, PCB, trademarks, patents, or reference-design artwork beyond its actual terms. |
| IP-012 | Contribution terms for normative text, schemas, reference software, hardware, CAD, and test data MUST provide the copyright and patent permissions needed by the selected specification and artifact licenses. Trademark permission and conformance certification remain separate, published policies. |
| IP-013 | A clean-room or independent-measurement process MUST preserve acquisition provenance, researcher and implementer roles, dated notes, input/output boundaries, independent design history, and tests. It reduces copying risk but MUST NOT be described as patent, design, trademark, contract, database, or overall legal clearance. |
| IP-014 | Commercial suppliers and manufacturing partners MUST be reviewed for component authenticity, authorized-channel status, licenses/royalties, restricted-use terms, indemnities, export/import constraints, and notification of lifecycle or material changes. Supplier representations MUST be retained but do not replace implementer due diligence. |
| IP-015 | The reference release MUST publish non-confidential residual risks, prohibited uses, unresolved dependencies, licenses obtained, required attribution/marking, redesign or feature omissions, next-review triggers, and contact for rights concerns. |

Product classification comes before a standards checklist. The same board may
be a 14+ maker controller, education product, radio product, machinery
component, or child-directed toy depending on presentation, intended use,
included parts, and market. Using a pre-certified radio module can reduce test
scope but does not certify the final enclosure, antenna environment, power
system, cables, software, or product.

| ID | Requirement |
| --- | --- |
| REG-001 | Before design freeze, each commercial or distributed reference SKU MUST record intended use and user, age grading, foreseeable misuse, target territories, economic-operator roles, product classification, and an applicability matrix of current laws, delegated acts, standards, guidance, labels, registrations, technical files, declarations, and post-market duties. |
| REG-002 | The EU market plan MUST explicitly assess the Radio Equipment Directive, EMC, electrical safety as applicable, RoHS, REACH, WEEE, batteries and transport when included, General Product Safety Regulation, packaging and consumer-price rules, privacy, the Cyber Resilience Act transition, and toy-safety rules if the product is or may be perceived as a toy. Non-EU releases require their own matrices. |
| REG-003 | Every safety/compliance conclusion MUST identify the exact final-product revision and configuration, standards editions, laboratory or competent reviewer, reports, deviations, critical components, firmware, antenna/enclosure/cable assumptions, and change-control triggers. Upstream module declarations are supporting evidence only. |
| REG-004 | The technical file MUST include risk assessment, schematics/BOM/CAD, software and update architecture, intended/foreseeable use, safety instructions, test reports, supplier declarations, production controls, traceability, incident/vulnerability handling, corrective-action and recall process, and retention period required by the applicable regime. |
| REG-005 | Security and update obligations MUST be planned for the declared support lifetime, including vulnerability intake, triage, coordinated disclosure, authenticated update and recovery, dependency monitoring, reporting deadlines, owner notification, and end-of-support behavior. An open-source publication alone does not discharge a commercial manufacturer's duties. |
| REG-006 | Child-directed or education configurations MUST complete specific chemical, mechanical, electrical, thermal, small-parts, strangulation/cable, battery, misuse, hygiene, accessibility, labeling, and digital-product-passport assessments required for the target date and market. A `14+` label MUST reflect genuine intended use and risk evidence rather than serve as a workaround. |
| REG-007 | Production and procurement change control MUST prevent an alternate component, resin, finish, battery, radio, antenna, connector, cable, supplier, firmware, or enclosure change from silently invalidating safety, EMC/radio, environmental, IP, cost, or conformance evidence. |
| REG-008 | Legal, certification, laboratory, documentation, registration, surveillance, update/support, warranty, recycling, and recall provisions MUST be included in the cost model and release schedule rather than deferred until after the retail target is announced. |

Relevant primary screening sources include the [EPO Espacenet and legal-status
tools](https://www.epo.org/en/searching-for-patents/technical/espacenet),
[WIPO PATENTSCOPE](https://www.wipo.int/en/web/patentscope/), the
[WIPO Global Brand Database](https://www.wipo.int/en/web/global-brand-database),
[EUIPO search](https://www.euipo.europa.eu/en/search-ip), and official national
registers. The EU [Radio Equipment Directive](https://single-market-economy.ec.europa.eu/sectors/electrical-and-electronic-engineering-industries-eei/radio-equipment-directive-red_en)
covers radio safety/health, EMC, and spectrum requirements. The
[Cyber Resilience Act](https://digital-strategy.ec.europa.eu/en/policies/cyber-resilience-act)
has reporting obligations applying from 11 September 2026 and main obligations
from 11 December 2027. The new
[EU Toy Safety Regulation](https://eur-lex.europa.eu/legal-content/EN/TXT/?uri=CELEX:32025R2509)
entered into force on 1 January 2026 and applies from 1 August 2030 after its
transition; the classification and rules actually applicable at the placement
date must be verified.

## 7. Hub requirements

### 7.1 Functional baseline

| ID | Requirement |
| --- | --- |
| HUB-001 | OMBR-HUB-1 MUST use a Raspberry Pi Zero 2 W or a wireless Raspberry Pi Compute Module Zero derived from the Zero 2 W architecture. A non-Pi substitute requires a separately versioned future compute profile. |
| HUB-002 | The replaceable reference implementation MUST support Raspberry Pi Zero 2 W; a compact production variant MAY support a wireless Compute Module Zero. |
| HUB-003 | The compute module MUST be mechanically replaceable in the reference developer hub without replacing the carrier PCB. |
| HUB-004 | A separate real-time safety MCU MUST own power limits and motion control, while a de-energize-to-trip hardware gate and independent watchdog MUST be able to disable every motion-power output even if that MCU is wedged or a normal enable GPIO is stuck. |
| HUB-005 | Linux MUST NOT be capable of bypassing a latched hardware overcurrent, overtemperature, battery, or emergency-stop fault. |
| HUB-006 | The hub MUST contain working Wi-Fi and BLE radios without an external dongle. Version 0.1 requires interoperable Wi-Fi behavior; BLE application interoperability remains a profile blocker. |
| HUB-007 | Core creation playback, physical stop, local API, and peripheral operation MUST work without internet access or a hosted account. |
| HUB-008 | The reference hub MUST provide at least four native peripheral ports; six are the design target. |
| HUB-009 | Every port MUST expose independent presence, power state, current, voltage, fault, descriptor, and safe-state information. |
| HUB-010 | A motor stall or port short MUST NOT brown out the Pi power rail. |
| HUB-011 | The hub MUST include a monotonic clock and SHOULD include an RTC or retained time source for timestamp continuity. |
| HUB-012 | The hub SHOULD include a six-axis IMU whose frame is declared in the component manifest. |
| HUB-013 | The owner MUST be able to enter recovery, restore a documented image, rotate credentials, and install self-built firmware. |
| HUB-014 | The hub MUST provide a physical stop input wired to both the safety MCU for diagnosis and the independent de-energize-to-trip output gate. |
| HUB-015 | The hub MUST visibly indicate at least booting, ready, wireless pairing, output armed, warning, and fault states. |

### 7.2 Reference compute profile

The first reference implementation, **Hub R0**, uses:

- Raspberry Pi Zero 2 W or Zero 2 W with headers;
- 64-bit Raspberry Pi OS Lite or another documented Linux distribution;
- a project-controlled carrier with safety MCU and protected power domains;
- removable storage during development;
- Wi-Fi for local network APIs and twin synchronization;
- BLE for commissioning, nearby discovery, and bounded local control; and
- off-device Workbench, CAD, and full physics simulation.

The 512 MB Pi is reserved for headless control, APIs, sandboxed programs,
telemetry, bounded package storage, and serving pre-generated assets. Full
authoring UI, authoritative CAD editing, source builds, and full physics
simulation are workstation responsibilities rather than hub requirements.

The Raspberry Pi Zero 2 W does not have a documented secure-boot process
equivalent to the Pi 4/CM4 flow described by Raspberry Pi's
[official secure-boot guide](https://pip.raspberrypi.com/categories/685-whitepapers-app-notes-compliance-guides/documents/RP-003466-WP/Boot-Security-Howto.pdf).
OMBR MUST report this limitation. A signed update system on Zero 2 W improves
software supply-chain integrity but does not create an immutable hardware root
of trust against physical microSD replacement.

### 7.3 Real-time safety controller

| ID | Requirement |
| --- | --- |
| SAFE-001 | The MCU MUST start with all high-power outputs disabled. |
| SAFE-002 | The MCU MUST require a valid versioned configuration and active heartbeat before arming outputs. |
| SAFE-003 | Loss of Linux heartbeat MUST enter the configured safe state within 100 ms for motion outputs. |
| SAFE-004 | Each active motion peripheral, or the smart adapter driving a passive motor, MUST independently enter safe state no later than 250 ms after its last valid motion command or heartbeat. |
| SAFE-005 | Stop, watchdog, overcurrent, overtemperature, undervoltage, and invalid-configuration events MUST be latched or explicitly acknowledged according to a published fault table. |
| SAFE-006 | The MCU MUST continue enforcing power and stop behavior while Linux reboots or is absent. |
| SAFE-007 | Communication between Linux and the MCU MUST be versioned, framed, checksummed, and covered by golden vectors and malformed-input tests. |
| SAFE-008 | The MCU MUST expose reset cause, fault cause, armed state, watchdog state, and per-port power state. |
| SAFE-009 | A firmware update MUST leave a recoverable bootloader or known-good image. |
| SAFE-010 | Safety parameters MUST have compile-time or hardware-enforced ceilings that an ordinary network client cannot increase. |
| SAFE-011 | The independent watchdog and output gate MUST default to output-disabled when unpowered, held in reset, unserviced, disconnected, or driven by an invalid level. |
| SAFE-012 | The physical stop loop MUST be normally closed or otherwise line-monitored so an open wire, disconnected switch, or invalid circuit enters the tripped state. |
| SAFE-013 | From a valid physical stop transition to removal of motion-power enable MUST be no more than 20 ms; mechanical stopping time, distance, and energy MUST be characterized separately for the creation. |
| SAFE-014 | Releasing the physical stop or clearing a fault MUST NOT restart motion. Re-arming requires a healthy stop loop, neutral commands, and a new explicit arm action. |
| SAFE-015 | Every actuator profile MUST declare whether safe stop means immediate coast, immediate electrical brake, or locally controlled deceleration followed by power removal, and MUST justify the choice against regenerative energy and mechanism hazards. |
| SAFE-016 | A test MUST demonstrate that a stuck MCU, stuck enable GPIO, crashed Linux host, and broken stop wire each result in de-energized motion outputs within their specified bounds. |
| SAFE-017 | The safety case MUST distinguish electrical output-disable latency from physical stopping performance and MUST NOT imply a safety-rated emergency-stop function without the applicable independent assessment. |

Raspberry Pi's Build HAT is evidence for this split architecture: its
low-level microcontroller handles time-sensitive peripheral functions while
the Pi communicates through a documented serial protocol.

### 7.4 Power architecture

The reference electrical target is a protected removable 2-cell battery pack,
but battery chemistry is not frozen in version 0.1.

Two power paths are distinguished:

- **OMBR-POWER-BENCH-P0** is a supervised, tethered engineering source class using an
  externally certified isolated SELV supply with bounded voltage/current. It
  cannot support a portable, battery, charging, toy, or OMBR-POWER-1 claim.
- **OMBR-POWER-BATTERY-1** is the planned portable source class and includes the
  battery, charging, serviceability, transport, thermal, and lifecycle
  requirements. It is not claimable in 0.1.

OMBR-POWER-1 is the aggregate future conformance profile. A claimant selects
at least one frozen source class and supplies its applicable evidence;
OMBR-POWER-BATTERY-1 is the expected first stable class, while
OMBR-POWER-BENCH-P0 remains prototype-only and cannot satisfy the aggregate
claim.

PWR-001, PWR-005, PWR-006, PWR-008, PWR-010, PWR-011, and PWR-012 through
PWR-015 apply to every hub power source. PWR-002 through PWR-004 and PWR-007
apply when a battery or charger is present. PWR-009 applies to every intended
commercial or child-facing product.

| ID | Requirement |
| --- | --- |
| PWR-001 | Compute, logic, and motor power domains MUST be separately regulated or switched so motor transients cannot violate Pi input limits. |
| PWR-002 | A battery assembly MUST provide per-cell supervision, chemistry-appropriate balancing where required, pack fuse, temperature and current measurement, low-voltage behavior, keyed chemistry/voltage identification, reverse-polarity protection, and defined wrong-pack behavior. |
| PWR-003 | A rechargeable battery profile MUST use a power-path design that defines behavior while operating, charging, shut down, and deeply discharged. |
| PWR-004 | A portable battery MUST be removable and replaceable using documented tools and without destructive disassembly. |
| PWR-005 | Port high power MUST remain disabled until presence and descriptor checks complete. |
| PWR-006 | Each high-power port MUST have independently enforceable current and thermal limits. |
| PWR-007 | A battery-powered hub MUST publish battery voltage, current, state of charge, temperature, health, charging state, and limit/fault state where measurable. |
| PWR-008 | Connector and conductor ratings MUST be derated against measured enclosure temperature rather than copied from a room-temperature headline rating. |
| PWR-009 | Before a commercial or child-facing product claims a stable OMBR profile, it MUST freeze its intended age and markets, complete a documented hazard and foreseeable-misuse analysis, and pass the applicable battery, radio, EMC, electrical, mechanical, thermal, chemical, and cybersecurity requirements. Merely listing failed or incomplete evidence is insufficient. |
| PWR-010 | The auxiliary logic supply for each external port MUST be independently current-limited and protected against short, ESD, reverse input, and backfeed. |
| PWR-011 | The power path and actuator modules MUST keep rail, pack, connector, and driver values within absolute and qualified limits during braking, deceleration, disconnect, full battery, and fault shutdown. The design MUST provide a tested clamp, energy sink, controlled coast, or equivalent safe policy and define pass thresholds for every supported motor class. |
| PWR-012 | Every source profile MUST satisfy the complete ELEC contract and additionally declare chemistry or upstream source class, output regulation, source impedance, capacitance, energy and I-squared-t limits, aggregate power budget, fuse/protection coordination, charging or absorption capability, and source-release or certification evidence. |
| PWR-013 | The bench profile MUST use a current-limited, isolated SELV source whose maximum voltage and available fault energy remain within the bench board's published absolute limits; improvised unprotected supplies are outside the profile. |
| PWR-014 | Source connect, disconnect, brownout, overvoltage, reverse connection, current-limit foldback, and recovery MUST leave high-power outputs disabled until a fresh healthy arm sequence. |
| PWR-015 | The hub MUST calculate or validate simultaneous port, logic, compute, charging, and transient budgets against the identified source and MUST reject an overcommitted configuration before arming. |

Hub R0 targets a grid-aligned enclosure near 88 by 56 by 32 mm before
attachments, rather than the much smaller BuWizz envelope. This is a packaging
target, not a frozen conformance dimension. The Pi board alone is 65 by 30 mm;
room is still needed for walls, antenna clearance, connectors, power
electronics, battery, and strain relief.

### 7.5 Universal electronic interface contract

Every electronic component is specified at its boundaries, not merely by a
marketing name or connector family. This applies to hub ports, batteries,
chargers, power inputs, motors, lights, sensors, adapters, cable ends, radio or
wired control hardware, replaceable compute and port boards, and every exposed
programming, debug, recovery, or factory interface. A purely mechanical or
optical interface explicitly declares that the electronic contract is not
applicable.

IC pins and nets wholly contained on one non-replaceable PCB remain documented
by the released schematic, BOM, datasheet references, layout constraints, and
derating record. Any rail or signal that crosses a separable, serviceable,
accessible, or independently replaceable boundary becomes an interface under
this section.

A component may embed its contract or reference an immutable contract and
endpoint profile contained in the same content-addressed release. The complete
resolved closure MUST remain available offline; a product page, private
database, firmware response, or unversioned “standard connector” label is not
the contract.

Electrical values use SI units and declare direction and conditions. In
particular, “minimum amperage” is not used as an ambiguous field: a source may
have a **minimum stable load**, while a sink may have a **minimum functional
operating current**. Those are distinct from sleep, quiescent, idle, nominal,
continuous-maximum, peak, inrush, protection, fault, leakage, and backfeed
currents. Zero means measured or specified zero within a stated bound; unknown
or not applicable is represented explicitly and never encoded as zero.

| ID | Requirement |
| --- | --- |
| ELEC-001 | Every component model MUST enumerate every external, cabled, charging, replaceable-subassembly, programming, debug, recovery, and factory interface. Each electrical, data, or combined interface MUST resolve to exactly one immutable interface contract and one endpoint electrical profile; a non-electronic interface MUST be marked not applicable by kind. |
| ELEC-002 | An interface contract MUST declare contract ID and revision, profile ID and version, maturity, source artifact and digest, compatible connector variants, complete contact map, protocols, cable constraints, hot-plug model, grounding/isolation model, environmental envelope, and evidence artifacts. |
| ELEC-003 | Every connector variant MUST name manufacturer or project owner, series, exact housing/header/receptacle and contact part numbers, gender, circuit count, pitch, keying, polarization, latch/retention, mating variants, contact material and plating, termination method, supported conductor sizes, required tooling, temperature range, published mating-cycle rating, touch state, source drawing or datasheet, and the view orientation used by its pin numbering. |
| ELEC-004 | Every contact position MUST declare number, name, function, source/sink/bidirectional/passive direction, rail or signal domain, return or differential mate, shield treatment, staged-mate order where present, maximum contact voltage and derated continuous current, reserved/no-connect behavior, and wrong-contact or partial-mate outcome. A diagram and machine-readable map MUST agree. |
| ELEC-005 | Every data interface MUST declare physical and electrical layer, exact protocol and schema versions, topology, addressing, arbitration, termination, nominal and data rates, permitted ranges, clock tolerance, encoding, framing, byte order, maximum frame/payload, startup and version negotiation, timeouts, retry and duplicate rules, flow control, errors, update/recovery behavior, security/session behavior, message schemas, and golden vectors. |
| ELEC-006 | Every powered rail at an endpoint MUST declare polarity and direction; guaranteed operating minimum, nominal, and maximum voltage; absolute minimum and maximum; allowed ripple/noise; dip, surge, and transient magnitude and duration; undervoltage and overvoltage thresholds and hysteresis where implemented; and measurement point and accuracy. Logic pins additionally declare input-low/high, output-low/high, common-mode, fail-safe, and unpowered-pin limits when applicable. |
| ELEC-007 | Every powered rail MUST declare the complete current envelope appropriate to its role: minimum functional sink current, minimum stable source load, shutdown/sleep, quiescent, idle, nominal, continuous maximum, peak maximum with duration and duty cycle, inrush with duration and source-impedance condition, current-limit minimum/nominal/maximum with tolerance and response, short-circuit or fault current with clearing time, leakage, and permitted backfeed. |
| ELEC-008 | A current value MUST state whether it is consumed, delivered, passed through, limited, interrupted, or merely survived. Connector/contact capacity, semiconductor peak current, configured current limit, motor stall current, battery capability, cable ampacity, and continuous product rating MUST NOT be substituted for one another. |
| ELEC-009 | Every endpoint profile MUST declare whether it is a source, sink, bidirectional power-path, or passive pass-through in every power state. Reverse energy, regenerative energy, charging, power-role swap, and backfeed are forbidden unless an explicit negotiated profile and tested energy path permit them. |
| ELEC-010 | Startup, attach, presence detection, precharge, rail sequencing, protocol-ready, arm, normal shutdown, emergency disable, hot plug, partial insertion, cable removal, brownout, reboot, and recovery states MUST declare which contacts and rails may be energized, their timing, and the resulting component state. |
| ELEC-011 | Every rail and signal MUST name its return and isolation domain. The contract MUST declare protective earth, chassis, logic ground, power return, analog reference, cable shield/drain, common-mode range, isolation class, working/test voltage, creepage/clearance basis where applicable, shield termination, ground-loop assumptions, and behavior when one return opens. |
| ELEC-012 | Each endpoint MUST document reverse-polarity, overvoltage, undervoltage, overcurrent, short-circuit, overtemperature, ESD, EFT/burst, surge, miswire, unpowered-pin injection, latch-up, backfeed, and regenerative-energy protection. For every protection, the threshold range, response time, retry/latch behavior, safe state, damage boundary, and reset procedure MUST be stated. |
| ELEC-013 | Bulk and local capacitance, discharge or bleed time, precharge, inductive kick, motor braking, cable inductance, battery/source impedance, and other stored-energy paths MUST be included in transient and fault analysis. Disconnect or output disable MUST NOT leave an undocumented hazardous or damaging voltage. |
| ELEC-014 | A cabled interface MUST declare permitted topology; total and per-segment length; stub length; conductor material and minimum area or gauge; contact and round-trip resistance; capacitance and inductance limits; required twist, shield, drain, and termination; bend and temperature derating; voltage drop; and branch/aggregate current rules. |
| ELEC-015 | Signal-integrity evidence MUST cover worst permitted cable, connector/contact aging, endpoint count, supply range, temperature, clock tolerance, common mode, interference, and topology. Eye, waveform, timing, error-rate, or protocol-margin limits and fixture bandwidth MUST be published as appropriate to the physical layer. |
| ELEC-016 | Every electrical rating MUST declare its environmental and thermal conditions, including ambient and component temperature range, enclosure and airflow, humidity/condensation, altitude if relevant, PCB copper and heat sinking, adjacent loaded contacts, cable bundle, duty cycle, and firmware configuration. Derating curves or tables MUST replace unsupported room-temperature headline ratings. |
| ELEC-017 | Before connection or arming, the hub or validator MUST calculate the safe intersection of source, sink, cable, connector/contact, protocol, configuration, and environmental limits. No descriptor or adapter may raise a hardware, cable, or upstream-source ceiling; an empty or contradictory intersection remains unpowered. |
| ELEC-018 | Every claimed numeric limit MUST be tagged manufacturer-specified, design-calculated, measured, inferred, estimated, or provisional and link its source. Measurement evidence MUST record exact revision and sample, instrument and calibration, fixture, wiring, firmware/configuration, conditions, uncertainty, raw data, analysis, and pass/fail rule. |
| ELEC-019 | Unknown, contradictory, unverified, expired, or not-applicable values MUST be represented explicitly. A required operating, absolute, current, pinout, connector, protocol, or fault value that is unknown or contradictory prevents a stable interface claim and prevents high-power arming unless a separately tested conservative commissioning profile applies. |
| ELEC-020 | Programming, debug, boot-mode, test, and factory interfaces MUST publish connector or pad geometry, pinout, voltage/current and unpowered limits, protocol/tool versions, authorization, normal-use disable state, recovery procedure, and production lock policy. Documentation MUST preserve owner reflashing and recovery without leaving an unauthenticated hazardous control path. |
| ELEC-021 | A change to connector fit/keying, contact map, direction, operating or absolute limit, protocol, timing, power sequence, protection, cable constraint, or safe/fault behavior MUST create a new immutable contract or endpoint-profile revision and compatibility record. Silent electrical changes under one product revision are forbidden. |
| ELEC-022 | The public conformance suite MUST include contract/schema validation plus powered and unpowered mating, wrong/partial insertion, min/nom/max operating points, ripple/transient, inrush, continuous/peak load, current limit, short, reverse, backfeed, brownout, hot plug, thermal derating, ESD/EMC pre-compliance, cable corner, protocol margin, disconnect, and recovery tests applicable to each interface. |
| ELEC-023 | Every COTS semiconductor, protection part, connector/contact, cable material, or module that determines an interface guarantee MUST be identified by manufacturer, exact part and package, datasheet title and revision, critical ratings, approved alternates, and project derating rationale. An alternate that can change a guarantee requires a new calculation and validation record before substitution. |
| ELEC-024 | Analog, PWM, pulse, frequency, and discrete interfaces MUST declare transfer function, units, source/load impedance, reference, range, saturation, filtering and bandwidth, sampling or edge timing, minimum/maximum/neutral pulse values, jitter, calibration, loss-of-signal, and error indication in addition to the voltage/current contract. An informal label such as “PWM” or “servo” is insufficient. |
| ELEC-025 | Voltage, current, power, energy, temperature, and fault telemetry MUST declare measurement point, range, resolution, accuracy and uncertainty, sampling and reporting rate, bandwidth/filtering, latency, saturation, calibration, stale behavior, and whether it is measured, calculated, or estimated. Telemetry MUST NOT be treated as independent protection unless the protection path and worst-case response are separately specified and tested. |
| ELEC-026 | Every interface release MUST include a machine-readable fault matrix for applicable pin-to-pin, pin-to-rail, pin-to-return, open contact/return/shield, reversed or wrong mate, wrong voltage, stalled or overloaded load, stuck data, termination loss, cable short/open, ESD/transient, overheating, and loss of local control. Each row declares detection, bounded voltage/current/energy/time/temperature, containment scope, safe state, diagnostic, latch/retry behavior, and recovery. |
| ELEC-027 | ESD, EFT/burst, surge where applicable, conducted/radiated immunity, and emissions evidence MUST state test level, coupling method, performance criterion, grounding, exact cable and load, operating mode, orientation, failures, and post-test recovery. “Protected” or “EMC tested” without levels and conditions is not evidence. |
| ELEC-028 | A runtime descriptor MAY carry a bounded machine-readable subset for discovery and pre-arm checks, but it MUST identify the immutable full contract and endpoint-profile revision and digest. The validator still verifies exact connector/contact map, voltage after worst-case drop, steady/peak/inrush/regenerative current, cable/contact/protection/thermal capacity, fault energy, ground/common-mode/isolation/shield, protocol role/version/timing/topology/security, and aggregate source budget. |
| ELEC-029 | A stable electronic profile release MUST contain the complete resolved interface contracts, endpoint profiles, source and evidence artifacts, COTS/derating record, fault matrix, semantic compatibility rules, protocol schemas and golden vectors, and at least one tested source-cable-load combination. Prose, a product page, or a connector drawing alone cannot support a full-interface claim. |

## 8. Native peripheral link

### 8.1 Architectural choice

OMBR uses a project-owned native link and optional adapters. It does not make
a proprietary brick connector the foundation of the ecosystem.

The native link carries:

- protected motor or battery power;
- protected low-power logic supply;
- ground;
- a differential data pair;
- presence, wake, or staged-power identification; and
- a shield or drain where the cable design requires it.

The working transport for R0 is CAN FD through licensed, off-the-shelf
controller silicon, with a fully project-owned open application layer.
Classic CAN remains a research option only. It is not permitted on the R0 CAN
FD segment because the draft envelope exceeds an eight-byte Classic CAN frame
and mixed Classic/FD nodes require an explicit isolated-segment and downgrade
design. The final connector also remains open.

I2C is permitted inside a component but MUST NOT be the default external cable
transport. USB-C MUST NOT carry undocumented proprietary motor voltage or
signals that violate the USB-C specification.

### 8.2 Link requirements

| ID | Requirement |
| --- | --- |
| LINK-001 | The physical interface MUST be keyed, touch-safe, polarized, and resistant to accidental shorting. |
| LINK-002 | No externally touchable contact MAY remain energized at motor power before presence and policy checks complete. |
| LINK-003 | The connector MUST define first-mate/last-break behavior or an electrical strategy that removes high power before data/presence disconnect. |
| LINK-004 | The production connector MUST be qualified for the project's expected frequent reconfiguration lifecycle. |
| LINK-005 | The cable and connector MUST survive published insertion, withdrawal, pull, bend, drop, mis-mating, and contamination tests. |
| LINK-006 | The interface SHOULD have at least two qualified manufacturing sources, or openly licensable tooling and a migration plan. |
| LINK-007 | The physical layer MUST use differential signaling for external cables. |
| LINK-008 | The bus MUST define topology, termination, maximum cable length, stub length, bit timing, grounding, shielding, and EMC fixtures. |
| LINK-009 | Each peripheral MUST expose a descriptor before high-power arming. |
| LINK-010 | A descriptor MUST state model ID, hardware revision, firmware revision, roles, capabilities, safe state, calibration revision, and immutable interface-contract and endpoint-profile IDs, revisions, and digests plus the bounded ELEC-028 pre-arm subset. |
| LINK-011 | A hub MUST reject or leave unpowered a component when the safe intersection required by ELEC-017 and ELEC-028 is empty, contradictory, unknown, expired, or exceeds policy or measured capability. |
| LINK-012 | A module MUST NOT backfeed any hub rail unless an explicitly negotiated power-source profile permits it. |
| LINK-013 | Unknown message types and optional fields MUST be ignored or rejected according to the version rules without unsafe side effects. |
| LINK-014 | Every message that can change physical state MUST carry source, sequence, command ID, and the relative validity semantics defined by the TIME requirements. |
| LINK-015 | The application protocol MUST be openly published with byte-level framing, state machines, errors, test vectors, and conformance tests. |
| LINK-016 | Descriptors and peripheral-originated data MUST be treated as untrusted; a peripheral MUST NOT be able to raise hardware safety ceilings, arm another port, or issue actuator commands to another component. |
| LINK-017 | On a shared medium, state-changing messages MUST authenticate the active hub session, or the physical architecture MUST isolate links so one peripheral cannot impersonate the hub to another peripheral. |
| LINK-018 | Bus flooding, duplicate node identity, dominant-bus failure, and malformed traffic MUST lead to bounded safe stop and a documented port-isolation diagnostic procedure. |
| LINK-019 | Stable CBOR payloads MUST use RFC 8949 Core Deterministic Encoding, definite lengths, unique map keys, and schema-defined numeric types; the diagnostic JSON mapping and CDDL or equivalent data definitions MUST ship with golden vectors. |
| LINK-020 | CRC-32C MUST use the Castagnoli polynomial 0x1EDC6F41, reflected processing, initial value 0xffffffff, and final xor 0xffffffff over the exact serialized header and payload bytes. |
| LINK-021 | HELLO MUST list supported major and minor ranges. Peers select the highest shared major and then highest shared minor; no shared major leaves outputs disabled and returns incompatible-version. Unknown state-changing types or nonzero reserved flags are rejected without side effects. |
| LINK-022 | Every cable type MUST have a versioned component and endpoint profile resolving the complete ELEC contract for both ends and declaring contact/conductor mapping, length, conductor gauge and material, twist/shield/drain, resistance, capacitance, inductance, impedance where applicable, propagation delay, current/voltage and fault-withstand limits, bend/flex, strain relief, temperature derating, source release, and validation digest. |
| LINK-023 | A passive cable MUST be identifiable by durable human-readable part and revision marking. Optional electronic identification MUST use the open descriptor protocol and MUST NOT be required to manufacture an otherwise passive compatible cable. |
| LINK-024 | Reference cables MUST be field-replaceable and use documented, obtainable connector/contact systems assemblable with commodity tools or an openly reproducible connector process that does not require undisclosed custom tooling. Cable releases MUST include assembly, continuity, pull, voltage-drop, and load-test fixtures. |
| LINK-025 | The hub MUST keep high power disabled when the installed or configured cable type is absent, incompatible, underrated, or contradictory to endpoint limits; a cable declaration MUST NOT raise a physical port ceiling. |
| LINK-026 | Extension, splitter, hub, and adapter cables MUST declare topology, cumulative voltage drop, termination, branch and total current limits, hot-plug behavior, and fault isolation; a passive Y cable MUST NOT be assumed safe for a point-to-point profile. |

CAN and CAN FD do not provide sender authentication. R0 bench framing therefore
does not yet satisfy LINK-017 on an untrusted shared bus. Before OMBR-LINK-1 is
frozen, the reference design MUST select and test either per-port physical
isolation or a compact authenticated-session construction with replay
protection and owner-recoverable keys. This limitation is also documented in
[CAN in Automation's security overview](https://can-cia.org/services/publications/can-community-news/09-2025).

### 8.3 Node identity

A BLE address, Wi-Fi MAC, USB path, or bus node number MUST NOT serve as the
persistent twin identity.

Each component exposes:

- **modelId**: stable URI for a design;
- **modelRevision**: immutable released design revision;
- **instanceId**: owner-resettable UUID URN used by local APIs;
- **factoryId**: optional private manufacturing identity, never advertised by
  default;
- **componentId**: stable creation-local path or UUID;
- **bootId**: new UUID after every boot; and
- **artifactDigest**: SHA-256 digest of the exact released component manifest.

UUIDs follow [RFC 9562](https://www.rfc-editor.org/rfc/rfc9562.html).

### 8.4 Message framing

The P0 application layer uses deterministic CBOR payloads. JSON is the
normative diagnostic representation of the same data model. CBOR follows
[RFC 8949](https://www.rfc-editor.org/rfc/rfc8949.html).

Every physical-link message has:

| Field | Size | Meaning |
| --- | ---: | --- |
| protocolVersion | 1 byte | Major wire version |
| messageType | 1 byte | HELLO, DESCRIBE, CONFIGURE, COMMAND, STATE, EVENT, HEARTBEAT, ACK, ERROR, UPDATE, or TIME_SYNC |
| flags | 2 bytes | Request, response, error, fragmented, or urgent bits |
| sourceNode | 2 bytes | Session-local assigned node ID |
| destinationNode | 2 bytes | Node ID or broadcast |
| sequence | 4 bytes | Monotonic per-source sequence, wraps modulo 2^32 |
| payloadLength | 2 bytes | Number of CBOR payload bytes |
| payload | 0 to transport limit | Canonical CBOR map |
| crc32c | 4 bytes | CRC-32C over header and payload |

Multi-byte integers are little-endian in P0. This table is a prototype
envelope, not a stable wire claim. The final CAN identifier allocation,
authenticated-message field or isolated-port design, fragmentation,
minor-version encoding, timing table, and JSON/CBOR schemas MUST be frozen
with golden vectors before OMBR-LINK-1.

### 8.5 Required exchanges

1. The unpowered or auxiliary-powered module announces presence.
2. The hub assigns a session node ID.
3. Both sides exchange HELLO with supported protocol versions.
4. The module returns DESCRIBE and its signed or hashed component manifest.
5. The hub validates electrical requirements and owner policy.
6. The safety MCU enables high power if required.
7. The hub sends CONFIGURE with limits, safe state, heartbeat, and time base.
8. The module acknowledges and enters ready, but remains neutral.
9. The hub explicitly sends ARM before non-neutral commands.
10. HEARTBEAT, STATE, EVENT, and time synchronization continue during use.
11. Timeout, STOP, fault, disconnect, or shutdown enters safe state and removes
    high power as defined by the port profile.

### 8.6 Command lifetime and time synchronization

Wall-clock timestamps are for records. They never determine whether a motion
command remains valid.

| ID | Requirement |
| --- | --- |
| TIME-001 | An interactive motion command MUST carry validForMs, an unsigned relative lifetime measured from complete authenticated receipt by the immediate recipient. |
| TIME-002 | validForMs MUST be greater than zero and no more than 250 ms for interactive duty, voltage, velocity, position, hold, or brake commands. |
| TIME-003 | An active control source MUST renew an interactive command or heartbeat at least every 50 ms; a receiver MUST enter safe state when validity expires even if Linux, Wi-Fi, or the broker remains connected. |
| TIME-004 | Expiry MUST use the recipient's monotonic clock. Wall-clock correction, daylight-saving time, NTP steps, and RTC failure MUST NOT extend command validity. |
| TIME-005 | Session identity, boot ID, sequence, and authenticated replay protection MUST prevent a delayed or repeated frame from receiving a fresh lifetime. |
| TIME-006 | The safety MCU MUST monitor peripheral heartbeats and remove the affected port's motion power within 250 ms of link loss or invalid traffic. |
| TIME-007 | A scheduled trajectory MUST identify hub boot ID and signed 64-bit monotonic nanoseconds, and time synchronization MUST publish offset, drift estimate, round-trip delay, and uncertainty. A node MUST reject new scheduled motion when uncertainty exceeds 5 ms. |
| TIME-008 | A locally stored autonomous timeline MAY exceed 250 ms only after explicit local activation and validation; it remains subject to physical stop, local limits, watchdogs, and a declared loss-of-controller policy. |

### 8.7 Capability classes

The base protocol defines properties, actions, and events. A capability
descriptor includes:

- stable ID and human-readable title;
- property, action, or event kind;
- semantic type;
- SI unit and optional display unit;
- data schema, minimum, maximum, neutral, safe value, resolution, and precision;
- readable, writable, observable, and retained flags;
- nominal and maximum update rate;
- coordinate frame for spatial values;
- command modes, ramp behavior, brake/coast support, timeout, latency, noise
  or uncertainty, saturation, and environmental limits when applicable;
- required authorization scope; and
- extension namespace and version.

Runtime state is a separate envelope. It adds boot ID, sequence, source and
hub-receive timestamps, value, unit, quality, calibration revision, health,
stale/saturation state, and measured or estimated uncertainty. Static
descriptors MUST NOT be mutated to carry live values.

The model is compatible with the
[W3C Web of Things Thing Description 1.1](https://www.w3.org/TR/wot-thing-description11/).
OMBR uses a constrained profile so embedded devices do not need a general
JSON-LD engine.

### 8.8 P0 prototype connector

The development prototype MAY use the Molex Micro-Fit 3.0 eight-circuit
[0430450800 right-angle header](https://www.molex.com/en-us/products/part-detail/0430450800),
[0430250800 receptacle housing](https://www.molex.com/en-us/products/part-detail/430250800),
and an identified contact such as the
[0430300001 20–24 AWG terminal](https://www.molex.com/en-us/products/part-detail/430300001)
solely to validate staged power, cable construction, signal integrity, and the
application protocol. Molex lists 8.5 A per contact for the header and 7 A for
that terminal, but those are part ratings under manufacturer conditions—not an
assembled cable, multi-contact, enclosed port, source, protection, or
continuous OMBR rating. Both cited connector components document only 30
mating cycles. That is not adequate evidence for a frequently reconfigured
child-facing production ecosystem.

The syntax example now shows the required contract shape and a provisional
P0 pin map, endpoint envelopes, and protocol binding. Its values are
illustrative and do not authorize construction or power-up. A buildable P0
board still requires released schematics, exact COTS/derating records,
measured interface evidence, and a completed fault matrix under ELEC-001
through ELEC-029.

P0 is therefore:

- explicitly non-conformant with future OMBR-LINK-1;
- limited to supervised engineering prototypes;
- protected by per-port switching and current limits; and
- replaceable without changing the logical protocol or component manifest.

Connector selection for 0.2 MUST publish a scored test report covering
touch-safety, lifecycle, insertion force, pull-out, high-temperature derated
current, hot plug, signal integrity, field-replaceable cables, sourcing, and
open tooling.

## 9. Mechanical compatibility

### 9.1 Coordinate and grid

OMBR's canonical physical coordinate basis is right-handed +X, +Y, +Z. A
project MAY attach application-specific labels such as forward, left, up,
machine-base, workpiece, tool-center-point, end-effector, wheel-contact, or
gravity to named frames, but those labels are metadata rather than universal
axis meanings. A stationary machine is not required to invent a forward
direction, and a Creation may have multiple anchored roots.

Twin and simulation values use metres, radians, kilograms, seconds, amperes,
volts, kelvin, and derived SI units. CAD MAY use millimetres if the manifest
declares the conversion.

The brick exchange grid uses:

- 8.0 mm nominal horizontal module;
- 9.6 mm nominal brick height;
- 3.2 mm nominal plate-height increment; and
- connection frames located on exact integer or declared half-grid positions.

These values correspond to LDraw's 20 LDU brick width, 24 LDU brick height,
8 LDU plate height, and approximate 0.4 mm per LDU. LDraw explicitly warns
that real-world conversions are approximations. They MUST NOT be used as
manufacturing tolerances without physical metrology. See the
[LDraw format specification](https://www.ldraw.org/article/218.html).

### 9.2 Public reference sources and authority

No public LEGO or BrickLink source reviewed for this draft supplies an openly
licensed, complete engineering definition of the functional geometry,
tolerances, materials, loads, and lifecycle of its element catalog. The public
sources form useful but narrower evidence layers:

| Source | Reliable use in OMBR | Boundary that MUST be preserved |
| --- | --- | --- |
| [LEGO piece-number guidance](https://www.lego.com/en-gb/service/help-topics/article/identifying-lego-set-and-piece-numbers), [Pick a Brick](https://www.lego.com/en-us/pick-and-build/pick-a-brick), and official building instructions | Manufacturer-published names, Design IDs for shapes, Element IDs for shape/color combinations, set inventories, images, and current regional availability | Identity and retail evidence only; these sources do not publish editable engineering solids, interface tolerances, materials, gear profiles, load ratings, or lifecycle data |
| [LEGO Technic building guidance](https://www.lego.com/en-ca/service/help-topics/article/tips-for-building-with-lego-technic-elements) and its [1:1 measurement chart](https://www.lego.com/cdn/product-assets/product.bi.additional.info.pdf/42055_X_Measurements.pdf) | Informative beam/axle length identification and the Technic convention of naming those lengths in stud modules | A print aid is not a dimensioned manufacturing drawing, calibration artifact, tolerance, or fit specification |
| [BrickLink Studio](https://studiohelp.bricklink.com/hc/en-us/articles/5404381697559-Introduction-to-Studio), owned by the LEGO Group since its [BrickLink acquisition](https://www.lego.com/en-us/aboutus/news/2019/november/lego-bricklink) | Public virtual building, catalog lookup, model assembly, instructions, LDraw/LXFML import, and documented [LDraw export](https://studiohelp.bricklink.com/hc/en-us/articles/6502197862679-Exporting-to-other-formats) | Studio documents [missing parts](https://studiohelp.bricklink.com/hc/en-us/articles/8117942037911-Missing-parts) and approximate [collision detection](https://studiohelp.bricklink.com/hc/en-us/articles/5412820155927-Collision); its [software/assets license](https://studiohelp.bricklink.com/hc/en-us/articles/6606313426711-Studio-Software-License-Agreement) is not an open-CAD redistribution license |
| [LDraw format and units](https://www.ldraw.org/article/218.html) plus the [LDraw Parts Library](https://library.ldraw.org/) | Open, mature part/model exchange, visualization proxies, library identifiers, origins, and per-file attribution | LDraw is explicitly unofficial and community-run; its real-world unit conversion is approximate, “Official” means accepted by LDraw, licenses vary by file, and a mesh is not manufacturing or fit authority |
| OMBR Mechanical Reference Library | Project-authored functional interfaces, parametric source for OMBR-native replacements, drawings, gauges, measured fit/load/life evidence, simulation semantics, and stable open identifiers | This is the normative source for OMBR-native semantics; optional third-party aliases do not transfer ownership, endorsement, or rights to reproduce third-party parts |

The reviewed official public sources do not expose an engineering solid,
dimensioned functional interfaces and tolerances, connection-force
distributions, gear tooth definition, or load and wear-life qualification.
OMBR therefore does not label Pick a Brick, instructions, Builder, Studio, or
LDraw data as “official engineering CAD.” The core specification deliberately
stops at the functional engineering data needed to identify, procure, model,
mate, test, replace, and simulate a part. Production-mould design and tooling
are out of scope. If a manufacturer later publishes or licenses a more
authoritative engineering artifact, its exact revision, permission, scope,
and digest can be added without changing the authority of existing records.

External identities remain namespaced. `lego-design-id`, `lego-element-id`,
`bricklink-item-number`, and `ldraw-file` are not interchangeable and are not
assumed to map one-to-one. OMBR tools may link to or import data under its
actual terms; they MUST NOT extract proprietary application assets, bulk-copy
a restricted catalog, or redistribute third-party geometry without compatible
permission.

### 9.3 Mechanical Reference Library

The openly licensed OMBR Mechanical Reference Library is an independently
authored set of component records, functional models, and mating profiles. It
is designed for engineering selection, procurement, brick-CAD assembly,
kinematics, dynamics, open replacement fabrication, and physical conformance
rather than rendering alone. Its starter taxonomy covers:

- structural bricks, plates, tiles, studless beams/liftarms, frames, panels,
  brackets, shells, and project-authored chassis parts;
- studs, tubes, round holes, friction and free pins, cross axles and holes,
  bushings, axle-pins, perpendicular/angled connectors, hinges, ball joints,
  universal joints, and turntables;
- spur, bevel, double-bevel, crown, worm, rack, differential, clutch, pulley,
  belt, chain, sprocket, and linear transmission elements;
- wheels, tyres, hubs, casters, tracks, rollers, and rail-guidance elements;
  and
- springs, elastomers, string, hose, pneumatic elements, counterweights, and
  other flexible or stored-energy parts.

Every library part is a component model decomposed into one or more named
rigid, static, or flexible bodies whose interfaces point to exact revisions of
reusable mechanical mating profiles. Repeated features such as beam holes are
explicit named frames or a deterministic, expanded frame generator whose
output is content-addressed. The creation graph separately records fixed mates,
kinematic joints, multi-member transmissions, flexible elements, assembly
hierarchy, pose authority, and design/as-built/runtime state. This lets the
same graph describe a rover drivetrain, differential, closed-loop linkage,
vehicle steering rack, crane, or stationary conveyor without pretending every
relationship is a binary static connection.

A gear record is not merely a toothed visual mesh. It captures gear type,
tooth system, tooth count, module or circular pitch, pressure and helix/cone
angles, pitch/base/tip/root diameters, addendum, dedendum, profile shift,
root fillet, face width, clearance, backlash, bore or axle fit, datum and axis,
material and fabrication basis where known, runout, permitted mates and centre distances, torque/speed/
temperature limits, efficiency assumptions, lubrication, duty, and wear-life
evidence. A mesh relation records both gear revisions, contacting features,
axis frames, centre distance, orientation, signed ratio, phase where relevant,
backlash/compliance, efficiency, limits, and validation evidence.

### 9.4 Requirements

| ID | Requirement |
| --- | --- |
| MECH-001 | Every component MUST declare a named origin, orientation, bounding box, keep-outs, centre of mass, and connection frames. |
| MECH-002 | The hub enclosure MUST provide side mounting suitable for studless beam constructions and at least one top or bottom brick attachment strategy. |
| MECH-003 | The brick-compatible mounting shell MUST be separable from the electronics chassis so geometry can evolve without respinning the carrier. |
| MECH-004 | Source CAD MUST remain parametric for grid count, wall thickness, clearance class, hole fit, manufacturing process, and shrink compensation. |
| MECH-005 | The enclosure MUST publish antenna, thermal, cable-bend, connector-access, button, light-pipe, and battery-removal keep-outs. |
| MECH-006 | A compatibility release MUST include loose, nominal, and tight fit coupons for each supported manufacturing process. |
| MECH-007 | Dimensions and tolerances MUST be based on a documented measurement study across multiple genuine reference parts, types, ages, cavities, and lots. |
| MECH-008 | A claim of brick or beam compatibility MUST identify the tested elements, sample count, process, material, environmental conditions, and pass criteria. |
| MECH-009 | The project MUST publish go/no-go gauges and insertion/retention test procedures. |
| MECH-010 | Released OMBR-native CAD and replacement-fabrication source MUST avoid logos, source-indicating marks, or copied ornamental housings belonging to another manufacturer. |
| MECH-011 | Hub, battery, motor or actuator, light, sensor, adapter, and cable-retention models MUST declare their outer envelope in SI units and brick-grid modules, accessible mounting frames, assembly clearance, and collision geometry. |
| MECH-012 | The reference starter family MUST provide measured attachment strategies for selected studless-beam holes and pins, cross axles, and top or bottom stud connections; each strategy is a separate tested compatibility claim. |
| MECH-013 | The project MUST publish a machine-readable reference-part and attachment-frame library with source, revision, license, provenance, geometry fidelity, and measured-versus-nominal status for every entry. |
| MECH-014 | Cable models and assemblies MUST include connector sweep, minimum bend, strain-relief, removal-tool, and moving-joint keep-outs so CAD and simulation can detect unroutable or unsafe placements. |
| MECH-015 | A mechanical component release MUST include assembly orientation, fasteners, torque where applicable, disassembly sequence, wear interfaces, and replacement limits; glue or destructive joining requires a documented reason and replaceable subassembly boundary. |
| MECH-016 | Each mechanical reference catalog MUST declare publisher, stable catalog ID and revision, source URI, review date, immutable snapshot digest where redistribution is permitted, license or access terms, authority class, covered evidence levels, identifier semantics, and limitations. Dynamic retail or API results MUST additionally record query, locale, and retrieval time. |
| MECH-017 | A part's external aliases MUST be separately namespaced and independently sourced. A tool MUST NOT infer equality among a LEGO Design ID, LEGO Element ID, BrickLink Item Number, LDraw filename, or OMBR part ID merely because their strings resemble one another. |
| MECH-018 | Catalog identity, availability, visual geometry, connection metadata, dimensional reference, tolerance distribution, material specification, load/life evidence, and open-replacement source MUST be separately claimable evidence levels. Every value MUST carry source, revision, method, uncertainty where measured, and authoritative/provisional/unknown status. |
| MECH-019 | A third-party proprietary asset MAY be an optional local import or visual reference but MUST NOT be required to build, validate, manufacture, simulate, or repair the open starter system. Restricted assets and catalogs MUST NOT be vendored, extracted, or bulk mirrored without explicit compatible permission. |
| MECH-020 | “Official” MUST name its authority. `manufacturer-published`, `manufacturer-approved`, `ldraw-official`, `community`, `independently-measured`, and `ombr-native` MUST remain distinct labels; an LDraw Official part MUST NOT be presented as LEGO-approved engineering CAD. |
| MECH-021 | Every OMBR Mechanical Reference Library record MUST publish an immutable part-record ID and revision, name, category, design/variant/offer identity class, source and rights status, artifact digests where distributable, external aliases, datum and coordinate convention, procurement/lifecycle status, provenance, engineering-evidence coverage, maturity, and known limitations. An external purchased-part record MUST NOT claim unavailable native CAD; an OMBR-native replacement additionally publishes the editable and derived artifacts required by MECH-023. |
| MECH-022 | Every part MUST separately describe nominal envelope, functional connection geometry, collision volume, moving sweep, assembly/removal and tool clearances, visual geometry, datums, tolerances, fit classes, material/process/finish, mass properties, allowed loads, environmental limits, wear surfaces, life, and evidence. Unknown fields MUST be explicit and MUST NOT be filled from a visual mesh. |
| MECH-023 | OMBR core does not standardize production moulds or tooling. A purchased-part record MUST publish the available engineering data needed for selection and use—identity, revision, envelope, functional interfaces, fit, material family where documented, mass, load/speed/environment/life limits, procurement status, and evidence—and mark unavailable values unknown. An OMBR-native replacement MUST additionally publish open parametric geometry, fabrication assumptions, drawings, gauges, and physical tests sufficient for its chosen accessible process such as FDM, resin printing, CNC, or a documented service bureau. |
| MECH-024 | OMBR-native parts intended to mate with third-party elements SHOULD preserve only the necessary functional grid and interfaces while using original, non-confusing appearance and project marks. Compatibility metrology does not authorize copying logos, ornamental surfaces, protected designs, or undisclosed manufacturing data. |
| MECH-025 | Every mechanical interface MUST reference an exact mechanical mating-profile revision and declare mating role, datum frame, insertion or engagement axis, allowed orientations, assembly path, fit class, retained degrees of freedom, detachability, tool access, and wrong/partial-mate outcome. |
| MECH-026 | A mating profile MUST define both sides of the functional interface; nominal geometry and tolerance stack; clearance/interference policy; insertion, withdrawal, retention, torque, friction, and wear bands where applicable; compatible material/process classes; conditioning; cycle life; gauges; test method; and a compatibility matrix. Connector appearance or nominal grid alignment alone is insufficient. |
| MECH-027 | Repeated studs, tubes, holes, teeth, links, or track features MUST expand deterministically into stable named frames and interfaces. A generator MUST declare parameters, coordinate order, source/version, output digest, and collision/connection rules so another implementation produces the same graph. |
| MECH-028 | Every gear, rack, worm, sprocket, pulley, or similar transmission part MUST publish its full functional tooth/groove profile, axis and phase convention, dimensional tolerances, bore/axle fit, permitted mate families, material and fabrication basis where known, runout and concentricity, load/speed/thermal/duty limits, efficiency, backlash/compliance, lubrication policy, wear life, fixtures, and evidence; tooth count plus a render mesh is insufficient. |
| MECH-029 | Every gear mesh or transmission connection MUST resolve exact part and feature revisions and declare geometry, centre distance or belt/chain length and tension, axis relationship, signed ratio, direction, phase where relevant, efficiency/loss, backlash/compliance, load/speed limits, interference and axial-retention checks, and validation status. |
| MECH-030 | A multi-stage transmission MUST derive and expose overall ratio, direction, reflected inertia, backlash/compliance, loss/efficiency range, limiting member, and output load envelope from its resolved stages. A manually entered summary MUST be checked against the graph. |
| MECH-031 | Fixed, revolute/continuous, prismatic, screw, universal, spherical, planar, and floating kinematic joints MUST declare body/frame endpoints, axes, allowed and constrained degrees of freedom, limits, zero/reference state, friction/damping/compliance, preload, load envelope, breakaway or retention, state observability, and safe behavior. Gear, rack, worm, belt, chain, differential, clutch, cam, lead-screw, or tendon relations are transmissions; detachability belongs to a mate/joint policy; flexible routing is a separate element. |
| MECH-032 | Structural parts and assemblies used to support a declared load MUST publish applicable load cases, support assumptions, safety factor policy, stiffness/deflection, buckling or pull-out limits, fatigue/cycle assumptions, failure modes, and validation evidence; a collision-safe CAD placement is not a load rating. |
| MECH-033 | Wheels, tyres, tracks, rails, and ground-contact models MUST declare rolling/steering axes, effective loaded radius or path, width/envelope, hub fit, material/compliance, friction basis, permitted load/speed, runout, slip assumptions, wear, and surface/environment conditions. |
| MECH-034 | Springs, elastomers, hoses, string, pneumatics, counterweights, and other stored-energy or flexible elements MUST declare rest state, routing/attachment, constitutive or force/displacement behavior, travel/pressure/tension limits, hysteresis/creep, damping, fatigue, failure containment, release procedure, and simulation approximation. |
| MECH-035 | Mechanical conformance MUST validate identity and license resolution, deterministic artifact generation, frame/interface graph closure, tolerance stacks, interference and tool access, kinematic freedom, transmission math, load/energy limits, and physical fit/force/wear evidence. Passing an LDraw, Studio, STEP, or mesh import alone MUST NOT establish OMBR-MECH-1. |
| MECH-036 | Mechanical catalog records MUST separately identify design/shape, material/color/packaging element or SKU, engineering model, OMBR replacement, and seller offer. Lifecycle and availability are independently sourced, dated, regional, and condition-specific; a price is a timestamped currency/quantity/tax/shipping observation, never an intrinsic part property. |
| MECH-037 | Every catalog datum and asset MUST declare data and asset licenses or terms, redistribution/cache status, use basis (`authored`, `licensed`, `measured`, `factual-metadata`, or `unknown`), jurisdictions where reviewed, review status/date, and limitations. Missing restrictions MUST NOT be interpreted as permission. |
| MECH-038 | A mapping from an external part to an OMBR-native replacement MUST declare substitution class, exact revisions, covered and uncovered interfaces/properties, dimensional and behavioral evidence, rights status, and limitations. Visual resemblance or shared aliases are insufficient. |
| MECH-039 | Every physical component used for dynamics or moving collision MUST decompose into one or more named rigid, static, or flexible bodies. Each body MUST declare frame, visual/collision/keep-out geometry, material/contact references, load-envelope references, and known or explicit-unknown mass, center of mass, and inertia in a named expression frame with evidence. |
| MECH-040 | Fixed mates, kinematic joints, transmissions, flexible elements, and transmission networks MUST be distinct first-class objects with stable IDs and body-aware endpoints. A transmission MAY have more than two members, as required by differentials and clutches, and the constraint graph MAY contain closed loops even though frame parentage remains acyclic. |
| MECH-041 | Assemblies MUST declare hierarchy, reusable subassemblies, members/BOM, roots and anchors, build/disassembly artifacts, configurations, and placement authority. An instance is either explicitly anchored relative to a named frame or constraint-derived; redundant absolute and connection poses MUST NOT silently overdetermine it. |
| MECH-042 | Design configuration, as-built calibration, initial simulation state, commanded state, and observed runtime joint/transmission state MUST be separate layers. Telemetry MUST NOT rewrite design joints, limits, mates, or placement authority. |
| MECH-043 | Every actuator and position/force/motion sensor capability MUST bind to exact joint or transmission coordinates with axis, sign, zero/offset, units, reduction, range, limits, calibration and observability. A component-level capability without a mechanical state mapping is insufficient for simulation parity. |
| MECH-044 | Every CAD, LDraw, glTF, STEP, SDF, URDF, or other derivative MUST declare exact format/version/profile/extensions; source units, handedness, up axis and frame; scale/transform into the manifest; semantic-ID-to-node mappings; fidelity/authority; and a machine-readable loss report. The manifest owns connectivity and kinematics that an asset format cannot express. |
| MECH-045 | Mechanical-library completeness MUST be demonstrated by a versioned semantic-family/evidence/availability/open-replacement coverage matrix over structures; studs/tubes; pins/holes; axles/bushes; connectors; joints; gears/racks/worms; differentials/clutches; belts/chains/pulleys; wheels/tracks/rails; flexible/stored-energy elements; and generic fasteners. Completeness does not require redistribution of restricted third-party records or geometry. |
| MECH-046 | A mechanical offer observation MUST record seller, exact SKU/condition, region, currency, amount, quantity and pack basis, MOQ, tax and delivery inclusion, stock/lead-time observation, source URI, and retrieval time. A dynamic-query catalog snapshot MUST additionally retain query, locale, API/version/terms and permitted snapshot digest. |
| MECH-047 | Pneumatic and hydraulic bodies, attachments, motion, stored-energy hazards, and mechanical loads belong in MECH; medium, pressure, flow, leakage, valves, reservoirs, and safe venting require a future separately versioned `OMBR-FLUID-1` profile and MUST NOT be invented as unversioned mechanical fields. |

Until the metrology study is complete, hole diameter, stud clutch geometry,
and third-party fit/tolerance assumptions remain reference-CAD parameters rather than frozen
normative numbers.

### 9.5 Trademark and compatibility wording

The project name, domain, logo, and certification mark MUST NOT contain LEGO,
MINDSTORMS, or another third party's mark. The LEGO logo MUST NOT appear on
project hardware or sites. Descriptive compatibility text SHOULD be limited
to factual wording such as “compatible with selected LEGO® Technic elements”
and accompanied by a clear non-affiliation statement after appropriate legal
review.

The [LEGO Fair Play policy](https://www.lego.com/en-it/legal/notices-and-policies/fair-play)
is a useful trademark warning but is not commercial product clearance. Open
licensing does not grant third-party trademark, design, or patent rights.

LEGO® is a trademark of the LEGO Group of companies, which does not sponsor,
authorize, or endorse this specification.

## 10. Peripheral requirements

### 10.1 Common component contract

| ID | Requirement |
| --- | --- |
| PER-001 | Every peripheral MUST resolve a complete ELEC contract for each electronic boundary and implement discovery, descriptor, health, heartbeat, safe-state, fault, and update semantics appropriate to its role. |
| PER-002 | Every observable value MUST declare its unit, valid range, resolution, nominal rate, source timestamp, quality, and calibration revision. |
| PER-003 | Every actuator command MUST declare accepted modes, bounds, ramp behavior, timeout, safe state, and whether braking or coasting is used. |
| PER-004 | A component MUST reject commands outside its locally enforced electrical, thermal, kinematic, or configuration limits. |
| PER-005 | A component MUST distinguish requested, accepted, applied, and measured values where those states differ. |
| PER-006 | A component MUST expose model, hardware, bootloader, firmware, and protocol revisions plus its source-release URL or digest. |
| PER-007 | A component MUST remain recoverable after interrupted firmware update. |
| PER-008 | A component with stored calibration MUST support export, reset, provenance, and revision tracking. |
| PER-009 | Coordinate-bearing sensors and actuators MUST name a manifest frame and use the canonical coordinate convention. |
| PER-010 | A component MUST expose standardized faults and MAY add namespaced vendor faults. |

Required common faults include:

- command-timeout;
- heartbeat-timeout;
- overcurrent;
- short-circuit;
- undervoltage;
- overvoltage;
- overtemperature;
- encoder or sensor fault;
- stalled or blocked;
- calibration-invalid;
- descriptor-invalid;
- incompatible-version;
- update-failed; and
- internal-error.

### 10.2 Motor and position actuator profile

An OMBR-MOTOR-1 device may be a smart motor, a smart gearbox, or an adapter
that drives a passive two-wire motor. The ecosystem MUST NOT require a
controller inside every inexpensive DC motor.

The planned profile has two explicit capability sets:

- **MOTOR-OPEN-LOOP** is an identified passive motor plus a smart adapter. It
  requires duty-cycle, coast, brake where electrically supported, current
  limiting, command expiry, and a configured motor model. It cannot advertise
  position, velocity, hold, or trajectory control without measured feedback.
- **MOTOR-FEEDBACK** adds measured position and/or velocity, homing and
  position-valid state, hold, limits, and time-tagged trajectories appropriate
  to the declared sensor topology.

Voltage, velocity, absolute position, relative position, hold, and trajectory
commands are required only when the corresponding capability is advertised.
STOP always preempts lower-priority commands.

| ID | Requirement |
| --- | --- |
| MOTOR-001 | Duty-cycle zero, coast, and brake MUST be distinct where braking is electrically supported; hold MUST be distinct and available only for a feedback device that advertises position hold. |
| MOTOR-002 | Position or velocity claims MUST be backed by measured feedback; timed open-loop movement MUST NOT be described as position control. |
| MOTOR-003 | Direction conventions MUST be defined relative to the component frame and output shaft. |
| MOTOR-004 | The identified motor model or cartridge release MUST publish nominal and permitted voltage, no-load speed/current, stall current/torque, continuous region, thermal limit, gear ratio, backlash, and output geometry; encoder resolution is required only when feedback exists. An adapter MUST refuse to arm an unknown passive motor except under a separately tested conservative generic policy. |
| MOTOR-005 | Characterization MUST identify supply voltage, temperature, sample count, measurement method, uncertainty, and whether each value is measured or manufacturer-declared. |
| MOTOR-006 | Continuous current and torque ratings MUST be based on a published thermal test, not the connector or driver's peak rating. |
| MOTOR-007 | The smart motor or adapter MUST always enforce configured current and electrical limits locally. It MUST enforce temperature, velocity, and position limits only when the corresponding sensors and capabilities are present, and MUST never imply an unmeasured limit. |
| MOTOR-008 | Motion commands MUST expire and enter the configured safe state without network, Pi, or broker availability. |
| MOTOR-009 | Every module SHOULD report current, voltage, applied output, limit state, and fault state; it SHOULD report temperature, position, and velocity only when measured or explicitly marked as an estimate with uncertainty. |
| MOTOR-010 | Output shaft, axle adapter, mounting points, cable exit, and collision volume MUST have editable source CAD and gauges. |
| MOTOR-011 | A feedback motor MUST declare position topology as relative, bounded single-turn, modulo single-turn, or multi-turn and MUST define encoder rollover and retained-state behavior. |
| MOTOR-012 | Absolute-position capability MUST expose homed and positionValid state plus zero provenance; after boot, motor replacement, calibration reset, or detected slip it MUST reject absolute commands until validity is re-established. |
| MOTOR-013 | An absolute target MUST select shortest, positive, negative, or continuous path behavior and define modulo/wrap semantics; an omitted path mode MUST NOT cause motion. |
| MOTOR-014 | Homing MUST define direction, speed, current/force ceiling, travel/time ceiling, reference sensor or stop, cancellation, failure, and resulting zero uncertainty. |
| MOTOR-015 | A motion actuator MUST declare rotary or linear output, continuous or bounded travel, output coupling, mechanical limits, compliance, backdrivability, holding behavior, and stored-energy hazards. |
| MOTOR-016 | Servo-angle, linear-position, and continuous-rotation modes MUST be distinct capabilities with declared units, range, neutral, feedback basis, and loss-of-command behavior; pulse timing or a display label alone does not establish position control. |
| MOTOR-017 | Gearbox, axle, wheel, linkage, and linear-output adapters MUST publish editable geometry, ratio or pitch, efficiency assumptions, backlash or compliance, load limits, and the frame transformation from motor to output. |
| MOTOR-018 | Actuators using springs, pneumatics, vacuum, gravity loads, or another stored-energy mechanism MUST declare isolation, depressurization or restraint, safe state, residual energy, and manual recovery in addition to electrical output disable. |

The first reference family should include:

1. a low-cost passive M-size DC motor with an open housing and axle adapter;
2. a smart M-size position motor with quadrature or absolute feedback;
3. a higher-torque L-size position motor; and
4. a two-wire legacy motor adapter containing the H-bridge and protection.

### 10.3 Light profile

| ID | Requirement |
| --- | --- |
| LIGHT-001 | Light capabilities MUST distinguish monochrome intensity, RGB, RGBW, addressable pixels, and effects. |
| LIGHT-002 | Intensity MUST define whether it is linear electrical drive, calibrated relative output, luminous flux, or another quantity. |
| LIGHT-003 | RGB values MUST declare color space and transfer function; linear sRGB is the default interchange representation. |
| LIGHT-004 | A calibrated color-capable light SHOULD expose CIE xy or XYZ in addition to device channels. |
| LIGHT-005 | Effects MUST be bounded programs with explicit duration, repetition, priority, cancellation, and safe-state behavior. |
| LIGHT-006 | The module MUST enforce LED current and thermal limits locally. |
| LIGHT-007 | The release MUST publish channel current, voltage range, refresh or PWM rate, flicker information, thermal behavior, optical measurement method, and expected lifetime assumptions. |

### 10.4 Sensor profile

The initial standard sensor classes are:

- distance and proximity;
- reflectance, ambient light, and color;
- touch, button, and contact;
- force, load, and pressure;
- rotation and position;
- acceleration, angular velocity, and magnetic field;
- temperature and humidity; and
- electrical voltage, current, power, and energy.

| ID | Requirement |
| --- | --- |
| SENSOR-001 | Sensor outputs MUST use SI units or a specifically named dimensionless scale. |
| SENSOR-002 | The descriptor MUST state range, resolution, sample rate, latency, noise or uncertainty, saturation behavior, and environmental limits. |
| SENSOR-003 | Invalid, stale, saturated, warming-up, uncalibrated, and faulted data MUST be distinguishable from a valid zero. |
| SENSOR-004 | Multi-axis values MUST declare axis order, handedness, frame, and covariance or per-axis uncertainty where available. |
| SENSOR-005 | Calibration MUST identify procedure, reference equipment, timestamp, software revision, operator or process, and resulting parameters. |
| SENSOR-006 | A simulator MUST be able to reproduce the sensor's range, latency, rate, clipping, and a declared noise model. |

### 10.5 Adapter profile

Adapters preserve useful purchased equipment without making a proprietary
connector or undocumented waveform the OMBR native link. Initial adapter
targets include passive two-wire motors and lights, selected PF-style
four-contact devices, selected Powered Up/LPF2 devices where lawfully
implementable, conventional three-wire RC servos through a separately powered
interface, and protected short-reach maker sensors such as Grove modules. Each
target is a separate tested profile; physical fit never implies protocol or
servo equivalence.

| ID | Requirement |
| --- | --- |
| ADAPTER-001 | An adapter descriptor MUST identify exact upstream and downstream connector, electrical, protocol, power, component-revision, and capability profiles plus the evidence and source release for each supported combination. |
| ADAPTER-002 | Voltage, current, grounding, isolation, level shifting, termination, signal direction, timing, power sequencing, backfeed, and fault energy MUST be explicitly designed and tested on both sides. A passive shape converter cannot claim translation it does not perform. |
| ADAPTER-003 | The adapter MUST enforce the lower of its own limits, both endpoint limits, configured component limits, cable limits, and hub policy and MUST NOT propagate a descriptor that raises any ceiling. |
| ADAPTER-004 | Capability translation MUST publish exact units, range, neutral, quantization, rate, latency, timeout, safe state, calibration, error, and unsupported-feature mapping with golden vectors and hardware traces. |
| ADAPTER-005 | An unavailable identity, encoder, feedback, calibration, update, or fault feature MUST remain unavailable or explicitly estimated with provenance; an adapter MUST NOT fabricate conformance by inventing feedback. |
| ADAPTER-006 | Wrong device, reversed or partial insertion, wrong voltage, short, open wire, hot plug, brownout, restart, bus fault, and removal under load MUST lead to a bounded documented safe outcome. |
| ADAPTER-007 | A conventional RC servo adapter MUST provide the declared isolated or common-ground power rail and bounded pulse interface itself; it MUST NOT expose raw hub motor power as if it were a three-wire servo supply. |
| ADAPTER-008 | A legacy PF-style or Powered Up/LPF2 adapter claim MUST name exact tested device and revision combinations. Similar plug geometry or community reports alone are insufficient. |
| ADAPTER-009 | External I2C, UART, GPIO, or analog maker modules MAY connect only through a protected adapter profile that bounds cable length, voltage, current, ESD, address conflict, bus lock, and fault propagation; they do not become the native external link. |
| ADAPTER-010 | Adapter firmware, PCB, connector source where project-controlled, enclosure, cable, fixtures, protocol mapping, and recovery method MUST meet the same open release contract as a native peripheral. |
| ADAPTER-011 | Legacy factory IDs, radio addresses, port numbers, or device paths MUST NOT replace OMBR owner-resettable instance identity; replacement and pairing history remains in the as-built twin. |
| ADAPTER-012 | The compatibility matrix MUST record failed, partial, unsafe, and revision-sensitive combinations as well as successful ones, including supply conditions and whether evidence is manufacturer-documented or independently measured. |

### 10.6 Component-level reference candidates

The following parts are informative starting points for engineering prototypes,
not an approved production BOM. A release cannot conform by copying this
table; it must publish its exact BOM, alternates, calculations, and test
evidence.

| Function | R0 candidate | Reason and required validation |
| --- | --- | --- |
| Linux compute | Raspberry Pi Zero 2 W | Required Wi-Fi/BLE and Linux baseline; treat as replaceable COTS boundary |
| Safety MCU | RP2040 | Mature open tooling and precedent in Raspberry Pi Build HAT; verify independent watchdog and fail-safe output gating |
| CAN FD controller | MCP2518FD | SPI-attached CAN FD controller with Linux support; validate timing, load, and licensing through purchased silicon |
| CAN FD transceiver | MCP2562FD class | Widely available differential physical layer; validate standby, ESD, common mode, and cable faults |
| Smart motor MCU | STM32G0B1 class with FDCAN, or RP2040 plus external controller | Compare BOM, open toolchain, boot recovery, timer/encoder capture, and lifecycle |
| Brushed motor driver | DRV8876 class | Integrated current regulation and protection; characterize real enclosure thermals and motor transients |
| Shaft feedback | Quadrature magnetic encoder or AS5600-class absolute encoder | Compare startup position, magnet tolerance, update rate, latency, and shaft geometry |
| RGBW driver | TLC59711-class constant-current PWM driver | Good channel resolution; validate current accuracy, thermal limits, and flicker |
| Distance sensor | VL53L1X-class time-of-flight module | Compact ranging baseline; publish field-of-view, surface, ambient-light, and crosstalk tests |
| Color sensor | AS7341-class spectral sensor | More descriptive than a single RGB estimate; publish illumination and material calibration |
| IMU | BMI270-class six-axis sensor | Compact, supported class; publish frame, calibration, rate, filtering, and temperature behavior |
| Force front end | NAU7802-class bridge ADC plus documented load cell | Low-cost force profile; publish creep, hysteresis, temperature, overload, and mechanical fixture |

The first open peripheral starter release MUST include the open-loop and
feedback motion paths, light, distance/proximity, color/reflectance, and
touch/force sensors, cables, and applicable adapters required by ECO-002. Each
release includes editable electronics, firmware, mechanical CAD, fixtures,
and characterization data satisfying section 6.

## 11. Creation and control model

### 11.1 Control sources

A control source may be a generic USB or Bluetooth gamepad, open handheld
remote, keyboard, touch UI, phone motion sensor, assistive input, program,
sensor, timeline, or network client. It exposes typed capabilities; the
binding model does not special-case a particular vendor's button numbers.

The initial purchasable benchmarks are generic HID gamepads and the LEGO
Powered Up Remote 88010. The complete open workflow must not require that
proprietary remote: an openly documented handheld implementation or generic
HID profile provides the substitutable path.

| ID | Requirement |
| --- | --- |
| INPUT-001 | A control source descriptor MUST declare model and instance identity, connection type, capabilities, value schemas, units, ranges, neutral values, event semantics, nominal and maximum rate, latency, and source-release or benchmark provenance. |
| INPUT-002 | Analog axes and triggers MUST declare raw and normalized range, center or rest value, polarity, dead-zone basis, resolution, saturation, and calibration revision; a client MUST NOT assume every axis is symmetric. |
| INPUT-003 | Buttons, hats, switches, encoders, gestures, and motion inputs MUST declare momentary, maintained, relative, absolute, repeat, and multi-state behavior rather than reducing every input to a boolean press. |
| INPUT-004 | Input events MUST carry source, boot or connection session, sequence, monotonic timestamp where available, value, and quality; duplicate, reordered, stale, and rate-excess events MUST have deterministic handling. |
| INPUT-005 | Calibration MUST be exportable, owner-resettable, device/revision-specific, and visibly distinguish factory, measured, and user settings. Invalid calibration MUST fall back to a documented safe neutral policy. |
| INPUT-006 | A wireless or battery-powered source SHOULD expose connection quality, battery state, charging state, sleep state, and low-battery warning without using a stable tracking identifier as public discovery identity. |
| INPUT-007 | Disconnect, sleep, focus loss, stale input, process suspension, profile change, or source replacement MUST emit or synthesize bounded neutralization and release its control lease. |
| INPUT-008 | A wireless source that can cause state changes MUST use authenticated pairing or an authorized hub/client session; discovery presence alone MUST NOT grant control. |
| INPUT-009 | Output capabilities such as rumble, light, display, sound, or force feedback MUST be separately declared, bounded, cancellable, permission-controlled, and safe on disconnect. |
| INPUT-010 | A software stop button or handheld stop command MUST preempt ordinary mappings but MUST NOT be labeled as a safety-rated emergency stop unless independently assessed; the physical hub stop remains available. |
| INPUT-011 | Reference keyboard, touch, and gamepad mappings MUST be remappable, keyboard-accessible where applicable, non-color-dependent, and serializable in the portable control profile. |
| INPUT-012 | The project MUST publish either open source handheld-controller hardware and firmware or a complete generic-HID gamepad profile, mapping wizard, fixtures, and test vectors sufficient to operate the starter system without a proprietary remote. |
| INPUT-013 | Input benchmarks MUST record hardware/firmware revision, OS and driver, connection, report descriptor, sample and event rates, end-to-end latency, jitter, range, neutral drift, disconnect behavior, battery behavior, and mapping provenance. |

### 11.2 Creation graph

A Creation contains:

- metadata and license;
- component models and instances;
- mechanical and electrical connections;
- control sources;
- profiles and bindings;
- timelines;
- program descriptors, source and build artifacts, dependency locks,
  permissions, execution targets, and runtime bindings;
- safety policies within hardware ceilings;
- CAD, visualization, and simulation assets;
- calibration links;
- twin synchronization policy; and
- a content-addressed release manifest.

A Creation MUST remain portable between a physical hub and a conforming
simulator. Deployment binds stable component IDs to discovered instance IDs;
it MUST NOT rewrite the design source merely because a physical device was
replaced.

### 11.3 Bindings

Bindings preserve BrickController2's useful input behavior while targeting
typed capabilities rather than integer channels.

A binding may define:

- input source and capability;
- target component and capability;
- scale and offset;
- inversion;
- lower and upper clamp;
- dead zone;
- active zone;
- linear, cubic, signed-cube-root, or registered transfer curve;
- rate and acceleration limit;
- mixing with other inputs;
- registered stateful behaviors such as toggle, alternating, circular,
  ping-pong, accelerator, or stop;
- priority and arbitration group; and
- a timeline or program trigger.

| ID | Requirement |
| --- | --- |
| CTRL-001 | Normalized input ranges MUST declare min, max, and neutral; -1.0 to +1.0 with neutral 0.0 is the default analog convention. |
| CTRL-002 | Bindings MUST produce the same result within 1e-6 normalized output for the same ordered input events, initial state, and configuration. |
| CTRL-003 | All transforms MUST be serializable, versioned, and available to both hardware and simulator. |
| CTRL-004 | Multiple commands to one target MUST use explicit priority and arbitration rules. |
| CTRL-005 | STOP, hardware limit, and safety-controller decisions MUST preempt user mappings. |
| CTRL-006 | A disconnected or stale control source MUST produce a documented neutralization event rather than silently holding motion. |
| CTRL-007 | Profile activation MUST validate target existence, capability compatibility, units, bounds, and required safety policy. |
| CTRL-008 | The baseline transform order MUST be normalize, dead-zone removal, inversion, curve, scale, offset, mixing, clamp, rate/acceleration limiting, and arbitration. |
| CTRL-009 | For normalized x and dead zone d, dead-zone removal is zero when absolute(x) is at most d; otherwise it is sign(x) times (absolute(x) minus d) divided by (1 minus d). |
| CTRL-010 | Baseline curves are linear(x)=x, cubic(x)=x cubed, and signed-cube-root(x)=sign(x) times the real cube root of absolute(x). The ambiguous name logarithmic is a deprecated import alias for signed-cube-root only. |
| CTRL-011 | Stateful transform state starts at the target neutral value. Highest numeric priority wins; equal priority is resolved by lexicographic binding ID after STOP and safety preemption. |
| CTRL-012 | A stateful behavior not defined by the public transform registry MUST fail profile validation; an implementation MUST NOT guess from a display name. |
| CTRL-013 | Timeline version 1 uses monotonic-relative integer milliseconds, starts at zero, and has strictly increasing points; wall time, file order ties, and floating-point timestamps MUST NOT decide execution order. |
| CTRL-014 | Timeline values and interpolation MUST validate against the target capability data type, unit, bounds, resolution, modes, and safe state before activation; nonnumeric values permit step interpolation only. |
| CTRL-015 | A forever-repeat timeline requires explicit local owner activation, a visible active state, bounded resource use, cancellation, loss-of-controller policy, and continued physical-stop, watchdog, limit, and safety enforcement. |
| CTRL-016 | Hub and simulator timeline engines MUST produce the same ordered target values within the capability and selected simulation fidelity tolerances for one versioned timeline and initial state. |
| CTRL-017 | Every control profile, timeline, and activatable program MUST reference a project safety policy whose target entries resolve, cover every commanded actuator capability, type-check each safe value, remain within hardware ceilings, and require explicit re-arming after a stop or latched fault. |

Implementations MAY calculate internally at greater precision but serialize
finite IEEE 754 binary64 values. NaN and infinities are invalid. Each
operation is rounded only when written to a capability whose resolution
requires quantization.

### 11.4 Timelines

A timeline is an ordered list of time-tagged control points or actions. It may
loop and interpolate, but it MUST declare:

- time base and units;
- target capability;
- interpolation method;
- preemption and cancellation;
- end behavior;
- tolerance for late points;
- whether execution is hub-side or peripheral-side; and
- the exact creation and capability schema versions.

Motion scheduling uses a monotonic clock. Wall-clock time is metadata, not the
execution time base. Time-sensitive points SHOULD run on the hub or peripheral
rather than depend on network arrival.

### 11.5 State semantics

OMBR distinguishes:

- **desired**: what a controller or profile requests;
- **accepted**: what passed authorization, bounds, and arbitration;
- **applied**: what the actuator controller attempted to apply;
- **reported**: what the device currently reports;
- **observed**: what a sensor or estimator measured; and
- **simulated**: what a named simulation run produced.

State records carry component ID, capability ID, boot ID, sequence, source
timestamp, hub-receive timestamp, value, unit, quality, calibration revision,
and optional uncertainty. Consumers MUST NOT infer that desired equals
applied or observed.

### 11.6 Portable programming model

OMBR does not define a mandatory new programming language. Ordinary Python,
.NET/C#, C/C++, Rust, JavaScript/TypeScript, ROS, visual tools, and future
languages can target the same asynchronous capability SDK. The first
reference toolchain provides Python and .NET/C# golden programs because both
run on the Pi and on developer machines; support for another language is a
runtime profile rather than an editor fork.

Programs run on an explicit hub, workstation, simulator, or supported
peripheral target. They do not run inside the Visual Studio Code extension
host or a twin webview. A workstation program is a remote control source and
therefore has expiring leases; a hub program remains subject to the safety MCU,
local limits, and physical stop.

The program lifecycle is absent, staged, validated, installed, inactive,
starting, running, stopping, failed, or rolling-back. Every transition has a
request ID, actor, timestamp, result, prior revision, and resulting digest.

| ID | Requirement |
| --- | --- |
| PROG-001 | Every deployable program MUST have a versioned descriptor declaring program ID and revision, language, runtime and ABI, entry point, execution target, source and deployable artifact digests, required capabilities and authorization scopes, resource ceilings, lifecycle policy, and source/build provenance. |
| PROG-002 | Program source, dependency locks, build settings, generated outputs, released debug symbols, and SBOMs MUST be content-addressed project artifacts; editor state or a client database MUST NOT substitute for them. |
| PROG-003 | Ordinary programs MUST access hardware through published OMBR capability and control contracts and MUST NOT directly access output-enable signals, native peripheral buses, safety-controller registers, or unrestricted GPIO used by safety functions. |
| PROG-004 | A program targeting simulation and physical hardware MUST observe the same capability IDs, units, state layers, errors, time semantics, and authorization model. Target selection MUST be explicit and visible. |
| PROG-005 | Install, validate, activate, start, stop, restart, status, log, and removal operations MUST be available through public versioned service and CLI contracts rather than private UI commands. |
| PROG-006 | A runtime MUST enforce declared CPU, memory, storage, process, device, network, and log limits. Program failure or exhaustion MUST NOT prevent physical stop, watchdog service, hub recovery, or another program's safety handling. |
| PROG-007 | Arbitrary code upload, interactive debug, and shell-like access MUST be disabled by default and require an owner-authorized, auditable, time-bounded developer session. |
| PROG-008 | Program installation and activation MUST be transactional, health-checked, auditable, and rollback-capable; a failed revision MUST NOT replace the last known-good active revision. |
| PROG-009 | A visual-programming tool MUST store or export an openly specified editable graph or source representation and compile through the same program/package model; a proprietary editor-only file is insufficient. |
| PROG-010 | A workstation or remotely hosted program remains an expiring control source and MUST obey TIME requirements, arbitration, authorization, local limits, target boot identity, and disconnect neutralization. |
| PROG-011 | A descriptor MUST pin compatible OMBR specification, program-descriptor, capability-schema, SDK, and runtime-interface ranges. Unsupported ranges MUST fail before installation or activation. |
| PROG-012 | The reference toolchain MUST provide independently buildable Python and .NET/C# SDK examples for discovery, telemetry, bounded command, timeline activation, simulation target selection, and clean cancellation; other languages use the same public schemas. |
| PROG-013 | Dependency resolution MUST use a committed lock or equivalent complete resolution. A reproducible build MUST use verified cached or content-addressed inputs and MUST NOT silently consume newer registry content. |
| PROG-014 | Secrets MUST be referenced by owner-managed identifiers and injected at activation; secret values MUST NOT appear in source packages, descriptors, environment exports, logs, simulation snapshots, or twin recordings. |
| PROG-015 | Program stdout, stderr, diagnostics, metrics, and lifecycle events MUST be bounded, timestamped, attributable to program and boot IDs, streamable through public APIs, and exportable in an open format. |
| PROG-016 | Program exit, crash, kill, lease loss, debugger disconnect, target reboot, or supervisor failure MUST cancel its outstanding interactive commands and release its control and resource leases within published bounds. |
| PROG-017 | A deployable build MUST record source digest, builder identity, build type, complete declared inputs, parameters, runtime base, SBOM, output digest, and whether independent reproduction matched. |
| PROG-018 | Autostart or restart-always policy MUST require explicit owner activation and a locally enforceable safety policy; reinstall, workspace opening, editor connection, or source checkout MUST NOT arm or start motion. |
| PROG-019 | Debugging MUST preserve command expiry, arbitration, resource ceilings, local limits, watchdogs, and physical stop; a breakpoint MUST NOT leave an unbounded motion command active. |
| PROG-020 | The same program bundle MUST be deployable without semantic rewriting by the VS Code Workbench, headless CLI, or an independent client; target-specific build artifacts are selected by declared platform metadata. |

## 12. Wireless and local network interfaces

### 12.1 Roles

- BLE provides nearby and recovery discovery in 0.1. Commissioning and bounded
  local control remain experimental until GATT characteristics, procedures,
  fragmentation, rates, pairing, and recovery are frozen.
- Wi-Fi provides authenticated APIs, package transfer, development access,
  MQTT twin synchronization, and higher-rate telemetry.
- Wired USB provides development, recovery, diagnostics, and optional network
  gadget access.
- The native peripheral link connects hub ports to modules.

All transports map to the same capability meanings. A transport adapter MUST
NOT invent a different unit, sign, safe state, or identity.

### 12.2 Discovery

| ID | Requirement |
| --- | --- |
| NET-001 | A hub MUST advertise a project-assigned BLE service UUID and an mDNS service type after those identifiers are registered by project governance. |
| NET-002 | Discovery MUST reveal only instance ID, product class, protocol version, pairing state, and connection information needed to begin an authorized session. |
| NET-003 | A stable factory serial, Wi-Fi MAC, or other tracking identifier MUST NOT be advertised by default. |
| NET-004 | The local HTTPS service MUST expose a well-known document that links the hub manifest, Thing Description, API version, and owner-controlled display name. |

Version 0.1 does not invent permanent UUIDs or DNS service names before the
project has governance and a registry. Prototype identifiers MUST be clearly
marked experimental and MUST change before 1.0.

### 12.3 HTTP and event API

The planned reference API root is **/ombr/v1**. Before OMBR-HUB-1 becomes
claimable it will be described by a checked-in OpenAPI 3.1.1 document and JSON
Schema 2020-12 request/event schemas. Those artifacts do not yet exist in
version 0.1, so the following is an endpoint plan rather than an interoperable
API contract:

- hub status, health, clock, and versions;
- discovered, connected, and configured components;
- capabilities and current states;
- creation upload, validation, activation, and export;
- semantic diagnostics and revisioned project transactions;
- profile and timeline activation;
- installed runtimes, program staging, deployment history, lifecycle, logs,
  metrics, and time-bounded debug sessions;
- command submission, cancellation, and result;
- event and telemetry subscription over WebSocket;
- twin snapshots, layer and history queries, reconciliation, recording, replay,
  and trace export;
- calibration read and controlled update;
- logs, diagnostics, and conformance evidence;
- credential, pairing, and owner-key management; and
- update staging, validation, activation, and rollback.

| ID | Requirement |
| --- | --- |
| API-001 | Every state-changing request MUST be authenticated, authorized, bounded, and assigned a request or command ID. |
| API-002 | Motion commands MUST include a deadline and MUST NOT be retried after expiry. |
| API-003 | Batch commands MUST define atomicity; partial application without an explicit per-item result is forbidden. |
| API-004 | API errors MUST use stable machine-readable codes plus safe human-readable detail. |
| API-005 | Unknown optional JSON properties MUST be preserved or ignored according to the schema; unknown required extensions MUST fail validation. |
| API-006 | API versions MUST appear in routes and documents; firmware version MUST NOT be mistaken for protocol version. |
| API-007 | Network disconnection MUST NOT disable local stop, local autonomous control, or local safety. |
| API-008 | A project, profile, deployment, calibration, or twin mutation MUST carry an expected revision or equivalent optimistic-concurrency token; a conflict MUST be returned explicitly and MUST NOT be silently resolved by last writer wins. |
| API-009 | The reference Workbench MUST NOT depend on undocumented or privileged endpoints unavailable to an independently implemented authorized client, except a separately specified physical factory or recovery interface. |
| API-010 | Long-running validation, build, deployment, simulation, calibration, update, and replay operations MUST return a stable operation ID, progress, cancellation semantics, bounded logs, and a final machine-readable result. |
| API-011 | Developer and debug sessions MUST expose scope, actor, target, creation revision, start, absolute maximum lifetime, inactivity expiry, and revocation; closing a client MUST NOT leave an immortal session. |
| API-012 | Checked-in OpenAPI and event schemas MUST generate at least TypeScript, .NET, and Python client contract tests whose serialized requests and observable results match the same golden vectors. |

### 12.4 MQTT digital-twin profile

OMBR-TWIN-1 uses MQTT 5 when synchronizing through a broker. The broker MAY
run locally on the hub, another local machine, or an owner-chosen remote
service. MQTT behavior follows the
[OASIS MQTT 5.0 specification](https://docs.oasis-open.org/mqtt/mqtt/v5.0/mqtt-v5.0.html).

Topic root:

    ombr/v1/{owner-namespace}/{instance-id}/

owner-namespace is 1 to 63 lowercase ASCII letters, digits, or hyphens, starts
with a letter or digit, and contains no MQTT wildcard. instance-id is the
lowercase UUID text of the bound hub instance without the urn:uuid prefix.
Topic levels MUST NOT contain U+0000, slash, plus, or hash. Remote broker URIs
MUST use mqtts or wss; unencrypted mqtt is permitted only on an explicitly
loopback-bound local broker.

Required suffixes:

| Topic | Retain | QoS | Purpose |
| --- | --- | ---: | --- |
| presence | Yes | 1 | Online/offline last-will state and boot ID |
| reported | Yes | 1 | Device-authoritative bounded state snapshot |
| desired | Yes | 1 | Revisioned non-imperative desired configuration |
| telemetry/{stream} | No | 0 by default | High-rate, expiring measurements |
| events/{class} | No | 1 when loss matters | Fault, lifecycle, pairing, and user events |
| commands/{target} | Never | 1 | Expiring imperative command with ID and deadline |
| command-results/{client} | No | 1 | Correlated acceptance, completion, rejection, or expiry |

Imperative motor commands MUST NOT be retained. A device MUST reject expired,
duplicate, unauthorized, wrong-boot, or stale-sequence commands. Desired and
reported state use explicit revisions and acknowledgement; silent
last-writer-wins is forbidden for safety-relevant configuration.

MQTT is a bounded runtime-state, desired-configuration, event, and telemetry
transport. It is not a source-CAD editor, arbitrary file synchronization
channel, program uploader, or substitute for revisioned project transactions.
Source and package changes use content-addressed files or the authenticated
HTTPS transaction API.

## 13. Digital twin, CAD, and simulation

### 13.1 Twin layers

OMBR links four distinct representations:

1. **Design twin**: released geometry, interfaces, capabilities, mass,
   inertia, electrical limits, and simulation parameters.
2. **As-built and calibrated twin**: exact component instances, BOM variants,
   wiring, firmware, replacements, and calibration.
3. **Runtime twin**: desired, accepted, applied, reported, and observed state,
   events, health, and bounded history.
4. **Simulation twin**: immutable design snapshot, world, initial state,
   physics settings, controller build, seeds, and expected trace tolerances.

Runtime telemetry MUST NOT mutate source CAD or an immutable simulation
release. Calibration produces a new calibrated record linked to source and
procedure.

### 13.2 Semantic authority

The OMBR project manifest is authoritative for identity, relationships,
interfaces, capabilities, and asset hashes. Native CAD is authoritative for
editable geometry. SDF is authoritative for the version 1 simulation
interchange profile.

Visual Studio Code documents, `.vscode` settings, extension global state,
webview scene graphs, standalone-client databases, search indexes, previews,
and caches are projections or conveniences. They are never the only semantic
authority for a Creation, program, deployment, credential, or twin layer.

No external format is forced to represent manufacturing, assembly, runtime
state, and simulation by itself.

### 13.3 Project package

An OMBR project is a directory or ZIP-compatible archive with this logical
layout:

    project.ombr/
      manifest.json
      README.md
      LICENSES/
      parts/
        {model-id}/component.json
      assembly/
        creation.mpd
      cad/
        source/
        exchange/
      manufacturing/
        pcb/
        print/
        drawings/
      electronics/
        interfaces/
        endpoint-profiles/
        protocols/
        cots-and-derating/
        evidence/
        fault-matrices/
      visual/
        creation.glb
      simulation/
        model.sdf
        world.sdf
        run-manifest.json
      ros/
        robot.urdf
      control/
        profiles.json
        timelines.json
        safety-policies.json
      programs/
        {program-id}/
          program.json
          src/
          locks/
          build/
          symbols/
      deployments/
      recordings/
      twin/
        thing-description.json
      calibration/
      tests/
      sbom/

The archive contains relative paths only, rejects path traversal and
case-collision, and records SHA-256 for every normative or deployable asset.
The companion
[JSON Schema](schema/ombr-project.schema.json) and
[syntax-rover example](examples/syntax-rover.ombr.json) define the
minimum manifest structure for this draft.

The 0.1 schema is a syntax seed for design models and placements, not a
complete OMBR-TWIN-1 conformance schema. Before 0.2 it will split immutable
design placements from as-built deployment bindings, replacement history, and
runtime boot/state envelopes. A design placement's componentId is not a
physical instanceId, and MQTT is enabled only after a hub placement is bound
to an owner-resettable physical instance.

Its `programs` array is likewise a syntax seed for PROG-001 descriptors, not a
claimable OMBR-RUNTIME-1 contract. Runtime registries, signed deployment
records, secret bindings, debug sessions, and lifecycle event envelopes remain
separate versioned artifacts to define before 0.2.

The schema's `safetyPolicies` array gives profiles, timelines, and programs a
resolvable project authority for target safe values, command age, stop
behavior, neutral-before-arm, and re-arm policy. Semantic validation still has
to prove capability typing, complete actuator coverage, and compliance with
the immutable hardware and firmware ceilings.

The `interfaceContracts` and `electricalEndpointProfiles` arrays demonstrate
the ELEC split between a reusable connector/pin/protocol/cable contract and a
source-, sink-, bidirectional-, or passive-endpoint envelope. Every electronic
component interface references exact revisions of both. This 0.1 syntax is
deliberately verbose enough to expose ambiguous current ratings, negative
transients, source/load roles, protocol details, sequencing, and protection;
it is still a prototype pending stable field semantics, fixtures, fault-matrix
schema, compatibility algorithm, and measured golden hardware.

The current binding object is also a syntax seed: it serializes the simple
scale, offset, inversion, dead-zone, curve, clamp, priority, and arbitration
case. Active zones, mixing, rate and acceleration limits, stateful transforms,
and timeline/program triggers require the versioned transform registry, full
binding schema, and golden vectors scheduled in Phase 0; this file cannot
support an OMBR-CONTROL-1 claim.

#### Semantic validation

JSON Schema validates the portable document shape; it cannot prove all graph,
file, numeric, and physical relationships. An OMBR conformance validator
additionally enforces:

| ID | Requirement |
| --- | --- |
| PKG-001 | IDs that are unique by specification MUST be unique, and every artifact, model, component, frame, interface, capability, endpoint, and generator reference MUST resolve. |
| PKG-002 | Every packaged path MUST exist exactly once, remain inside the archive root, match case exactly, not be a symbolic or hard link, and match its declared digest and bounded size. |
| PKG-003 | The validator MUST check unit-quaternion tolerance, acyclic frame parentage, connected assembly rules, minimum not exceeding maximum, continuous limits not exceeding peak limits, and neutral/safe values matching their capability schema and bounds. |
| PKG-004 | Role and profile rules MUST enforce required physical, electrical, calibration, safety, and asset fields even when the common JSON syntax leaves them optional for another role. |
| PKG-005 | The base profile MUST reject a manifest over 8 MiB, JSON nesting deeper than 64, more than 20,000 archive entries, any single expanded entry over 1 GiB, total expanded content over 4 GiB, or an expansion ratio over 100 to 1 before materializing the archive. |
| PKG-006 | Extension payloads MUST be bounded to 256 KiB per extension and 128 extension keys in the base profile; a future large-data extension MUST reference a separately hashed artifact instead of embedding it. |
| PKG-007 | Every safety-policy reference MUST resolve to exactly one policy; every target MUST resolve to one capability, its safe value MUST type-check and remain in bounds, commanded actuators MUST be covered, and project values MUST NOT weaken role-profile or hardware ceilings. |
| PKG-008 | Every electronic interface contract, endpoint profile, connector variant, pin, rail, signal, protocol, source artifact, schema/vector, evidence, fault-matrix, and cable reference MUST resolve at the exact declared revision, and each endpoint profile MUST refer back to the same contract revision as the component interface. |
| PKG-009 | Electrical semantic validation MUST check operating and absolute voltage ordering, transient containment, temperature ordering, nonnegative consumption/capacity magnitudes, minimum/nominal/maximum current-limit ordering, continuous not exceeding peak, nonzero peak/inrush duration when their magnitude exceeds continuous/idle values, role-appropriate minimum-current semantics, rail-to-pin and signal-to-pin references, and explicit zero-versus-unknown status. |
| PKG-010 | An electrical connection MUST resolve source, every cable/adapter segment, and sink roles and prove the ELEC-017/ELEC-028 intersection including connector mating, contact map, return/shield, voltage at the load, all current states, fault energy, cable/thermal limits, signal/protocol/timing/topology/security, and aggregate source budget before it can be activated. |
| PKG-011 | Mechanical validation MUST resolve every catalog, part record, external alias, artifact, body, material/contact/load reference, interface, mating/fit profile and exact revision, and MUST report unavailable or legally non-redistributable optional assets without silently substituting them. |
| PKG-012 | Every mate MUST prove role/profile compatibility, orientation and symmetry, engagement and assembly path, fit/retention/detachability, tolerance stack, tool/removal access, collision/keep-out, and wrong/partial-mate outcome. |
| PKG-013 | Every assembly MUST validate roots, anchors, placement authority, body/frame endpoint resolution, joint axes/DOF/limits, initial configuration, closed-loop constraints, and absence of contradictory redundant poses within the declared solver tolerance. |
| PKG-014 | Dynamic validation MUST resolve body mass/inertia expression frames, material/contact assumptions, loads, supports, safety factors, stored energy, environmental/conditioned state and limiting-member evidence; explicit unknowns block fidelity/load claims that require them. |
| PKG-015 | Every transmission and network MUST validate member/profile revisions, geometry and centre/routing/tension constraints, ratio/sign/phase, efficiency/loss, backlash/compliance, speed/load limits, reflected properties, limiting member, actuator/sensor mapping, and derived summary against the graph. |
| PKG-016 | Every imported or derived mechanical asset MUST validate declared format edition/extensions, units/axes/handedness, scale/transform, semantic node mappings, authority/fidelity, source digest/generator, rights, and loss report; round-trip tests MUST prove that required IDs and semantics survive. |

URI format, SemVer, SPDX expressions, content digests, archive limits, and
cross-document references are semantic assertions. Validators MUST configure
format assertion or perform equivalent explicit checks; JSON Schema format
annotations and default values do not insert or validate these semantics by
themselves.

### 13.4 Asset formats

| Asset | Requirement and role |
| --- | --- |
| Native parametric CAD | REQUIRED preferred source for project-authored mechanical parts |
| STEP AP242, ISO 10303-242:2025 | REQUIRED neutral B-rep and assembly exchange for manufactured parts |
| 3MF Core 1.3.0 | REQUIRED printable derivative for released printable parts |
| STL | OPTIONAL legacy convenience derivative |
| glTF/GLB 2.0 | REQUIRED lightweight visualization derivative |
| LDraw File Format 1.0.2 LDR/MPD | REQUIRED for the OMBR brick-CAD exchange profile; optional for components outside that profile |
| SDFormat 1.12 | REQUIRED canonical simulation interchange target for the future OMBR-SIM-1 |
| URDF with exact parser and ROS distribution recorded | RECOMMENDED generated ROS compatibility export |
| OpenUSD with exact core specification and tool versions recorded | OPTIONAL scene composition and collaborative exchange |

Every artifact records its exact format edition and extensions. A newer format
edition is not silently accepted as equivalent; adding a permitted edition
requires a specification update and compatibility evidence.

glTF is a delivery format, not mechanical authority. URDF is useful for ROS
kinematic trees but is not the canonical brick assembly or world format. LDraw
parts retain their original attribution and license; project-authored parts
remain separately identified.

Relevant primary specifications include
[glTF 2.0](https://registry.khronos.org/glTF/specs/2.0/glTF-2.0.html),
[STEP AP242:2025](https://www.iso.org/standard/84300.html),
[SDFormat](https://sdformat.org/spec/),
[URDF](https://docs.ros.org/en/rolling/Tutorials/Intermediate/URDF/URDF-Main.html),
[3MF Core 1.3.0](https://3mf.io/spec/core-v1-3-0/), and the
[LDraw format](https://www.ldraw.org/article/218.html).

### 13.5 Frames and physical properties

| ID | Requirement |
| --- | --- |
| TWIN-001 | Every component, connector, joint, sensor, actuator output, and assembly root MUST have a stable named frame. |
| TWIN-002 | Transforms MUST use metres and unit quaternions in the manifest; Euler-angle UI values are derived. |
| TWIN-003 | A rigid component MUST declare mass, centre of mass, and inertia tensor with provenance or an explicit unknown state. |
| TWIN-004 | Visual and collision geometry MUST be separate assets or separately addressable nodes. |
| TWIN-005 | Mechanical connections MUST declare mating frames, allowed orientations, insertion depth, tolerance class, and detachability. |
| TWIN-006 | Electrical connections MUST identify both endpoints, cable model, port assignment, and negotiated profile. |
| TWIN-007 | Every derived asset MUST identify its source revision, generator and version, settings, and content digest. |
| TWIN-008 | A component replacement MUST create an as-built event and preserve prior history. |
| TWIN-009 | A twin viewer MUST visibly distinguish design, as-built or calibrated, runtime, and simulation layers and show source, target, boot ID, timestamp, quality, uncertainty, staleness, and provenance where applicable. |
| TWIN-010 | Authoring changes MUST identify their base revision and serialize as deterministic project edits; undo, redo, merge conflict, external-file change, and failed-save behavior MUST NOT create hidden viewer-only state. |
| TWIN-011 | Telemetry, discovery, simulation, and live overlays MUST NOT silently rewrite design source. Promoting an observation or calibration into an authoritative layer requires an explicit revisioned action and provenance. |
| TWIN-012 | A client cache, database, generated scene graph, thumbnail, or search index MUST be disposable and reconstructable from authoritative source, deployment records, and permitted runtime history. |
| TWIN-013 | Every live or simulated view and command surface MUST identify the selected Creation revision and exact physical or simulation target; combining values from targets without an explicit comparison mode is forbidden. |
| TWIN-014 | External assets MUST be content-addressed and license-identified before use. Offline mode MUST fail with a specific missing-artifact result rather than silently substituting a network or stale cache version. |
| TWIN-015 | The manifest's body, interface, mate, joint, transmission, assembly, and capability-binding graphs are semantic authority. CAD, LDraw, glTF, STEP, SDF, URDF, scene, and solver files are source or derivative artifacts with declared coverage; a tool MUST NOT infer missing normative mechanics from an opaque scene. |
| TWIN-016 | Design configuration, as-built measurements/calibration, initial simulation state, requested/accepted/applied control state, and observed runtime mechanical state MUST remain separate, revisioned layers with explicit promotion/reconciliation. |
| TWIN-017 | Mechanical asset generation and import/export MUST publish coordinate/semantic mappings and loss reports and pass identity, pose, joint, transmission, material/mass, collision, and capability-binding round-trip tests for every fidelity tier claimed. |

### 13.6 Reproducible simulation

Every released simulation includes:

- SDF and asset digests;
- simulator, physics engine, and plugin versions;
- OCI image digest where a container is provided;
- OS and CPU architecture;
- fixed time step and real-time factor;
- solver, contact, friction, and collision settings;
- gravity and environment;
- initial component and joint state;
- all random and sensor-noise seeds;
- controller or firmware build digest;
- expected trace metrics and numeric tolerances; and
- declared fidelity level.

OMBR does not promise bit-for-bit identical floating-point physics across
platforms. Conformance instead uses trajectories, timing, energy, collisions,
and sensor results with published tolerances.

Required reference scenarios include:

- unloaded motor step and ramp;
- loaded motor acceleration and safe stop;
- blocked motor and current-limit fault;
- steering position and return-to-neutral;
- distance sensor against reference surfaces;
- light intensity and color command replay;
- communication loss and heartbeat timeout;
- brownout isolation and Pi reboot;
- timeline playback and cancellation; and
- recorded hardware trace replay against the simulation model.

| ID | Requirement |
| --- | --- |
| SIM-001 | Version 0.1 simulation tolerances are characterization only and MUST NOT support an OMBR-SIM-1 conformance claim. |
| SIM-002 | A stable simulation profile MUST define named fidelity tiers with mandatory scenarios, metrics, sample windows, initial conditions, and maximum allowed error ceilings. |
| SIM-003 | Claimant-selected tolerances MAY accompany a report but MUST NOT replace or loosen the selected fidelity tier's ceilings. |
| SIM-004 | Conformance reference traces and acceptance calculations MUST be public, versioned, independently reproducible, and tied to exact hardware characterization data. |

### 13.7 Round-trip acceptance

The future golden ecosystem rover is complete only when:

1. the VS Code Workbench and headless CLI independently load the same project;
2. its brick and custom parts load from the project manifest;
3. mechanical and electrical connections validate;
4. a reference program builds to the same declared artifact digest through the
   Workbench and documented headless build;
5. the SDF model exposes the same capability IDs and units;
6. the program, Creation, and control profile run against the simulation target;
7. an interactive control lease expires to neutral during the remote-control test;
8. the unchanged manifest and program deploy to a physical hub;
9. discovered instances bind to manifest component IDs;
10. live state updates the runtime twin without mutating design source; and
11. hardware and simulation traces compare within the selected stable fidelity
    tier's mandatory limits; during version 0.1 the comparison is reported as
    characterization only.

## 14. Security, privacy, and owner control

### 14.1 Threat model baseline

The project MUST consider:

- an unauthenticated nearby BLE client;
- a hostile device on the local Wi-Fi network;
- a malicious or malformed peripheral;
- a compromised user program on Linux;
- package, firmware, or dependency tampering;
- a malicious project or editor workspace that attempts code execution;
- a compromised editor extension, tooling dependency, developer service, or
  simulator plugin;
- webview content injection, leaked editor secrets, and confusion between a
  local, remote, container, browser, simulator, or physical execution target;
- unauthorized program upload, debug, shell, firmware, or recovery access;
- replayed, duplicated, delayed, retained, or reordered commands;
- physical access to ports, storage, debug pads, and battery;
- a stolen hub or leaked backup;
- denial of service and resource exhaustion; and
- a cloud or broker outage.

### 14.2 Requirements

| ID | Requirement |
| --- | --- |
| SEC-001 | Initial ownership pairing MUST require physical presence through a button, removable secret, QR code, or equivalent local action. |
| SEC-002 | Every shipped unit MUST have unique credentials; default shared passwords are forbidden. |
| SEC-003 | Wi-Fi control MUST use authenticated encrypted transport and scoped authorization. |
| SEC-004 | If BLE commissioning or control is implemented experimentally, it MUST use authenticated pairing and application-level authorization for state-changing actions; stable BLE control requires the future interoperable GATT profile. |
| SEC-005 | Credentials MUST be revocable, rotatable, exportable where appropriate, and recoverable through an owner-controlled physical process. |
| SEC-006 | Commands MUST be protected against replay by session, boot ID, sequence, command ID, deadline, and authorization checks. |
| SEC-007 | Packages, schemas, and firmware MUST be size-bounded and validated before allocation, extraction, or activation. |
| SEC-008 | User applications MUST run with least privilege and MUST NOT gain direct access to safety-controller output enables. |
| SEC-009 | Updates MUST be signed and rollback-protected, while allowing the owner to add or replace trusted keys through a documented recovery path. |
| SEC-010 | A known-good recovery image or bootloader MUST remain available after failed update. |
| SEC-011 | Security events MUST be logged locally with bounded retention and no secret values. |
| SEC-012 | Safety MUST remain local and functional during authentication, broker, internet, or cloud failure. |
| SEC-013 | No telemetry, crash report, account, or analytics transmission MAY be enabled without informed owner action. |
| SEC-014 | The system MUST support complete local data export and deletion. |
| SEC-015 | An untrusted project or workspace MAY be parsed, schema-validated, and rendered with passive bounded viewers, but MUST NOT execute generators, build scripts, programs, tasks, simulator plugins, pairing, deployment, debug, firmware, recovery, or physical-control operations until explicit trust is granted. |
| SEC-016 | Private keys, pairing credentials, refresh tokens, and secret values MUST be stored outside projects in an OS-, editor-, hardware-, or service-protected owner store; workspace settings and source-controlled files are not credential stores. |
| SEC-017 | Custom HTML or webview content MUST use least capabilities, a restrictive content security policy, bounded local resource roots, no unapproved remote content, sanitization, schema-validated messages, and extension- or service-side authorization for every state change. |
| SEC-018 | Developer, debug, shell, factory, and recovery authority MUST be separately scoped, visibly active, time-bounded, revocable, and locally logged; ordinary remote-control authority MUST NOT imply any of them. |
| SEC-019 | A tool MUST display and authenticate the developer-service execution locus and target. `localhost` MUST NOT be assumed to mean the user's physical workstation in a remote, container, WSL, SSH, or browser workspace. |
| SEC-020 | Optional tooling telemetry MUST default off, respect the host editor or OS telemetry preference, publish its event schema, and exclude project content, source, secrets, stable device IDs, network identifiers, precise location, and raw sensor recordings. |
| SEC-021 | Extensions, plugins, simulators, generators, runtime bases, and SDK dependencies MUST be version-pinned, SBOM-listed, integrity-checked, and covered by the same vulnerability, provenance, update, and rollback policy as other executable artifacts. |
| SEC-022 | A project, program descriptor, or twin record MUST NOT place credentials in URI userinfo or any network endpoint field. It MAY name a logical credential slot, but the secret value and owner-store binding remain external; semantic validation MUST reject userinfo in every declared endpoint URI. |

The Zero 2 W secure-boot gap is a declared residual risk. OMBR-HUB-1 on that
compute profile can provide signed artifacts, measured hashes, least
privilege, and owner-controlled recovery, but MUST NOT claim hardware-verified
boot against an attacker who can replace storage.

### 14.3 Safety is not authentication

Authentication does not make a motion command safe. Every authorized command
still passes through:

1. schema and unit validation;
2. capability and range validation;
3. profile and arbitration;
4. safety policy and deadline checks;
5. safety-controller ceilings; and
6. local peripheral limits and watchdog.

## 15. Software, programming tools, firmware, and updates

### 15.1 Open tooling layers

The reference developer platform has replaceable layers:

1. **Portable source and schemas** define Creations, components, programs,
   profiles, assets, deployment records, and twin layers.
2. **Tooling core** implements parsing, semantic validation, migration, package
   construction, API clients, twin reconciliation, and build plans without a
   dependency on an editor or UI framework.
3. **Developer service (`ombrd`)** owns long-lived discovery, pairing,
   credentials, native USB/Bluetooth access, device sessions, simulator
   processes, recordings, and developer-session authorization.
4. **Headless CLI (`ombr`)** exposes the same operations for terminals, CI,
   teaching images, and independent clients.
5. **Tool clients** provide the Visual Studio Code Workbench, an optional
   cross-platform .NET application, and future editor or browser adapters.
6. **Hub services and program supervisor** expose the public hub API and remain
   independent from workstation UI code.

The reference tooling core and developer service may use .NET 10 because this
repository already targets it across shared, desktop, mobile, and Linux
projects. That implementation choice is not an OMBR wire requirement. An
independent implementation passes on observable schemas, API/CLI behavior,
and conformance evidence rather than internal language or framework.

The baseline CLI command families are `init`, `validate`, `migrate`, `pack`,
`doctor`, `discover`, `pair`, `build`, `simulate`, `deploy`, `run`, `debug`,
`logs`, `record`, `replay`, `twin`, `control`, `update`, `recover`, and
`evidence`. Exact options and JSON results remain a pre-0.2 contract.

| ID | Requirement |
| --- | --- |
| DEV-001 | Every substantive Workbench operation MUST use a published OMBR schema, API, SDK, developer-service protocol, or CLI contract; a conforming client MUST NOT depend on undocumented hub behavior. |
| DEV-002 | The tooling core MUST remain free of Visual Studio Code, MAUI, Avalonia, or another UI-framework dependency and MUST be independently usable by the CLI and tests. |
| DEV-003 | The developer service MUST expose its version, build digest, supported schema/API/runtime ranges, execution locus, native capabilities, and active sessions through a documented local protocol. |
| DEV-004 | The CLI MUST offer non-interactive operation, stable exit categories, machine-readable JSON output, cancellation, bounded logs, and no prompts when explicitly placed in automation mode. |
| DEV-005 | Device discovery, pairing, deployment, control, simulation, and twin reconciliation MUST produce equivalent semantic results whether initiated by the CLI, VS Code extension, or an independent authorized client. |
| DEV-006 | The developer service MUST be independently installable. A client-bundled copy MUST use the same versioned protocol and MUST NOT be the only distribution of native device access. |
| DEV-007 | Native USB, Bluetooth, serial, simulator, compiler, and container operations MUST execute in the declared service or target process, not a webview; an editor extension MUST NOT make native Node modules its only device backend. |
| DEV-008 | Local developer-service IPC MUST be user-scoped and protected against another local account. Network-exposed service endpoints MUST use authenticated encryption, explicit enablement, scoped credentials, and a published bind address. |
| DEV-009 | A client MUST negotiate hub API, project schema, developer-service, simulator, runtime, SDK, and debug-adapter capabilities before mutation and MUST provide a safe read-only fallback or specific incompatible-version result. |
| DEV-010 | A client that rewrites a project MUST preserve understood and unknown optional fields and extensions; inability to preserve them MUST force read-only behavior or an explicit, previewable migration. |

### 15.2 Visual Studio Code reference Workbench

The primary reference authoring environment is an open-source Visual Studio
Code extension. It integrates existing language support instead of inventing a
mandatory OMBR programming language. JSON Schema and semantic diagnostics
cover project files; a reusable Language Server Protocol implementation may
add cross-file completion, references, refactoring, and diagnostics. Build,
validate, simulate, deploy, and test operations appear as tasks. A supported
runtime may expose debugging through the editor-neutral Debug Adapter Protocol.

The digital twin is a custom text editor or view over authoritative project
files. Its 3D scene is a projection. Edits pass through the extension host and
normal document edit, save, undo, backup, conflict, and source-control
behavior; a webview cannot save private authoritative state.

| ID | Requirement |
| --- | --- |
| DEV-011 | The reference extension MUST cover project creation and validation, program build/deploy/debug, device discovery and pairing, component and capability inspection, bounded remote control, twin inspection, simulation launch or attach, telemetry, recording/replay, diagnostics, update, and recovery entry points. |
| DEV-012 | The extension MUST use stable public extension APIs and MUST NOT require proposed APIs for a stable release. It SHOULD be tested on Visual Studio Code and at least one compatible Code-OSS distribution. |
| DEV-013 | The extension source, dependency lock, build instructions, tests, SBOM, third-party notices, signed checksums, build provenance, and source-to-VSIX reproduction result MUST be published. An offline-installable VSIX MUST be available outside any marketplace. |
| DEV-014 | `.vscode` settings, extension caches, global state, window layout, custom-editor backup, and private client databases MUST NOT be required to interpret, build, deploy, or simulate a project. |
| DEV-015 | In an untrusted workspace the extension MUST enforce SEC-015 in command handlers as well as menus; hiding a command is insufficient because commands may be invoked indirectly. |
| DEV-016 | Credentials MUST use the editor's protected secret storage or the developer service's owner store and MUST NOT be synchronized through workspace settings. |
| DEV-017 | A twin custom editor MUST use the workspace document as authority, emit minimal deterministic edits, participate in undo/redo and save/backup, detect external changes, and never depend on a live webview to save. |
| DEV-018 | Webviews MUST enforce SEC-017, remain themeable and keyboard accessible, and send versioned schema-validated messages to the extension; they MUST NOT connect directly to a hub or broker with owner credentials. |
| DEV-019 | Desktop, remote, container, virtual, and web extension modes MUST publish a capability matrix. Browser or virtual mode MAY edit, validate, and render bounded assets but MUST disable native build, device, simulation, deployment, and control unless an explicitly authenticated developer service is reachable. |
| DEV-020 | The extension MUST show whether its service runs on the local UI machine, remote workspace, container, or another host and MUST show the separate physical or simulation target before every state-changing operation. |
| DEV-021 | An interactive control view MUST display connection, target, Creation revision, boot ID, armed state, controlling lease, desired and applied output, faults, limits, staleness, and a stop action. Window blur, view disposal, input loss, source disconnect, or lease expiry MUST neutralize continuous commands. |
| DEV-022 | Keyboard, gamepad, touch, and on-screen continuous controls MUST use explicit dead-man or sustained-input semantics and the TIME command lifetime; a stuck key, lost key-up event, backgrounded client, or closed laptop MUST NOT hold motion. |
| DEV-023 | Editor tasks and debug configurations generated from a project MUST use structured argument arrays or equivalently safe process invocation and MUST NOT concatenate untrusted workspace values into a shell command. |
| DEV-024 | The extension MAY integrate third-party Python, C#, C/C++, Rust, ROS, CAD, and simulator extensions, but core OMBR validation, packaging, device recovery, and export MUST remain usable without a proprietary extension. |
| DEV-025 | Extension telemetry MUST satisfy SEC-013 and SEC-020 and MUST remain functionally optional; declining telemetry MUST NOT disable any local feature. |

### 15.3 Alternative cross-platform .NET client

A standalone .NET application is a peer Workbench for mobile operation,
classroom use, dedicated operator consoles, and users who do not want an IDE.
BrickController2 is the interim partial implementation and compatibility
client. It may evolve toward this role by moving reusable OMBR model, API, and
service code out of its UI and vendor-specific receiver layers.

UI framework selection is non-normative. .NET MAUI officially covers Android,
iOS, macOS through Mac Catalyst, and Windows; this repository additionally has
an experimental GTK4 Linux head. A future desktop-first client may retain that
approach or use an open .NET UI such as Avalonia or Uno after platform,
accessibility, packaging, licensing, and long-term-maintenance evaluation.

| ID | Requirement |
| --- | --- |
| DEV-026 | A .NET Workbench claim MUST use the same public project, program, hub, developer-service, twin, and simulation contracts and pass the same golden workflow as the reference extension; shared appearance is not required. |
| DEV-027 | Every client release MUST publish an exact platform and feature matrix. An unsupported BLE, USB, simulator, CAD, debug, background-control, or recovery function MUST be disabled with an explanation rather than emulated unsafely. |
| DEV-028 | Mobile backgrounding, suspension, screen lock, process termination, network change, and Bluetooth loss MUST revoke interactive control leases and neutralize client-originated continuous motion. |
| DEV-029 | A client-specific database MAY cache discovery and presentation state, but canonical projects, deployment records, recordings, credentials, and twin authority MUST remain exportable through standard OMBR contracts. |
| DEV-030 | The VS Code extension, standalone .NET client, and CLI MUST each be removable without making owner hardware, projects, credentials, programs, recordings, or recovery images inaccessible to another conforming tool. |

### 15.4 Common software and update requirements

| ID | Requirement |
| --- | --- |
| SW-001 | Reference hub services MUST run on ARM64 Linux and SHOULD also run on desktop Linux for simulation and CI. |
| SW-002 | Hardware access MUST be separated from creation, capability, API, program, and twin logic through documented interfaces. |
| SW-003 | BrickController2 is an interim compatibility client. OMBR support MUST use public OMBR APIs and capability providers; behavior implemented only in its UI, SQLite model, or vendor-specific byte stream is non-normative. |
| SW-004 | Every public API, wire schema, package schema, program descriptor, developer-service protocol, CLI contract, and stored model MUST have an independent version. |
| SW-005 | Stored Creations, programs, deployments, calibration, and twin records MUST support validated migration with preview, backup, and rollback; silent destructive migration is forbidden. |
| SW-006 | Builds MUST record source commit, dependency locks, toolchain, build flags, SBOM, provenance, and output digest. |
| SW-007 | Firmware and hub-service updates MUST be staged, verified, activated, health-checked, and rolled back on failed boot or heartbeat. |
| SW-008 | The owner MUST be able to build and install the complete project-controlled OMBR software and firmware stack from public source. Required upstream binary firmware or boot components MUST be pinned, redistributable, hashed, SBOM-listed, replaceable when upstream permits, and explicitly outside the open-source claim. |
| SW-009 | The reference developer service and CLI MUST run locally on supported Windows, macOS, and Linux systems without an account; platform-specific native helpers MUST have source, reproducible packages, and a declared support matrix. |
| SW-010 | Plugins, generators, importers, and simulator adapters MUST declare permissions, inputs, outputs, versions, licenses, and trust requirements and MUST run with the least access practical for their function. |
| SW-011 | Release repositories MUST retain signed source tags, dependency locks, schemas, migration tools, recovery images, and prior compatible installers for the published support lifetime. |
| SW-012 | Security and safety fixes MUST include affected versions, mitigations, test evidence, coordinated disclosure status, and an owner-controlled update path; forced cloud enrollment is not an acceptable fix. |

The existing Linux headless host is a useful starting point for ARM64 BLE
access, but its current API is GATT-level and transitional. It is not the OMBR
developer-service or hub conformance API. OMBR layers the component,
capability, Creation, program, safety, and twin contracts above it. See
[the current headless API documentation](../../../docs/linux-headless-api.md).

## 16. Versioning, governance, and conformance

### 16.1 Versioning

Specification releases use semantic versioning:

- major: incompatible wire, schema, connector, electrical, identity, or
  required-behavior change;
- minor: backward-compatible capability, profile, or optional-field addition;
- patch: clarification or erratum with no required observable behavior change.

Released specifications are immutable. Errata are separate, linked documents.
A physical product exposes specification version, profile, model revision,
PCB revision, BOM revision, enclosure revision, bootloader version, firmware
version, and source-release location.

### 16.2 Extensions

The project maintains a public registry for:

- capability semantic types;
- units and value schemas;
- fault codes;
- message types;
- profile IDs;
- manifest extensions;
- BLE UUIDs and mDNS service names; and
- reserved electrical and connector features.

Extension identifiers are absolute HTTPS URIs under a namespace the publisher
controls, for example https://example.org/ombr/extensions/feature/1. The URI
contains the extension's major version. A document lists requiredExtensions
separately from optional extension payloads. Every required URI MUST have a
matching payload, and an implementation MUST reject a document if it does not
understand that exact required version.

### 16.3 Conformance claim

The following syntax is reserved for a future stable release and MUST NOT be
used by a version 0.1 implementation:

A claim has this form:

    OMBR {profile} {spec-version}; model {model-id}@{revision};
    tested by {suite-version}; report {digest-or-url}

Passing one profile does not imply another. Mechanical fit does not imply
electrical or behavioral compatibility.

### 16.4 Required conformance evidence

| Area | Evidence |
| --- | --- |
| Schema | JSON Schema validation, unknown-field, required-extension, size, and malicious archive tests |
| Wire protocol | Golden frames, version negotiation, fragmentation, duplicate, reorder, timeout, and malformed-message tests |
| Capability | Units, bounds, neutral, precision, access, stale data, quality, and typed-command tests |
| Control source | Descriptor, calibration, range/neutral, event order, rate, latency, mapping, authentication, focus/sleep/disconnect neutralization, feedback, battery, and accessibility tests |
| Safety | Boot inhibit, watchdog, stop latency, current limit, thermal limit, disconnect, brownout, and fault-latch tests |
| Network | Pairing, credential rotation, authorization, replay, rate limit, MQTT expiry, non-retained motion, and offline tests |
| Security and privacy | Malicious peripheral/project/package inputs, least privilege, local IPC, secret-store and no-userinfo checks, developer-session expiry, update integrity/rollback, dependency provenance, privacy export/delete, ownership transfer, physical recovery, and denial-of-service tests |
| Power source | Source identity, voltage/current/transient, isolation, fuse/protection, budget, connect/disconnect, foldback, brownout, reverse, charging/battery when present, thermal, service, and recovery tests |
| Cable and link assembly | Cable identity, pinout, continuity, voltage drop, current/thermal derating, bend, pull, strain, misconfiguration, topology, and fault tests |
| Mechanical | Catalog/rights resolution, body/mass/material/load records, datum and fit gauges, insertion/retention/removal, mates, joints/closed loops, multi-member transmissions, capability-state binding, collision/tool access, drop, cable pull, thermal/antenna, accessible replacement-fabrication, asset coordinate/semantic mapping and round-trip tests |
| Electronic interface | Resolved contract/profile closure; exact connector/contact/pinout; source-cable-sink intersection; operating, absolute, ripple, transient, sleep/idle/nominal/continuous/peak/inrush/limit/fault/leakage/backfeed envelopes; sequencing, grounding/isolation, hot plug, cable/signal margin, thermal derating, telemetry, COTS/alternate records, fault matrix, ESD/EMC evidence, and recovery tests |
| Motor | Torque-speed-current, control modes, encoder, backlash, thermal, stall, stop, lifetime, and sample variation |
| Light | Current, color/intensity, update rate, flicker, thermal, effects, and lifetime assumptions |
| Sensor | Range, accuracy, uncertainty, rate, latency, calibration, saturation, drift, frame, and simulator model |
| Adapter | Exact endpoint/revision matrix, power and signal translation, identity/capability mapping, unsupported features, wrong-device, fault, hot-plug, recovery, and failed-combination evidence |
| Twin | Hashes, frame graph, asset generation, deployment binding, state sync, replacement history, and round trip |
| Simulation | Pinned run manifest, reference scenarios, seeded noise, tolerance comparison, and hardware trace parity |
| Program runtime | Reproducible build, lock integrity, permission denial, resource exhaustion, lifecycle, rollback, crash, lease cleanup, logs, and debug-safety tests |
| Developer tooling | Headless CLI, VS Code, independent client, offline install, workspace trust, secret handling, target locality, conflict, round trip, and remote-control expiry tests |
| Whole ecosystem | Released hub/IMU, power source, two cables, input control, open-loop and feedback actuation, light, distance, color/reflectance, touch/force, mechanics, program, Workbench, twin, simulator, replacement/repair, recovery, and independent-build integration run |
| Legal/IP and market access | Exact revision/act/territory/date scope, mark/design/patent/copyright/database/license and protocol-research registers, counsel/competent-review evidence IDs, mitigations/residual risks, product classification, conformity route, technical file, declarations, traceability, and post-market owner |
| Open release and service | Editable-source audit, license/SPDX, prior-art ledger, reproducible builds, SBOM/provenance, mirrors, fixtures, repair/disassembly, spares, end-of-life, and primary-host-loss drill |
| Governance and documentation | Contribution/patent terms, public decision/appeal, registry allocation, conflicts, archives, security response, accessible/offline documentation, translation, and classroom-material review |
| Implementation catalog | Scoped family-coverage matrix, exact design/variant/offer identities, requirement matches, openness/license grades, current lifecycle/availability, critical alternates, BOM closure, change history, and unresolved-slot report |
| Cost and affordability | Dated quantity-tier BOM and quotes, landed COGS, yield/test pilot, NRE/compliance/support/warranty/channel/tax model, optimistic/base/stress cases, exact included/excluded SKU, actual advertised offer, and price-claim expiry |
| Market benchmark | Exact purchased revisions, provenance, raw measurements, fixtures, uncertainty, compatibility dimensions, lifecycle, and side-by-side reference report |

At least two independent interoperable implementations of the native link and
manifest MUST pass the public suite before the project declares version 1.0.
The reference VS Code Workbench and an independently implemented CLI or .NET
client MUST also complete the same golden project without private endpoints or
client-specific semantic state.

### 16.5 Governance minimum

Before 0.2, the project establishes:

| ID | Requirement |
| --- | --- |
| GOV-001 | Specifications, registries, reference source, issues, roadmap, decision records, conformance results, and release automation MUST be publicly readable without an account and available through documented mirrors. |
| GOV-002 | The project MUST publish a code of conduct, contribution guide, contributor license and patent terms, security policy, private reporting channel, trademark policy, and release/deprecation/support policy before accepting any normative interface or specification contribution. |
| GOV-003 | Contribution terms MUST preserve contributor copyright and downstream open-source rights and MUST NOT require exclusive assignment or enable a private edition to close community contributions. |
| GOV-004 | Specification contributions MUST carry an explicit royalty-free patent commitment appropriate to the adopted specification license, with disclosure, recusal, and conflict procedures reviewed by qualified counsel. |
| GOV-005 | Material technical decisions MUST have a public proposal, alternatives, evidence, review period, recorded rationale, named decision makers, conflict disclosures, and appeal path. |
| GOV-006 | Governance SHOULD include maintainers from multiple organizations and user roles. Quorum, voting, removal, succession, release authority, and emergency security authority MUST be documented. |
| GOV-007 | Public meetings MUST publish agenda and minutes or provide an equivalent asynchronous written process; participation MUST NOT require travel, paid membership, or a proprietary collaboration tool. |
| GOV-008 | Project trademarks and conformance marks MUST remain separate from copyright and patent permissions. Forks and independent implementations MAY describe factual compatibility without implying endorsement. |
| GOV-009 | Registry namespaces and extension allocation MUST use published, viewpoint-neutral criteria with collision, transfer, abandonment, and appeal rules. |
| GOV-010 | Donations, sponsorships, donated engineering, reference-hardware vendors, and maintainer financial conflicts MUST be disclosed without granting a sponsor private conformance exceptions or unpublished roadmap control. |
| GOV-011 | Normative releases are immutable, content-addressed, archived in more than one administrative domain, and accompanied by source, rendered text, schemas, tests, minutes, errata process, and verification instructions. |
| GOV-012 | Translation and accessible-format contributions SHOULD be first-class, but the canonical language and process for resolving translation conflicts MUST be declared for each release. |
| GOV-013 | Before any profile is claimable, its release MUST include a machine-readable matrix mapping claimant type to exact requirement IDs, mandatory, conditional, or not-applicable rules, allowed not-applicable rationale, evidence IDs, test versions, and aggregation logic. Prefix-level orientation tables are not a conformance calculation. |

## 17. Implementation catalog and cost model

### 17.1 Implementation catalog and candidate openness

The implementation catalog records what OMBR can reuse, adapt, purchase, or
must design to build the open reference system. The initial, deliberately
provisional match is published in
[IMPLEMENTATION-CATALOG.md](IMPLEMENTATION-CATALOG.md). It covers the
controller, power, native link, cables, motors and other actuators, lights,
sensors, mechanics, firmware, developer tooling, CAD, twin, and simulation.
Section 21 separately evaluates obtainable systems and components as
benchmarks; benchmark similarity is never implementation qualification.

“Exhaustive” is bounded and testable. For a release it means complete semantic
family coverage plus every selected design, variant, and procurement record in
the declared source/date/region scope. It does not mean scraping or
redistributing every third-party SKU, image, CAD file, or seller listing.
External catalog source, design identity, color/material element or SKU,
engineering model, and seller offer are separate records. A discontinued part
may still have stock; a current design may have no current offer.

OMBR uses this openness ladder for implementation candidates:

| Grade | Meaning |
| --- | --- |
| `OH-certified` | Complete editable design is released under an open-hardware license and has a current OSHWA certification record for the exact revision. |
| `open-design` | Complete editable design and commercial make/modify/distribute rights are published under an explicit open-hardware license, but no exact-revision certification is claimed. |
| `open-source-software` | Preferred source is under an OSI-approved software license with reproducible build and dependency evidence assessed separately. |
| `source-available` | Some source is published, but permissions or required editable artifacts do not satisfy the Open Source Hardware Definition or Open Source Definition. |
| `documented-COTS` | A purchasable closed component has adequate public datasheets and lifecycle evidence; it remains an explicit, replaceable non-open boundary. |
| `opaque` | Required information or permission is missing. An opaque part cannot be a mandatory constituent of the open reference path. |

| ID | Requirement |
| --- | --- |
| CAT-001 | Every catalog release MUST declare its source scope, regions, observation interval, inclusion and exclusion rules, record count, family-coverage matrix, identifier-mapping coverage, unresolved records, and whether it is a complete scoped inventory or a curated subset. |
| CAT-002 | Catalog data MUST distinguish source catalog, design identity, material/color/packaging variant or SKU, detailed engineering model, OMBR-native replacement, lifecycle claim, availability observation, and seller offer. These identities MUST NOT be collapsed into one part number. |
| CAT-003 | Every implementation candidate MUST record manufacturer and exact MPN or immutable project design ID, revision, lifecycle, official documentation, architecture role, interfaces, matched and unmet OMBR requirement IDs, evidence, limitations, security status, source/build status, and an openness grade from this section. Unknown values MUST be explicit. |
| CAT-004 | Every license and openness claim MUST name exact artifacts, holders, SPDX expressions or terms, preferred editable source, allowed commercial acts, and review date. Public documentation, a downloadable binary, a public repository, or an open SDK MUST NOT be used to label closed silicon or an incomplete board design open hardware. |
| CAT-005 | A selected component MUST have a versioned requirement-to-evidence match. Marketing similarity, connector shape, nominal voltage, or availability alone MUST NOT establish suitability. |
| CAT-006 | Each critical single-source component MUST have at least one evaluated alternate or a documented redesign/migration path, pin/software impact, qualification work, last-time-buy policy, and maximum acceptable interruption. |
| CAT-007 | Availability, lead time, lifecycle, and price are dated, regional observations with condition, quantity, MOQ, seller or quote, currency, tax, shipping, and evidence. They MUST NOT be immutable properties of the design. |
| CAT-008 | Project-controlled schematics, PCB, firmware, CAD, fixtures, protocols, and tests MUST expose and isolate every documented-COTS boundary sufficiently to replace it without changing unrelated public semantics. |
| CAT-009 | Third-party metadata, geometry, images, documentation, and measured data MUST each carry their own provenance, rights, redistribution/cache status, attribution, revision, digest where obtainable, and limitations. “No restriction found” MUST NOT be treated as permission. |
| CAT-010 | The catalog family matrix MUST cover at least compute, safety control, storage, power input and conversion, port protection and physical layer, connectors and cables, motor drive, motors and position actuators, lights, distance/proximity, color/reflectance, touch/force, orientation/motion, environmental sensors, user input, structural parts, pins/axles/joints, gears and other transmissions, wheels/tracks/rails, flexible/stored-energy parts, firmware, SDK/runtime, developer clients, CAD, twin, simulation, fixtures, repair, and compliance evidence. |
| CAT-011 | A stable reference release MUST resolve a currently obtainable, qualified candidate for every mandatory slot and publish complete BOM closure including passives, contacts, fasteners, consumables, programming/test fixtures, and licensed software. An acquisition class or generic marketplace description is not a resolved candidate. |
| CAT-012 | Catalog snapshots and changes MUST be content-addressed, diffable, archivable under their actual permissions, and refreshed at every specification/reference-hardware release. Restricted sources MAY be represented by factual metadata and links without mirroring restricted content. |

### 17.2 Affordability target and cost requirements

The cost target is a design gate, not a claim that prototype quantities already
meet it. `OMBR-AFFORDABLE-HUB-1` means an assembled, tested base controller is
offered at an advertised price of no more than **EUR 49.99 including the
applicable German VAT**, excluding delivery, for a named date and sales
channel. At 19% VAT, EUR 49.99 leaves approximately EUR 42.01 net revenue
before payment, channel, warranty, compliance, and development costs.

The target SKU contains the enclosure and brick/beam mounting features,
Raspberry Pi Zero 2 W, required storage, safety MCU and hardware output gate,
Wi-Fi and Bluetooth, IMU, protected logic and motor power paths, at least four
qualified native/controller ports, recovery path, indicators, buttons, and all
internal contacts and fasteners needed to operate as a controller. It excludes
delivery, external power supply or battery, charger, external cables, motors,
lights, sensors, gamepad, and loose construction elements. Any differently
scoped product MUST use a different cost-profile name.

| ID | Requirement |
| --- | --- |
| COST-001 | A reference-hardware decision MUST include a costed BOM for quantities 1, 10, 100, and 1000 plus the intended production lot. Each input MUST record exact MPN/revision, supplier or quote, currency, quantity break, MOQ, stock and lead time, price date and expiry, tax/shipping/duty basis, yield assumption, and qualified alternate. |
| COST-002 | Landed COGS MUST include compute and storage, PCB and PCBA, components, connectors and contacts, enclosure and fasteners, assembly, programming, calibration, functional test, fixtures amortization, expected yield/scrap/rework, inbound freight, duty and brokerage, licenses/royalties, packaging, and required accessories in the advertised SKU. |
| COST-003 | The commercial model MUST separately show landed COGS, non-recurring engineering, tooling/fixture and certification amortization, security/update support, warranty/returns reserve, payment/platform fees, distributor and retailer margins, outbound fulfilment, VAT and other taxes, net revenue, contribution margin, and cash requirement. A BOM subtotal MUST NOT be called a retail cost. |
| COST-004 | Every public price comparison MUST use the same included contents, tax and delivery basis, region, date, channel, warranty, and volume or state the differences prominently. Foreign-currency inputs MUST use a dated published rate plus a declared reserve. |
| COST-005 | `OMBR-AFFORDABLE-HUB-1` MUST satisfy the exact EUR 49.99 VAT-inclusive base-SKU scope above through an actual generally available offer; a coupon, subsidy, loss-leading batch, bare PCB, self-build BOM, mandatory add-on, or price excluding tax MUST NOT establish the claim. |
| COST-006 | Before the target is called feasible, a design-to-cost budget MUST demonstrate positive contribution margin in the stated direct or channel model after all COST-002 and COST-003 items. For direct sale in Germany, the draft planning ceiling is EUR 27 landed COGS at 1000 units; this planning value MUST be replaced by quotes and is not a conformance limit. |
| COST-007 | Community self-build, direct retail, distributor/retailer, education bundle, and complete starter-kit scenarios MUST be modeled separately. Success in one channel MUST NOT be generalized to another. |
| COST-008 | A complete starter kit containing power, cables, actuator, light, sensors, and construction elements has its own contents and price target; it MUST NOT be implied by the EUR 49.99 controller-only target. |
| COST-009 | Cost reduction MUST NOT remove the independent safe-stop path, required protection, rated contacts and conductors, owner recovery, test coverage, open editable source, legally required evidence, or declared thermal/current margin. |
| COST-010 | Cost gates MUST run at concept, architecture freeze, EVT, DVT, PVT, launch, and at least quarterly while sold, using optimistic, base, and stress cases for price, FX, yield, freight, warranty, and volume. Breach requires an explicit rescope, redesign, price/profile change, or stop decision. |
| COST-011 | Certification, legal/IP review, security maintenance, vulnerability handling, documentation, spares, warranty, and end-of-life obligations MUST be budgeted; volunteer labor or grants MAY be reported separately but MUST NOT be silently valued at zero in a sustainable retail model. |
| COST-012 | The affordability claim requires dated production quotes, a pilot build with measured yield/test time, an applicable compliance plan, and a documented sales-channel agreement or direct-fulfilment model. Until then its status is `target-unproven`. |

## 18. Reference implementation plan

### Phase 0: specification and bench proof

Deliver:

- this specification plus component, project, interface-contract,
  endpoint-electrical-profile, fault-matrix, safety-policy, control-binding,
  timeline, and program syntax schemas and golden vectors;
- draft ecosystem-release, compatibility-matrix, conformance-evidence,
  implementation-catalog, cost-model, and benchmark-record/report schemas;
- governance package covering the specification license, contribution and
  patent terms, code of conduct, decisions/appeals, security, trademark,
  registry, archival, release, and support processes;
- tooling-core skeleton, headless validator/packager CLI, and read-only VS Code
  project/twin explorer;
- machine-readable implementation-catalog and price-observation schemas,
  family-coverage matrix, first complete controller/peripheral/mechanical/
  software candidate match, and retained failed-candidate records;
- quote-ready four-port hub BOM and cost model at quantities 1, 10, 100, 1000,
  and intended production volume, with direct/channel scenarios and an
  explicit `OMBR-AFFORDABLE-HUB-1` gap report;
- benchmark record schema/template, fixture metadata contract, initial
  purchase/sample plan, and open-prior-art reuse ledger;
- Pi-to-safety-MCU protocol prototype;
- protected single-port power and communication bench board;
- connector candidate matrix and fixture plus complete P0 native-port,
  bench-source, bench-cable, hub-port, motor, and sensor contracts;
- semantic source-cable-sink compatibility validator with min/nom/max,
  continuous/peak/inrush/current-limit/fault/backfeed and protocol checks;
- simulated motor, light, and distance-sensor components;
- threat model and safety-state table; and
- initial territorial IP register and clean-room/benchmark evidence plan for
  connector, attachment, adapter, dataset, naming, controller architecture,
  mechanical library, and twin/tooling work; and
- initial product-classification and EU market-access matrix covering the
  maker/education intent, radio/EMC/environmental/cybersecurity duties, toy
  transition decision, economic-operator roles, test plan, and cost owner.

Exit criteria:

- command-to-safe-state works without Linux cooperation;
- schema and protocol golden vectors run in CI;
- every electronic interface in the bench Creation resolves a complete
  contract/profile closure and the validator rejects deliberately incompatible
  source, cable, connector, pinout, voltage/current, ground, and protocol cases;
- a simulated and bench component share one capability descriptor;
- CLI and extension report the same golden-project diagnostics;
- connector P0 limitations are documented by measurement; and
- contribution/patent terms are effective before normative contributions, with
  the public decision, registry, security, trademark, and archival processes
  operational; and
- the catalog has no unclassified mandatory slot, while every unresolved
  critical component, rights issue, regulation, and affordability assumption
  has an owner, evidence plan, and stop/redesign trigger.

### Phase 1: Hub R0 developer carrier

Deliver:

- replaceable Pi Zero 2 W;
- safety MCU;
- four prototype ports, with six-port layout study;
- separated compute and motor rails;
- protected removable development battery or bench-power input;
- BLE discovery or experimental commissioning, checked-in local HTTPS/event
  API schemas, and MQTT broker/client;
- first developer-service build, generated TypeScript/.NET/Python SDK
  contracts, VS Code device view, and lease-bounded remote control;
- open KiCad source, BOM, firmware, enclosure source, and fixtures;
- quote-backed P0 price book, measured assembly/test time and yield, and an
  updated direct-retail affordability gap without claiming EUR 49.99 unless
  every COST gate actually passes;
- complete COTS/derating records, interface contracts, endpoint profiles,
  electrical fault matrices, and characterization reports for the carrier,
  source, compute, recovery, and every exposed port; and
- transitional BrickController2 device provider using public OMBR contracts.

Exit criteria:

- four mixed modules enumerate and fail safe;
- motor stall does not reboot Linux;
- all outputs neutralize within the published timeout;
- CLI, VS Code, and BrickController2 compatibility client observe the same
  component identities and capability values;
- the owner can reimage and recover without vendor infrastructure;
- source-to-artifact build is reproducible;
- exact selected candidates, alternates, rights status, and market-access
  change triggers resolve in the release catalog;
- hub, source, cable, compute, and port electrical envelopes reproduce within
  their stated uncertainty and all four ports pass simultaneous-load, inrush,
  brownout, short, backfeed, hot-plug, and thermal-derating tests; and
- hub, source, control-input, and power benchmark records publish raw evidence.

### Phase 2: open peripheral starter set

Deliver:

- passive M motor and smart position motor;
- RGBW light;
- distance sensor;
- color or reflectance sensor and touch or force sensor;
- low-cost passive motor/light adapter;
- at least two cable lengths with open drawings, tooling, fixtures, and
  electrical/mechanical characterization;
- exact BOMs, source CAD, fixtures, and characterization datasets;
- complete ELEC contracts, endpoint profiles, COTS/derating records, fault
  matrices, protocol vectors, and source-cable-load evidence for every motor,
  light, sensor, adapter, and cable interface;
- SDF models with hardware trace comparison; and
- side-by-side cable, actuator, light, and sensor benchmark datasets and class
  ranges.

Exit criteria:

- each device passes common and role-profile tests;
- at least two independently built modules interoperate with Hub R0; and
- measured models meet their declared simulation tolerance.

### Phase 3: programmable mechanical and digital workflow

Deliver:

- metrology dataset and fit classes;
- brick-compatible hub, motor, light, sensor, cable-retention shells, and
  adapter plates with a licensed reference-part/frame library;
- FreeCAD or equivalent source, STEP, 3MF, GLB, LDraw, SDF, and URDF exports;
- complete VS Code Workbench for project/program authoring, build, deploy,
  debug, remote control, twin, simulation, recording, and recovery;
- independently installable developer service and CLI plus an independent
  .NET client completing the same golden workflow;
- Python and .NET/C# reference programs and openly specified visual-program
  interchange example;
- CAD-to-simulation-to-deployment workflow; and
- live twin viewer with recording and replay.

Exit criteria:

- the golden ecosystem rover passes the round-trip acceptance in section 13.7;
- the integration prototype publishes an ECO-001 through ECO-012 gap checklist
  with evidence for every delivered row and no unsupported ecosystem claim;
- multiple fabrication processes pass declared fit classes;
- all derived assets reproduce from released source; and
- mechanical benchmark records reproduce from published fixtures and analysis.

### Phase 4: specification 1.0

Deliver:

- production connector selected and qualified;
- complete hub, battery/power, cable, actuator, light, sensor, mechanical,
  firmware, program, Workbench, twin, simulation, repair, and education
  release manifest;
- hardware revisions that incorporate EMC, thermal, lifecycle, and safety
  results;
- qualified territorial IP review and recorded dispositions for the selected
  controller architecture, connector, attachments, gears, adapters, protocols,
  datasets, product name, and marks;
- final-SKU product classification, applicable conformity assessment,
  technical file, declarations/marking, traceability, cybersecurity/update
  support, incident/recall owner, and post-market budget for every intended
  territory and configuration;
- an actual generally available EUR 49.99 offer if and only if
  `OMBR-AFFORDABLE-HUB-1` passes, otherwise a published target-gap report and
  separately named/priced hub profile rather than a misleading affordability
  claim;
- two independent implementations;
- OSHWA certification submissions for eligible project-controlled reference
  hardware, without using certification as a safety claim;
- complete public conformance suite;
- security and regulatory engineering files for the intended market; and
- stable governance and conformance mark.

Exit criteria:

- a complete ecosystem starter release passes OMBR-ECOSYSTEM-1, including
  ECO-001 through ECO-012 and every required constituent, benchmark, and
  governance requirement under the published requirement/evidence matrices;
  and
- every version-1 profile claim is backed by the required public conformance
  record rather than prototype or roadmap status.

## 19. Decisions required before 0.2

| Decision | Options to prototype | Acceptance evidence |
| --- | --- | --- |
| Production native connector | Project-owned contact system, rugged commodity connector, other open/toolable design | Mating lifecycle, touch safety, insertion force, pull/bend, current derating, SI/EMC, hot plug, two-source/tooling, cost |
| Electronic contract/profile semantics | Fixed typed envelopes, condition/evidence records, reusable value profiles, or compatible combination | Complete source-cable-sink expression, zero/unknown/not-applicable handling, negative transient and reverse-energy support, semantic validation, compact runtime subset, round trip, and independent implementation |
| Native physical transport/topology | Independently switched point-to-point CAN FD segments, qualified shared CAN FD, alternative differential bus; Classic CAN only through an isolated adapter profile | Latency, bus load, hot plug, error confinement, cable topology, fault isolation, licensing, silicon availability, firmware complexity |
| Link trust boundary | Per-port physical isolation, authenticated hub sessions, or both | Spoofing, replay, malicious-node, key recovery, bus flood, and port-isolation tests |
| Link framing and versioning | Exact CAN IDs, canonical CBOR subset, fragmentation, CRC, HELLO selection, timing, and unknown-field rules | Golden vectors across two independent implementations plus malformed/version-skew tests |
| Fail-safe stop architecture | Independent watchdog and de-energize gate circuit options | Stuck MCU/GPIO, broken stop loop, power/reset, stop latency, restart inhibit, and regenerative-energy tests |
| Hub port count | Four native plus expander, or six native | PCB/enclosure fit, thermals, battery, simultaneous current, cost |
| Battery profile | Protected removable 2S Li-ion, LiFePO4, certified external pack | Safety, supply chain, charging, replaceability, motor performance, transport, age group |
| Compute production profile | Replaceable Zero 2 W, soldered wireless CM0, both | Availability, openness boundary, storage reliability, RF, repairability, assembly cost |
| OMBR-native replacement fabrication | FDM, resin, CNC, documented service bureau, hybrid | Fit distribution, strength, heat, RF, surface, lifecycle, accessible source/process, unit cost; production-mould engineering remains outside OMBR scope |
| Affordable hub architecture | Four fully protected ports; two-port affordable hub plus expander; owner-supplied-Pi community carrier; higher-priced full hub | Quote-backed BOM/COGS at all quantities, port isolation/current/thermal evidence, exact SKU contents, EUR 42.01 net-revenue budget at German EUR 49.99 gross, direct and channel margin, pilot yield/test time, compliance/support/warranty cost, stress case |
| Intended age and market | 14+ maker kit, education kit, child-directed toy | Formal product classification and applicable safety/compliance plan |
| BLE and local API | Discovery-only BLE versus full commissioning/control GATT; OpenAPI endpoint and event model | Two-client interoperability, security, fragmentation, recovery, rate, and offline tests |
| Simulation fidelity tiers | Kinematic, control, dynamic, and sensor tiers | Mandatory metrics, reference traces, sample windows, and maximum error ceilings |
| Developer-service protocol and locality | JSON-RPC over stdio or user-scoped socket, authenticated loopback HTTPS/WebSocket, gRPC, or compatible combination | Windows/macOS/Linux packaging, remote/container/browser locality, authentication, version negotiation, cancellation, streaming, recovery, and independent client |
| Workbench composition | Thin TypeScript extension plus .NET service/core, TypeScript core plus native helpers, or another UI-neutral split | Offline VSIX, Code-OSS test, startup/memory, native driver maintenance, reproducible build, CLI parity, and no editor-only semantics |
| Baseline program runtimes | Python and .NET/C# first; native, Rust, JavaScript, ROS, and visual profiles later | Pi resource use, deterministic cancellation, SDK parity, locks, SBOM, sandbox, debugging, and simulator/hardware golden programs |
| Program isolation | systemd/cgroup service, OCI container, restricted process sandbox, WebAssembly runtime, or profile-specific combination | CPU/memory/storage/process/network enforcement, device denial, startup, update, debugging, failure containment, and Pi overhead |
| Debug contract | DAP bridge per runtime, runtime-native adapter, trace-only debugging, or combination | Breakpoint safety, disconnect cleanup, source mapping, versioning, security, open redistribution, and physical/simulation parity |
| Project editing and conflicts | Whole-document revision, JSON Patch, domain commands, CRDT for selected records, or combination | Deterministic serialization, undo/redo, external edits, unknown-field preservation, merge conflicts, audit, and two-client tests |
| Standalone .NET UI | Evolve MAUI plus Linux GTK head, Avalonia, Uno, split platform heads, or no stable standalone profile | Windows/macOS/Linux/mobile matrix, accessibility, packaging, open licensing, performance, maintenance, and SDK/core reuse |
| Visual programming interchange | Open graph IR, generated ordinary source with reversible metadata, or both | Diffability, round trip, merge, accessibility, offline use, multi-editor implementation, and same program descriptor/runtime |
| Offline registry and archival | Static signed index plus content-addressed mirrors, OCI-compatible registry, package-manager adapters, or combination | Full export, mirror reconstruction, signature/key recovery, namespace governance, dependency closure, and primary-host loss drill |
| IP and clean-room strategy | Reuse with compatible permission, independently specified interface, clean-room implementation, license negotiation, geometry/protocol redesign, or feature omission | Qualified territorial patent/design/trademark/copyright/database review, provenance, reviewer separation where needed, and recorded mitigation |
| Specification license | Community Specification 1.0 or another patent-aware open specification agreement | Legal review, contributor patent commitment, adoption friction |

## 20. Success criteria

The concept has succeeded when a third party can, without private information:

1. reproduce the released hub, protected power path, cables, open-loop and
   feedback actuation, light, sensor, and brick-compatible enclosures from
   editable sources and public fixtures;
2. implement the native link and hub APIs from the specification and pass the
   public suite without copying reference code;
3. create a component in CAD with machine-readable mounting, capability, and
   simulation metadata plus complete resolved connector/contact/pinout,
   protocol, voltage/current, signal, sequence, protection, cable, evidence,
   and fault-matrix contracts for every electronic interface;
4. add it to a brick assembly, route its cables, validate fit and power, and
   simulate its declared behavior;
5. write and debug ordinary Python or .NET/C# code against the same typed SDK
   for simulation and physical targets;
6. build, package, deploy, run, stop, update, roll back, and export the
   unchanged Creation and program through the headless CLI;
7. repeat the same workflow and bounded remote operation through the open VS
   Code Workbench and an independent .NET or other client;
8. observe the same capability IDs, units, state layers, safety limits, and
   trace semantics in simulation and reality;
9. reconcile design, as-built, calibrated, runtime, and simulation twins
   without telemetry silently changing source;
10. repair, recalibrate, reflash, replace, and recover every project-controlled
    part with documented tools and preserve prior history;
11. mirror all required source, packages, CAD, firmware, documentation, and
    recovery artifacts and operate after every primary host or external service
    is unavailable; and
12. fork the project, implement compatible products, and participate in
    governance without private agreements or trademark dependence;
13. select every mandatory hardware, mechanical, and software constituent from
    a complete scoped catalog that distinguishes genuinely open artifacts from
    documented COTS, publishes alternates and lifecycle/offer evidence, and
    contains no opaque required dependency; and
14. show an auditable exact-SKU cost and market-access record: either the
    controller is actually offered under `OMBR-AFFORDABLE-HUB-1` with positive
    sustainable unit economics and all required safety/open/legal/regulatory
    evidence, or the release states the gap and uses a different price/profile
    without hiding mandatory costs.

## 21. Benchmark program and market context

This section is informative and deliberately evidence-bounded.
Official-source evidence was reviewed on 13 July 2026.

| System | Documented strength | Gap addressed by OMBR |
| --- | --- | --- |
| BrickController2 | Broad receiver support, transport abstraction, gamepad and HTTP inputs, profiles, sequences, normalized output, local storage | No hub PCB, open peripheral electrical standard, source CAD, or digital-twin contract |
| BuWizz 3.0 Pro | Six ports, high motor power, replaceable battery, current sensing, BLE, and brick-compatible mounting | Its official product page documents BLE, not Wi-Fi, and does not document an open PCB/CAD/BOM or digital-twin contract |
| Raspberry Pi Build HAT | Pi integration, four LPF2 ports, and an RP2040 handling low-level control; firmware is now open | It is an accessory rather than a complete open hub ecosystem, and LPF2 is not a complete open native connector specification |
| M5Stack | Standard module families, schematics, software libraries, Grove and M-Bus interfaces, and a coherent maker ecosystem | It is not a Pi/Linux-based brick-mechanical system |
| SPIKE/MINDSTORMS large hub | Six ports, BLE/USB, IMU, removable battery, and brick geometry | Official specifications do not document Wi-Fi or an open, reproducible hardware design |

The defensible opportunity statement is:

> None of the compared official product documentation demonstrates the full
> combination of Raspberry Pi-class Linux compute, Wi-Fi and Bluetooth,
> brick-compatible mounting, open carrier and peripheral hardware, an open
> native wired peripheral contract, and synchronized CAD/simulation assets.

BuWizz facts are drawn from its
[official product page](https://buwizz.com/shop/buwizz-3-0-pro/). M5Stack's
[ecosystem documentation](https://docs.m5stack.com/en/learn/intro) describes
its module and interface approach. Raspberry Pi documents the
[Build HAT](https://www.raspberrypi.com/products/build-hat/) and its
[open firmware](https://github.com/raspberrypi/buildhat). LEGO Education's
[SPIKE Prime Large Hub technical specification](https://assets.education.lego.com/v3/assets/blt293eea581807678a/bltf512a371e82f6420/5f8801baf4f4cf0fa39d2feb/techspecs_techniclargehub.pdf)
supports the corresponding comparison row.

### 21.1 BrickController2 concepts retained, not imposed

OMBR retains these repository concepts while removing vendor-specific
assumptions:

- A **Creation** remains the user-owned unit of a robot or model.
- A **Control Profile** remains a collection of input-to-action bindings.
- A **Control Source** exposes typed, bounded capabilities rather than a fixed
  gamepad layout.
- A **Binding** maps one capability to one or more actuator actions and may
  apply inversion, dead zones, curves, scaling, limiting, and mixing.
- A **Timeline** retains reusable value/duration control points, looping, and
  interpolation.
- A **Component** replaces a hard-coded receiver type.
- A **Capability** replaces assumptions that every channel is a motor.
- A stable instance UUID replaces the current device-type-plus-BLE-address
  identity.
- Requested, accepted, applied, and observed values are distinct states.

The source model is visible in
[Creation.cs](../../../BrickController2/BrickController2/CreationManagement/Creation.cs),
[ControllerProfile.cs](../../../BrickController2/BrickController2/CreationManagement/ControllerProfile.cs),
[ControllerAction.cs](../../../BrickController2/BrickController2/CreationManagement/ControllerAction.cs),
[Sequence.cs](../../../BrickController2/BrickController2/CreationManagement/Sequence.cs),
and the generic
[HTTP capability model](../../../BrickController2/BrickController2/InputDeviceManagement/HttpControl/HttpControlModels.cs).

BrickController2 is a useful compatibility client and proving ground for
device discovery, profiles, gamepads, sequences, and remote-control behavior.
Its MAUI views, local database, concrete receiver classes, and application
lifecycle are not normative OMBR interfaces. A hub, project, program, digital
twin, or simulator MUST remain usable through the public schemas and APIs
without installing BrickController2 or Visual Studio Code.

### 21.2 Purchasable-component benchmark program

OMBR reference classes are grounded in components that builders can buy or
obtain, not only idealized requirements. The benchmark catalog records exact
product and hardware revision, purchase date and region, lifecycle state,
official documentation, independently measured samples, uncertainty, and the
OMBR profile or adapter behavior being compared. Availability and price are
evidence fields, never permanent conformance properties.

The initial informative benchmark set is:

| OMBR area | Purchasable or documented reference examples | What is benchmarked, not copied |
| --- | --- | --- |
| Hub/controller | [BuWizz 3.0 Pro](https://buwizz.com/shop/buwizz-3-0-pro/), [LEGO Hub 88009](https://www.lego.com/de-de/product/hub-88009), [M5Stack controller/module families](https://docs.m5stack.com/en/learn/intro) | Envelope, mounting, ports, battery service, radio workflow, responsiveness, ecosystem modularity |
| Battery, charging, and bench power | Replaceable packs in current brick hubs, documented USB-C power paths, and certified current-limited maker bench supplies | Energy, voltage/current, service, charging while operating, protection, thermal behavior, fault energy, connector and lifecycle |
| Human input and remote control | Generic USB/Bluetooth HID gamepads, [LEGO Powered Up Remote 88010](https://www.lego.com/de-de/product/remote-control-88010), keyboards, touch and assistive inputs | Capability descriptors, range/neutral, rate, latency, mapping, disconnect, battery, accessibility and feedback |
| Passive motion | Currently sold PF-style M, L, XL, buggy, train, and micro motors from multiple vendors | Envelope class, mounting, axle output, voltage, speed, current, torque, thermal behavior, cable exit |
| Feedback motion and steering | [LEGO Technic Large Motor 88013](https://www.lego.com/de-de/product/technic-large-motor-88013) and currently sold compatible position motors or steering servos | Position topology, range, zeroing, repeatability, backlash, speed, load, current, safe stop; connector shape alone is not equivalence |
| Lights | [LEGO Powered Up Light 88005](https://www.lego.com/de-de/product/light-88005), PF-style dual lights, SBrick Light, addressable RGB modules | Mounting, intensity/color capability, current, update rate, thermal behavior, effects, cable handling |
| Distance/proximity | [M5Stack ToF4M](https://shop.m5stack.com/products/time-of-flight-distance-unit-vl53l1x) and [ultrasonic units](https://shop.m5stack.com/products/ultrasonic-distance-unit-i-o-rcwl-9620), plus legally acquired brick-system sensors | Range, field of view, surface/ambient sensitivity, rate, latency, uncertainty, saturation, envelope and mounting |
| Color/reflectance | [M5Stack Color Unit](https://shop.m5stack.com/products/color-unit) and [reflective-sensor units](https://shop.m5stack.com/products/infrared-reflective-sensor-unit) | Illumination geometry, raw and calibrated channels, color space, distance, ambient rejection, rate and repeatability |
| Motion/orientation | [M5Stack 6-axis IMU Unit](https://shop.m5stack.com/products/6-axis-imu-unitmpu6886) and hub-integrated IMUs | Axis frame, selectable range, noise, bias, drift, rate, latency, calibration and temperature behavior |
| Force/touch | [M5Stack scale kit with Weight Unit and load cells](https://shop.m5stack.com/products/scale-kit-with-weight-unit), exact switches, and bumper/contact sensors selected by the acquisition plan | Force range, overload, creep, hysteresis, rate, mechanics, calibration and replacement; an HX711 front end alone is not a force reference |
| Cables/adapters | Current PF-style and Powered Up extension leads, Grove/HY2.0 modules, and project P0 cables | Usable length, routing, bend/pull, contact lifecycle, voltage drop, keying, repair, identification and adapter boundaries |
| Brick mechanics | Selected genuine studless beams, pins, axles, bricks and plates plus M5Stack units with documented compatible holes | Grid, fit distributions, insertion/retention, envelope, mounting access, wear, material/process variation and legally redistributable reference frames |

The repository's current
[controllers and powered equipment survey](../../../docs/controllers-and-equipment.md)
is a seed catalog, not conformance evidence by itself. It distinguishes
Power Functions-style power/control, Powered Up/LPF2 identification and
feedback, and incompatible servo signaling that a similar plug can hide.
Rows without an exact linked model are acquisition classes only; Phase 0 must
replace them with archived manufacturer evidence and purchased exact SKUs,
revisions, dates, and regions before they define any numeric target.

| ID | Requirement |
| --- | --- |
| BENCH-001 | Every benchmark record MUST identify manufacturer, product name and number, hardware and firmware revision where observable, acquisition source, purchase or observation date, region, sample count, lifecycle state, official source URLs plus archived capture and content digest, and whether each datum is documented, measured, inferred, or unknown. |
| BENCH-002 | Raw measurements, fixture source, instrument and calibration data, environment, supply conditions, procedure, uncertainty, analysis code, photos permitted for redistribution, and anonymized sample results MUST accompany a published benchmark conclusion. Data, code, fixture, image, and report licenses MUST be explicit. |
| BENCH-003 | A reference class MUST specify which dimensions are envelope, attachment, electrical, protocol, capability, performance, usability, or lifecycle targets. Meeting one dimension MUST NOT imply another. |
| BENCH-004 | Claims MUST use `benchmark-comparable`, `mechanically compatible`, `electrically adaptable`, `protocol adaptable`, `behaviorally compatible`, or `drop-in compatible` precisely. `Drop-in compatible` requires every relevant mechanical, electrical, protocol, behavioral, safety, and performance test. |
| BENCH-005 | A proprietary purchased product MAY be a measurement reference or supported adapter endpoint, but MUST remain an identified COTS boundary and MUST NOT become required closed source for the open starter workflow. |
| BENCH-006 | Benchmark geometry and behavior MUST be independently measured or taken from redistribution-permitted sources. The project MUST NOT copy logos, firmware, PCB artwork, ornamental housing surfaces, or restricted CAD. Independent measurement alone does not grant patent, design, trademark, copyright, database, or other rights. |
| BENCH-007 | A numeric stable class target MUST be supported by at least three physical samples across two lots or documented revisions. If that evidence cannot be obtained, the target remains provisional and cannot support a stable benchmark or compatibility profile. The report MUST separately state sample count, lots/revisions, and number of independent products and manufacturers. |
| BENCH-008 | A component class MUST publish minimum, target, and stretch ranges where appropriate rather than selecting one competitor's accidental value as a universal requirement. Safety ceilings remain absolute and are not benchmark averages. |
| BENCH-009 | Adapters MUST identify exactly which mechanical, electrical, identity, feedback, command, calibration, and update features they translate, pass through, emulate, or do not support. |
| BENCH-010 | The catalog MUST retain discontinued and failed combinations with lifecycle and evidence status so builders can repair old systems and avoid repeating unsafe compatibility assumptions. |
| BENCH-011 | Availability evidence MUST be refreshed for each specification release and marked by date and region. A retail listing alone MUST NOT establish electrical, behavioral, safety, or open-hardware compliance. |
| BENCH-012 | The first stable starter family MUST publish side-by-side benchmark reports for its hub, battery/charger or bench source, control input, each cable class, mechanical attachment set, open-loop motor path, feedback actuator, light, and every included sensor against at least one obtainable functional reference. |

## 22. Primary references

### Repository

- [BrickController2 README](../../../README.md)
- [Controllers and powered equipment](../../../docs/controllers-and-equipment.md)
- [OMBR implementation catalog](IMPLEMENTATION-CATALOG.md)
- [OMBR legal, IP, and market-access register](LEGAL-IP-REGISTER.md)
- [Linux headless API](../../../docs/linux-headless-api.md)
- [Unofficial BuWizz protocol notes](../../../BuWizz_protocol.md)

### Compute and hardware

- [Raspberry Pi Zero 2 W product brief](https://datasheets.raspberrypi.com/rpizero2/raspberry-pi-zero-2-w-product-brief.pdf)
- [Raspberry Pi Zero 2 W product information portal](https://pip.raspberrypi.com/categories/584-raspberry-pi-zero-2-w)
- [Raspberry Pi RP2040 specifications](https://www.raspberrypi.com/products/rp2040/specifications/)
- [Raspberry Pi Pico SDK](https://github.com/raspberrypi/pico-sdk)
- [Raspberry Pi Build HAT](https://www.raspberrypi.com/products/build-hat/)
- [Build HAT serial protocol](https://datasheets.raspberrypi.com/build-hat/build-hat-serial-protocol.pdf)
- [Build HAT firmware](https://github.com/raspberrypi/buildhat)
- [Raspberry Pi HAT+ specification](https://datasheets.raspberrypi.com/hat/hat-plus-specification.pdf)
- [Raspberry Pi Compute Module Zero](https://www.raspberrypi.com/products/compute-module-zero/)
- [Raspberry Pi boot security guide](https://pip.raspberrypi.com/categories/685-whitepapers-app-notes-compliance-guides/documents/RP-003466-WP/Boot-Security-Howto.pdf)
- [M5Stack ecosystem overview](https://docs.m5stack.com/en/learn/intro)
- [BuWizz 3.0 Pro](https://buwizz.com/shop/buwizz-3-0-pro/)
- [SPIKE Prime Large Hub technical specification](https://assets.education.lego.com/v3/assets/blt293eea581807678a/bltf512a371e82f6420/5f8801baf4f4cf0fa39d2feb/techspecs_techniclargehub.pdf)
- [TI TPS3431 watchdog](https://www.ti.com/product/TPS3431)
- [TI DRV8876 motor driver](https://www.ti.com/product/DRV8876)
- [TI TPS25947 eFuse](https://www.ti.com/product/TPS25947)
- [TI TPS54302 buck converter](https://www.ti.com/product/TPS54302)
- [Microchip MCP2518FD CAN FD controller](https://www.microchip.com/en-us/product/mcp2518fd)
- [Microchip MCP2562FD CAN FD transceiver](https://www.microchip.com/en-us/product/MCP2562FD)
- [Molex Micro-Fit 3.0 eight-circuit header](https://www.molex.com/en-us/products/part-detail/0430450800)
- [Molex Micro-Fit 3.0 eight-circuit receptacle](https://www.molex.com/en-us/products/part-detail/430250800)
- [Molex Micro-Fit 3.0 20–24 AWG female contact](https://www.molex.com/en-us/products/part-detail/430300001)
- [Molex Micro-Fit 3.0 product specification](https://www.molex.com/content/dam/molex/molex-dot-com/products/automated/en-us/productspecificationpdf/430/43045/PS-43045-001.pdf)

### Open hardware and licensing

- [Open Source Hardware Definition](https://oshwa.org/definition/)
- [OSHWA sharing best practices](https://oshwa.org/resources/sharing-best-practices/)
- [OSHWA Certification Requirements](https://certification.oshwa.org/requirements.html)
- [CERN Open Hardware Licence variants](https://ohwr.org/licences/)
- [Community Specification 1.0](https://github.com/CommunitySpecification/1.0)
- [Open Web Foundation Agreement 1.0](https://www.openwebfoundation.org/the-agreements/the-owf-1-0-agreements-granted-claims/owfa-1-0)
- [REUSE specification](https://reuse.software/spec/)
- [Reproducible Builds documentation](https://reproducible-builds.org/docs/)
- [SLSA 1.2](https://slsa.dev/spec/v1.2/)
- [SPDX 3.0](https://spdx.dev/use/specifications/)

### Open robotics and reuse precedents

- [Pybricks](https://pybricks.com/learn/intro/story-mission/)
- [ev3dev](https://www.ev3dev.org/)
- [Open Roberta Lab](https://www.open-roberta.org/about/)
- [KiCad](https://www.kicad.org/)
- [FreeCAD](https://www.freecad.org/)
- [OpenSCAD](https://openscad.org/)
- [LeoCAD](https://www.leocad.org/)
- [ROS 2 documentation](https://docs.ros.org/en/rolling/)
- [Gazebo documentation](https://gazebosim.org/docs/latest/getstarted/)

### Protocols and schemas

- [W3C Web of Things Thing Description 1.1](https://www.w3.org/TR/wot-thing-description11/)
- [MQTT 5.0](https://docs.oasis-open.org/mqtt/mqtt/v5.0/mqtt-v5.0.html)
- [CBOR, RFC 8949](https://www.rfc-editor.org/rfc/rfc8949.html)
- [UUIDs, RFC 9562](https://www.rfc-editor.org/rfc/rfc9562.html)
- [JSON Schema 2020-12](https://json-schema.org/draft/2020-12)
- [OpenAPI 3.1](https://spec.openapis.org/oas/v3.1.1.html)
- [Bosch CAN FD overview](https://www.bosch-semiconductors.com/products/ip-modules/can-protocols/can-fd/)
- [CAN security limitation](https://can-cia.org/services/publications/can-community-news/09-2025)
- [USB Type-C specification](https://www.usb.org/document-library/usb-type-cr-cable-and-connector-specification-release-25)

### Developer tooling

- [Visual Studio Code Extension API overview](https://code.visualstudio.com/api/extension-capabilities/overview)
- [Code - OSS source and license](https://github.com/microsoft/vscode)
- [VS Code extension hosts](https://code.visualstudio.com/api/advanced-topics/extension-host)
- [Remote extensions](https://code.visualstudio.com/api/advanced-topics/remote-extensions)
- [Web extensions](https://code.visualstudio.com/api/extension-guides/web-extensions)
- [Virtual workspaces](https://code.visualstudio.com/api/extension-guides/virtual-workspaces)
- [Custom editors](https://code.visualstudio.com/api/extension-guides/custom-editors)
- [Webview security](https://code.visualstudio.com/api/extension-guides/webview)
- [Workspace Trust](https://code.visualstudio.com/api/extension-guides/workspace-trust)
- [VS Code API and SecretStorage](https://code.visualstudio.com/api/references/vscode-api)
- [Extension telemetry](https://code.visualstudio.com/api/extension-guides/telemetry)
- [Task providers](https://code.visualstudio.com/api/extension-guides/task-provider)
- [Language Server Protocol](https://microsoft.github.io/language-server-protocol/)
- [Debug Adapter Protocol](https://microsoft.github.io/debug-adapter-protocol/)
- [VS Code extension packaging and VSIX distribution](https://code.visualstudio.com/api/working-with-extensions/publishing-extension)
- [.NET MAUI supported platforms](https://learn.microsoft.com/en-us/dotnet/maui/supported-platforms?view=net-maui-10.0)
- [Avalonia supported platforms](https://docs.avaloniaui.net/docs/supported-platforms)
- [Avalonia source and license](https://github.com/AvaloniaUI/Avalonia)
- [Uno Platform requirements](https://platform.uno/docs/articles/getting-started/requirements.html)
- [Uno Platform source and license](https://github.com/unoplatform/uno)

### CAD and simulation

- [LEGO piece-number guidance](https://www.lego.com/en-gb/service/help-topics/article/identifying-lego-set-and-piece-numbers)
- [LEGO Pick a Brick](https://www.lego.com/en-us/pick-and-build/pick-a-brick)
- [LEGO Technic building guidance](https://www.lego.com/en-ca/service/help-topics/article/tips-for-building-with-lego-technic-elements)
- [LEGO Technic measurement chart](https://www.lego.com/cdn/product-assets/product.bi.additional.info.pdf/42055_X_Measurements.pdf)
- [BrickLink Studio introduction](https://studiohelp.bricklink.com/hc/en-us/articles/5404381697559-Introduction-to-Studio)
- [BrickLink Studio license](https://studiohelp.bricklink.com/hc/en-us/articles/6606313426711-Studio-Software-License-Agreement)
- [LDraw Parts Library](https://library.ldraw.org/)
- [LDraw file format](https://www.ldraw.org/article/218.html)
- [LDraw legal and parts-library information](https://www.ldraw.org/legal-info)
- [glTF 2.0](https://registry.khronos.org/glTF/specs/2.0/glTF-2.0.html)
- [STEP AP242:2025](https://www.iso.org/standard/84300.html)
- [SDFormat](https://sdformat.org/spec/)
- [URDF](https://docs.ros.org/en/rolling/Tutorials/Intermediate/URDF/URDF-Main.html)
- [3MF Core 1.3.0](https://3mf.io/spec/core-v1-3-0/)
- [Open Container Initiative image specification](https://specs.opencontainers.org/image-spec/)

### Legal, IP, regulation, and cost

- [LEGO Fair Play](https://www.lego.com/en-in/legal/notices-and-policies/fair-play)
- [EU Trade Mark Regulation](https://eur-lex.europa.eu/legal-content/EN/TXT/?uri=CELEX:32017R1001)
- [EU Designs Regulation, consolidated 1 July 2026](https://eur-lex.europa.eu/legal-content/EN/TXT/?uri=CELEX:02002R0006-20260701)
- [EPO Espacenet](https://www.epo.org/en/searching-for-patents/technical/espacenet)
- [EPO legal-event data](https://www.epo.org/en/searching-for-patents/helpful-resources/first-time-here/legal-event-data)
- [WIPO PATENTSCOPE](https://www.wipo.int/en/web/patentscope/)
- [WIPO Global Brand Database](https://www.wipo.int/en/web/global-brand-database)
- [EUIPO IP search](https://www.euipo.europa.eu/en/search-ip)
- [EU Radio Equipment Directive](https://eur-lex.europa.eu/eli/dir/2014/53/oj/eng)
- [EU General Product Safety Regulation](https://eur-lex.europa.eu/eli/reg/2023/988/2026-05-29/eng)
- [EU Cyber Resilience Act](https://eur-lex.europa.eu/eli/reg/2024/2847/2024-11-20/eng)
- [EU Batteries Regulation](https://eur-lex.europa.eu/eli/reg/2023/1542/oj)
- [EU Toy Safety Regulation](https://eur-lex.europa.eu/legal-content/EN/TXT/?uri=CELEX:32025R2509)
- [EU VAT rules and rates](https://europa.eu/youreurope/business/taxation/vat/vat-rules-rates/index_en.htm)
- [ECB exchange-rate methodology](https://data.ecb.europa.eu/methodology/exchange-rates)

## Appendix A. Requirement summary

The normative requirements in this draft are grouped by prefix and listed in
their logical document order:

| Prefix | Area |
| --- | --- |
| PRIN | Cross-cutting design principles |
| ECO | Whole-ecosystem completeness and integration |
| OPEN | Open source hardware, software, tooling, documentation, and release artifacts |
| IP | Patents/FTO, designs, trademarks, copyright/database rights, licensing, and clean-room evidence |
| REG | Product classification, conformity, market access, and post-market obligations |
| HUB | Hub functions and serviceability |
| SAFE | Real-time safety controller |
| PWR | Battery and power |
| ELEC | Complete electronic component and interface contracts |
| LINK | Native peripheral interface |
| TIME | Motion-command lifetime and synchronization |
| MECH | Brick-grid mechanical compatibility |
| PER | Common peripheral behavior |
| MOTOR | Motor and position actuator |
| LIGHT | Light |
| SENSOR | Sensor |
| ADAPTER | Legacy and maker-component adaptation |
| INPUT | Handheld, gamepad, UI, and other control sources |
| CTRL | Creation bindings and control |
| PROG | Portable programs, runtimes, lifecycle, and debugging |
| NET | Discovery and network |
| API | HTTP and event API |
| PKG | Project archive and semantic validation |
| TWIN | CAD, identity, assets, and digital twin |
| SIM | Simulation fidelity and parity |
| SEC | Security, privacy, and ownership |
| DEV | Developer service, CLI, VS Code, .NET, and operator clients |
| SW | Common software, firmware, storage, and updates |
| GOV | Open governance, contribution, registries, and release stewardship |
| CAT | Implementation candidates, openness grades, lifecycle, availability, offers, and BOM closure |
| COST | Quantity-tier costing, landed COGS, channels, sustainability, and affordability claims |
| BENCH | Purchasable-component benchmarks and compatibility claims |

## Appendix B. Draft maturity labels

- **Specified**: normative behavior and acceptance evidence are defined.
- **Prototype**: a reference mechanism exists but may change incompatibly.
- **Research**: requirements are known; the design choice is still open.
- **Blocked**: external legal, safety, supply, or technical evidence is needed.

Current status:

| Area | Maturity |
| --- | --- |
| Object, capability, creation, and twin model | Prototype syntax schema; full body/mate/joint/multi-member-transmission/assembly model plus design/as-built/simulation/runtime state split still required |
| Pi/safety-MCU architecture | Prototype pending independent watchdog, output-gate, and stop-loop circuit evidence |
| Electronic interface contract and endpoint schema | Prototype syntax with resolved example contracts; stable field semantics, fault-matrix schema, semantic validator, COTS selection, fixtures, and measured evidence pending |
| Open release contract | Specified |
| Whole-ecosystem completeness contract | Specified requirements; integrated starter release not yet built |
| Legal/IP register | Screening framework published; OMBR mark, feature-specific FTO, modular-design, CAD/catalog, contributor-patent, and commercial compatibility reviews remain open |
| Product classification and EU market access | Requirements specified; exact intended age/use/SKU, RED/EMC/environmental/cyber/toy applicability, conformity path, test evidence, and economic operator remain open |
| Motor, light, and sensor behavioral profiles | Research requirements; fixtures and limits not frozen |
| Cable models and reference cable family | Research; P0 cable is illustrative only |
| Adapter profiles | Requirements specified; endpoint fixtures, rights review, and measured compatibility matrix pending |
| Control-source and handheld profile | Requirements specified; generic-HID descriptors, open handheld reference, latency fixtures, and mappings pending |
| CAN FD application framing | Prototype |
| P0 Micro-Fit connector | Prototype, explicitly not production-conformant |
| Production connector | Research |
| Bench power source class | Prototype-only requirements and example; source fixture not built |
| Battery and charging implementation | Research; OMBR-POWER-BATTERY-1 not frozen |
| Brick/beam functional fit and tolerance evidence | Blocked on metrology; production-mould engineering is outside scope |
| BLE commissioning/control and OpenAPI | Research; radio/discovery only in 0.1 |
| Simulation fidelity tiers | Research; 0.1 results are characterization only |
| Program descriptor and runtime profiles | Prototype syntax pending golden Python/.NET bundles and sandbox contract |
| Developer service and headless CLI | Architecture specified; protocol and implementation planned |
| VS Code Workbench | Architecture specified; reference extension planned |
| Independent .NET Workbench | BrickController2 is an interim partial client; stable framework and parity suite undecided |
| Open governance and archival | Required before 0.2; specification license and institutions not yet established |
| Commercial trademark and product classification | Blocked on qualified legal/compliance review |
| Full hardware BOM and KiCad design | Planned reference implementation |
| Implementation component catalog | Broad family coverage and P0 candidate match published; machine-readable records, exact alternates, purchases, build evidence, and qualification pending |
| EUR 49.99 controller target | Target-unproven; current pre-quote four-port model estimates EUR 39–57 COGS at 1000 units and indicates roughly EUR 65–95 initial direct retail, so small-batch or distributor EUR 49.99 is not presently credible |
| Purchasable-component benchmark catalog | Seeded by repository market survey; controlled measurements and class ranges not yet published |
