<!-- SPDX-License-Identifier: MIT -->

# HOMBRE implementation catalog for OMBR 0.1

Version: 0.1.0-seed

Reviewed: 13 July 2026

Status: Informative candidate match; no component is qualified by this file

This HOMBRE companion to the [OMBR draft](OMBR-SPEC.md) maps the complete
ecosystem to open designs, open-source software, and documented commercial
components that can accelerate a reference implementation. It is a decision
and acquisition catalog, not an endorsement, safety approval, compatibility
result, inventory guarantee, or declaration that every listed item is open
hardware.

## 1. Scope and interpretation

The catalog covers every functional family needed for the controller, power,
ports, cables, motors and other actuators, lights, sensors, mechanical system,
developer tools, digital twin, CAD, simulation, fixtures, repair, education
and classroom-offline delivery, and the release process. “Complete” here means
that every required family has a row and every
selected reference design has BOM closure. It does not mean copying every
third-party product listing or CAD asset.

Candidate status has four stages:

- `screen`: public information supports further evaluation;
- `bench`: exact parts have been acquired and a fixture plan exists;
- `qualified`: exact revision passed the named OMBR tests;
- `selected`: a released reference BOM pins the exact revision and alternates.

Every row in this seed remains `screen`. Prices are planning observations, not
quotes, and are excluded from qualification. The normative openness grades,
candidate fields, and price-observation rules are CAT-001 through CAT-013 and
COST-001 through COST-014 in the main specification.

## 2. Architecture-to-implementation coverage

Architecture, implementation, and procurement are separate records. An
architecture slot states a responsibility and open interface; a candidate is
one exact design, product, or software release that may fill that slot; a
supplier observation records where and on what dated commercial terms that
candidate could be obtained. Replacing a supplier does not silently replace
the engineering candidate, and replacing a candidate does not redefine the
architecture contract; the normative controls are CAT-001 through CAT-013 in
the main specification.

### 2.1 Candidate coverage by architecture block

| Architecture block | Authority and required boundary | Existing seed candidates | Main unresolved candidate or supplier search |
| --- | --- | --- | --- |
| Creation design and source control | Git-tracked OMBR folder, manifest, lock, editable sources, deterministic release | Git plus OMBR JSON Schema and planned tooling core | Canonical packer/validator, migration engine, source/release UI, large-artifact mirror policy |
| Design and EDA | Native editable source plus versioned loss-reporting adapters | KiCad; FreeCAD; OpenSCAD; LeoCAD/LDraw exchange | Round-trip fidelity, deterministic generation, rights, supported release matrix |
| Creation Portal | Public listing/publish/fetch/fork/mirror contracts; Git and released digests remain authority | Open Git-forge class, OCI-compatible artifact distribution, signed static index | Exact open portal stack, identity/trust, moderation, storage, search, federation, operating cost |
| Digital twin and observation | OMBR design/as-built/simulation/runtime layers; revisioned reconciliation | OMBR project model; MQTT 5 open broker/client class | Exact twin store, history/query, reconciliation, retention, offline merge, backup and migration |
| Simulation | Same App API/capabilities behind a virtual provider; pinned run manifest | Gazebo with SDFormat; optional ROS 2 bridge | Pinned stack, capability adapter, deterministic clock/replay, fidelity and hardware-trace proof |
| Developer tooling | Public schemas, `ombrd`, `ombr`, VS Code and independent client parity | Planned headless core/service/CLI; VS Code extension; BrickController2 as interim .NET client | Implementations, cross-platform packaging, golden workflows and independent client |
| Native runtime and App API | Program supervisor, private scoped IPC, logical capability bindings, common broker | Python and .NET first; OpenAPI/JSON/event schema reuse | Supervisor/sandbox, local transport, generated SDKs, clock/random/replay provider, portable artifact option |
| Compute-profile hub controllers | Common OMBR-HUB boundary plus exactly one capability-based Mini, Midi, or Maxi compute profile | BBC micro:bit V2 plus Wi-Fi companion as Mini screen reference; Pi Zero 2 W as Midi; Jetson Orin Nano class as Maxi | Exact carriers/BOMs, cross-tier application evidence, radio/RF, power/thermal, lifecycle, pilot yield and tier-specific cost |
| Independent safety and port plane | SAFE, PWR, native-link and port contracts remain below application compute on every tier | RP2040, TPS3431, DRV8876, MCP2518FD/MCP2562FD classes | Common carrier versus tier adapters, fault proof, stop-chain independence and simultaneous-port qualification |
| Native link and cables | Complete electronic contract, protected ports, cable and connector profiles | CAN FD P0, Micro-Fit P0, protected switching | Production topology, connector, contacts/tooling, cable assembly suppliers and qualification |
| Sensors | OMBR-SENSOR electrical, calibration, timing, twin and simulation profiles | VL53 class, TCS3472x class, NAU7802/load-cell class, ICM/BMI IMU classes | Exact MPN/revision, open PCB/carrier, calibration fixtures, lot variation and simulator parity |
| Actuators and lights | OMBR-MOTOR and OMBR-LIGHT behavior, safety, mechanics and models | DRV8876 path, traceable motor-cartridge class, feedback sensor class, documented LEDs | Exact motor/gear/light sources, winding and gearbox data, lot consistency, lifetime and second source |
| Mechanics | OMBR Mechanical Reference Library, mating profiles, source CAD and evidence | HOMBRE-authored parametric parts, purchased references, LDraw exchange | Metrology, load/fit/life evidence, accessible fabrication, material/process control and rights review |
| Control sources | OMBR-CONTROL capabilities, leases, disconnect and accessibility behavior | Generic HID class, open handheld path, third-party remote as benchmark only | Open reference handheld or complete generic-HID qualification and accessible input coverage |
| Third-party bridges | Versioned adapters over files, SDK, CLI, APIs and events with permission/provenance/loss reports | ROS 2, CAD/EDA adapters, notebooks, CI, MQTT and operator clients | Adapter registry, semantic mappings, isolation, trust, maintenance and golden round trips |
| Multi-Creation coordination | Versioned Collective release, MQTT 5 topic/payload contract, task state machines, expiring assignments, fenced leases and reconciliation | Eclipse Mosquitto/Paho class; Sparkplug as a session/topic precedent; new HOMBRE orchestrator and audit store | Exact broker/client versions, authorization, leader failover, partition recovery, duplicate delivery, clock uncertainty and two-Creation golden suite |
| AI and vision | Versioned model packages, camera calibration and timing, accelerator-neutral tensors/results, replay and bounded fallback | ONNX/ONNX Runtime and OpenCV/GStreamer classes; Jetson runtime as a pinned Maxi implementation dependency | Representative models/datasets, camera/accelerator candidates, accuracy/latency/power evidence, privacy and simulator parity |
| Five-axis arm plus gripper | Six named position channels, kinematics, measured workspace/payload/repeatability, LeRobot adapter, local safety and twin evidence | Pinned upstream SO-101 artifacts as benchmark inputs; HOMBRE brick mechanisms and actuator candidates remain open | Physical comparison pending, plus joint/structure selection, cable routing, complete power/thermal envelope and side-by-side acceptance results |
| Automated assembly workcell and farm | Git-versioned recipes, parts/fixtures/tools, work-product identity, positive inspection, recovery and local workcell protection | HOMBRE Collective/orchestrator plus optional ROS 2, MoveIt and Gazebo adapters | Part presentation and grasp strategy, joining/cable operations, metrology, yield, restart/recovery, machinery assessment and staged pilot cell |
| Education and learning | Same OMBR Creation, App API, simulator, Git/release and evidence workflow with readiness-appropriate guidance | HOMBRE education curriculum draft; Open Roberta precedent; classroom-local Git/Portal and offline bundle | Educator review, protected kit, accessible materials, pilot outcomes, privacy/safeguarding, localization, equipment and delivery cost |
| Build, evidence and release | Reproducible source/build, SBOM, provenance, tests, mirrors and recovery | SPDX, REUSE, SLSA and OCI-compatible content addressing | Exact tools/profiles, signing/key policy, independent rebuild service and long-term archive operators |

Each architecture slot record carries `slot ID`, lifecycle roles,
responsibility and authority, required profiles and interfaces, candidate IDs,
status, substitution boundary, and evidence gap. Each implementation candidate
records the exact revision or MPN/source digest, holder or manufacturer,
openness and license, matched and unmet requirements, test evidence, risk,
alternates, and cost references. A supplier or offer observation separately
records seller/provider, channel and SKU, region, date, quantity and MOQ,
currency, tax, shipping, stock or lead time, lifecycle, unit and landed price,
source or quote digest, and expiry. The analogous supplier record for software
is an exact upstream release and distribution channel.

### 2.2 Controller-family candidates

Mini, Midi, and Maxi are usability names, not processor-brand mandates. A
replacement qualifies by the applicable `OMBR-COMPUTE-*` capability contract
and must preserve App API, package, simulation, native-link, and safety
semantics. The complete hub includes the carrier, radio, storage where needed,
protected power and ports, and independent safety path; a development board by
itself is not a HOMBRE controller.

| Tier | OMBR profile and screen reference | Published reference facts useful to screening | Required HOMBRE completion and principal limits | Current disposition |
| --- | --- | --- | --- | --- |
| Mini | `OMBR-COMPUTE-MCU-1`; [BBC micro:bit V2](https://tech.microbit.org/hardware/) plus a qualified Wi-Fi companion | nRF52833 64 MHz Cortex-M4F, 512 KB flash, 128 KB RAM, BLE/custom 2.4 GHz radio and edge connector; the board itself has no Wi-Fi | Project-controlled carrier must add Wi-Fi, independent safety, protected load power/ports and recovery; constrained App API subset and explicit memory/flash budgets | Screen reference for low-cost embedded control, not an all-in-one hub |
| Midi | `OMBR-COMPUTE-LINUX-1`; [Raspberry Pi Zero 2 W](https://www.raspberrypi.com/products/raspberry-pi-zero-2-w/) | 1 GHz quad-core Cortex-A53, 512 MB RAM, 2.4 GHz Wi-Fi, Bluetooth/BLE, microSD, CSI and USB OTG in a 65 × 30 mm board | Trimmed headless runtime, qualified storage and independent safety/ports; no local large-model or full desktop/simulator assumption | Primary Hub R0 and affordable-controller screen reference |
| Maxi | `OMBR-COMPUTE-AI-1`; [Jetson Orin Nano 8 GB module](https://www.nvidia.com/en-us/autonomous-machines/embedded-systems/jetson-orin/) with the [developer kit](https://docs.nvidia.com/jetson/orin-nano-devkit/user-guide/index.html) used only for development | AI accelerator, six-core CPU, 8 GB LPDDR5 and multi-camera/high-speed I/O class; production modules and developer kits have different lifecycle status | Project-controlled carrier must add and qualify Wi-Fi/BLE, storage, 25 W-class power/thermal design, independent safety/ports and pinned BSP/inference stack | Optional AI/vision controller; explicitly outside the EUR 49.99 Midi target |

The Mini reference is intentionally a challenge to the common-interface goal:
its MCU cannot host the same Linux processes as Midi or Maxi. Cross-tier
portability therefore means the same Creation capabilities and observable App
API behavior, with target-specific artifacts or a declared split between local
control and remote compute. It does not mean identical binaries or timing.

#### 2.2.1 Midi Hub R0 carrier candidates

The shortest credible implementation path is an open project-controlled
carrier around a replaceable Raspberry Pi Zero 2 W. The carrier contains a
separate safety MCU, independent output-disable gate/watchdog, protected power
conversion, motor/port drivers, physical-layer transceivers, current/voltage
monitoring, connectors, recovery access, and an IMU. Project-controlled KiCad,
firmware, enclosure, fixtures, and tests can be open. The Pi, semiconductor
dies, microSD card, contacts, and commodity motor cartridges remain named COTS
boundaries.

| Slot | Primary candidate | Alternate or path | Openness grade | Why it is useful | Blocking evidence |
| --- | --- | --- | --- | --- | --- |
| Linux compute/radio | [Raspberry Pi Zero 2 W](https://www.raspberrypi.com/products/raspberry-pi-zero-2-w/) | Future separately profiled Compute Module Zero or interface-compatible Linux module | `documented-COTS` | User-selected core; official page specifies 1 GHz quad-core, 512 MB, Wi-Fi, BLE, 65 × 30 mm, and a USD 15 target | Current volume quote, exact header/assembly path, power transient, thermal/RF-in-enclosure, storage reliability, supply beyond official January 2030 commitment |
| Safety/real-time MCU | [RP2040](https://www.raspberrypi.com/products/rp2040/specifications/) with the [BSD-3-Clause Pico SDK](https://github.com/raspberrypi/pico-sdk) | Integrated-CAN STM32G0B1/SAM C21 class or another supported MCU after license/supply review | `documented-COTS`; SDK is `open-source-software` | Low-cost, extensively documented, deterministic PIO/DMA, and Raspberry Pi states production through at least January 2041 | Independent watchdog/output gate remains external; fault analysis, firmware profile and alternate qualification |
| Hardware output gate | [TI TPS3431](https://www.ti.com/product/TPS3431) plus discrete de-energize-to-trip enable chain and line-monitored stop | Dedicated supervisor selected by fault analysis | `documented-COTS` parts in an `open-design` circuit | External watchdog remains effective if either processor or its normal software path fails | Schematic/FMEA, stuck-pin and broken-wire proof, stop latency, diagnostic coverage and common-cause analysis |
| Brushed motor drive | Four [TI DRV8876](https://www.ti.com/product/DRV8876) devices, one per port | Two [DRV8955](https://www.ti.com/product/DRV8955) dual-motor devices or another qualified DRV89xx/discrete design | `documented-COTS` | Per-port current feedback/regulation and fault status give the cleanest measurement and isolation model; 4.5–37 V and 3.5 A are device limits, not an OMBR continuous rating | PCB copper, current-sense accuracy, stall/regen/thermal/EMC and simultaneous-port tests; compare alternate cost/telemetry behavior |
| Native data controller/physical layer | [Microchip MCP2518FD](https://www.microchip.com/en-us/product/mcp2518fd) SPI CAN FD controller plus [MCP2562FD](https://www.microchip.com/en-us/product/MCP2562FD) transceiver for shared-bus P0 | Integrated-CAN safety MCU; or a transceiver per point-to-point/fault-isolated port | `documented-COTS` | Linux support, ECC-protected message RAM and mature differential signaling | One controller/transceiver is a shared multidrop bus, not four isolated links; topology, dominant-fault containment, hot plug, ESD/EMC, authentication and cost remain blockers |
| Port power switch/current | Protected high-side switch/eFuse with current report per port | Driver-integrated sensing plus shared protected rail where fault isolation remains proven | `documented-COTS` | Independent off, inrush limiting, short/thermal status and observability | Exact device and derating, reverse/regen path, leakage/backfeed, foldback interaction, fault containment and price |
| Logic power | [TI TPS54302](https://www.ti.com/product/TPS54302) 5 V/3 A synchronous buck plus 3.3 V regulation | Second-sourced controller/module that exposes full design evidence | `documented-COTS` | 4.5–28 V input candidate for separating noisy motor/battery domain from compute | Efficiency/heat, actual Pi transient margin, startup/hold-up, conducted/radiated emissions, brownout and reverse-current tests |
| Input protection | [TI TPS25947](https://www.ti.com/product/TPS25947), fuse, TVS, bulk-energy control and discharge path | Protection IC plus external MOSFET network | `documented-COTS` in an `open-design` circuit | Reverse-current/polarity blocking, adjustable protection and fault reporting provide a strong P0 basis | A single device's aggregate limit, battery/bench profiles, regen path, surge sources, safe discharge and certification plan |
| Storage | Industrial or high-endurance A1 microSD in replaceable slot | Read-only root with data partition; future eMMC compute profile | `documented-COTS` | Obtainable and owner-replaceable | Exact card qualification, corruption/power-cut cycling, image reproducibility, endurance, counterfeiting and lifecycle |
| IMU | TDK ICM-42688-P or Bosch BMI270 class | At least one pin/software-independent alternate | `documented-COTS` | Widely documented six-axis sensing | Exact part lifecycle, calibration/noise/drift/thermal tests, coordinate frame and simulator model |
| Prototype connector | Molex Micro-Fit 3.0 eight-circuit family used by P0 | Production connector study; no proprietary legacy plug as the native standard | `documented-COTS` | Published part numbers, drawings, contacts, tooling and ratings; field-assemblable | Size/cost, touch safety, specified 30-cycle durability, frequent user reconfiguration, wrong-mate behavior, cable bend and single-source mechanics; P0 is not production selection |
| Carrier PCB and enclosure | New HOMBRE reference KiCad carrier conforming to OMBR plus original FreeCAD/OpenSCAD shell and beam/brick mounting cage | Multiple fabricators and accessible replacement processes | Intended `open-design` | HOMBRE controls schematics, layout, source CAD, fixtures and repair | Design does not yet exist; DFM/DFT, RF/thermal, tolerance/fit, legal/design review, pilot yield and cost |

No row above is `open hardware` merely because it has a datasheet or open SDK.
The reference product's honest claim is: the carrier, enclosure, firmware,
protocols, fixtures, and tools are released as open designs/software; the
named Pi and semiconductor/contact/storage components are replaceable COTS.

## 3. Controller and electrical family coverage

| Family | Required engineering record | Candidate pool or implementation path | Current disposition |
| --- | --- | --- | --- |
| Compute-profile family | Mechanical/electrical interface, GPIO ownership, resources, power/transient, thermal, radio, firmware, runtime and lifecycle | micro:bit V2 plus Wi-Fi companion for Mini; Pi Zero 2 W for Midi; Jetson Orin Nano class for Maxi; substitutions qualify by capability | Three separate carrier/BOM and benchmark records required |
| Radio | Complete-hub Wi-Fi and BLE versions, antennas, coexistence, credentials, regulatory region and recovery | Mini companion module TBD; Midi integrated Pi radio; Maxi carrier module TBD | Exact Mini/Maxi radio modules and RF evidence missing |
| AI accelerator | Supported model formats, precisions, memory, throughput, power modes, thermal throttling and runtime lifecycle | None required for Mini/Midi; Jetson-class accelerator is the Maxi screen reference | Accelerator-neutral workloads and exact-model benchmarks required |
| Camera and high-speed I/O | Connectors, lane mapping, supported modes, synchronization, bandwidth, power and calibration | Midi CSI/USB for modest vision; Maxi multi-camera/USB/Ethernet class | Exact cameras, trigger/sync path and two-stream load proof missing |
| Compute storage | Capacity, endurance, power-loss, authenticity, imaging, model/dataset size and replacement | Mini internal flash; Midi high-endurance microSD; Maxi NVMe/eMMC-class path | Exact media and tier-specific recovery/image procedure TBD |
| Compute thermal system | Ambient limits, sensors, throttling, sustained workload, airflow and touch/brick-clearance constraints | Passive Mini/Midi candidates; designed heatsink/airflow path for Maxi | Enclosure-level sustained benchmarks required |
| Safety MCU | Clock/reset/watchdog, boot/recovery, flash, I/O, timing, failure and update model | RP2040 primary; compare STM32/NXP alternates | Bench proof required |
| Independent stop | Normally closed/line-monitored input, supervisor, gate, latches and diagnostics | Discrete open circuit plus supervisor; do not rely on Linux or one MCU | Architecture blocker |
| Storage | Capacity, endurance, power-loss, authenticity, imaging and replacement | High-endurance microSD; optional industrial card profile | Exact card TBD |
| Motor rail input | Source contract, fuse, reverse/hot-plug/surge/regen, voltage and current envelopes | 2S battery and certified SELV bench profiles remain separate | Profile decision pending |
| 5 V compute rail | Efficiency, current transient, ripple, sequencing, hold-up, thermals | TPS54302 P0 candidate; qualify second source | Parametric selection pending |
| 3.3 V/control rails | Noise, sequencing, current, shutdown/backfeed | Buck or LDO according to measured load/noise | Parametric selection pending |
| Motor bridges | Four-quadrant behavior, coast/brake, PWM, current, regen, short and thermal | 4 × DRV8876 primary screen candidate; compare 2 × DRV8955 cost/telemetry trade | Thermal/current bench required |
| Per-port power | Default-off, current limit, telemetry, discharge, reverse blocking | High-side switch/eFuse per port or proven grouped design | Cost/safety trade study |
| Bus controller/transceiver | Differential level, common-mode, ESD, standby, termination, fault | MCP2518FD + MCP2562FD shared-bus P0; integrated-CAN MCU and per-port transceiver alternatives | Topology/part pending |
| Linux–MCU link | Framing, checksum, boot/version, deadline and privilege | SPI or UART with public golden vectors | Prototype required |
| Voltage/current monitor | Accuracy, bandwidth, shunt/thermal, calibration and faults | Driver current sense plus rail/port ADC; dedicated monitor if needed | Measurement architecture pending |
| Temperature | Junction/board/enclosure/connector/battery sensing and limits | Local digital/analog sensors plus driver telemetry | Placement simulation pending |
| IMU | Axis, range, noise, bias, calibration, rate and timestamp | ICM-42688-P/BMI270 class | Exact part pending |
| RTC/time retention | Accuracy, backup behavior, privacy and replacement | Low-power I2C RTC optional; network time cannot be sole continuity source | Requirement/price trade study |
| User controls | Stop, power, pairing, recovery and reset semantics | Rated buttons/switches with accessible original cap design | Human-factors and lifecycle tests |
| Indicators | Ready, armed, pairing, warning and fault without color-only cues | Discrete or addressable LEDs; independent fault indicator path | Optical/accessibility design pending |
| USB/service | Power vs data role, ESD, boot/recovery, owner access | Pi USB OTG plus protected internal/service header | Final enclosure/access policy pending |
| Debug/programming | SWD/UART/test pads, authorization, fixtures and normal-mode isolation | Open pogo fixture and documented header/pad map | Fixture source required |
| Antenna/RF keep-out | Exact Pi antenna zone, plastic/material, cable and ground interaction | Original shell around Pi published keep-out | Chamber/pre-compliance evidence required |
| PCB/PCBA | Stack-up, copper, creepage/clearance, thermal, assembly/test/yield | KiCad source, two fabricators, automated test | Not designed |
| Connectors/contacts | Housing/contact MPN, keying, ratings, tooling, mating and second source | Micro-Fit P0; production selection open | Major cost/size blocker |
| Cable | Conductors, twist/shield, current/voltage drop, bend/pull, strain relief and repair | Two P0 lengths using commodity contacts; original external retention | Prototype drawings/tests required |
| Enclosure/mounting | Original appearance, beam holes, stud attachment, connector/button/antenna/thermal access | OMBR-native parametric cage and shell | Metrology/design/IP review required |

## 4. Peripheral family coverage

The desired openness pattern is an open project-controlled PCB, firmware,
connector/mounting shell, calibration fixture and simulation model around a
well-documented COTS sensing element, LED, motor cartridge, encoder, switch, or
load cell. A genuinely complete open motor or semiconductor design can replace
that boundary later without changing the capability contract.

| Reference family | Screen candidates | Open/project-controlled work | Key qualification |
| --- | --- | --- | --- |
| Passive brushed motor | Documented metal-gearmotor or replaceable DC motor cartridge; generic untraceable marketplace motors are not stable references | Housing, output axle/gear interface, cable/PCB, suppression, characterization fixture and model | Torque-speed-current map, stall/thermal, brush EMI, gearing/backlash, shaft/axle load, lot variation and life |
| Feedback motor | Same motor class plus quadrature Hall/optical encoder or magnetic angle sensor | Encoder PCB, calibration, closed-loop firmware, housing/output geometry and twin | Counts/angle, zeroing, range, repeatability, control stability, loss behavior and mechanical stops |
| Steering/linear actuator | Gearmotor plus angle/position sensing; standard RC servo only behind an explicit adapter | Original mechanism/mount and safe controller/adapter | Endpoint current, backlash, holding/return behavior, linkage loads and failsafe |
| Stepper adapter | Commodity bipolar stepper cartridge and DRV/TMC class driver | Native smart adapter, power/current profile, connector and firmware | Current/thermal, missed-step detection, stop state, EMC and regeneration |
| White/RGBW light | Discrete LEDs with public optical/electrical data and constant-current/PWM driver | Open light PCB, diffuser/mount, firmware, calibration and thermal model | Intensity/color/flicker, junction temperature, current, optics, eye/skin exposure and life |
| Addressable light | Addressable LEDs only where protocol/supply behavior is sufficiently documented | Open adapter/controller and ordinary-light fallback | Protocol timing, power/inrush, failure propagation, update rate, thermal and source continuity |
| Distance/ToF | ST VL53L1X/VL53L4 class | Open PCB, aperture/mount, calibration and simulator model | Range/FOV/rate, target reflectance, ambient light, crosstalk, saturation and cover window |
| Ultrasonic distance | Documented transducer/front-end pair rather than anonymous module | Open analog/digital board, horn/mount and firmware | Beam, blind zone, temperature, crosstalk, ringing, latency and acoustic safety |
| Color/ambient light | ams OSRAM TCS3472x-class or documented multi-channel spectral sensor | Illumination ring, shroud, calibration targets, open PCB/firmware/model | Distance/angle, illumination, ambient rejection, color space, repeatability and material dependence |
| Reflectance/line | Vishay/ON-style IR emitter and phototransistor/photodiode pair | Open geometry, current driver, analog front end, calibration card | Surface/distance, ambient, crosstalk, response, emitter ageing and eye exposure |
| Touch/contact | Rated microswitch, conductive contact or force-sensitive element | Original brick/beam bumper, debouncing and capability model | Force/travel/hysteresis, impact/overtravel, cycles, cable and safe failure |
| Force/weight | Traceable load cell plus HX711/NAU7802-class front end | Open amplifier/controller PCB, mount, calibration weights/procedure and twin | Range, overload, creep, hysteresis, temperature, excitation, rate and mechanical load path |
| IMU/motion | ICM-42688-P/BMI270-class | Open PCB/mount, calibration, timestamping and model | Axis/range, noise, bias/drift, temperature, vibration and magnetic contamination if compass added |
| Magnetic position | AS5600/other documented angle sensor or quadrature Hall switches | Magnet/airgap geometry, PCB, calibration and mechanical retention | Nonlinearity, speed, field interference, lost magnet, temperature and absolute-zero behavior |
| Temperature/humidity/pressure | Sensirion/Bosch class documented sensors | Open enclosure flow path, PCB, calibration and model | Response, self-heating, condensation, contamination, accuracy, drift and placement |
| Sound | Documented MEMS microphone only in an explicit privacy profile | Open PCB, local processing, indicator, permissions and deletion/export path | Acoustic response, clipping, privacy/consent, retention and radio/API exposure |
| Camera/vision | Pi camera or documented USB/UVC camera as optional non-starter component | Mount, permissions, local API and privacy model | Bandwidth/latency, calibration, privacy, licensing, compute/RAM and lighting |
| Legacy PF-style adapter | Lawfully acquired connector/cable and independently specified electrical behavior | Protected bridge/adapter, exact revision matrix and fixtures | Rights review, polarity/servo waveform, current, wrong-device, feedback limits and failed combinations |
| Legacy LPF2/Powered Up adapter | Lawfully acquired cable or licensed connector path; no native dependence | Electrically isolated/protected protocol adapter and clean-room evidence | Connector rights/supply, mode negotiation, power, identity, position feedback and protocol legality |
| Generic maker adapter | Grove/Qwiic/STEMMA/servo interfaces only as explicitly powered adapters | Open bridge PCB, isolation/level shifting, profiles and safe defaults | Voltage/pinout variants, current, hot-plug, pull-ups, address conflict and misleading plug compatibility |

## 5. Mechanical engineering catalog coverage

HOMBRE does not plan production moulds. It needs enough lawful, source-traceable
engineering information to select, procure, attach, load, simulate, replace,
and where useful fabricate original OMBR-native alternatives.

| Family | External engineering references | OMBR-native deliverable | Missing evidence |
| --- | --- | --- | --- |
| Catalog identity and availability | LEGO Design/Element IDs and Pick a Brick for factual identity/retail evidence; BrickLink only under its terms | Independent part-record IDs, sparse aliases, dated offers and lifecycle | Bounded acquisition list and rights review |
| Visual/assembly exchange | Per-file reviewed [LDraw](https://library.ldraw.org/) assets; [LeoCAD](https://www.leocad.org/) | Namespaced import/export mapping and loss report | Per-file license inventory and coordinate/semantic round trip |
| Bricks, plates and tiles | Purchased samples and lawful public identification data | Functional attachment/envelope records; original adapter or replacement CAD only after review | Multi-lot metrology, fit and design-right review |
| Studless beams, frames and panels | Purchased genuine and independent parts | Hole/grid profiles, original hub/peripheral frames and fit gauges | Dimensional/force/lifecycle distributions |
| Pins, holes, axles and bushes | Purchased reference elements | Versioned mating profiles, coupons, insertion/retention/torque tests | Material/process/conditioning matrix |
| Connectors, hinges and ball/universal joints | Purchased parts and lawful geometry proxies | Joint frames, DOF/limits/load records and original replacements where cleared | Kinematic/load/life and design review |
| Gears, racks, worms and differentials | Purchased reference parts; community visual CAD is not tooth authority | Independent analytical profiles, generators, fixtures, mesh/transmission semantics and original gears | Exact tooth/metrology, wear/load, compatibility and FTO/design review |
| Pulleys, belts, chains and sprockets | Standard engineering formulas and purchased examples | Open profiles, length/tension/routing and transmission models | Material, friction, fatigue and sourcing |
| Wheels, tyres, tracks, rails and rollers | Purchased examples and measured interfaces | Original hubs/wheels where useful, contact models and fixtures | Loaded radius, friction/slip, runout, load/speed/wear |
| Springs, elastomers, string and hose | Commodity specified stock and measured examples | Attachment/routing, constitutive curve, stored-energy handling and model | Creep/fatigue/environment and release safety |
| Enclosures and mounting cages | Pi mechanical drawing plus independently measured brick/beam interfaces | Original parametric FreeCAD/OpenSCAD source, STEP/3MF/drawings/gauges | RF/thermal, load/drop, fit/life and design review |
| Fasteners and generic structure | ISO/DIN commodity fasteners/extrusions where appropriate | Exact BOM, torque/tool/service records and CAD | Supplier/material/finish and pull-out evidence |

There is no known public, open, manufacturer-issued catalog that supplies the
full engineering CAD, tolerances, materials, loads and life data for the LEGO
element range. BrickLink Studio is a useful proprietary building tool, not an
open manufacturing-CAD source. LDraw is the strongest open community assembly
interchange, but its units and geometry are not physical-fit authority.
HOMBRE's path is therefore independent records and metrology plus original,
open replacement designs, not a copied mould-parts library.

## 6. Source control, portal, software, twin, and simulation match

| Layer | Primary open candidate | Role and boundary | Decision status |
| --- | --- | --- | --- |
| Creation source and release | [Git](https://git-scm.com/) plus the planned HOMBRE reference `ombr` CLI implementing OMBR validation, lock resolution, and deterministic packing | Git is the mutable collaboration/history layer; an OMBR release is the immutable, content-addressed engineering artifact | Folder, lock, provenance, and release requirements specified; schemas and golden pack vectors pending |
| Portal collaboration | [Forgejo](https://forgejo.org/docs/latest/) as a self-hostable Git forge candidate | Repository hosting, review, issues, releases, mirroring, and federation-adjacent workflows; it does not become authority for Creation semantics or conformance | Prototype integration; verify release API, permissions, export, moderation, backup, migration, and operating cost |
| Portal artifact distribution | [OCI Distribution](https://opencontainers.org/) with [ORAS](https://oras.land/docs/client_libraries/overview/) for arbitrary content-addressed bundles, plus a signed static index fallback | Store, fetch, copy, verify, and mirror exact Creation releases by digest while keeping Git source and OMBR manifests authoritative | Evaluate media types, referrers, offline closure, garbage collection, signatures, namespace governance, and independent registry compatibility |
| Carrier/MCU EDA | [KiCad](https://www.kicad.org/) | Preferred editable schematic/PCB source and production exports | Select unless a concrete blocker appears |
| Mechanical CAD | [FreeCAD](https://www.freecad.org/) plus [OpenSCAD](https://openscad.org/) for generators | Parametric product/assembly source; scripting and deterministic derivatives | Prototype both for round-trip and reproducibility |
| Brick assembly UI | [LeoCAD](https://www.leocad.org/) and LDraw ecosystem | Optional external assembly/visual exchange; not OMBR semantic authority | Adapter/import-export study |
| Dynamics/simulation | [Gazebo](https://gazebosim.org/docs/latest/getstarted/) and [SDFormat](https://sdformat.org/spec/) | Physics and sensor simulation derivatives | Pin a version and define fidelity/loss tests |
| Robotics integration | [ROS 2](https://docs.ros.org/en/rolling/) optional bridge | Interoperability, tooling and topics; not mandatory runtime semantics | Adapter profile later |
| Linux hub OS | Raspberry Pi OS Lite or another documented Linux for Midi; pinned Jetson Linux/JetPack-class stack for the Maxi reference | Vendor firmware, BSP and accelerator runtimes are recorded implementation boundaries rather than OMBR semantics | Reproducible Midi and Maxi image, update, recovery and lifecycle study |
| MCU runtime | micro:bit CODAL/MakeCode, Zephyr, Raspberry Pi Pico SDK, or another open RTOS/bare-metal stack according to role | Mini application execution and deterministic safety/port control remain distinct roles; exact license/dependency/build evidence required | Mini artifact and safety-MCU bench comparison |
| MQTT 5 messaging | [Eclipse Mosquitto](https://mosquitto.org/) broker and [Eclipse Paho](https://eclipse.dev/paho/) clients as initial open candidates | Implements the normative Collective topics, MQTT 5 properties, QoS, expiry and retained-presence subset; never the inner control loop or safety path | Pin versions; publish TLS/ACL, duplicate delivery, broker-loss, bridge and load evidence |
| Collective semantics precedent | [Eclipse Sparkplug](https://sparkplug.eclipse.org/specification/) and Tahu studied as an operational topic/session/payload precedent | Reuse lessons where compatible; HOMBRE does not claim Sparkplug compatibility and keeps Creation/job/recipe semantics in OMBR schemas | Record mapping and deliberate differences before reuse |
| Collective orchestrator | New HOMBRE service with durable job/task/lease/audit state, using an embedded or service database behind a public API | Expands a released Collective into authorized assignments, fences stale workers, reconciles state and emits reproducible run records | Select store/consensus boundary; prove restart, duplicate, partition, leader-epoch and compensation behavior |
| AI model package | [ONNX](https://onnx.ai/) plus [ONNX Runtime](https://onnxruntime.ai/) as portable screen candidates; accelerator-specific engines remain optional derivatives | Carries exact model digest, tensor contracts, preprocessing, calibration, provenance, license, benchmark and fallback metadata | Test representative Mini-offloaded, Midi and Maxi workloads; define loss reports for compiled engines |
| Vision pipeline | [OpenCV](https://opencv.org/) and [GStreamer](https://gstreamer.freedesktop.org/) classes behind OMBR camera/calibration/timestamp contracts | Capture, calibration, preprocessing, encoded transport, replay and simulation without making either library normative | Select exact cameras/codecs; measure synchronization, end-to-end age, CPU/GPU load, privacy and failure modes |
| SO-101/LeRobot bridge | [LeRobot v0.6.0](https://huggingface.co/docs/lerobot/v0.6.0/en/so101) adapter using the six-channel HOMBRE arm capability | Provides teleoperation, recording, replay and policy rollout interoperability; it does not make SO-101 geometry, servos or software normative | Implement plugin without upstream patches and run the physical side-by-side benchmark |
| Assembly planning bridge | Optional [ROS 2](https://docs.ros.org/en/rolling/), [MoveIt](https://moveit.picknik.ai/) and Gazebo adapters | Planning and simulation tools may consume released workcell/recipe/twin data; OMBR remains authority for identity, limits, jobs, evidence and safety separation | Demonstrate loss-reporting round trip, deterministic fixtures and recovery tasks in staged pilot cell |
| Hub API | OpenAPI 3.1 plus WebSocket/event schema | Editor-independent local API and generated clients | Schema not yet frozen |
| SDKs | Apache-2.0 generated TypeScript, .NET and Python libraries | Same public model for hardware and simulation | Generator/golden-vector work pending |
| Developer core | New headless `ombrd` service and `ombr` CLI | Owns discovery, build, deploy, debug, twin and simulation without UI lock-in | Architecture specified, not implemented |
| Primary workbench | Open VS Code extension over public APIs | Reference integrated UX; offline VSIX and Code-OSS compatibility required | Planned |
| Independent client | Cross-platform .NET client reusing public SDK/core | Confirms editor independence; BrickController2 is interim | Framework decision pending |
| Program runtimes | Python and .NET/C# first; native/Rust/JS/visual later | Ordinary source, permissions, lifecycle and debug | Golden programs/sandbox pending |
| Program supervision | Linux process supervision with cgroups, namespaces, seccomp and an independent safety-controller output gate; MCU and split-execution profiles use separately declared mechanisms; OCI or WebAssembly isolation only where a profile proves its overhead and limits | Enforce lifecycle, resource, filesystem, network, device, stop, update and recovery policy without making application compute the safety authority | Publish adversarial failure/resource tests for one Mini, Midi and Maxi reference |
| OMBR App API | Generated Python/.NET bindings and `ombr-appctl` over a private, scoped local IPC endpoint backed by one capability/control broker | Programs see logical capability IDs and the same contract against physical, simulation, and replay providers; this is separate from the owner/operator network API | Freeze protocol/schema, permissions, error model, version negotiation, clock/random services, and two-language golden vectors |
| Portable program option | [Wasmtime and WASI](https://docs.wasmtime.dev/) component runtime candidate | Potential target-independent artifact with capability-scoped host services and controlled time/random/network access; not required for Python/.NET parity | Measure Pi footprint, startup, determinism, debugging, sandbox escape surface, component-model maturity, and host-API stability |
| Trace, replay, and observability | OMBR canonical JSON/event vectors with [MCAP](https://mcap.dev/) evaluated for timestamped run archives and [OpenTelemetry](https://opentelemetry.io/docs/) evaluated for vendor-neutral logs, metrics, and traces | Capture reproducible App API inputs/results, lifecycle and parity evidence; neither format nor any telemetry service may become a mandatory cloud dependency | Define canonical event semantics, ordering, clock mapping, redaction, retention, offline readers, and lossless conversion tests |
| Package/provenance | SPDX, REUSE, SLSA and OCI-compatible content addressing | License/SBOM/build/offline mirror evidence | Exact profiles and tools pending |
| Documentation | Markdown plus reproducible accessible render | Diffable normative source and offline manuals | Build pipeline pending |

## 7. Planning cost model

### 7.1 Exact affordable-base scope

The `OMBR-AFFORDABLE-HUB-MIDI-1` target in the main specification is a Midi
controller only: original enclosure/mount, Pi Zero 2 W, storage, safety MCU/output gate,
Wi-Fi/BLE, IMU, protected logic and motor paths, at least four ports,
recovery, controls/indicators, internal contacts and fasteners. It does not
include delivery, external power/battery/charger, external cables, actuator,
light, sensor, gamepad, or loose construction parts.

Mini has its own BOM and may be less expensive, but no price claim follows
from the micro:bit board alone. Maxi, cameras, AI accelerators, workcell
fixtures, robots, tools, safety equipment, and farm infrastructure are
explicitly outside this controller target. COST-014 requires each of those
tiers and deployment scales to have a separate acquisition, energy,
maintenance, support, and operating-cost record.

Creation Portal infrastructure is also outside the controller SKU. Candidate
hosting, registry, object storage, mirrors, search, backups, bandwidth,
moderation, security response, migration, and support require a separate
COST-013 total-cost-of-ownership record with dated provider or self-hosting
observations; the local Git and offline release workflow cannot depend on that
funding model.

The official Pi page calls Zero 2 W a USD 15 computer. This is a useful design
anchor, not a German volume quote. For a current retail comparison, LEGO lists
its two-port Powered Up Hub 88009 at
[EUR 49.99 in Germany](https://www.lego.com/de-de/product/hub-88009), with
batteries excluded. HOMBRE must not imply feature or bundle parity from price
alone.

### 7.2 Price-book equations

For a German advertised gross price:

```text
net_revenue = gross_price / (1 + VAT_rate)
contribution = net_revenue - landed_COGS - channel_fees - warranty_reserve
```

At EUR 49.99 and 19% VAT, `net_revenue` is EUR 42.01 before payment,
fulfilment, channel margin, warranty, compliance/support amortization, and
profit. The planning COGS ceiling of EUR 27 at 1000 units would leave about
EUR 15 for those items in a direct-sale model. It is a design goal, not a
supplier quote or conformance limit.

### 7.3 Bottom-up pre-quote model

The following direct-component model uses the P0 candidates in section 2.
Values are euros excluding VAT and shipping as of 13 July 2026. Quantity tiers
without an exact linked distributor tier are planning assumptions. They do not
establish feasibility and must be replaced by formal quotes, tested yield,
test time, freight, duty, warranty, and lifecycle data.

| Direct component subsystem | Qty 1 | Qty 10 | Qty 100 | Qty 1000 |
| --- | ---: | ---: | ---: | ---: |
| Pi Zero 2 W | 16.30 | 16.00 | 15.00 | 13.50 |
| Qualified 16 GB microSD | 6.68 | 6.00 | 5.00 | 4.00 |
| RP2040, flash, clock and support | 1.50 | 1.35 | 1.10 | 0.95 |
| TPS3431, independent gate and stop input | 2.20 | 1.70 | 1.35 | 1.10 |
| Four DRV8876 devices and support components | 7.40 | 5.50 | 4.55 | 3.90 |
| MCP2518FD, MCP2562FD, clock, termination and ESD | 3.60 | 3.00 | 2.60 | 2.30 |
| eFuse, buck, TVS, inductors, load switches and input connector | 5.80 | 4.50 | 3.70 | 3.10 |
| Four Micro-Fit headers, Pi header, buttons and connectors | 8.00 | 6.40 | 5.40 | 4.40 |
| ADC/mux, sensing, temperature, UI and passives | 4.50 | 3.50 | 2.80 | 2.20 |
| **Indicative direct-component total** | **55.98** | **47.95** | **41.50** | **35.45** |

Current public price anchors include the
[Reichelt Raspberry Pi catalog](https://www.reichelt.de/de/de/shop/kategorie/Raspberry%20Pi/einplatinen-computer-8242),
[RP2040](https://www.reichelt.com/de/en/shop/product/raspberry_pi_-_rp2040_arm_cortex-m0_-306486),
[DRV8876](https://www.mouser.de/ProductDetail/Texas-Instruments/DRV8876PWPR),
[TPS25947](https://www.mouser.de/ProductDetail/Texas-Instruments/TPS259474LRPWR),
[TPS54302](https://www.mouser.de/ProductDetail/Texas-Instruments/TPS54302DDCR),
[MCP2518FD](https://www.digikey.de/en/products/detail/microchip-technology/MCP2518FDT-E-QBB/10231302),
and [MCP2562FD](https://www.mouser.de/ProductDetail/Microchip-Technology/MCP2562FD-E-SN).
Listings can change or be unavailable and are not authorized-volume quotes.

The more relevant product cost includes fabrication, assembly, enclosure,
programming, test, yield and basic packaging/warranty:

| Quantity | Components | PCB/assembly | No-mould enclosure | Test/calibration | Packaging, yield, warranty | Estimated COGS |
| ---: | ---: | ---: | ---: | ---: | ---: | ---: |
| 1 | EUR 54–60 | 35–60 | 12–25 | 10–20 | 3–6 | **EUR 114–171** |
| 10 | 45–51 | 10–18 | 8–15 | 5–10 | 3–6 | **EUR 71–100** |
| 100 | 38–45 | 4–8 | 5–9 | 2–5 | 2–5 | **EUR 51–72** |
| 1000 | 30–38 | 2.5–5 | 3.5–7 | 1–3 | 2–4 | **EUR 39–57** |

These manufacturing values are engineering estimates, not supplier quotes.
They still exclude engineering, legal/IP and certification NRE, long-term
security/support, insurance, outbound fulfilment, channel/payment fees and VAT.

The current architecture therefore does **not** demonstrate sustainable EUR
49.99 retail at small batches or at 1000 units. The estimate of EUR 39–57 COGS
leaves between roughly EUR 3 and negative EUR 15 against EUR 42.01 net revenue
before the excluded costs. A quote-backed initial direct retail range is more
likely to be approximately EUR 65–95 including VAT at 1000 units. EUR 49.99 may
become feasible around 10,000 or more lifetime units only if negotiated supply,
simplified/shared port electronics, high automated-test yield, an inexpensive
original enclosure, direct sale, and roughly EUR 29 or lower COGS are actually
demonstrated. Distributor retail at the same price would require a much lower
COGS, approximately EUR 18 in the current planning model, and is not presently
credible.

The project must compare explicit mitigations instead of silently dropping
features:

1. keep four protected ports and pursue volume/architecture savings;
2. define a separate two-port affordable hub plus open expander while retaining
   the four-port `OMBR-HUB-1` reference;
3. offer a community carrier that reuses an owner-supplied Pi/storage, under a
   different non-retail profile;
4. raise the complete four-port retail target; or
5. secure a transparent subsidy and label its scope and duration.

Independent stop, rated protection, open source, owner recovery, test, legal
review, and compliance evidence are not eligible cost cuts.

## 8. Acquisition and proof sequence

The lowest-risk next purchases and prototypes are:

1. acquire at least two exact micro:bit V2 revisions, three Pi Zero 2 W units,
   one Jetson Orin Nano development unit, storage/radio samples, and their
   published lifecycle and software records from authorized channels;
2. build an open one-port bench board with RP2040, TPS3431/independent gate, DRV8876,
   candidate transceiver, input protection and complete measurement points;
3. run the same capability, deployment, disconnect, recovery and simulation
   golden Creation against the Mini, Midi and Maxi reference paths;
4. characterize a passive motor, feedback motor, light, ToF sensor,
   color/reflectance sensor, switch/force sensor and two cable lengths;
5. acquire and physically characterize one exact SO-101 reference build, then
   freeze its raw workspace, payload, repeatability, rate, gripper, power,
   thermal and stop measurements before claiming equivalence;
6. build the source/cable/sink contract validator before a four-port carrier;
7. prove a two-Creation MQTT 5 Collective under duplicates, stale leases,
   broker restart, partition, orchestrator failover and time uncertainty;
8. automate one inspectable arm subassembly with prepared parts, fixture,
   recipe, work-product identity, positive inspection and manual recovery
   before attempting complete-arm or farm-level assembly;
9. collect real Mini/Midi/Maxi carrier, arm and workcell quotes at relevant
   quantities and measure assembly, calibration and test time;
10. run connector, thermal, brownout, stall, regen, ESD/EMC pre-compliance and
   stop-chain tests;
11. freeze only the records that pass, retaining failed candidates and reasons;
12. repeat cost, lifecycle, legal/IP and regulatory gates before architecture
   and design freeze.

The result should be a released machine-readable catalog and price book. This
Markdown seed defines the decision surface while the Phase 0 catalog schema,
offers, measurements and quotes are still missing.
