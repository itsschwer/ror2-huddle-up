# HUDdle UP

A \[ client-side \] mod that aims to expose more information in the HUD UI.

### *planned*
- <mark>add railgunner accuracy panel screenshot</mark>
- **add the sweeping line progress animation from skill cooldowns to the equipment cooldown**
<!--
- **item stack calculations? *(soft-dependency on LookingGlass? separate api mod?)***
- refactor loot tracking and loot panel string formatting
-->

## fuller item descriptions
replace the default short *(pickup)* descriptions in item (and equipment) tooltips with a combination of the short and detailed descriptions (and equipment cooldown)

***note**: this mod currently does not provide calculated item stack stats*

vanilla | HUDdleUP | LookingGlass
--- | --- | ---
![screenshot of item description tooltip for equipment without any mods](https://github.com/itsschwer/ror2-huddle-up/blob/main/xtra/demo-fuller-descriptions-vanilla.png?raw=true) | ![screenshot of item description tooltip for equipment with the HUDdleUp mod](https://github.com/itsschwer/ror2-huddle-up/blob/main/xtra/demo-fuller-descriptions.png?raw=true) | ![screenshot of item description tooltip for equipment with the LookingGlass mod](https://github.com/itsschwer/ror2-huddle-up/blob/main/xtra/demo-fuller-descriptions-lookingglass.png?raw=true)

## item tooltips in Artifact of Command menu
add tooltips to items in pickup picker menus *(e.g. command cubes, void potentials)* that show the (fuller) description of the item

## difficulty tooltip in run info panel
add a tooltip to the difficulty icon in the HUD that shows the description of the run's difficulty

![screenshot of run difficulty tooltip](https://github.com/itsschwer/ror2-huddle-up/blob/main/xtra/demo-run-difficulty.png?raw=true)

## rename equipment drones
replace the names of equipment drones in ally cards with the name of its held equipment

![screenshot of renamed equipment drone ally cards](https://github.com/itsschwer/ror2-huddle-up/blob/main//xtra/demo-equipment-drone-rename.png?raw=true)

## scoreboard shows chat history
show the chat history when the scoreboard is open.

----

## loot panel
adds a Loot panel to the hud to track how much loot is left on a stage *— only visible when the scoreboard is open*.

![screenshot of loot panel](https://github.com/itsschwer/ror2-huddle-up/blob/main/xtra/demo-loot-panel.png?raw=true)

### what is tracked?
- multishop terminals *(counts individual terminals; includes shipping requests, excludes equipment)*
- chests *(regular, large, legendary, category, large category)*
- adaptive chests
- chance shrines *(counts all potential drops if host)*
- equipment barrels *(includes equipment terminals)*
- lockboxes *(includes void variant)*
- ***once teleporter boss is defeated:***
    - scrapper *(includes cleansing pools; `@` if present)*
    - printers *(includes cauldrons; based on <u>input</u> item tier)*
    - void cradles *(includes void potentials)*
    - lunar pods *(does <u>not</u> include lunar buds (bazaar))*
- ***once teleporter is charged:***
    - cloaked chests
- enemy counts (per team)
- mountain shrine invitations

## railgunner accuracy panel
- active reload
    - run accuracy *-percentage-*
    - stage accuracy *-percentage-* (*-cardinal-*)
    - consecutive (best)
- weak point
    - stage ratio *a single shot can hit multiple enemies' weak points*
    - consecutive (best)

![screenshot of railgunner accuracy panel](https://github.com/itsschwer/ror2-huddle-up/blob/main/xtra/demo-railgunner-accuracy-panel.png?raw=true)


----

## see also
- [LookingGlass](https://thunderstore.io/package/DropPod/LookingGlass/) <sup>[*src*](https://github.com/Wet-Boys/LookingGlass)</sup> by [DropPod](https://thunderstore.io/package/DropPod/) — the more definitive quality-of-life UI mod
    - goes beyond exposing information *(e.g. sorts items in the scoreboard inventory and the Artifact of Command menu)*
- [StageRecap](https://thunderstore.io/package/Lawlzee/StageRecap/) <sup>[*src*](https://github.com/Lawlzee/StageReport)</sup> by [Lawlzee](https://thunderstore.io/package/Lawlzee/) — shows a window at the end each stage reporting how many interactables you have collected *(server-side)*
<!--  -->
- [DamageLog](https://thunderstore.io/package/itsschwer/DamageLog/) — another client-side UI mod, adding *"a damage log to the HUD to show what you have taken damage from recently."*
