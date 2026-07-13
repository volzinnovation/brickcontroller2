<!-- SPDX-License-Identifier: MIT -->

# OMBR legal, IP, and market-access register

Version: 0.1.0-seed

Reviewed: 13 July 2026

Status: Screening framework; all commercial-clearance rows remain open

This public companion to the [OMBR draft](OMBR-SPEC.md) records known legal,
intellectual-property, and regulatory work. It is not legal advice, a freedom-
to-operate opinion, product certification, or permission from any third party.
Only qualified counsel and competent conformity professionals working with an
exact design, intended act, market, and date can close the applicable gates.

## 1. Current executive register

| Area | Current status | Why it matters | Required next evidence | Release effect |
| --- | --- | --- | --- | --- |
| `OMBR` word/name clearance | `open-high` | The working name may conflict with earlier marks, names or domains in relevant countries/classes | EUIPO/WIPO and national searches, company/domain search, counsel review, owner and search date; repeat for final logo/device mark | Do not invest in commercial branding or certification marks before clearance |
| LEGO/TECHNIC/MINDSTORMS references | `controlled-open` | Factual compatibility can still become confusing mark use or imply endorsement | Naming/style guide, exact compatibility wording, test-linked claims, mark attribution, commercial review | Plain-text minimal references only; never in OMBR product/domain/package/badge names |
| Logos, colors and trade dress | `open-high` | Logos and source-indicating appearance create confusion risk independently of functional fit | Original visual identity and enclosure, overall-impression review, dated design history | No third-party logo, stylized mark, copied ornamental shell, or look-alike packaging |
| Modular element and interface designs | `open-high` | Technical or must-fit features are not automatically outside design protection; modular-system rules can remain relevant | Feature-by-feature interface-necessity map, alternatives, independent metrology/CAD history, registered/unregistered design searches and counsel review | Freeze only necessary mating geometry; redesign all nonfunctional appearance; affected release blocked until disposition |
| Connector/contact system | `open-high` | A new connector may engage patent/design/mark rights; a proprietary legacy plug can create supply and rights dependence | Candidate claims/design search, supplier terms, tooling/supply rights, exact interface FTO and alternate | P0 remains experimental; no production selection before gate |
| Hub electrical architecture | `open-high` | Power switching, motor drive, safety gating, hot plug and multi-port functions may meet live patent claims | Feature decomposition, claims/family/status search in target countries, counsel claim charts and mitigation | No commercial design freeze before FTO disposition |
| Motors, sensors, gears and adapters | `open-high` | Open CAD/firmware does not clear cartridge, encoder, gear/interface, adapter or protocol claims | Exact candidate/supplier terms, patent/design searches, interface tests, clean-room or license records | Each released component/revision needs its own record |
| LEGO/BrickLink/Studio catalog and CAD data | `restricted` | Website, instruction, image, Studio and bulk catalog rights do not form an open engineering library | Terms review per source; sparse factual aliases only; independent photos/renders/metrology/CAD; release-tree scanner | No scraping, bulk mirroring, Studio asset extraction or restricted redistribution |
| LDraw data | `per-file-review` | LDraw is community-authored and files can have different licenses; geometry/units are not fit authority | Per-file source, author, license, attribution, digest, modification and redistribution record | Optional visual/assembly import only after file review; never sole engineering authority |
| Legacy protocol interoperability | `open-high` | Observation/decompilation/repair exceptions are narrow, territorial and purpose limited | Lawful-acquisition file, unavailable-information record, necessity/minimization, jurisdictional review, clean-room outputs | No extracted firmware/source/keys/assets; publish only independently authored interface/test material |
| Specification contributor patents | `open-high` | A copyright license alone does not ensure implementations receive essential patent rights | Patent-aware specification license, contributor/corporate CLA or equivalent, essential-claim disclosure and counsel review | Normative 1.0 contributions blocked until policy is effective |
| Hardware/software/data licensing | `open-medium` | One project contains differently protected artifacts and reciprocal scopes | SPDX/REUSE inventory, preferred source, holders/notices, patent clauses, compatibility and outbound policy | CI blocks unknown/NC/ND/field-of-use/revocable required assets |
| Product classification | `open-high` | Hub, kit, toy, education product, machine component and radio product have different duties | Exact SKU/intended-use/age/market/economic-operator record and competent review | No CE/market claim against a generic source repository |
| EU radio/EMC/environment | `open-high` | Wi-Fi/Bluetooth makes the final controller radio equipment; upstream Pi testing is not final-product conformity | Final enclosure/antenna/cable/power configuration, RED/EMC/RoHS/REACH/WEEE matrix, risk file, lab plan and DoC owner | Commercial EU controller blocked until applicable conformity route passes |
| EU cybersecurity/data | `open-high` | RED cybersecurity duties, CRA transition, Data Act and privacy may apply to connected products/services | Product/security risk assessment, SBOM, update/recovery/support period, vulnerability/reporting owner, data inventory/export/delete/access design | Must be architected from Phase 0; not an after-launch documentation task |
| Toy/child-directed use | `decision-open` | Objective design/marketing/use may make a product a toy; a 14+ label is not decisive by itself | Intended-age/marketing decision, toy classification, chemical/mechanical/electrical/flammability/hygiene assessment and date-specific transition plan | Base draft remains maker/education research, not a toy claim |
| Machine/vehicle use | `decision-open` | “Machine” and “vehicle” are use cases, not one legal category; hazardous or occupant/public-road uses expand obligations | Sector/use classification, energy/risk assessment, guarding and separately qualified safety chain | Base profile excludes safety-related, occupant, public-road, medical, aviation and hazardous production control |
| Product liability, warranty, recall and insurance | `open-high` | Open source does not remove manufacturer/importer/distributor obligations | Economic-operator map, terms/warranty, traceability, incident/recall procedure, insurance and reserve | Include in cost and launch gate |

`Open-high` means the issue is material and not yet dispositioned. It does not
mean infringement is known. `Controlled-open` means a conservative interim
rule exists but final review is incomplete.

## 2. Required review record

Every legal/IP record must be tied to a concrete subject rather than to OMBR in
the abstract:

```text
review_id
subject_design_and_revision
firmware_software_data_and_asset_revisions
intended_acts: make | use | offer | sell | import | distribute | modify
target_countries_and_channel
launch_and_support_window
intended_user_age_and_use
feature_or_right_type
search_sources_queries_classes_and_dates
candidate_rights_families_jurisdictions_and_current_status
evidence_ids_and_confidential_opinion_reference
reviewer_and_qualified_counsel_status
decision: no-issue-identified-not-clearance | monitor | redesign |
          seek-license | obtain-counsel-opinion | stop
mitigation_owner_due_date_and_verification
residual_risk_and_next_review_trigger
```

The public record can omit privileged claim charts and advice, but it must not
replace them with an unexplained `cleared` flag.

## 3. Trademark and compatibility controls

The project will use OMBR as a working technical identifier until word and
device-mark clearance. No repository name alone proves availability.

Required compatibility style:

- third-party marks appear only where necessary to identify the referenced
  element, interface, product, benchmark, or adapter;
- the mark is plain text, not a logo, stylized type, badge, prominent heading,
  package namespace, domain, product family, or certification name;
- mechanical fit, electrical adaptation, protocol interoperability,
  behavioral compatibility, and visual similarity are distinct claims;
- the exact third-party product/interface ID and revision plus OMBR test report
  are adjacent or linked;
- owner attribution and a non-affiliation/non-sponsorship statement are nearby;
- comparison is accurate, current, necessary, and not disparaging or
  confusing; and
- commercial packaging and advertising receive fresh market-specific review.

The LEGO Group's [Fair Play guidance](https://www.lego.com/en-in/legal/notices-and-policies/fair-play)
warns that its logo is not for unofficial sites, describes limited
noncommercial referential use, and states that a disclaimer cannot make an
otherwise improper use proper. It is guidance from the rights holder, not a
commercial license or general legal safe harbor. EU referential-trademark
rules and honest-practices limits require case-specific review; see
[EU Trade Mark Regulation Article 14](https://eur-lex.europa.eu/legal-content/EN/TXT/?uri=CELEX:32017R1001).

## 4. Patents and freedom-to-operate workflow

FTO asks **what act, in which country, during which period, for which exact
features and revision**. It is not equivalent to a novelty search and cannot
be inferred from an open license or an expired famous patent.

The search plan covers:

1. Pi/carrier split, safety MCU, watchdog and output gate;
2. battery/input protection, rail separation, hot plug, regenerative energy,
   motor control and current sensing;
3. native connectors, contacts, keying, cables, port power and differential
   physical layer;
4. motor, encoder, servo/position, light, sensor, adapter and discovery
   behavior;
5. brick/beam/stud/pin/axle mating, shells, gears, racks, worms,
   differentials, joints and transmission assemblies;
6. capability descriptions, control leases, remote operation and safety
   behavior;
7. design/as-built/runtime twin synchronization, CAD semantics, simulation,
   calibration and trace comparison; and
8. Workbench, deployment, debugging, visual programming and package flow.

For each feature, search keywords and synonyms, IPC/CPC classifications,
applicants/inventors, citations and families in
[EPO Espacenet](https://www.epo.org/en/searching-for-patents/technical/espacenet),
[WIPO PATENTSCOPE](https://www.wipo.int/en/web/patentscope/), national tools
such as [USPTO Patent Public Search](https://www.uspto.gov/patents/search/patent-public-search/),
and target-country registers. Retain independent claims, family/jurisdiction,
priority/publication/grant dates, assignee, legal-status source/date,
expiry/fees/opposition, claim-chart evidence, reviewer, mitigation and next
review. EPO itself notes that legal-event/status data supports FTO work; the
official register must still be checked per territory.

## 5. Design, CAD, catalog, and database controls

The reference library separates four things:

1. a third-party catalog/source record;
2. sparse factual aliases and dated availability observations;
3. a lawful visual/assembly proxy with its own rights; and
4. an independently authored OMBR functional model or open replacement.

There is no public open manufacturer-grade LEGO element CAD/tolerance/load
catalog identified by this review. OMBR must not build one by extracting
BrickLink Studio, official instructions, product images, Pick a Brick, or
bulk catalog/API data. The [Studio license](https://studiohelp.bricklink.com/hc/en-us/articles/6606313426711-Studio-Software-License-Agreement),
[BrickLink terms](https://v2.bricklink.com/en-us/terms-of-service), and
[LEGO site terms](https://www.lego.com/en-us/legal/terms-of-use) require
asset-specific review; export capability is not relicensing.

LDraw may be imported only under each file's actual license and attribution.
Its approximate scale and community `Official` status do not make it LEGO
engineering CAD. Physical metrology and OMBR test evidence govern fit.

For compatibility geometry, the file retains:

- objective function and exact mating counterpart;
- necessary surfaces/features and alternatives considered;
- purchased sample provenance, metrology method and uncertainty;
- dated independent CAD history and contributor provenance;
- original nonfunctional geometry, shell, color and ornament;
- design/patent searches and qualified disposition; and
- overall-impression comparison for target markets.

The current consolidated [EU Designs Regulation](https://eur-lex.europa.eu/legal-content/EN/TXT/?uri=CELEX:02002R0006-20260701)
contains technical-function and must-fit provisions but also specific modular-
system treatment. No exception is assumed without counsel.

## 6. Protocol and repair research controls

OMBR promotes repair and interoperability subject to safety, security,
privacy, contract and IP law. A protocol research record identifies lawful
access/ownership, exact interoperability purpose, information unavailable from
the vendor, acts performed, necessity/minimization, territory, roles and
clean-room separation. Release outputs are independent interface descriptions,
test vectors and code—not copied firmware/source/comments/keys/assets.

The [EU Software Directive](https://eur-lex.europa.eu/legal-content/EN/TXT/?uri=CELEX:32009L0024)
contains narrow observation/testing and interoperability provisions for lawful
users; it is not blanket permission to copy firmware, CAD or catalogs. The
[EU Right-to-Repair Directive](https://eur-lex.europa.eu/legal-content/EN/TXT/?uri=CELEX:32024L1799)
does not eliminate IP, safety, product-scope or territorial analysis.

## 7. Open specification and artifact licensing

The final policy needs separate, compatible layers:

| Layer | Candidate policy | Open issue |
| --- | --- | --- |
| Normative specification/interfaces | Patent-aware agreement such as OWFa/Community Specification after counsel review | Exact copyright/patent grant, version binding, essential claims, withdrawals and governance |
| Specification contributions | Corporate-capable copyright and patent contribution agreement plus provenance attestation | Employer authority, known essential claims, confidential/third-party material and DCO role |
| Project hardware/CAD | Deliberately selected CERN-OHL-v2 variant | Strong (`S`), weak (`W`) or permissive (`P`) reciprocity boundary and adoption goals |
| New permissive software | Apache-2.0 where compatible | Dependency/kernel exceptions, generated code and plugin boundaries |
| Tutorials/images/data | Explicit CC BY/CC0 or suitable data license per artifact | Attribution, database rights, privacy and whether patents are relevant |
| Marks/conformance | Separate trademark and certification policy | Factual compatibility vs authorized certification, quality control and fork naming |

CC licenses chiefly address copyright and related rights; they must not be
treated as a complete standards-patent solution. Contributor grants cover only
claims the contributor controls. External FTO remains required.

## 8. EU product-classification matrix

This table is an issue-spotting map for the first intended market, not an
applicability conclusion.

| SKU/use | Primary areas to assess | Key point |
| --- | --- | --- |
| Wi-Fi/Bluetooth controller or radio sensor | RED, integrated-product EMC/safety/spectrum, RoHS/REACH/WEEE, GPSR, CRA transition, privacy/data and consumer duties | Pi compliance evidence is an input; final antenna/enclosure/cables/power/software are assessed together |
| Wired motor/light/sensor/module | EMC where apparatus, RoHS/REACH/WEEE, GPSR and use-specific rules | Low voltage does not mean no safety or consumer-product duties |
| Battery or battery-containing kit | EU Batteries Regulation, transport, protection, labeling/EPR, replacement, recycling and instructions | Base affordable profile excludes a battery; any included pack is a separately costed/profiled product |
| Passive brick/gear/adapter | GPSR, REACH/materials, design/IP; toy rules if intended for play by under-14 users | Passive does not mean unregulated or free of design rights |
| Child/play/education kit | Current and transition-date toy rules plus radio/electrical/environmental/cyber overlays | Marketing, appearance and objective use matter; `14+` text alone is not determinative |
| Stationary machine/automation component | Machinery rules depending on final function, EMC/electrical/product rules and end-system risk assessment | Base OMBR is not a safety-related control system or production-machine certification |
| Vehicle creation | Toy, mobile machinery, road/type approval, light transport, marine, unmanned aircraft or other sector rules depending on use | Base profile is non-occupant and off-public-road only |
| Digital twin/connected service | CRA, Data Act, GDPR/ePrivacy where personal data, consumer and contract duties | Local-first exportable telemetry supports compliance but does not prove it |

The [EU Blue Guide](https://eur-lex.europa.eu/legal-content/EN/TXT/?uri=CELEX:52022XC0629(04))
explains integrated-product and substantial-modification responsibilities. The
[Radio Equipment Directive](https://eur-lex.europa.eu/eli/dir/2014/53/oj/eng),
[General Product Safety Regulation](https://eur-lex.europa.eu/eli/reg/2023/988/2026-05-29/eng),
[Cyber Resilience Act](https://eur-lex.europa.eu/eli/reg/2024/2847/2024-11-20/eng),
[Batteries Regulation](https://eur-lex.europa.eu/eli/reg/2023/1542/oj), and
[Toy Safety Regulation transition](https://eur-lex.europa.eu/legal-content/EN/TXT/?uri=CELEX:32025R2509)
must be checked in their current consolidated forms for the actual release
date. Product-specific harmonized standards and competent advice are still
required.

## 9. Pre-release stop list

A public commercial reference product must not ship while any applicable item
below is unresolved:

- OMBR product/mark clearance and final compatibility style;
- feature- and territory-specific patent/FTO counsel disposition;
- registered/unregistered design and overall-impression disposition;
- unknown or incompatible source/CAD/data/software/hardware license;
- restricted catalog or application asset in the release tree;
- undocumented protocol-research provenance or copied protected material;
- unclear manufacturer/importer/distributor/responsible-person roles;
- missing product classification, risk assessment or applicable conformity
  route;
- incomplete radio/EMC/electrical/environmental/chemical evidence;
- missing security support, update/recovery, vulnerability and incident owner;
- unbudgeted compliance, warranty, recall, insurance, support or recycling
  obligation; or
- a material component, firmware, enclosure, antenna, cable, supplier or
  intended-use change that has not passed change control.

Open development can continue while a row is open, provided the risk is
visible and no clearance or conformance claim is made. Commercial architecture
and release freeze require the corresponding disposition.
