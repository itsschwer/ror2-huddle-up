# uiSpy

A \[ client-side \] mod that aims to expose more information in the UI.

## <mark>todo</mark>
- rename project
- describe patches in readme
- refactor loot tracking
- maybe track cleansing pools and lunar pods
- make loot panel only show with scoreboard
- add screenshots

## patches
- `RunDifficultyTooltip`
- `ItemNameInTerminalContext`

## loot panel
adds a Loot panel to the hud to track how much loot is left on a stage.

### what is tracked?

- multishop terminals *(counts individual terminals; includes equipment and shipping requests)*
- chests *(regular, large, legendary, category, large category)*
- adaptive chests
- chance shrines *(counts all potential drops if host)*
- equipment barrels ***todo:* try lumping equipment terminals in again?**
- lockboxes *(includes void variant)*
- *once teleporter boss is defeated:*
    - scrapper ***todo:* include cleansing pools? `SHRINE_CLEANSE_NAME`**
    - printers *(includes cauldrons; based on <u>input</u> item tier)*
    - void cradles *(includes void potentials)*
    - ***todo:* lunar pods? `LUNAR_CHEST_NAME`**
- *once teleporter is charged:*
    - cloaked chests

### config (todo, maybe)
- what to display in tooltip
- when to display in tooltip (granular?)
    - always
    - on tp boss defeat
    - on tp charged
- track more?

## see also
- [StageRecap](https://thunderstore.io/package/Lawlzee/StageRecap/) <sup>[*src*](https://github.com/Lawlzee/StageReport)</sup> by [Lawlzee](https://thunderstore.io/package/Lawlzee/) — released faster than me
- [DamageLog](https://thunderstore.io/package/itsschwer/DamageLog/) *(mine)* — another client-side UI mod, adding *"a damage log to the HUD to show what you have taken damage from recently."*
