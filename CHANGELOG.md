- Fix the equipment icon cooldown progress visual (`equipmentIconCooldownVisual`) appearing to start midway through when waiting for additional equipment charges after switching from a high cooldown equipment to a low cooldown equipment

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
