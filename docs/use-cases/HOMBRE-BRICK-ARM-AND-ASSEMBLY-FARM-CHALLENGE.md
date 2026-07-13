<!-- SPDX-License-Identifier: MIT -->

# HOMBRE brick arm and assembly-farm engineering challenge

Version: 0.1.0-draft

Date: 13 July 2026

Status: Informative engineering challenge; not a conformance claim, safety
approval, product plan, or statement of affiliation

Normative requirements live in the
[OMBR 0.1 draft](../../spec/ombr/0.1/OMBR-SPEC.md). This document turns the
proposed Mini/Midi/Maxi, AI, MQTT coordination, arm, and automated-assembly
requirements into one deliberately difficult system test.

SO-101 and LeRobot identify third-party reference projects. HOMBRE and OMBR
are independent and are not affiliated with, authorized by, or endorsed by
their maintainers or suppliers. A comparison must identify exact revisions
and physical samples and must not imply upstream certification.

## 1. Challenge outcome

The long-term demonstration is a locally owned farm of HOMBRE Creations that:

1. receives a released target design for a brick-compatible five-axis arm
   plus gripper;
2. resolves its exact BOM, programs, models, tools, fixtures, tests, and
   Assembly Recipe from Git/content digests;
3. coordinates feeders, manipulators, inspection, compute, and human tasks
   over the OMBR Collective and MQTT contracts;
4. assembles an arm from the declared supplied parts and qualified modules;
5. provisions firmware and identities, calibrates and tests the arm;
6. emits a complete as-built work-product twin and acceptance record; and
7. demonstrates the arm's capabilities against a dated physical SO-101
   reference through the same public benchmark suite.

“From parts” is intentionally bounded. The first farm consumes externally
manufactured actuators, electronics, cables, fasteners, and prepared
brick-compatible parts. It does not manufacture semiconductors, batteries,
motors, or raw material and is not described as self-replicating.

## 2. What this challenge reveals

### 2.1 Equivalence is a measurement problem

The upstream SO-101 materials define a useful mechanical/software starting
point but do not publish complete-arm reach, payload, Cartesian accuracy or
repeatability, end-effector speed, sustained duty, gripper force, full-system
latency, or stopping performance. HOMBRE therefore cannot honestly derive
“same capabilities” from the CAD, servo stall torque, or six joint names. The
project must purchase and characterize an exact reference arm beside the
candidate.

### 2.2 Compute tiers are resource profiles, not brands

Mini, Midi, and Maxi must preserve one Creation, capability, App API, safety,
and twin model. They may select different program/model artifacts and provide
different optional services. A program that needs 8 GB and an accelerator
must fail clearly on Mini; it must not silently change control meaning.

### 2.3 The Mini needs more than a micro:bit

The BBC micro:bit V2 application processor provides BLE and a custom 2.4 GHz
radio but no direct internet/Wi-Fi connection. A HOMBRE Mini that retains the
whole-system Wi-Fi/BLE promise therefore needs a qualified internal
Wi-Fi companion on its open carrier. Its USB interface processor is not
connected as an independent motor-safety controller, and its edge connector
cannot power arm actuators. Protected motor power, drivers, watchdog/output
gate, ports, and safety control remain separate.

### 2.4 MQTT coordinates; it does not servo or stop

MQTT 5 is appropriate for presence, bounded state, jobs, expiring task
assignments, results, and shared-resource observations. It is not the inner
joint-control loop, a retained motor-command store, or the sole workcell
protective channel. Every member completes only locally authorized bounded
work when the broker or orchestrator disappears.

### 2.5 Robotic assembly changes the product design

Human-buildable brick instructions are not automatically robot-buildable.
Loose pins, flexible cables, symmetric parts, backlash, snap forces, occluded
holes, and unrestricted bins are difficult. The target arm needs design for
robotic assembly: presentation states, grasp features, insertion paths,
compliance, fiducials or other identity cues, measurable postconditions, and
recoverable modular subassemblies.

## 3. Dated reference baseline

The baseline below uses:

- [LeRobot v0.6.0 SO follower implementation](https://github.com/huggingface/lerobot/blob/v0.6.0/src/lerobot/robots/so_follower/so_follower.py);
- [LeRobot v0.6.0 SO-101 assembly and calibration guide](https://huggingface.co/docs/lerobot/v0.6.0/en/so101);
- [The Robot Studio SO-ARM100/SO-101 repository pinned at the inspected
  commit](https://github.com/TheRobotStudio/SO-ARM100/tree/fda892cba81032c46c40976a48c9ceadbf40a9ca),
  commit
  `fda892cba81032c46c40976a48c9ceadbf40a9ca`; and
- its pinned [SO-101 simulation notes](https://github.com/TheRobotStudio/SO-ARM100/blob/fda892cba81032c46c40976a48c9ceadbf40a9ca/Simulation/SO101/README.md) and
  [new-calibration URDF](https://github.com/TheRobotStudio/SO-ARM100/blob/fda892cba81032c46c40976a48c9ceadbf40a9ca/Simulation/SO101/so101_new_calib.urdf).

This source set is the reviewed baseline. The repository commit is an
inspection anchor, not a claim that every externally linked supplier document
or later upstream page has identical licensing or content.

| Area | Published baseline | HOMBRE interpretation |
| --- | --- | --- |
| Controlled coordinates | `shoulder_pan`, `shoulder_lift`, `elbow_flex`, `wrist_flex`, `wrist_roll`, and `gripper` position actions/states | Five revolute arm coordinates plus one gripper coordinate form the public compatibility surface |
| Follower actuation | Six STS3215 servos with 1:345 gearing | Exact voltage/gear/firmware variant remains part of the benchmark identity; STS3215 is a candidate, not an OMBR requirement |
| Leader actuation | Mixed 1:191, 1:345, and 1:147 gearing | Leader teleoperation is one input implementation, not required arm hardware |
| Nominal simulated joint limits | Pan about ±110°, lift ±100°, elbow about ±96.8°, wrist flex ±95°, wrist roll about −157.2° to +162.8° | Useful provisional task limits; physical samples and calibration establish actual ranges and uncertainty |
| Observations/actions | Six floating-point positions; configured RGB/depth cameras may add observations | OMBR adapter exposes the same semantic channels plus ordinary typed diagnostics without changing upstream LeRobot |
| Gripper convention | LeRobot uses normalized `0..100`; the published simulation notes say this mapping is incomplete in its URDF/MJCF | HOMBRE must publish and validate physical aperture/contact mapping and correct simulator semantics |
| Calibration | Center axes, sweep physical ranges, store calibration against robot identity; wrist roll uses the full encoder range in current code | HOMBRE records fixture, offsets/signs/ranges, uncertainty, revision, replacement invalidation, and rebuild results |
| Compute | Host PC or compatible edge computer connects through the motor-bus adapter; no application computer is intrinsic to the arm | Midi may operate the arm; Maxi hosts vision/AI; Mini is suitable for bounded cell/member roles, not full vision policy inference |
| Software workflows | Teleoperation, camera capture, dataset recording, replay, model training and policy rollout through LeRobot | Preserve through a versioned adapter and public OMBR capabilities rather than a forked private API |
| Published artifacts and stated licenses | BOM/instructions, STEP/STL, Onshape-linked source, URDF/MJCF and Apache-2.0 repositories | Valuable upstream source; still audit every artifact, external link, COTS boundary, notice and modification |
| Safety evidence | Torque-disable-on-disconnect default and configurable relative-target clipping; no published safety-rated E-stop or arm-level stop claim found | Add independent local stop/output inhibit, command expiry, limits and measured stop behavior without calling the result safety-rated |

### 3.1 Values that must be measured

- complete reachable and task-useful workspace;
- payload as a function of pose, reach, speed, acceleration, duty, and gripper;
- Cartesian accuracy and repeatability;
- end-effector and joint velocity/acceleration under load;
- sustained simultaneous-axis duty, current, temperature, and derating;
- gripper aperture, holding force, slip, and repeatability;
- command/observation/camera latency and jitter;
- structural deflection, backlash, cable effects, and rebuild variation;
- command-loss and physical-stop time, distance, and residual energy; and
- validated simulation-to-physical errors.

## 4. Target system architecture

~~~mermaid
flowchart TB
    GIT["Git source and immutable releases<br/>arm, Collective, recipe, programs, models, tests"]
    PORTAL["Optional HOMBRE Portal<br/>publish, discover, verify, mirror, fork"]
    SIM["Collective simulation<br/>physics + material flow + failures + scheduling"]
    GIT <--> PORTAL
    GIT --> SIM

    subgraph CELL["Physical assembly workcell"]
        BROKER["Owner-local MQTT 5 broker"]
        ORCH["Replaceable orchestrator<br/>jobs, tasks, fenced resources, recovery"]
        SAFE["Independent workcell protection<br/>guard/interlocks/stops/restart inhibit"]

        MINI["Mini members<br/>feeders, gauges, simple tools"]
        MIDI1["Midi arm A<br/>local planning and motion"]
        MIDI2["Midi arm B / handling Creation"]
        MAXI["Maxi vision + AI<br/>compute + local safety/ports"]
        INVENTORY["Parts and fixture stations"]
        PRODUCT["Work product<br/>brick arm under assembly"]

        ORCH <--> BROKER
        BROKER <--> MINI
        BROKER <--> MIDI1
        BROKER <--> MIDI2
        BROKER <--> MAXI

        SAFE -. "independent inhibit" .-> MINI
        SAFE -. "independent inhibit" .-> MIDI1
        SAFE -. "independent inhibit" .-> MIDI2
        SAFE -. "independent inhibit" .-> MAXI

        INVENTORY --> MINI
        MINI --> PRODUCT
        MIDI1 --> PRODUCT
        MIDI2 --> PRODUCT
        MAXI --> PRODUCT
    end

    SIM <--> BROKER
    PRODUCT --> EVIDENCE["As-built twin, calibration,<br/>quality and acceptance evidence"]
    EVIDENCE --> GIT
~~~

The simulator may replace all physical member providers for a run, but it
uses the same Collective, job/task/resource, MQTT-visible, capability, model,
and evidence contracts. Workcell protection is never simulated into existence
for a physical deployment.

## 5. Controller family

These preliminary profiles are governed by HUB-001 through HUB-015,
COMPUTE-001 through COMPUTE-015, and SAFE-001 through SAFE-017. Exact floors
remain subject to prototype measurements and conformance vectors.

| Tier | OMBR profile | Reference candidate | Intended challenge role | Hard qualification |
| --- | --- | --- | --- | --- |
| Mini | `OMBR-COMPUTE-MCU-1` | BBC micro:bit V2-class board, open carrier, internal Wi-Fi companion | feeder, indicator, simple tool, sensor station, low-level educational member | BLE and Wi-Fi in complete hub; bounded compiled/embedded runtime; no Linux/full simulator claim; protected load power; separate safety controller/output gate |
| Midi | `OMBR-COMPUTE-LINUX-1` | Raspberry Pi Zero 2 W | arm-local service, MQTT client/optional broker, App API, trace, modest programs | 1 GHz quad-core Cortex-A53, 512 MB constraints made explicit; storage/recovery; Wi-Fi/BLE; independent safety MCU, watchdog, hardware output gate and protected ports; reproducible headless image |
| Maxi | `OMBR-COMPUTE-AI-1` | Jetson Orin Nano 8 GB module; Super developer kit during development | multi-camera perception, learned-policy inference, inspection, planning and optional orchestration | accelerator-neutral model contract; exact BSP/runtime; NVMe and cameras; complete carrier Wi-Fi/BLE; 7–25 W-class thermal/power characterization; independent safety MCU, watchdog, hardware output gate and protected ports; no GPU safety authority |

Primary candidate sources:

- [micro:bit V2 application processor and radio](https://tech.microbit.org/hardware/2-0-revision/),
  [current interfaces/power/dimensions](https://tech.microbit.org/hardware/), and
  [official statement that the device cannot directly connect to the internet](https://microbit.org/get-started/user-guide/offline/);
- [Raspberry Pi Zero 2 W specifications and lifecycle statement](https://www.raspberrypi.com/products/raspberry-pi-zero-2-w/); and
- [Jetson Orin Nano developer-kit capabilities](https://docs.nvidia.com/jetson/orin-nano-devkit/user-guide/index.html) and
  [module versus developer-kit lifecycle](https://developer.nvidia.com/embedded/lifecycle).

The EUR 49.99 VAT-inclusive target is `target-unproven` and applies only to the
separately scoped Midi base controller. It excludes delivery, external power
or battery, charger, external cables, motors, lights, sensors, gamepad and
loose construction parts. Maxi compute, cameras, arm and farm actuators,
broker/orchestrator infrastructure, fixtures, tools and assembly materials
have separate cost models.

## 6. Brick arm engineering baseline

This arm profile is governed by ARM-001 through ARM-016.

### 6.1 Mechanics

The released arm needs:

- a rigid base with Technic-grid attachment and an explicit anchoring load
  case;
- named rigid bodies, five revolute joints, gripper transmission, limits,
  zero/sign conventions, collision/keep-out geometry, and tool-center point;
- bearings or other qualified support so output shafts are not assumed to
  carry unsupported structural loads;
- published gear stages, ratio, phase, backlash, efficiency, retention,
  lubrication, wear, speed and load limits;
- shoulder/elbow counterbalance where needed, with stored-energy isolation and
  service procedure;
- protected cable routing through every swept configuration and a repeatable
  connector/service boundary;
- measured masses, centers of mass, inertias, compliance, deflection and
  stability; and
- modular joint/link/gripper subassemblies designed for both human and robotic
  assembly.

Standard purchased Technic-compatible pieces may be cataloged and referenced
without redistribution of restricted geometry. Project-authored bearing
housings, motor mounts, cable guides, joints, tools, fixtures, and replacement
parts require editable source CAD and their own rights/evidence records.

### 6.2 Actuation, power, and electronics

Each axis needs position feedback, local current/temperature/limit behavior,
command expiry, known safe state, and exact mapping to the mechanical joint.
The complete arm budget covers simultaneous idle, holding, motion, peak,
stall, inrush, regenerative energy, cable drop, driver loss, thermal steady
state, and compute isolation. A single servo's stall-torque figure is neither
a continuous joint rating nor an arm payload.

Three implementation routes should be compared:

1. adapter-driven exact smart-servo variants used by the pinned SO-101
   reference for early software and benchmark work;
2. documented COTS feedback actuators in original brick-compatible housings;
   and
3. a future OMBR-native open motor/gearbox/encoder/controller family.

Only route 3 can approach the strongest component-level open-hardware goal.
The first two remain honest documented-COTS boundaries.

### 6.3 Control, safety, and software

- The arm exposes the six stable channels through the OMBR App API.
- A LeRobot adapter implements the upstream robot/teleoperator contract
  without a private LeRobot fork.
- Local trajectory generation and command expiry continue when high-level
  coordination messages are delayed within their declared bounds.
- Joint, workspace, speed, current, thermal, collision, lease, and stop limits
  are enforced below AI and MQTT.
- Physical stop and independent output inhibition work during Linux crash,
  GPU hang, broker loss, network partition, update, and debugger pause.
- Every run pins Creation, Program, model, calibration, target, bindings,
  safety policy, runtime, inputs, clock, and trace identities.

## 7. Proposed arm acceptance matrix

These gates are deliberately marked **draft** until a physical reference
campaign establishes realistic thresholds.

| Test | Draft method | Candidate gate |
| --- | --- | --- |
| API compatibility | Run upstream-compatible teleoperate, record, replay and policy rollout through six named channels | No upstream patch; explicit unit/range/rate/observation loss report |
| Joint range | Sweep every physical axis under the same calibration fixture | Cover the frozen mandatory task ranges; report actual margins and cable/self-collision exclusions |
| Sustained command loop | Ten-minute six-axis position exercise, synchronized observations, optional two-camera capture | Proposed 60 Hz command target and 30 Hz camera target; fewer than 1% missed cycles; publish full latency/jitter/gap data |
| Workspace | Sample the same calibrated tool-center-point poses on reference and candidate | All mandatory task poses and proposed at least 95% of reference occupied workspace voxels |
| Repeatability | At least 30 returns to five representative poses after warm-up | Candidate three-sigma position/orientation result no worse than the same-test reference result |
| Payload | Loads at 50% and 80% reach over at least 100 pick/place cycles | Proposed at least 95% successful cycles, no brownout/protection trip, within thermal/deflection limits, and no worse than reference under the same test |
| Trajectory tracking | Replay one standard 60-second multi-axis trajectory at matched load | Compare joint RMSE, peak error, p95 lag and dropped cycles; threshold frozen from reference distribution |
| Gripper | Aperture/force/slip measurements plus at least 100 grasp/release cycles over a declared object set | Meet frozen task set and equal or exceed same-test reference success/force/repeatability |
| Power and thermal | Simultaneous-axis duty profiles to steady state in declared enclosure/ambient | No unsafe limit breach; publish current, voltage, temperature, throttle, protection and limiting member |
| Stop behavior | Command loss, local stop and physical-stop trials over representative poses/load/speeds | Independent inhibition within profile bounds; publish electrical latency and physical stopping time/distance separately |
| Rebuild variation | At least five independent builds or disassembly/reassembly cycles | Every build calibrates and passes; publish build/calibration time, offsets, frame variation, repeatability, faults and substitutions |
| Twin parity | Identical trajectories and sensor cases in pinned simulation and hardware | Publish workspace, joint, gripper, contact and sensor error metrics; no claim based on incomplete collision or gripper mapping |

The benchmark release contains fixture CAD, instrument calibration, raw data,
analysis, photos that may lawfully be redistributed, software/firmware/model
versions, environment, uncertainty, failures, and limitations.

## 8. Collective and MQTT contract

The Collective release pins member Creation releases and roles, workcell
frames, shared resources, recipes, task schemas, orchestration policy,
simulation, and safety boundary. Runtime bindings select actual member
instances and never enter public source with credentials.

Reference root:

    ombr/v1/{owner-namespace}/collectives/{collective-id}/

The normative topic families and requirements are COORD-001 through
COORD-025. The challenge specifically exercises:

- per-member retained presence and reported snapshots with boot/session
  identity plus independent presence aging;
- revisioned job and task state;
- non-retained directed assignments with MQTT Message Expiry Interval;
- MQTT Response Topic and Correlation Data plus payload trace/operation IDs;
- QoS 1 duplicate delivery and persistent idempotency windows;
- resource leases with monotonically increasing fencing tokens;
- per-member/per-service credentials and publish/subscribe ACLs;
- broker restart, delayed/reordered messages, partition and stale retained
  state;
- one active orchestrator leadership epoch with tested failover; and
- content-addressed job records linking all member Run records and evidence.

[MQTT 5.0](https://docs.oasis-open.org/mqtt/mqtt/v5.0/mqtt-v5.0.html)
supplies the transport features. The
[Eclipse Sparkplug specification](https://sparkplug.eclipse.org/specification/)
is a useful open precedent for MQTT topic, payload, and session-state
discipline in operational systems, but OMBR does not claim Sparkplug
compatibility and defines different Creation/job semantics.

## 9. Assembly decomposition

The decomposition and recipe evidence are governed by ASSEMBLY-001 through
ASSEMBLY-022.

A credible path avoids beginning with a bin of every loose part.

### Stage A — observable subassembly

Assemble a rigid link from oriented beams/connectors in a fixture. Verify part
identity, pin insertion force/displacement, seating, geometry, and pull test.
This proves recipe/task/evidence flow without cables or powered joints.

### Stage B — joint module

Install one feedback actuator into a prepared housing, attach output support
and transmission, connect a keyed cable, provision identity, calibrate the
joint, and run no-load/loaded tests. Human loading and any screw start remain
explicit tasks until automated.

### Stage C — arm chain

Join accepted base, shoulder, elbow, wrist, and gripper modules in fixtures;
route and inspect cables; validate the mechanical and electrical graphs; then
perform coordinated calibration and collision-limited motion.

### Stage D — complete accepted product

Install Midi controller and optional cameras, provision releases/credentials,
run safety and arm benchmarks, create the as-built twin, quarantine failures,
and publish only reviewed redacted evidence.

### Stage E — less prepared material

Only after stable Stage D evidence should the project add unordered-bin
singulation, loose flexible cables, mixed part variants, automatic fastener
feeding, tool changes, rework, and unattended recovery. Each expands the
failure and safety envelope.

Before unattended operation, unattended recovery, or a production claim, the
exact cell must have an intended-use and integrator/economic-operator map;
applicable machinery/work-equipment and workplace assessment; an independently
assessed protective system; commissioning and validation; operator training;
and camera/privacy, cybersecurity, incident, and maintenance records for its
jurisdiction and intended use. OMBR or MQTT conformance is not a regulatory or
safety approval.

## 10. Assembly Recipe minimum

Each operation has:

- immutable recipe/step ID and target product state;
- exact member capability and resource constraints;
- input parts/subassemblies and allowed presentation/quality states;
- tool, fixture, frames, approach/retreat and keep-outs;
- process parameters with units, ranges and safety ceilings;
- preconditions and expected observations;
- positive postcondition measurements and acceptance thresholds;
- bounded retry, reversal/compensation, quarantine and human-recovery paths;
- consumed/scrapped/reworked material transactions;
- generated work-product/twin/evidence changes; and
- software/model/calibration/clock/actor provenance.

A motor reaching an expected position does not prove a pin seated, a cable
latched, or the correct part was installed. At least two independent sensing
principles or one qualified direct gauge should be considered for critical
postconditions.

## 11. Git artifact layout

The arm remains an ordinary Creation source/release. The farm is a separate
Collective source/release that pins it:

```text
arm-5r-gripper.ombr/
  manifest.json
  mechanics/
  electronics/
  control/
  programs/
  twin/
  tests/
  evidence/
  release/

arm-farm.collective.ombr/
  collective.json
  ombr.lock.json
  members/roles.json
  workcells/
    frames.json
    resources.json
    safety-boundaries.json
  recipes/arm-assembly/
    recipe.json
    instructions.md
    fixtures/
    tests/
  orchestration/
    policies.json
    task-schemas/
  programs/
  simulation/
  tests/
  evidence/
  release/
  .ombr-local/
    bindings/
    jobs/
    inventory/
    work-products/
    recordings/
    secrets/
```

Live inventory, credentials, member bindings, jobs, and unfinished products
are operational state. A completed redacted job/quality record may be copied
into released evidence; it never silently mutates the arm design or recipe.

## 12. Initial implementation candidates

All entries are `screen` candidates, not selections or qualifications.

| Slot | Candidate | Why evaluate | Principal gap |
| --- | --- | --- | --- |
| Mini application board | BBC micro:bit V2 | Educational ecosystem, BLE, sensors/UI, documented hardware | No Wi-Fi/Linux; exact current revision/lifecycle; needs carrier, companion and separate safety/load power |
| Midi compute | Raspberry Pi Zero 2 W | Existing HOMBRE baseline with Wi-Fi/BLE and headless Linux | 512 MB/storage/security/resource limits and cost/supply evidence |
| Maxi development compute | Jetson Orin Nano Super developer kit | Multi-camera and accelerated inference prototype | Developer kit is not production-lifecycle hardware; production carrier/radio/storage/cooling, independent safety/ports and closed upstream boundaries |
| MQTT broker/client | Eclipse Mosquitto and Paho | Open MQTT 5 implementations under Eclipse governance | Single-node/bridge behavior, persistence, ACL/provisioning, failure tests and sustainable maintenance |
| Operational MQTT precedent | Eclipse Sparkplug/Tahu | Topic/payload/session-state and open reference implementation precedent | Not the HOMBRE job/recipe model; no implied compatibility |
| Arm software/reference | LeRobot 0.6.0 and physical SO-101 | Open workflows and a purchasable/measurable comparison object | Arm-level metrics unpublished; COTS servos/controller; simulator caveats and no HOMBRE safety path |
| AI model exchange | ONNX plus pinned target runtime artifacts | Accelerator-neutral semantic anchor with optimized deployment variants | Operator support, preprocessing equivalence, unsupported ops, quantization/accuracy and runtime licensing |
| Trace/observation | MCAP and OpenTelemetry | Open schemas/ecosystems for recorded streams and traces | Exact OMBR mappings, bounded storage, privacy/redaction and deterministic replay |
| Farm source/release | Git, Forgejo, OCI/ORAS | Existing HOMBRE source, Portal and content-addressed distribution candidates | Collective/recipe schemas, large evidence, offline closure and rights/moderation |

## 13. Stop conditions and open questions

Pause or rescope the challenge when:

- a proposed brick joint cannot meet measured reference load, repeatability,
  wear, thermal, or stopping bounds with acceptable safety margin;
- cable/connector or part presentation cannot be made reliably inspectable;
- the Mini Wi-Fi companion or Maxi carrier creates a proprietary mandatory
  control path;
- a learned model cannot meet the task under the documented lighting,
  calibration, latency and fallback envelope;
- MQTT/orchestrator failure can create unbounded motion or ambiguous physical
  ownership;
- the workcell protective concept relies on ordinary software/MQTT rather
  than an appropriate independent local mechanism;
- rights do not permit the intended CAD, model, dataset, firmware or mark use;
  or
- ten-build evidence shows that full-arm assembly is premature compared with
  a smaller modular subassembly target.

Open design decisions include actuator family, arm scale, acceptable custom
parts, bearings and counterbalance, end-effector interface, feeder strategy,
tool changes, camera/tactile set, broker/store/orchestrator stack, workcell
protective architecture, and the physical benchmark thresholds to freeze
after reference characterization.

## 14. Definition of success

The challenge succeeds when an independent team can fetch the public arm and
Collective releases, reproduce or source the declared members/tools/fixtures,
operate fully offline, run the same simulation and MQTT failure suite, build
the ASSEMBLY-022 campaign of at least ten consecutive attempted named-arm
products from the declared prepared parts, reconstruct one accepted
work-product record, and reproduce the published side-by-side arm measurements
without project-private knowledge.
