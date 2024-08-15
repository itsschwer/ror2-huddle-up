using HarmonyLib;

namespace LootTip.Patches
{
    [HarmonyPatch]
    internal static class ItemNameInTerminalContext
    {
        [HarmonyPostfix, HarmonyPatch(typeof(RoR2.PurchaseInteraction), nameof(RoR2.PurchaseInteraction.GetContextString))]
        private static void PurchaseInteraction_GetContextString(RoR2.PurchaseInteraction __instance, ref string __result)
        {
            if (__instance.displayNameToken == "MULTISHOP_TERMINAL_NAME") {
                RoR2.ShopTerminalBehavior terminal = __instance.GetComponent<RoR2.ShopTerminalBehavior>();
                // Item name display checks from: RoR2.UI.PingIndicator.RebuildPing()
                if (terminal != null && !terminal.pickupIndexIsHidden && terminal.pickupDisplay) {
                    string item = RoR2.Language.GetString(RoR2.PickupCatalog.GetPickupDef(terminal.CurrentPickupIndex()).nameToken);
                    if (!string.IsNullOrWhiteSpace(item)) {
                        System.Text.StringBuilder sb = new System.Text.StringBuilder(__result);
                        sb.Append("\n  <nobr><style=cStack>");
                        sb.Append(item);
                        sb.Append("</style></nobr>");
                        __result = sb.ToString();
                    }
                }
            }
        }
    }
}
