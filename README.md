# HUDdle UP

A \[ client-side \] mod that aims to expose more information in the HUD UI.

## <mark>todo</mark>
- rename project
- refactor loot tracking
    - clean up string formatting
    - try lumping equipment terminals into equipment barrels?
- add screenshots
- make icon
- configs to enable/disable each feature
- a separate drones panel?

## patches
- `RunDifficultyTooltip` — adds a tooltip to the difficulty icon in the HUD that shows the description of the difficulty
- `EquipmentDroneUseHeldEquipmentNameInAllyCard` *(host-only?)* — replaces the names of equipment drones in ally cards with the name of its held equipment
- `FullerDescriptions` — replace the default short descriptions in item and equipment tooltips with a combination of the short and detailed descriptions ***todo:* stack and cooldown calculations?**
- `PickupPickerTooltips` — add tooltips to items in pickup picker menus *(e.g. command cubes, void potentials)* that show the (fuller) description of the item

## loot panel
adds a Loot panel to the hud to track how much loot is left on a stage.

### what is tracked?
- multishop terminals *(counts individual terminals; includes **equipment** and shipping requests)*
- chests *(regular, large, legendary, category, large category)*
- adaptive chests
- chance shrines *(counts all potential drops if host)*
- equipment barrels ***todo:* try lumping equipment terminals in again?**
- lockboxes *(includes void variant)*
- *once teleporter boss is defeated:*
    - scrapper *(includes cleansing pools; `@` if present)*
    - printers *(includes cauldrons; based on <u>input</u> item tier)*
    - void cradles *(includes void potentials)*
    - lunar pods *(does <u>not</u> include lunar buds (bazaar))*
- *once teleporter is charged:*
    - cloaked chests
- enemy counts (per team)
- mountain shrine invitations

### config (todo, maybe)
- option to always display loot panel (instead of only w/ scoreboard)
- what to display in panel
- when to display in panel (granular?)
    - always
    - on tp boss defeat
    - on tp charged

## railgunner accuracy panel
- active reload
    - run accuracy *-percentage-*
    - stage accuracy *-percentage-* (*-cardinal-*)
    - consecutive (best)
- weak point
    - stage ratio *a single shot can hit multiple enemies' weak points*
    - consecutive (best)

## see also
- [StageRecap](https://thunderstore.io/package/Lawlzee/StageRecap/) <sup>[*src*](https://github.com/Lawlzee/StageReport)</sup> by [Lawlzee](https://thunderstore.io/package/Lawlzee/) — released faster than me
- [DamageLog](https://thunderstore.io/package/itsschwer/DamageLog/) *(mine)* — another client-side UI mod, adding *"a damage log to the HUD to show what you have taken damage from recently."*
