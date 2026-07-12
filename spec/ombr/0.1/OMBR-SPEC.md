<!-- SPDX-License-Identifier: MIT -->

# Open Modular Brick Robotics Specification

Version: 0.1.0-draft

Date: 12 July 2026

Status: Proposal for public review

Short name: OMBR

OMBR is a working technical identifier, not a final product name or
certification mark.

Current draft license: MIT under the repository's [LICENSE.txt](../../../LICENSE.txt).
The patent-aware specification license proposed in section 7.3 is a future
governance decision and does not silently relicense this draft.

## Abstract

This document specifies an open, brick-compatible robotics ecosystem built
around Raspberry Pi Zero-class compute. It covers:

- a Wi-Fi- and Bluetooth-capable controller hub;
- a separate real-time safety controller;
- an open native peripheral interface;
- openly documented motors, lights, sensors, cables, and adapters;
- a brick-grid mechanical interface and editable enclosure CAD;
- a transport-neutral capability and control model;
- an editor-independent programming, deployment, debugging, and operator
  toolchain;
- an offline-first digital twin linked to CAD and simulation; and
- licensing, governance, security, safety, and conformance requirements.

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

## 1. Status and interpretation

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

## 2. Vision

OMBR should make a physical brick robot and its digital representation two
views of the same creation:

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

### 2.1 Primary use cases

- remote-controlled vehicles with proportional throttle and steering;
- autonomous robots running Python, .NET, C/C++, ROS, or visual programs;
- one workspace for source, device discovery, build, deploy, debug, remote
  control, CAD-linked twin inspection, simulation, and trace comparison;
- classroom and maker projects that work without an internet connection;
- reusable motor, light, and sensor modules from multiple manufacturers;
- live telemetry, calibration, recording, and replay;
- CAD-first assembly, collision checking, and build instructions;
- software-in-the-loop and hardware-in-the-loop simulation; and
- adapters for selected legacy brick-motor ecosystems.

### 2.2 Non-goals

- cloning a proprietary product housing, logo, or ornamental design;
- claiming affiliation with or certification by the LEGO Group;
- making Linux the sole real-time motor or safety controller;
- requiring a hosted account, telemetry service, or public cloud;
- promising compatibility with every mechanically similar legacy connector;
- treating a rendered mesh as editable mechanical source;
- making version 0.1 suitable for safety-critical or industrial control; or
- claiming a commercial product is certified merely because its Pi module is.

## 3. Market and repository context

This section is informative and deliberately evidence-bounded.
Official-source evidence was reviewed on 12 July 2026.

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

### 3.1 BrickController2 concepts retained, not imposed

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

### 3.2 Purchasable-component benchmark program

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

## 4. Design principles

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

## 5. Terms and object model

| Term | Meaning |
| --- | --- |
| Hub | A networked controller that hosts compute, storage, control orchestration, port management, and local APIs. |
| Safety controller | The real-time MCU that owns watchdogs, output enables, power limits, and deterministic stop behavior. |
| Port | A physical connection point with independently described electrical, data, and safety limits. |
| Peripheral | A port-connected module. It may be an actuator, sensor, light, adapter, or port expander. |
| Component model | A versioned design shared by every manufactured instance of a component. |
| Component instance | A physical or simulated occurrence with owner-resettable identity, revision, and calibration. |
| Capability | A typed property, action, or event with units, bounds, access rules, and quality metadata. |
| Creation | A user-owned assembly graph, control configuration, asset set, and twin configuration. |
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

## 6. System architecture

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

### 6.1 Planned conformance profiles

Conformance will be modular after the relevant profile is frozen. No profile
in this table is claimable in version 0.1.

| Profile | Principal requirement groups | Additional evidence | Draft blocker |
| --- | --- | --- | --- |
| OMBR-HUB-1 | OPEN, HUB, SAFE, ELEC, applicable PWR, NET, API, SEC, SW | Hub, selected power-source, radio, safety, and recovery rows in section 17.4 | Fail-safe stop circuit, BLE/API contract, stable power profile, and applicable product classification |
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
| OMBR-ECOSYSTEM-1 | Every applicable profile plus ECO, BENCH, and GOV | Complete public source release, benchmark catalog, governance evidence, and system integration matrix | All constituent profile blockers plus independently reproduced end-to-end starter system |

The requirement-group column is orientation for this draft, not a computable
claim. GOV-013 requires every stable profile to replace it with an exact,
machine-readable claimant/applicability/evidence matrix.

P0 in section 9.8 is an engineering prototype only. OMBR-LINK-1 is blocked
by the connector, transport/topology, authenticated-session or isolation
choice, and exact framing/timing/version rules—not by the connector alone.

### 6.2 Whole-ecosystem completeness

OMBR specifies a complete programmable brick-robotics ecosystem, not only
a hub, editor, or connector. Its scope spans the controller and battery;
ports, plugs, and cables; sensors; lights and motion actuators; brick-grid
mounting and source CAD; firmware and user-program runtimes; developer and
operator tools; the digital twin and simulator; manufacturing, calibration,
test, repair, and teaching material; and the governance needed for independent
implementations.

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
        MECH["Brick-grid shells, frames, axles, mounts, cable routes"]
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
| ECO-001 | A complete ecosystem release MUST identify the exact hub, power, native link, cable, mechanical, motor or actuator, light, sensor, runtime, developer-tool, twin, and simulation profiles it implements, plus the exact documentation artifacts and conformance-suite versions it contains. |
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

## 7. Openness and release contract

### 7.1 Scope

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

### 7.2 Raspberry Pi boundary

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

### 7.3 Target licensing policy

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

### 7.4 Open prior art and upstream-first engineering

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

## 8. Hub requirements

### 8.1 Functional baseline

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

### 8.2 Reference compute profile

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

### 8.3 Real-time safety controller

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

### 8.4 Power architecture

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

### 8.5 Universal electronic interface contract

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

## 9. Native peripheral link

### 9.1 Architectural choice

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

### 9.2 Link requirements

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
| LINK-024 | Reference cables MUST be field-replaceable and either assemblable with documented commodity tools or accompanied by openly licensable contact, mould, and assembly tooling plus continuity and load-test fixtures. |
| LINK-025 | The hub MUST keep high power disabled when the installed or configured cable type is absent, incompatible, underrated, or contradictory to endpoint limits; a cable declaration MUST NOT raise a physical port ceiling. |
| LINK-026 | Extension, splitter, hub, and adapter cables MUST declare topology, cumulative voltage drop, termination, branch and total current limits, hot-plug behavior, and fault isolation; a passive Y cable MUST NOT be assumed safe for a point-to-point profile. |

CAN and CAN FD do not provide sender authentication. R0 bench framing therefore
does not yet satisfy LINK-017 on an untrusted shared bus. Before OMBR-LINK-1 is
frozen, the reference design MUST select and test either per-port physical
isolation or a compact authenticated-session construction with replay
protection and owner-recoverable keys. This limitation is also documented in
[CAN in Automation's security overview](https://can-cia.org/services/publications/can-community-news/09-2025).

### 9.3 Node identity

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

### 9.4 Message framing

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

### 9.5 Required exchanges

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

### 9.6 Command lifetime and time synchronization

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

### 9.7 Capability classes

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

### 9.8 P0 prototype connector

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

## 10. Mechanical compatibility

### 10.1 Coordinate and grid

OMBR's canonical physical coordinate system is right-handed:

- +X forward;
- +Y left; and
- +Z up.

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

### 10.2 Requirements

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
| MECH-010 | Manufacturing source MUST avoid logos, source-indicating marks, or copied ornamental housings belonging to another manufacturer. |
| MECH-011 | Hub, battery, motor or actuator, light, sensor, adapter, and cable-retention models MUST declare their outer envelope in SI units and brick-grid modules, accessible mounting frames, assembly clearance, and collision geometry. |
| MECH-012 | The reference starter family MUST provide measured attachment strategies for selected studless-beam holes and pins, cross axles, and top or bottom stud connections; each strategy is a separate tested compatibility claim. |
| MECH-013 | The project MUST publish a machine-readable reference-part and attachment-frame library with source, revision, license, provenance, geometry fidelity, and measured-versus-nominal status for every entry. |
| MECH-014 | Cable models and assemblies MUST include connector sweep, minimum bend, strain-relief, removal-tool, and moving-joint keep-outs so CAD and simulation can detect unroutable or unsafe placements. |
| MECH-015 | A mechanical component release MUST include assembly orientation, fasteners, torque where applicable, disassembly sequence, wear interfaces, and replacement limits; glue or destructive joining requires a documented reason and replaceable subassembly boundary. |

Until the metrology study is complete, hole diameter, stud clutch geometry,
and production tolerances remain reference-CAD parameters rather than frozen
normative numbers.

### 10.3 Trademark and compatibility wording

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

## 11. Peripheral requirements

### 11.1 Common component contract

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

### 11.2 Motor and position actuator profile

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

### 11.3 Light profile

| ID | Requirement |
| --- | --- |
| LIGHT-001 | Light capabilities MUST distinguish monochrome intensity, RGB, RGBW, addressable pixels, and effects. |
| LIGHT-002 | Intensity MUST define whether it is linear electrical drive, calibrated relative output, luminous flux, or another quantity. |
| LIGHT-003 | RGB values MUST declare color space and transfer function; linear sRGB is the default interchange representation. |
| LIGHT-004 | A calibrated color-capable light SHOULD expose CIE xy or XYZ in addition to device channels. |
| LIGHT-005 | Effects MUST be bounded programs with explicit duration, repetition, priority, cancellation, and safe-state behavior. |
| LIGHT-006 | The module MUST enforce LED current and thermal limits locally. |
| LIGHT-007 | The release MUST publish channel current, voltage range, refresh or PWM rate, flicker information, thermal behavior, optical measurement method, and expected lifetime assumptions. |

### 11.4 Sensor profile

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

### 11.5 Adapter profile

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

### 11.6 Component-level reference candidates

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
and characterization data satisfying section 7.

## 12. Creation and control model

### 12.1 Control sources

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

### 12.2 Creation graph

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

### 12.3 Bindings

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

### 12.4 Timelines

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

### 12.5 State semantics

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

### 12.6 Portable programming model

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

## 13. Wireless and local network interfaces

### 13.1 Roles

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

### 13.2 Discovery

| ID | Requirement |
| --- | --- |
| NET-001 | A hub MUST advertise a project-assigned BLE service UUID and an mDNS service type after those identifiers are registered by project governance. |
| NET-002 | Discovery MUST reveal only instance ID, product class, protocol version, pairing state, and connection information needed to begin an authorized session. |
| NET-003 | A stable factory serial, Wi-Fi MAC, or other tracking identifier MUST NOT be advertised by default. |
| NET-004 | The local HTTPS service MUST expose a well-known document that links the hub manifest, Thing Description, API version, and owner-controlled display name. |

Version 0.1 does not invent permanent UUIDs or DNS service names before the
project has governance and a registry. Prototype identifiers MUST be clearly
marked experimental and MUST change before 1.0.

### 13.3 HTTP and event API

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

### 13.4 MQTT digital-twin profile

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

## 14. Digital twin, CAD, and simulation

### 14.1 Twin layers

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

### 14.2 Semantic authority

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

### 14.3 Project package

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

URI format, SemVer, SPDX expressions, content digests, archive limits, and
cross-document references are semantic assertions. Validators MUST configure
format assertion or perform equivalent explicit checks; JSON Schema format
annotations and default values do not insert or validate these semantics by
themselves.

### 14.4 Asset formats

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

### 14.5 Frames and physical properties

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

### 14.6 Reproducible simulation

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

### 14.7 Round-trip acceptance

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

## 15. Security, privacy, and owner control

### 15.1 Threat model baseline

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

### 15.2 Requirements

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

### 15.3 Safety is not authentication

Authentication does not make a motion command safe. Every authorized command
still passes through:

1. schema and unit validation;
2. capability and range validation;
3. profile and arbitration;
4. safety policy and deadline checks;
5. safety-controller ceilings; and
6. local peripheral limits and watchdog.

## 16. Software, programming tools, firmware, and updates

### 16.1 Open tooling layers

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

### 16.2 Visual Studio Code reference Workbench

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

### 16.3 Alternative cross-platform .NET client

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

### 16.4 Common software and update requirements

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

## 17. Versioning, governance, and conformance

### 17.1 Versioning

Specification releases use semantic versioning:

- major: incompatible wire, schema, connector, electrical, identity, or
  required-behavior change;
- minor: backward-compatible capability, profile, or optional-field addition;
- patch: clarification or erratum with no required observable behavior change.

Released specifications are immutable. Errata are separate, linked documents.
A physical product exposes specification version, profile, model revision,
PCB revision, BOM revision, enclosure revision, bootloader version, firmware
version, and source-release location.

### 17.2 Extensions

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

### 17.3 Conformance claim

The following syntax is reserved for a future stable release and MUST NOT be
used by a version 0.1 implementation:

A claim has this form:

    OMBR {profile} {spec-version}; model {model-id}@{revision};
    tested by {suite-version}; report {digest-or-url}

Passing one profile does not imply another. Mechanical fit does not imply
electrical or behavioral compatibility.

### 17.4 Required conformance evidence

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
| Mechanical | Datum inspection, fit gauges, insertion/retention, drop, cable pull, thermal, antenna, and manufacturing-process tests |
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
| Market benchmark | Exact purchased revisions, provenance, raw measurements, fixtures, uncertainty, compatibility dimensions, lifecycle, and side-by-side reference report |
| Open release and service | Editable-source audit, license/SPDX, prior-art ledger, reproducible builds, SBOM/provenance, mirrors, fixtures, repair/disassembly, spares, end-of-life, and primary-host-loss drill |
| Governance and documentation | Contribution/patent terms, public decision/appeal, registry allocation, conflicts, archives, security response, accessible/offline documentation, translation, and classroom-material review |

At least two independent interoperable implementations of the native link and
manifest MUST pass the public suite before the project declares version 1.0.
The reference VS Code Workbench and an independently implemented CLI or .NET
client MUST also complete the same golden project without private endpoints or
client-specific semantic state.

### 17.5 Governance minimum

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

## 18. Reference implementation plan

### Phase 0: specification and bench proof

Deliver:

- this specification plus component, project, interface-contract,
  endpoint-electrical-profile, fault-matrix, safety-policy, control-binding,
  timeline, and program syntax schemas and golden vectors;
- draft ecosystem-release, benchmark-record/report, compatibility-matrix, and
  conformance-evidence schemas;
- governance package covering the specification license, contribution and
  patent terms, code of conduct, decisions/appeals, security, trademark,
  registry, archival, release, and support processes;
- tooling-core skeleton, headless validator/packager CLI, and read-only VS Code
  project/twin explorer;
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
  connector, attachment, adapter, dataset, and naming work.

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
  operational.

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
- hub, source, control-input, and power benchmark records publish raw evidence;
  and
- hub, source, cable, compute, and port electrical envelopes reproduce within
  their stated uncertainty and all four ports pass simultaneous-load, inrush,
  brownout, short, backfeed, hot-plug, and thermal-derating tests.

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

- the golden ecosystem rover passes the round-trip acceptance in section 14.7;
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
  connector, attachments, adapters, datasets, product name, and marks;
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
| Mechanical production process | FDM, resin, CNC, injection moulding, hybrid | Fit distribution, strength, heat, RF, surface, lifecycle, tooling openness |
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
    governance without private agreements or trademark dependence.

## 21. Primary references

### Repository

- [BrickController2 README](../../../README.md)
- [Controllers and powered equipment](../../../docs/controllers-and-equipment.md)
- [Linux headless API](../../../docs/linux-headless-api.md)
- [Unofficial BuWizz protocol notes](../../../BuWizz_protocol.md)

### Compute and hardware

- [Raspberry Pi Zero 2 W product brief](https://datasheets.raspberrypi.com/rpizero2/raspberry-pi-zero-2-w-product-brief.pdf)
- [Raspberry Pi Zero 2 W product information portal](https://pip.raspberrypi.com/categories/584-raspberry-pi-zero-2-w)
- [Raspberry Pi Build HAT](https://www.raspberrypi.com/products/build-hat/)
- [Build HAT serial protocol](https://datasheets.raspberrypi.com/build-hat/build-hat-serial-protocol.pdf)
- [Build HAT firmware](https://github.com/raspberrypi/buildhat)
- [Raspberry Pi HAT+ specification](https://datasheets.raspberrypi.com/hat/hat-plus-specification.pdf)
- [Raspberry Pi Compute Module Zero](https://www.raspberrypi.com/products/compute-module-zero/)
- [Raspberry Pi boot security guide](https://pip.raspberrypi.com/categories/685-whitepapers-app-notes-compliance-guides/documents/RP-003466-WP/Boot-Security-Howto.pdf)
- [M5Stack ecosystem overview](https://docs.m5stack.com/en/learn/intro)
- [BuWizz 3.0 Pro](https://buwizz.com/shop/buwizz-3-0-pro/)
- [SPIKE Prime Large Hub technical specification](https://assets.education.lego.com/v3/assets/blt293eea581807678a/bltf512a371e82f6420/5f8801baf4f4cf0fa39d2feb/techspecs_techniclargehub.pdf)
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

- [LDraw file format](https://www.ldraw.org/article/218.html)
- [LDraw legal and parts-library information](https://www.ldraw.org/legal-info)
- [glTF 2.0](https://registry.khronos.org/glTF/specs/2.0/glTF-2.0.html)
- [STEP AP242:2025](https://www.iso.org/standard/84300.html)
- [SDFormat](https://sdformat.org/spec/)
- [URDF](https://docs.ros.org/en/rolling/Tutorials/Intermediate/URDF/URDF-Main.html)
- [3MF Core 1.3.0](https://3mf.io/spec/core-v1-3-0/)
- [Open Container Initiative image specification](https://specs.opencontainers.org/image-spec/)

### Trademark guidance

- [LEGO Fair Play](https://www.lego.com/en-it/legal/notices-and-policies/fair-play)

## Appendix A. Requirement summary

The normative requirements in this draft are grouped by prefix:

| Prefix | Area |
| --- | --- |
| ECO | Whole-ecosystem completeness and integration |
| BENCH | Purchasable-component benchmarks and compatibility claims |
| PRIN | Cross-cutting design principles |
| OPEN | Open source hardware, software, tooling, documentation, and release artifacts |
| HUB | Hub functions and serviceability |
| SAFE | Real-time safety controller |
| PWR | Battery and power |
| ELEC | Complete electronic component and interface contracts |
| LINK | Native peripheral interface |
| MECH | Brick-grid mechanical compatibility |
| PKG | Project archive and semantic validation |
| PER | Common peripheral behavior |
| ADAPTER | Legacy and maker-component adaptation |
| MOTOR | Motor and position actuator |
| LIGHT | Light |
| SENSOR | Sensor |
| TIME | Motion-command lifetime and synchronization |
| INPUT | Handheld, gamepad, UI, and other control sources |
| CTRL | Creation bindings and control |
| NET | Discovery and network |
| API | HTTP and event API |
| TWIN | CAD, identity, assets, and digital twin |
| SIM | Simulation fidelity and parity |
| SEC | Security, privacy, and ownership |
| PROG | Portable programs, runtimes, lifecycle, and debugging |
| DEV | Developer service, CLI, VS Code, .NET, and operator clients |
| SW | Common software, firmware, storage, and updates |
| GOV | Open governance, contribution, registries, and release stewardship |

## Appendix B. Draft maturity labels

- **Specified**: normative behavior and acceptance evidence are defined.
- **Prototype**: a reference mechanism exists but may change incompatibly.
- **Research**: requirements are known; the design choice is still open.
- **Blocked**: external legal, safety, supply, or technical evidence is needed.

Current status:

| Area | Maturity |
| --- | --- |
| Object, capability, creation, and twin model | Prototype syntax schema; design/deployment/runtime split still required |
| Pi/safety-MCU architecture | Prototype pending independent watchdog, output-gate, and stop-loop circuit evidence |
| Electronic interface contract and endpoint schema | Prototype syntax with resolved example contracts; stable field semantics, fault-matrix schema, semantic validator, COTS selection, fixtures, and measured evidence pending |
| Open release contract | Specified |
| Whole-ecosystem completeness contract | Specified requirements; integrated starter release not yet built |
| Purchasable-component benchmark catalog | Seeded by repository market survey; controlled measurements and class ranges not yet published |
| Motor, light, and sensor behavioral profiles | Research requirements; fixtures and limits not frozen |
| Cable models and reference cable family | Research; P0 cable is illustrative only |
| Adapter profiles | Requirements specified; endpoint fixtures, rights review, and measured compatibility matrix pending |
| Control-source and handheld profile | Requirements specified; generic-HID descriptors, open handheld reference, latency fixtures, and mappings pending |
| CAN FD application framing | Prototype |
| P0 Micro-Fit connector | Prototype, explicitly not production-conformant |
| Production connector | Research |
| Bench power source class | Prototype-only requirements and example; source fixture not built |
| Battery and charging implementation | Research; OMBR-POWER-BATTERY-1 not frozen |
| Brick manufacturing tolerances | Blocked on metrology |
| BLE commissioning/control and OpenAPI | Research; radio/discovery only in 0.1 |
| Simulation fidelity tiers | Research; 0.1 results are characterization only |
| Program descriptor and runtime profiles | Prototype syntax pending golden Python/.NET bundles and sandbox contract |
| Developer service and headless CLI | Architecture specified; protocol and implementation planned |
| VS Code Workbench | Architecture specified; reference extension planned |
| Independent .NET Workbench | BrickController2 is an interim partial client; stable framework and parity suite undecided |
| Open governance and archival | Required before 0.2; specification license and institutions not yet established |
| Commercial trademark and product classification | Blocked on qualified legal/compliance review |
| Full hardware BOM and KiCad design | Planned reference implementation |
