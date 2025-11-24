- Show stage name in `LootPanel`

### 1.2.3
- Fix *"only visible when the scoreboard is open"* HUD panels to respect the *Toggle Inventory* gameplay setting
    - Does not (yet?) apply to `scoreboardShowChat` as that does not have a direct reference to the HUD

### 1.2.2
- Add new configuration option `fullerDescriptionsOnPickUp`
    - Defaults to `false`
- Rename main configuration section from empty string to `_`
    - *So that section appears correctly in new(?) r2modman config editor*

### 1.2.1
- Try appying all Harmony patches instead of stopping at first failure
    - *So that if a feature fails, other features may still be applied and used*

## 1.2.0
- Initial fix for the Alloyed Collective patch
    - <mark>*This version of the mod is not compatible with earlier patches of the game. Please use the previous version of this mod if playing on an older game version!*</mark>

### 1.1.1
- Refactor `HUDPanel` to be more robust
- Try-catch **`DekuDesu-MiniMapMod`** compatibility hook to avoid spamming `NullReferenceException` in the log (unknown trigger)

## 1.1.0
- Add `MultiplayerConnectionPanel`
- Add `BanditComboPanel` *(host-only)*
- Fix survivor body check on clients failing sometimes
    - *Fix `RailgunnerAccuracyPanel` sometimes not appearing on clients that are playing as Railgunner*
- Note in readme that mountain shrine invitations tracker in `LootPanel` only works on host
    - *Vanilla property is not networked (i.e. value is not shared with clients)*
- Fix incompatibility with **`DekuDesu-MiniMapMod`**

### 1.0.3
- Fix the equipment icon cooldown progress visual (`equipmentIconCooldownVisual`) appearing to start midway through when waiting for additional equipment charges after switching from a high cooldown equipment to a low cooldown equipment
- Fix incompatibility with **`Bubbet-RiskUI`**/**`score-RiskUIRevived`**
    - Improve `EquipmentCooldownProgressVisual` null-checking
    - Refactor `HUDPanel`

### 1.0.2
- Fix incompatibility between `equipmentIconCooldownVisual` and **`TeamMoonstorm-Starstorm2`**'s "Composite Injector" item
    - *Remove cooldown progress visuals (blue lines) from "Composite Injector" extra equipment icon slots*
        - These would appear even if the player does not have a "Composite Injector", as **Starstorm 2** always generates the extra equipment icon slots (but hides them afterwards)

### 1.0.1
- Fix `EquipmentDroneUseHeldEquipmentNameInAllyCard` spamming `NullReferenceException` in the log when an ally holding an equipment dies *(e.g. Happiest Mask elite)*
- Add mod compatibility section
    - Explicitly state that HUDdleUP can be used alongside LookingGlass

# 1.0.0
- Initial release
