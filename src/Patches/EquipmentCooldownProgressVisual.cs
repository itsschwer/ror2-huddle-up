using HarmonyLib;
using RoR2.UI;
using UnityEngine;

namespace HUDdleUP.Patches
{
    [HarmonyPatch]
    internal static class EquipmentCooldownProgressVisual
    {
        [HarmonyPostfix, HarmonyPatch(typeof(HUD), nameof(HUD.Awake))]
        private static void HUD_Awake(HUD __instance)
        {
            if (!Plugin.Config.EquipmentIconCooldownVisual) return;

            if (__instance.skillIcons.Length <= 0 || !__instance.skillIcons[0]) return; // Icon accessing logic based on RoR.UI.HUD.Update()
            if (!__instance.skillIcons[0].cooldownRemapPanel || !__instance.skillIcons[0].cooldownRemapPanel.gameObject) {
                Plugin.Logger.LogWarning($"{nameof(EquipmentCooldownProgressVisual)}> Could not initialize — missing {nameof(SkillIcon.cooldownRemapPanel)}. This warning can safely be ignored if RiskUI is installed.");
                return;
            }

            GameObject toClone = __instance.skillIcons[0].cooldownRemapPanel.gameObject;
            foreach (EquipmentIcon icon in __instance.equipmentIcons) {
                Behaviours.EquipmentCooldownPanel.Init(icon, toClone);
            }
        }
    }
}
