﻿using HarmonyLib;

namespace HUDdleUP.Patches
{
    [HarmonyPatch]
    internal static class RunDifficultyTooltip
    {
        [HarmonyPostfix, HarmonyPatch(typeof(RoR2.UI.CurrentDifficultyIconController), nameof(RoR2.UI.CurrentDifficultyIconController.Start))]
        private static void CurrentDifficultyIconController_Start(RoR2.UI.CurrentDifficultyIconController __instance)
        {
            if (!Plugin.Config.RunDifficultyTooltip) return;

            if (RoR2.Run.instance != null) {
                RoR2.UI.TooltipProvider tooltip = TooltipHelper.AddTooltipProvider(__instance.GetComponent<UnityEngine.UI.Graphic>());
                RoR2.DifficultyDef difficultyDef = RoR2.DifficultyCatalog.GetDifficultyDef(RoR2.Run.instance.selectedDifficulty);
                tooltip.titleColor = difficultyDef.color;
                tooltip.titleToken = difficultyDef.nameToken;
                tooltip.bodyToken = difficultyDef.descriptionToken;
            }
        }
    }
}
