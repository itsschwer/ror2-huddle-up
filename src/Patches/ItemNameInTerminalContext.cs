using HarmonyLib;

namespace LootTip
{
    [HarmonyPatch]
    internal static class ItemNameInTerminalContext
    {
        [HarmonyPostfix, HarmonyPatch(typeof(RoR2.PurchaseInteraction), nameof(RoR2.PurchaseInteraction.GetContextString))]
        private static void PurchaseInteraction_GetContextString(RoR2.PurchaseInteraction __instance, ref string __result)
        {
            if (__instance.displayNameToken == "MULTISHOP_TERMINAL_NAME") {
                RoR2.ShopTerminalBehavior terminal = __instance.GetComponent<RoR2.ShopTerminalBehavior>();
                if (terminal != null) {
                    // Item display name logic from: RoR2.UI.PingIndicator.RebuildPing()
                    string item = (terminal.pickupIndexIsHidden || !terminal.pickupDisplay) ? "?" : RoR2.Language.GetString(RoR2.PickupCatalog.GetPickupDef(terminal.CurrentPickupIndex()).nameToken ?? RoR2.PickupCatalog.invalidPickupToken);

                    System.Text.StringBuilder sb = new System.Text.StringBuilder(__result);
                    sb.Append(" <nobr><style=cStack><i>");
                    sb.Append(item);
                    sb.Append("</i></style></nobr>");
                    __result = sb.ToString();
                }
            }
        }
    }
}
