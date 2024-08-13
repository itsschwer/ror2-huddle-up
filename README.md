# uiSpy

A \[ client-side \] mod that aims to expose more information in the UI.

## <mark>todo</mark>
- rename project
- describe patches in readme
- refactor loot tracking
    - clean up string formatting
    - try lumping equipment terminals into equipment barrels?
- add screenshots
- make icon

## patches
- `RunDifficultyTooltip`
- `ItemNameInTerminalContext`

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
    - scrapper *(includes cleansing pools (second tick))*
    - printers *(includes cauldrons; based on <u>input</u> item tier)*
    - void cradles *(includes void potentials)*
    - lunar pods *(does <u>not</u> include lunar buds (bazaar))*
- *once teleporter is charged:*
    - cloaked chests

### config (todo, maybe)
- option to always display loot panel (instead of only w/ scoreboard)
- what to display in panel
- when to display in panel (granular?)
    - always
    - on tp boss defeat
    - on tp charged
- a separate drones panel?

## see also
- [StageRecap](https://thunderstore.io/package/Lawlzee/StageRecap/) <sup>[*src*](https://github.com/Lawlzee/StageReport)</sup> by [Lawlzee](https://thunderstore.io/package/Lawlzee/) — released faster than me
- [DamageLog](https://thunderstore.io/package/itsschwer/DamageLog/) *(mine)* — another client-side UI mod, adding *"a damage log to the HUD to show what you have taken damage from recently."*
