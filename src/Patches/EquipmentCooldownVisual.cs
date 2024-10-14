using HarmonyLib;
using RoR2.UI;
using UnityEngine;

namespace HUDdleUP.Patches
{
    [HarmonyPatch]
    internal static class EquipmentCooldownVisual
    {
        [HarmonyPostfix, HarmonyPatch(typeof(HUD), nameof(HUD.Awake))]
        private static void HUD_Awake(HUD __instance)
        {
            // Icon accessing logic based on RoR.UI.HUD.Update()
            if (__instance.skillIcons.Length <= 0 || !__instance.skillIcons[0]) return;

            GameObject toClone = __instance.skillIcons[0].cooldownRemapPanel.gameObject;
            foreach (EquipmentIcon icon in __instance.equipmentIcons) {
                Behaviours.CooldownPanel.Init(icon, toClone);
            }
        }
    }
}
