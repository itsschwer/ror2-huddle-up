# HUDdle UP

A client-side mod that aims to expose more information in the HUD UI.

<!--
### *planned*
- **item stack calculations? *(soft-dependency on LookingGlass? separate api mod?)***
- refactor loot tracking and loot panel string formatting
-->

### configuration
each feature can be toggled on or off

> *[***OptionGenerator***](https://thunderstore.io/package/6thmoon/OptionGenerator/) *(+[***Risk Of Options***](https://thunderstore.io/package/Rune580/Risk_Of_Options/))* can be used to change the configuration in-game, rather than editing the file or using **r2modman**. If using ***OptionGenerator**, note that most toggles will not take effect until the next stage.*

### compatibility
- can be used alongside [***LookingGlass***](https://thunderstore.io/package/DropPod/LookingGlass/)
    - There is some feature overlap, so please adjust the following configuration options to fit personal preference:
        HUDdleUP | LookingGlass
        ---      | ---
        `fullerItemDescriptions` <br/> `fullerEquipmentDescriptions` | `Misc` > `Item Stats`
        `fullerDescriptionsOnPickUp` | `Misc` > `Full Item Description On Pickup`
        `commandMenuItemTooltips` | `Command Settings` > `Command Tooltips`

## fuller item descriptions
replace the default short *(pickup)* descriptions in item (and equipment) tooltips with a combination of the short and detailed descriptions (and equipment cooldown)

***note**: this mod currently does not provide calculated item stack stats*

vanilla | LookingGlass | HUDdleUP
--- | --- | ---
![screenshot of item description tooltip for equipment without any mods](https://github.com/itsschwer/ror2-huddle-up/blob/main/xtra/demo-fuller-descriptions-vanilla.png?raw=true) | ![screenshot of item description tooltip for equipment with the LookingGlass mod](https://github.com/itsschwer/ror2-huddle-up/blob/main/xtra/demo-fuller-descriptions-lookingglass.png?raw=true) | ![screenshot of item description tooltip for equipment with the HUDdleUp mod](https://github.com/itsschwer/ror2-huddle-up/blob/main/xtra/demo-fuller-descriptions.png?raw=true)

## item tooltips in Artifact of Command menu
add tooltips to items in pickup picker menus *(e.g. command cubes, void potentials)* that show the (fuller) description of the item

## equipment icon cooldown visual
add the cooldown progress visual from skill icons to the equipment icon

![screenshot of equipment icon cooldown visual](https://github.com/itsschwer/ror2-huddle-up/blob/main/xtra/demo-equipment-cooldown-visual.png?raw=true)

## difficulty tooltip in run info panel
add a tooltip to the difficulty icon in the HUD that shows the description of the run's difficulty

![screenshot of run difficulty tooltip](https://github.com/itsschwer/ror2-huddle-up/blob/main/xtra/demo-run-difficulty.png?raw=true)

## rename equipment drones
replace the names of equipment drones in ally cards with the name of its held equipment

![screenshot of renamed equipment drone ally cards](https://github.com/itsschwer/ror2-huddle-up/blob/main//xtra/demo-equipment-drone-rename.png?raw=true)

## scoreboard shows chat history
show the chat history when the scoreboard is open

----

## loot panel
adds a Loot panel to the hud to track how much loot is left on a stage *— only visible when the scoreboard is open*

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
- mountain shrine invitations <sup>*(~~※ unfortunately, this only works on host.~~ works on Alloyed Collective patch?)*</sup>
- stage name

## drone panel
adds a Drone panel to the HUD to track how many interactable drones are left on a stage *— only visible when the scoreboard is open*

initial | teleporter boss defeated | teleporter fully charged
--- | --- | ---
![screenshot of drone panel 1](https://github.com/itsschwer/ror2-huddle-up/blob/main/xtra/demo-drone-panel-1.png?raw=true) | ![screenshot of drone panel 2](https://github.com/itsschwer/ror2-huddle-up/blob/main/xtra/demo-drone-panel-2.png?raw=true) | ![screenshot of drone panel 3](https://github.com/itsschwer/ror2-huddle-up/blob/main/xtra/demo-drone-panel-3.png?raw=true)

*each screenshot is from a different stage! not meant to be the same drones at different teleporter phases!*

### what is tracked?
- gunner turrets
- triple drone shop *(counts individual terminals)*
- ***before teleporter boss is defeated:***
    - broken drones total
- ***once teleporter boss is defeated:***
    - broken drones by tier
    - drone scrapper *(`@` if present)*
    - drone combiner station *(`@` if present)*
- ***once teleporter is charged:***
    - broken drones by type *(if >0)*
<!--  -->
<mark>***known issue:** broken upgrade drones (from drone combiner station) are not tracked*<mark>

## railgunner accuracy panel
adds an Accuracy panel to the hud to track your accuracy with landing perfect reloads and hitting weak points

![screenshot of railgunner accuracy panel](https://github.com/itsschwer/ror2-huddle-up/blob/main/xtra/demo-railgunner-accuracy-panel.png?raw=true)

- active reload
    - run accuracy *-percentage-*
    - stage accuracy *-percentage-* (*-cardinal-*)
    - consecutive (best)
- weak point
    - stage ratio *— a single shot can hit multiple enemies' weak points*
    - consecutive (best)

## bandit combo panel
adds a Combo panel to the hud to track your consecutive cooldown resets when using the special skill "Lights Out"

> *※ unfortunately, this feature only works correctly on host — let me know on GitHub if there's a workaround!*

![screenshot of bandit combo panel](https://github.com/itsschwer/ror2-huddle-up/blob/main/xtra/demo-bandit-combo-panel.png?raw=true)

- lights out cooldown resets
    - run accuracy *-percentage-*
    - stage accuracy *-percentage-* (*-cardinal-*)
    - consecutive (best)
        > *※ due to how this is tracked internally, the consecutive tracker will not reset to 0 immediately upon miss — it updates the next time "Lights Out" is used instead.*

## multiplayer connection panel
adds a Connection panel to the HUD to check multiplayer latency (ping) *— only visible when the scoreboard is open*

![screenshot of multiplayer connection panel](https://github.com/itsschwer/ror2-huddle-up/blob/main/xtra/demo-multiplayer-connection-panel.png?raw=true)


----

## see also
- [LookingGlass](https://thunderstore.io/package/DropPod/LookingGlass/) <sup>[*src*](https://github.com/Wet-Boys/LookingGlass)</sup> by [DropPod](https://thunderstore.io/package/DropPod/) — the more definitive quality-of-life UI mod
    - goes beyond exposing information *(e.g. sorts items in the scoreboard inventory and the Artifact of Command menu)*
- [StageRecap](https://thunderstore.io/package/Lawlzee/StageRecap/) <sup>[*src*](https://github.com/Lawlzee/StageReport)</sup> by [Lawlzee](https://thunderstore.io/package/Lawlzee/) — shows a window at the end each stage reporting how many interactables you have collected *(server-side)*
<!--  -->
- [DamageLog](https://thunderstore.io/package/itsschwer/DamageLog/) — another client-side UI mod, adding *"a damage log to the HUD to show what you have taken damage from recently."*
