<!-- SPDX-License-Identifier: MIT -->

# HOMBRE education curriculum draft

Version: 0.1.0-draft

Date: 13 July 2026

Status: Informative proposal for educator, learner, engineering, and safety review

Relationship: HOMBRE is the Holistic Open Modular Brick Robotics Ecosystem;
the [OMBR Specification](../../spec/ombr/0.1/OMBR-SPEC.md) defines its technical
contracts. This document proposes how to teach with that ecosystem. It is not
an OMBR conformance profile, accredited program, teacher certification,
product age classification, or substitute for institutional laboratory and
safeguarding rules.

Current draft license: MIT under the repository's [LICENSE](../../LICENSE.txt).
A dedicated open curriculum and media licensing policy remains a governance
decision before a stable release.

## 1. Purpose

HOMBRE should help learners acquire professional engineering habits while
they are still working on approachable systems. A first Creation can be as
simple as a sensor controlling a light, but it should still introduce a small,
honest version of the real lifecycle: understand a need, state a requirement,
identify hazards, design, commit source, simulate or predict, build, measure,
test, document, review, and improve.

The curriculum spans mechanical, electrical/electronic, software, and systems
engineering. It uses one project-led spiral rather than three disconnected
subject blocks. Each project revisits all disciplines at greater depth and
uses the same open Creation structure, OMBR interfaces, App API, simulator,
validation tools, Git history, evidence, and release workflow used outside the
classroom.

The goal is not to make beginner work look artificially professional. The
goal is to make good engineering visible and achievable from the beginning,
with complexity, mathematics, tools, and physical access matched to learner
readiness.

## 2. Curriculum principles

1. **Authentic from project one.** Requirements, source control, safety,
   measurement, tests, review, and documentation begin small but never vanish.
2. **One multidisciplinary lifecycle.** Mechanics, electronics, software,
   controls, human use, cost, and lifecycle consequences meet in every
   Creation.
3. **Evidence over demonstration.** A successful demo is an observation, not
   proof of correctness, safety, repeatability, or fitness for another use.
4. **Prediction before measurement.** Learners calculate or simulate an
   expected result, measure reality, quantify the difference, and explain it.
5. **Safe failure is designed.** Every powered or moving activity declares a
   safe state, stop method, energy boundary, supervision rule, and recovery
   path before activation.
6. **Source is inspectable.** A guided or visual interface may reduce initial
   complexity, but learners can always reach the underlying files, contracts,
   program, diagnostics, and evidence.
7. **Git is engineering history.** Commits, issues, review, tags, provenance,
   reversibility, and releases matter more than treating a repository as cloud
   storage.
8. **Simulation is a model.** It has assumptions, fidelity, seeds, and error;
   it is compared with measurements rather than presented as truth.
9. **Open work is responsible work.** Licensing, attribution, privacy,
   accessibility, security, trademark use, and third-party asset rights are
   part of engineering practice.
10. **Repair and sustainability are design inputs.** Material, energy, cost,
    disassembly, spares, updates, reuse, and end of life are considered before
    a project is called complete.
11. **Multiple kinds of Creation matter.** Courses include stationary
    machines, instruments, mechanisms, and automation as well as vehicles and
    mobile robots.
12. **Reflection closes the loop.** Learners record limitations, failed ideas,
    review feedback, and the evidence behind each design change.

## 3. Competency pathways

Placement is based on demonstrated readiness, not age alone. Typical contexts
and hours are planning ranges, not legal age recommendations.

| Pathway | Typical context | Scope | Indicative guided time |
| --- | --- | --- | ---: |
| Explorer | Beginner, lower-secondary, outreach, or first maker experience | Protected modules, simple structures, guided Git, visual code or guided Python, basic measurement | 20–30 hours |
| Builder | Secondary, introductory vocational, or established maker course | Python, mechanisms, sensors, power budgets, CAD, simulation, team Git, structured tests | 45–60 hours |
| Engineer | Upper-secondary, vocational, first-year tertiary, or advanced maker | Python or C#, control, complete interfaces, calibration, CI, digital-twin comparison, system verification | 90–120 hours |
| Studio | Tertiary, vocational specialization, educator development, or capstone | Peripheral/controller design, PCB and open hardware, uncertainty, lifecycle, cost, compliance, independent release | One semester or equivalent |

An institution may deliver one pathway, combine them, or use individual
modules. It should publish prerequisites, selected outcomes, equipment tier,
contact time, independent work, assessment, and omitted topics.

## 4. HOMBRE learning outcomes

A learner completing the Engineer core should be able to:

| ID | Learner can... |
| --- | --- |
| HLO-01 | translate a human need into stakeholders, measurable requirements, constraints, acceptance tests, and a basic hazard analysis; |
| HLO-02 | design and explain a mechanical structure or transmission using frames, geometry, constraints, ratio, speed, torque, load, fit, tolerance, backlash, and failure limits; |
| HLO-03 | read and check a complete electronic interface contract, including connector and pinout, source/sink roles, protocol, voltage, full current envelope, grounding, sequencing, protection, cable, and fault behavior; |
| HLO-04 | select, connect, calibrate, and characterize sensors, actuators, and lights while reporting uncertainty, sample conditions, operating limits, and failure behavior; |
| HLO-05 | write a bounded, cancellable program against logical OMBR capabilities and run it through the App API against eligible physical and simulation providers; |
| HLO-06 | distinguish design, as-built, calibrated, simulated, desired, applied, reported, and observed state in a digital twin; |
| HLO-07 | use Git commits, issues, review, tags, dependency locks, and immutable Creation releases as traceable engineering evidence; |
| HLO-08 | plan and execute unit, integration, system, regression, and acceptance tests linked to requirements and hazards; |
| HLO-09 | analyze simulation and physical traces, quantify discrepancies, and state whether evidence supports parity, characterization, or only an observation; |
| HLO-10 | produce instructions and source sufficient for another team to reproduce, operate, diagnose, repair, verify, and fork a Creation; |
| HLO-11 | apply open-source/open-hardware licensing, attribution, provenance, privacy, security, trademark, and third-party asset rules at an introductory level; |
| HLO-12 | treat accessibility, repairability, energy, materials, cost, lifecycle, and sustainability as measurable design requirements; |
| HLO-13 | collaborate through roles, plans, design reviews, constructive feedback, conflict resolution, and clear technical communication; and |
| HLO-14 | explain assumptions, failures, uncertainty, ethical choices, and limitations honestly instead of presenting a working demonstration as complete validation. |

Explorer and Builder courses use subsets with simpler mathematics and stronger
scaffolding. Studio work deepens the same outcomes and may add formal
requirements, PCB design, manufacturing, statistics, control theory,
cybersecurity, or regulatory analysis.

## 5. Learning lifecycle

Every substantial project follows the same visible loop:

~~~mermaid
flowchart LR
    NEED["Need and stakeholders"] --> REQ["Requirements, hazards, acceptance"]
    REQ --> DESIGN["Mechanical, electronic, and software design"]
    DESIGN --> PREDICT["Calculate and simulate"]
    PREDICT --> BUILD["Build and program"]
    BUILD --> VERIFY["Measure, test, and review"]
    VERIFY --> OPERATE["Operate and observe"]
    OPERATE --> IMPROVE["Explain gaps, revise, release or fork"]
    IMPROVE --> REQ
~~~

Git records the authored history. The Creation release records the exact
engineering baseline. The digital twin links design, simulation, as-built,
and observed state without silently changing one into another.

## 6. Module sequence

The modules form a spiral. `M00` and `M01` start every pathway; later modules
are revisited at the depth appropriate to the pathway.

| Module | Core engineering content | Typical activity | Required portfolio evidence |
| --- | --- | --- | --- |
| M00 — Engineer responsibly | Need, stakeholders, requirements, accessibility, hazards, safe state, stop, teamwork | Review a sensor-to-light brief and reject unsafe or untestable claims | Design brief, requirements, acceptance checks, initial hazard log |
| M01 — A Creation as source | HOMBRE/OMBR object model, folder structure, Git, commit, issue, review, lock, release, Portal and provenance | Make and review the first change to a sample Creation | Valid source tree, meaningful commits, review record, reproducible typed development snapshot |
| M02 — Structures and motion | Frames, constraints, free-body thinking, joints, fit, tolerance, friction, gears, ratio, torque, speed, backlash | Build a geared indicator, gate, lift, conveyor, or vehicle subsystem | CAD/sketch, calculations, assembly record, dimensional and motion measurements |
| M03 — Electricity and interfaces | Voltage, current, resistance, power, source/sink, pinout, current envelopes, grounding, protection, sequencing and fault energy | Complete a safe-to-power review before connecting modules | Interface worksheet, source-load intersection, power budget, wiring check |
| M04 — Sense and actuate | Sampling, noise, calibration, uncertainty, motors, feedback, lights, range, saturation and limits | Characterize one sensor and one actuator across multiple points | Raw data, calibration method, plot/table, model, operating envelope and limitations |
| M05 — Program behavior | Capabilities, units, events, state machines, concurrency, time, cancellation, leases, errors and tests | Implement the same bounded behavior in simulation and hardware | Program source, dependency lock, unit tests, logs and clean stop evidence |
| M06 — Systems integration | Architecture, interfaces, identities, bindings, configuration, dependencies, trade-offs and failure propagation | Integrate independent team subsystems against agreed contracts | Architecture diagram, interface matrix, requirement trace, integration log |
| M07 — CAD and digital twin | Datums, frames, mates, joints, BOM, source versus derived assets, design/as-built/runtime layers | Assemble and inspect a Creation before physical construction | Editable CAD, OMBR graph, BOM, validation report and twin-layer screenshots/data |
| M08 — Simulation and control | Model fidelity, initial state, time step, seeds, feedback control, replay, SIL/HIL and comparison | Tune in simulation, deploy, record both traces and explain the gap | Run manifests, controller revision, traces, metrics, uncertainty and comparison class |
| M09 — Verification engineering | Fixtures, repeatability, sample size, uncertainty, unit/integration/system/acceptance tests, regression and CI | Convert a failure into a reproducible test and verified correction | Verification matrix, fixture record, raw results, CI output and signed review |
| M10 — Secure and open release | Least privilege, secrets, dependencies, SBOM, licenses, attribution, rights, privacy and untrusted content | Audit and pack a Creation for classroom-local sharing | Release recipe, lock, SBOM, license/provenance report, redaction and digest verification |
| M11 — Lifecycle and economics | Repair, disassembly, spares, energy, materials, sourcing, cost, support, update and end of life | Compare two designs using technical, lifecycle and cost evidence | Costed BOM, repair plan, lifecycle risks, substitution record and decision rationale |
| M12 — Capstone studio | Complete design–simulate–build–operate–observe–improve lifecycle | Solve an open problem and submit a reproducible release | Complete engineering portfolio, Creation release, demonstration and independent review |

## 7. Pathway composition

| Module | Explorer | Builder | Engineer | Studio |
| --- | :---: | :---: | :---: | :---: |
| M00–M01 | Core | Core | Core | Review/mentor |
| M02–M05 | Guided core | Core | Core | Advanced application |
| M06–M09 | Demonstration and selected exercises | Guided core | Core | Advanced application |
| M10–M11 | Guided awareness | Selected core | Core | Full trade study and release |
| M12 | Small individual/team challenge | Team project | Team capstone | Independent or partner capstone |

Suggested mathematics progression:

- Explorer: arithmetic, scale, units, ratios, coordinates, averages and simple
  graphs;
- Builder: algebra, geometry, trigonometry where useful, rate, basic
  statistics, error bands, torque and electrical power;
- Engineer: vectors, functions, sampling, probability/statistics, feedback,
  tolerances, uncertainty and energy; and
- Studio: discipline-appropriate calculus, dynamics, control, signal
  processing, reliability, optimization or engineering economics.

The practical task drives the mathematics, but the calculation must remain
visible and checked rather than hidden in a tool.

## 8. Project progression

### Project 0 — Safe reaction

A button, distance sensor, or simulated event controls a light. Learners define
one functional requirement, one safe state, and one capability mapping;
predict or simulate the response; build and measure it; record one Git issue
and commit; pass one repeatable test and peer review; write minimal build/use
notes; pack a typed development snapshot; and reflect on one discrepancy or
improvement. Explorer delivery can use protected modules and a guided editor
while still exposing the files and evidence.

### Project 1 — Predictable motion

Build a geared indicator, barrier, lift, conveyor, plotter axis, steering
module, or vehicle subsystem. Predict ratio, direction, travel, speed, torque
or load; measure it; record backlash, friction, fit, and limitations. Add a
bounded command and disconnect stop.

### Project 2 — Sensing machine

Create a sorter, environmental monitor, greenhouse controller, material test
stand, accessible signal, or mobile inspection device. Add calibration,
uncertainty, state-machine software, error handling, power budget, wiring
instructions, and an integration test.

### Project 3 — Twin-first system

Design a Creation in CAD, validate interfaces, run the selected Program Release
in simulation, deploy it to hardware, capture both Run Contexts and traces, and
explain discrepancies. A learner must distinguish model error, build
variation, calibration error, timing, software defect, and uncontrolled input.

### Project 4 — Team capstone

Solve an open stakeholder problem. Publish to a classroom-local Portal or
export an offline bundle containing the Git-backed Creation, editable CAD,
software, BOM, instructions, evidence, licenses, and known limitations. An
independent team must be able to verify and rebuild it. Public publication is
optional and never required for assessment.

## 9. Standard engineering portfolio

Assessment uses the reviewable engineering record, not only final function:

- need, stakeholders, requirements and acceptance criteria;
- hazard log, safe-state decision and activation checklists;
- issue/plan, contribution history and peer review;
- mechanical reasoning, calculations, editable CAD and assembly evidence;
- electronic interface specification, wiring and power budget;
- source code, dependency lock, static checks and automated tests;
- calibration procedure, raw data, uncertainty and model;
- simulation and physical run manifests, traces and comparison report;
- verification matrix with failures, corrections and regression evidence;
- BOM, cost, licenses, SBOM, provenance and substitution decisions;
- assembly, wiring, operation, troubleshooting, recovery and repair
  instructions; and
- individual reflection on assumptions, team decisions, limitations and next
  improvement.

Suggested weighting for an Engineer pathway:

| Dimension | Weight |
| --- | ---: |
| Systems reasoning, requirements and interfaces | 20% |
| Verification, measurement and evidence | 20% |
| Functional multidisciplinary implementation | 15% |
| Safety, fault handling and recovery | 15% |
| Reproducibility, Git and release quality | 15% |
| Accessibility, openness, sustainability and lifecycle | 10% |
| Collaboration, review and communication | 5% |

The weighting is a starting point. Safety gates are pass/fail and cannot be
compensated by marks elsewhere:

1. **safe to power** — electrical source, connection, polarity, protection,
   current, cable, stored energy, enclosure and supervision checks pass;
2. **safe to move or energize** — mechanical stability, guarding/clearance,
   limits, safe state, stop, command lifetime, bystander and recovery checks
   pass; and
3. **approved for the intended sharing scope** — secrets, personal data,
   executable content, licenses, attribution, marks, provenance, rights
   limitations and publication scope are reviewed under the institution's
   authorization process; the gate is not a legal-clearance opinion.

## 10. Laboratory and equipment tiers

| Tier | Permitted baseline | Example equipment | Exclusions without a higher approved tier |
| --- | --- | --- | --- |
| Protected learning kit | Preassembled current-limited HOMBRE modules and enclosed power | Protected hub, cables, sensors, lights, small actuators, rulers/calipers, barriers | Component-level power work, soldering, exposed energy storage |
| Measurement lab | Supervised low-voltage wiring and characterization | Current-limited isolated bench source, multimeters, calipers, force/load fixtures, optional oscilloscope | Mains, battery-pack construction, high-force or high-speed mechanisms |
| Advanced electronics lab | Qualified supervision for PCB/peripheral prototypes | ESD area, soldering, microscope, oscilloscope/logic analyzer, electronic load, thermal measurement and protected test enclosure | Work outside institutional electrical, chemical, ventilation and machine rules |

Core coursework excludes mains voltage, unprotected lithium cells, charger or
battery-pack fabrication, safety-critical control, exposed high-energy
mechanisms, occupant-carrying or public-road vehicles, medical/aviation use,
and hazardous production machinery. A higher education institution may add
separately governed advanced work only after its own qualified risk review,
facilities, training, supervision, and applicable legal/compliance process.

Every lab publishes emergency and incident procedures, supervision and
competency rules, tool training, PPE where applicable, energy limits, stop and
isolation methods, damaged-equipment quarantine, and accessibility
accommodations. HOMBRE documentation does not replace those local duties.

## 11. Teaching, inclusion, privacy, and responsible tools

- Use teams with rotating mechanical, electronic, software, verification,
  safety, documentation, and release responsibilities; every learner must
  demonstrate individual understanding across disciplines.
- Provide keyboard-accessible tools, text alternatives, non-color-only state,
  adjustable physical tasks, screen-reader-compatible instructions, and roles
  that do not exclude a learner from substantive engineering decisions.
- Prefer classroom-local Git, package mirrors, simulator, and Portal. Do not
  require a vendor account, public profile, home internet, personal device, or
  public release.
- Use institution-controlled identities and minimum data. Publication by or
  about minors requires teacher control, consent and safeguarding process,
  rights/privacy review, redaction, moderation, and an offline alternative.
- Treat downloaded Creations, plugins, scripts, CAD generators, and program
  bundles as untrusted. Preview does not execute code.
- AI-assisted code, CAD, analysis, or writing must be declared when required
  by the course, checked against primary evidence, reviewed and tested by the
  learner, free of submitted secrets/personal data, and attributable under the
  institution's policy. A learner must be able to explain and defend the
  resulting engineering decision.
- Grade failed but rigorous experiments fairly when requirements, method,
  evidence, diagnosis, and learning are sound. Do not encourage hidden failure
  or last-minute unsafe demonstrations.

## 12. Instructor and delivery package

A released module should provide:

- outcome, prerequisite, vocabulary, misconception and time maps;
- instructor guide, lesson plan, learner brief and accessible alternatives;
- exemplar Creation source, release recipe, expected evidence and known faults;
- equipment, consumable, spare, inspection and setup lists;
- risk assessment template, activation gates and emergency notes;
- formative checks, portfolio rubric and answer/evidence guide;
- fault-injection cases and troubleshooting decision trees;
- offline Workbench/simulator/package mirror instructions;
- classroom-local Git and Portal setup or a no-server workflow;
- privacy, publication and third-party asset guidance;
- localization-ready source, printable output and text alternatives; and
- module change log covering safety, tool, prerequisite and outcome changes.

Teacher preparation covers Git and review, OMBR identities and interfaces,
electronic limits, mechanical hazards, calibration/uncertainty, App API and
simulation semantics, digital-twin layers, safe fault injection, accessibility,
privacy, and evidence-based assessment. Advanced electronics modules require
appropriately qualified instructors or laboratory partners.

## 13. Curriculum source and release structure

The curriculum should model the source practice it teaches:

    hombre-curriculum/
      curriculum.json
      README.md
      LICENSES/
      outcomes/
        hombre-learning-outcomes.md
        outcome-module-matrix.csv
      pathways/
        explorer.md
        builder.md
        engineer.md
        studio.md
      modules/
        M00-engineer-responsibly/
          module.md
          instructor-guide.md
          learner-brief.md
          assessment.md
          safety.md
          accessibility.md
          assets/
          examples/
      projects/
      rubrics/
      safety/
      accessibility/
      localization/
      tools/
      releases/
        recipe.json
      sbom/
      tests/

The source is ordinary Git-friendly text and open media. A tagged curriculum
release records its semantic version, commit, outcome/module mapping, selected
OMBR specification and tool versions, exact sample-Creation releases, software
locks, licenses, SBOM, safety changes, generated printable artifacts, and
offline dependency closure. Hosted learning-management state is never the
only copy of curriculum meaning or learner-facing material.

Student work uses the OMBR Creation folder and release contract from the main
specification rather than a curriculum-only package format.

## 14. Pilot and continuous improvement

The first pilot should use at least two different settings and record:

- pathway, prior experience, selected modules, contact and independent hours;
- completion and safety-gate results;
- pre/post evidence against selected HLOs, not only satisfaction;
- accessibility barriers and accommodations;
- instructor preparation and support load;
- equipment failures, consumables, setup time and per-team cost;
- Git/release/Portal workflow failures and offline recovery;
- misconceptions, weak assessments and curriculum defects;
- incident and near-miss learning under the institution's process; and
- learner/instructor feedback with privacy-preserving publication.

Curriculum changes are reviewed like engineering changes: an issue states the
observed problem and evidence, a change identifies affected outcomes/modules/
safety notes, reviewers assess it, tests rebuild the offline/printable release,
and the release notes explain incompatibilities.

No learning-effectiveness claim should be generalized beyond the observed
cohort, setting, instrument, and uncertainty. Formal alignment to a national
or regional curriculum starts only after selecting the exact jurisdiction,
education level, framework edition, language, and assessment context.

## 15. Informative framework influences

These sources inform the draft; HOMBRE does not claim endorsement, formal
alignment, accreditation, or compliance with them.

| Source | Practice used in this draft | Boundary |
| --- | --- | --- |
| [CDIO Standards 3.0](https://www.cdio.org/content/cdio-standards-30) | Integrated conceive–design–implement–operate context, early engineering introduction, design/build experience, active learning, assessment and continuous improvement | HOMBRE adapts the lifecycle and does not claim to be a CDIO program |
| [ABET 2026–2027 engineering criteria](https://www.abet.org/accreditation/accreditation-criteria/criteria-for-accrediting-engineering-programs-2026-2027/) | Measurable outcomes involving design, experimentation, communication, ethics, teamwork and continuous improvement | A HOMBRE module or pathway is not an ABET-accredited degree program |
| [INCOSE Systems Engineering Handbook](https://www.incose.org/resources-publications/technical-publications/se-handbook/) | Lifecycle, interacting elements, requirements, interfaces, verification and systems thinking | The handbook is a professional reference; detailed content may require licensed access |
| [NIST Secure Software Development Framework 1.1](https://csrc.nist.gov/pubs/sp/800/218/final) | Secure-development practices integrated into the lifecycle, dependency/provenance awareness and learning from vulnerabilities | This curriculum uses selected introductory practices, not an SSDF conformity claim |
| [Git documentation](https://git-scm.com/book/en/v2.html) | Version history, commits, reviewable change, tags, branches, merging and reversibility | HOMBRE adds domain validation, locks and immutable Creation releases |
| [W3C Web Accessibility Initiative](https://www.w3.org/WAI/standards-guidelines/wcag/) | Perceivable, operable, understandable and robust digital material plus testable accessibility evidence | WCAG primarily governs web content; physical accessibility needs additional design and user evidence |
| [UNESCO Engineering Report](https://www.unesco.org/en/basic-sciences-engineering/report) | Engineering's societal role and explicit environmental, social and economic sustainability context | Project-level reflection is not proof of SDG impact or institutional alignment |

## 16. Open decisions

- first pilot pathway, learner context, language, institution and class size;
- minimum common outcomes and which remain pathway-specific;
- visual-programming source/interchange and progression into ordinary code;
- reference protected kit and equipment/cost tiers;
- classroom-local identity, Git and Portal deployment model;
- educator training and qualification expectations per lab tier;
- validated assessment instruments and portfolio moderation;
- accessibility co-design partners and physical task alternatives;
- curriculum/media license, contributor terms and translation policy;
- privacy/safeguarding model for student portfolios and publication;
- mappings to selected national, vocational or tertiary frameworks; and
- governance for safety notices, errata, module deprecation and long-term
  archive support.

The next curriculum milestone should select one pilot context and turn M00,
M01, M03, M05, and Project 0 into complete teachable packages, followed by a
Builder-scale sensing-machine project that exercises the full multidisciplinary
portfolio.
