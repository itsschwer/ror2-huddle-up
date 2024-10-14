using HarmonyLib;
using RoR2;

namespace HUDdleUP.Patches
{
    [HarmonyPatch]
    internal static class ScoreboardShowChat
    {
        [HarmonyPostfix, HarmonyPatch(typeof(RoR2.UI.ChatBox), nameof(RoR2.UI.ChatBox.UpdateFade))]
        private static void ChatBox_UpdateFade(RoR2.UI.ChatBox __instance)
        {
            if (!Plugin.Config.ScoreboardShowChat) return;

            if (__instance.fadeGroup != null) {
                Rewired.Player inputPlayer = LocalUserManager.GetFirstLocalUser()?.inputPlayer;
                // Scoreboard visibility logic from RoR2.UI.HUD.Update()
                if (inputPlayer != null && inputPlayer.GetButton("info")) {
                    __instance.fadeGroup.alpha = 1;
                }
            }
        }
    }
}
