# HUDdle UP

A \[ client-side \] mod that aims to expose more information in the HUD UI.

## <mark>todo</mark>
- refactor loot tracking
    - clean up string formatting
- add screenshots
- make icon
- configs to enable/disable each feature
- a separate drones panel?
- **item stack calculations? *(separate api mod?)***
- **add the sweeping(?) animation from skill cooldowns to the equipment cooldown**

## patches
- `RunDifficultyTooltip` — adds a tooltip to the difficulty icon in the HUD that shows the description of the difficulty
- `EquipmentDroneUseHeldEquipmentNameInAllyCard` — replaces the names of equipment drones in ally cards with the name of its held equipment
    - *does not update if the drone changes equipment (until next stage)*
        - ***technical**: checks `nameLabel` for equipment drone but replaces with equipment name*
- `FullerDescriptions` — replace the default short descriptions in item (and equipment) tooltips with a combination of the short and detailed descriptions (and equipment cooldown)
- `PickupPickerTooltips` — add tooltips to items in pickup picker menus *(e.g. command cubes, void potentials)* that show the (fuller) description of the item

## loot panel
adds a Loot panel to the hud to track how much loot is left on a stage.

### what is tracked?
- multishop terminals *(counts individual terminals; includes shipping requests, excludes equipment)*
- chests *(regular, large, legendary, category, large category)*
- adaptive chests
- chance shrines *(counts all potential drops if host)*
- equipment barrels *(includes equipment terminals)*
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
