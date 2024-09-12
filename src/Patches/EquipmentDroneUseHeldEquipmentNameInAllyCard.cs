using HarmonyLib;

namespace HUDdleUP.Patches
{
    [HarmonyPatch]
    internal static class EquipmentDroneUseHeldEquipmentNameInAllyCard
    {
        [HarmonyPostfix, HarmonyPatch(typeof(RoR2.UI.AllyCardController), nameof(RoR2.UI.AllyCardController.LateUpdate))]
        private static void AllyCardController_LateUpdate(RoR2.UI.AllyCardController __instance)
        {
            if (__instance.nameLabel.text != RoR2.Language.GetString("EQUIPMENTDRONE_BODY_NAME")) return;
            if (!__instance.sourceMaster?.inventory) return;
            // Is it worth trying to use a tooltip instead? (considerations: swapping bodies, managing color, managing target graphic, ability to show equipment description)
            RoR2.EquipmentDef equipment = RoR2.EquipmentCatalog.GetEquipmentDef(__instance.sourceMaster.inventory.currentEquipmentIndex);
            if (equipment == null) return;
            __instance.nameLabel.text = RoR2.Language.GetString(equipment.nameToken);
        }
    }
}
