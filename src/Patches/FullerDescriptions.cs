using HarmonyLib;
using RoR2;
using System.Text;

namespace LootTip.Patches
{
    [HarmonyPatch]
    internal static class FullerDescriptions
    {
        [HarmonyPostfix, HarmonyPatch(typeof(RoR2.UI.ItemIcon), nameof(RoR2.UI.ItemIcon.SetItemIndex))]
        private static void ItemIcon_SetItemIndex(RoR2.UI.ItemIcon __instance)
        {

            ItemDef item = ItemCatalog.GetItemDef(__instance.itemIndex);
            if (item == null || __instance.tooltipProvider == null) return;

            __instance.tooltipProvider.overrideBodyText = GetCombinedDescription(item.descriptionToken, item.pickupToken);
        }

        [HarmonyPostfix, HarmonyPatch(typeof(RoR2.UI.EquipmentIcon), nameof(RoR2.UI.EquipmentIcon.SetDisplayData))]
        private static void EquipmentIcon_SetDisplayData(RoR2.UI.EquipmentIcon __instance)
        {
            EquipmentDef item = __instance.currentDisplayData.equipmentDef;
            if (item == null || __instance.tooltipProvider == null) return;

            __instance.tooltipProvider.overrideBodyText = GetCombinedDescription(item.descriptionToken, item.pickupToken);
        }

        internal static string GetCombinedDescription(string descriptionToken, string pickupToken)
        {
            StringBuilder sb = new();
            string longDescription = Language.GetString(descriptionToken);
            string shortDescription = Language.GetString(pickupToken);
            bool hasDescription = (longDescription != descriptionToken) && (longDescription != shortDescription);

            if (hasDescription) {
                sb.AppendLine($"<style=cStack>{shortDescription}</style>");
                sb.AppendLine();
                sb.Append(longDescription);
            }
            else {
                sb.Append(shortDescription);
            }

            return sb.ToString();
        }
    }
}
