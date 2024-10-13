using HarmonyLib;

namespace HUDdleUP.Patches
{
    [HarmonyPatch]
    internal static class EquipmentDroneUseHeldEquipmentNameInAllyCard
    {
        [HarmonyPostfix, HarmonyPatch(typeof(RoR2.UI.AllyCardController), nameof(RoR2.UI.AllyCardController.LateUpdate))]
        private static void AllyCardController_LateUpdate(RoR2.UI.AllyCardController __instance)
        {
            if (!__instance.sourceMaster || !__instance.sourceMaster.inventory) return;            // No inventory
            RoR2.EquipmentDef equipment = RoR2.EquipmentCatalog.GetEquipmentDef(__instance.sourceMaster.inventory.currentEquipmentIndex);
            if (equipment == null) return;                                                         // No equipment
            string equipmentName = RoR2.Language.GetString(equipment.nameToken);
            if (__instance.nameLabel.text == equipmentName) return;                                // Name already matches equipment
            RoR2.CharacterBody body = __instance.sourceMaster.GetBody();
            if (body.baseNameToken != "EQUIPMENTDRONE_BODY_NAME") return;                          // Not an equipment drone

            __instance.nameLabel.text = RoR2.Language.GetString(equipment.nameToken);
        }
    }
}
