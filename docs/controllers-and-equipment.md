# Controllers and powered equipment

This page is a practical overview of controllers, receiver/hubs, motors, servos, and lights that are relevant to BrickController2. It updates and expands the 2023/2024 Spielzeug-Fuchs article [*Klemmbaustein Motoren – Eine umfassende Übersicht*](https://spielzeug-fuchs.de/klemmbaustein-motoren-eine-umfassende-uebersicht/).

Availability was checked on **12 July 2026**, primarily in manufacturers' German or European stores. Stock and regional availability can change. “Supported” below means that BrickController2 has a device implementation; it does not guarantee that every peripheral which fits a receiver's socket has been tested. Products for which no verifiable manufacturer, distributor, current retail, or archived market catalog was found are omitted.

This survey is also the seed inventory for HOMBRE's purchasable-component
benchmark program, defined by the
[OMBR draft](../spec/ombr/0.1/OMBR-SPEC.md). A listing here is not yet an OMBR
benchmark or compatibility result; that requires exact revision
identity, controlled measurements, public fixtures, uncertainty, raw data, and
the claim categories defined by BENCH-001 through BENCH-012.

## Buying guidance in 2026

The shortest current-product recommendations are:

- **Accurate steering and up to four Powered Up devices:** [BuWizz 3.0 Pro](https://buwizz.com/shop/buwizz-3-0-pro/) with a compatible Powered Up position motor is the clearest currently sold four-port option supported by BrickController2. It also has two PF-style outputs.
- **An existing PF-style motor, light, or servo collection:** an SBrick/SBrick Plus from the [SBrick catalog](https://sbrick.com/shop/) can keep that equipment useful. Confirm actual SBrick/SBrick Plus stock before designing around it; the current shop emphasizes SBrick Light. BuWizz 2.0 is covered under [Legacy / Used products](#legacy--used-products).
- **A simple two-port train or lighting model:** LEGO® [Hub 88009](https://www.lego.com/de-de/product/hub-88009) pairs with [Remote 88010](https://www.lego.com/de-de/product/remote-control-88010), [Train Motor 88011](https://www.lego.com/en-de/product/train-motor-88011), and [Light 88005](https://www.lego.com/de-de/product/light-88005). The [Technic Large Motor 88013](https://www.lego.com/de-de/product/technic-large-motor-88013) is another currently listed Powered Up motor.
- **Six PF-style receiver channels at lower cost:** the Mould King 6.0 module remains listed by retailers; one example is this [Mould King 6.0 listing](https://mouldking.ch/Power-Module-6.0/MK01-M0019-01). Do not assume proportional servo compatibility: confirm the exact receiver and servo revisions first.
- **Lighting-focused builds:** [SBrick Light products](https://sbrick.com/shop/) are currently listed. PFx Brick supports more elaborate motor and lighting control, but the [Bluetooth A216 listing](https://shop.fxbricks.com/products/pfx-brick) was sold out when checked.

> **Retail-link disclaimer:** These links are provided only as unsponsored, unaffiliated web finds. BrickController2 and its contributors receive no commission or other benefit from them. Except for clearly identified manufacturer stores, the linked sellers are not affiliated with the manufacturers named here. The project has not purchased from, tested, audited, or otherwise verified any linked retailer as a reliable supplier. Check seller identity, stock, revision, price, taxes, shipping, warranty, and return terms yourself before ordering.

## Start with the connector and receiver

Motors and lights are not controlled directly by the phone or computer. BrickController2 sends commands to a receiver/hub, and that device supplies power and, where applicable, a position-control signal to the equipment.

The two major connector families are:

- **Power Functions (PF) style:** the legacy LEGO® four-contact stacked plug, also used by many CaDA and Mould King products. Mechanical compatibility does not guarantee correct servo behavior.
- **Powered Up / LPF2 style:** the newer six-contact plug used by LEGO® Powered Up, Control+, BOOST, SPIKE, and recent BuWizz equipment. It supports identification and position feedback as well as power.

Adapters can bridge the physical connectors, but they do not necessarily translate identification, feedback, or servo-control protocols.

## What BrickController2 can control

### Input controllers

| Controller | Connection | Current availability | Notes |
| --- | --- | --- | --- |
| Generic gamepad | Bluetooth or USB | Widely available | Best choice for proportional throttle and steering. Actual platform support varies. |
| [LEGO® Powered Up Remote 88010](https://www.lego.com/de-de/product/remote-control-88010) | Bluetooth LE | Available from LEGO® in some European markets | Two button clusters, but no analog stick. BrickController2 uses it as an input device. LEGO® lists it for Hub 88009 and Move Hub 88006. |
| Device motion sensor | Device API | Depends on phone/tablet/computer | Can provide proportional input where the platform exposes it. |
| HTTP virtual controller | HTTP | Part of BrickController2 | Programs can provide buttons and proportional axes; see the [MK 3.8 HTTP controller example](../example/http_mk38_motor_test_client.py) and the [Linux headless API](linux-headless-api.md). |

LEGO® still lists the [Powered Up Remote 88010](https://www.lego.com/pl-pl/product/remote-control-88010), and its second-half 2026 catalog explicitly pairs 88010, Hub 88009, and Train Motor 88011 with new train-ready sets. This is useful evidence that the basic consumer Powered Up train ecosystem remains active in 2026.

### Receiver and hub families

| Receiver/hub | App channels | Equipment connection | BrickController2 behavior | Availability snapshot |
| --- | ---: | --- | --- | --- |
| [SBrick / SBrick Plus](https://sbrick.com/shop/) | 4 | PF style | Proportional bidirectional output; suitable for PF motors, PF lights, and compatible PF-style servos | Manufacturer remains active, but its shop currently emphasizes SBrick Light; some SBrick/SBrick Plus entries are read-more or intermittently unavailable |
| [SBrick Light](https://sbrick.com/shop/) | 8 light ports | SBrick Light LEDs | Brightness/color output | SBrick Light and kits are offered in the current shop |
| [BuWizz 3.0 Pro](https://buwizz.com/shop/buwizz-3-0-pro/) | 6 | 4 Powered Up + 2 PF | Normal motor output; closed-loop position-servo and stepper modes on the four Powered Up ports | Current product; replaceable 2300 mAh battery and USB-C charging |
| [LEGO® Hub 88009](https://www.lego.com/de-de/product/hub-88009) | 2 | Powered Up | Motor/light output through the LEGO® Wireless Protocol | Back-orderable in Germany when checked |
| [Mould King M0006 / 4.0 powered module](https://www.mouldkingbausteine.com/collection/zubehor/products/mould-king-m0006-2/) | 4 | PF style | Four channel values; supported by the Mould King 4.0 device implementation | A German retail listing showed the M0006 module for sale when checked |
| [Mould King 6.0 powered module](https://mouldking.ch/Power-Module-6.0/MK01-M0019-01) | 6 | PF style | Sends the receiver's native BLE channel commands | A European retailer showed the module in stock when checked |
| [CaDA Race Car C51074W](https://cada.toys/produkt/race-car-289-teile/) receiver | model-specific | CaDA | Native receiver commands | The receiver is supplied as part of an app-controlled model; this is not generic support for every CaDA battery box |
| [PFx Brick A216](https://shop.fxbricks.com/products/pfx-brick) | 2 PF + 8 light | PF style plus light dock | BrickController2 controls PF motor/light ports and dedicated light channels; audio is outside current app support | The Bluetooth unit was sold out when checked; accessories and documentation remain active |

## Motors and lights

### LEGO® Power Functions and compatible PF-style equipment

New PF-shaped motors, servos, and lights from CaDA, Mould King, and other vendors remain available. Their electrical behavior and quality are not standardized.

Typical equipment includes:

- M, L, XL, and high-speed/buggy drive motors;
- vendor-compatible steering servos;
- train motors;
- dual white LED lights;
- CaDA micro motors, a useful omission from many older comparisons;
- vendor light strings and extension cables.

The current Mould King catalog still lists M/L/XL, buggy, and servo products, although stock is mixed. One current official-store collection showed five of seven motor sets in stock and the XL and buggy sets sold out. Treat model numbers and motor revisions carefully: listings from different regional stores are not always consistent.

The PFx Brick remains one of the most flexible PF lighting devices: it has two PF outputs and eight dedicated light channels, and its current [quick-start guide](https://www.fxbricks.com/downloads/PFxBrickQuickStartGuideRevA.pdf) documents PF motors, PF LEDs, and third-party LEDs through its light accessory board.

### LEGO® Powered Up / Control+ equipment

Powered Up motors with integrated rotation sensing are the best fit for accurate servo positioning. BrickController2 can command target angles rather than merely applying motor power when the receiver implementation supports servo mode.

The 2026 German LEGO® store snapshot is uneven:

- Technic Large Motor 88013 was [available](https://www.lego.com/de-de/product/technic-large-motor-88013).
- Powered Up Light 88005 was [available](https://www.lego.com/de-de/product/light-88005).
- Hub 88009 was back-orderable.

Hub 88009, Train Motor 88011, Remote 88010, Light 88005, and Large Motor 88013 have the clearest current consumer availability. For a new four-port Technic build, the actively sold BuWizz 3 is the clearest BrickController2-supported route.

## Servo motors: what “PWM” means here

“PWM servo” is ambiguous in this ecosystem. Three different control methods must not be mixed.

### 1. Powered Up position motors

These are ordinary geared motors with an internal rotation sensor. The hub closes the position loop. BrickController2 already supports this model:

1. Select `ServoMotor` as the channel output type.
2. Set the maximum steering angle and center/base angle.
3. Map a proportional controller axis from `-100%` to `+100%`.
4. BrickController2 translates the axis into a target position and sends a LEGO® Wireless Protocol absolute-position command.

This path is implemented for LEGO® Control+/Technic hubs and for the four Powered Up ports on BuWizz 3. BuWizz 3 also supports automatic centering/calibration. The official [LEGO® Wireless Protocol](https://lego.github.io/lego-ble-wireless-protocol-docs/) distinguishes adjustable PWM power commands from position commands; position control is not achieved by adding ordinary duty-cycle PWM in the app.

### 2. CaDA and Mould King PF-shaped servos

An older article, [*Klemmbaustein Motoren – Eine umfassende Übersicht*](https://spielzeug-fuchs.de/klemmbaustein-motoren-eine-umfassende-uebersicht/), correctly warns that these currently sold third-party servos require a control waveform and must not simply be connected to a plain battery box. It overstates interoperability, however. A PF-shaped plug alone is not proof of proportional servo compatibility.

In Mould King 4.0/6.0 systems, BrickController2 sends channel values over Bluetooth; the receiver firmware then creates the electrical waveform at the socket. The app cannot directly select its PWM carrier frequency. Independent measurements report that some 4.0/6.0 receiver revisions produce about 470 Hz on C1/C2 while a tested Mould King servo behaved proportionally near 980/1200 Hz, resulting in full-left/center/full-right operation or unstable intermediate positions. See the detailed [measurement and proof of concept](https://www.eurobricks.com/forum/forums/topic/199132-custom-proportional-controller-for-mould-king-servo-motor/). This is community evidence, not a manufacturer specification, and hardware revisions may differ.

Consequences:

- Adding a generic PWM percentage to the BrickController2 UI will **not** fix incompatible Mould King receiver firmware.
- A SBrick or BuWizz PF output may work with some CaDA/Mould King servos, but each combination needs bench testing.
- Conventional three-wire RC servos need a separate 5–6 V supply and roughly 50 Hz control-pulse interface. They must not be wired directly to PF or Powered Up ports without a purpose-built adapter.
- Never hold a steering servo against a mechanical stop. Set conservative endpoints first and check current and temperature.

## Legacy / Used products

These products are supported by BrickController2 but are retired, discontinued, set-derived without a current standalone supply, or normally available only on the used market. They are useful for maintaining existing creations but should not be the basis of a new build unless replacement hardware has already been secured.

### Legacy input controllers

| Controller | Connection | BrickController2 notes |
| --- | --- | --- |
| [LEGO® Power Functions remotes 8885/8879](https://www.lego.com/en-gb/themes/power-functions/faq) | Infrared | Discontinued. Support requires an Android device with an IR emitter. Remote 8879 provides stepped proportional commands; 8885 is bang-bang control. |

### Legacy receivers and hubs

| Receiver/hub | App channels | Equipment connection | BrickController2 behavior | Lifecycle |
| --- | ---: | --- | --- | --- |
| [BuWizz 1](https://buwizz.com/tech-specs/) | 4 | PF style | Proportional motor/light output and selectable power level | Superseded; used market |
| [BuWizz 2.0](https://buwizz.com/shop/) | 4 | PF style | Proportional motor/light output, four voltage modes, and PF servo positions | Superseded by BuWizz 3.0 Pro; still documented and sometimes offered as remaining new stock or bundles; see [technical specifications](https://buwizz.com/tech-specs/) |
| [LEGO® BOOST Move Hub 88006](https://www.lego.com/en-us/product/move-hub-88006) | 2 external plus 2 internal motors | Powered Up | External ports and integrated motors | Retired product |
| [LEGO® Technic Hub 88012](https://www.lego.com/en-de/product/technic-hub-88012) | 4 | Powered Up | Normal power plus closed-loop servo/stepper positioning for motors with encoders | Retired/not in stock in Germany |
| [LEGO® WeDo 2.0 Core Set 45300 and Smart Hub](https://education.lego.com/en-us/products/lego-education-wedo-2-0-core-set/45300/) | 2 | LPF2/Powered Up family | Motor/light output | Retired education product |

### Legacy motors, servos, and lights

LEGO® discontinued Power Functions sales in 2020. Original LEGO® PF M, L, XL, train, servo, and LED parts are now used-market items. The original PF Servo Motor uses the two PF control contacts, C1 and C2, to select center and intermediate left/right positions. The receiver must generate the correct two-line waveform; this is not the 50 Hz, 1–2 ms pulse signal used by a conventional three-wire hobby RC servo.

BrickController2 can operate an original PF servo through these legacy or current PF-compatible receivers:

- **SBrick/SBrick Plus or BuWizz 1/2:** a proportional signed channel value produces PF-compatible output steps. The current [BuWizz user guide](https://buwizz.com/USERGUIDE.pdf) documents all 15 PF servo positions.
- **PF IR receiver:** remote 8879 provides stepped positions; direct IR support needs an Android IR emitter.
- **PFx Brick:** electrically supports PF equipment, but BrickController2 currently exposes its two PF ports as generic output channels. Dedicated PF-servo behavior still needs implementation and physical verification.

Technic Hub 88012, Technic XL Motor 88014, Large Hub 88016, and Medium Angular Motor 88018 were retired or unavailable in the German LEGO® store when checked. LEGO® Education also retired the SPIKE portfolio on 30 June 2026. Remaining distributor stock may exist, and LEGO® says the SPIKE app will receive maintenance support until 30 June 2031; see the [official retirement notice](https://education.lego.com/pl-pl/spike-update-2026/).

## Research gaps

The following should be verified on real hardware before turning this overview into a strict compatibility guarantee:

- exact servo behavior of the linked Mould King [4.0](https://www.mouldkingbausteine.com/collection/zubehor/products/mould-king-m0006-2/) and [6.0](https://mouldking.ch/Power-Module-6.0/MK01-M0019-01) hardware revisions;
- proportional positions and neutral behavior for current CaDA servo/receiver revisions;
- PF-servo behavior on PFx Brick through BrickController2 rather than the vendor app;
- SBrick and SBrick Plus new-unit stock versus support-only catalog pages.

When adding results, record receiver firmware/hardware revision, motor/servo part number, supply voltage, tested app version, supported range, neutral/failsafe behavior, and whether the result is manufacturer-documented or measured.

## Disclaimer

BrickController2 is an independent open-source project. It is not affiliated with, authorized by, sponsored by, or endorsed by the LEGO® Group or any of the other manufacturers named in this document. LEGO® is a trademark of the LEGO® Group. All other product names and trademarks belong to their respective owners. Retail links are unsponsored and unaffiliated web finds, not endorsements or guarantees. The project has not tested linked retailers as reliable suppliers and receives no commission from purchases.
