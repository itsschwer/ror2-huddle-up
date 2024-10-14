using HarmonyLib;
using RoR2;
using System.Text;

namespace HUDdleUP.Patches
{
    [HarmonyPatch]
    internal static class FullerDescriptions
    {
        [HarmonyPostfix, HarmonyPatch(typeof(RoR2.UI.ItemIcon), nameof(RoR2.UI.ItemIcon.SetItemIndex))]
        private static void ItemIcon_SetItemIndex(RoR2.UI.ItemIcon __instance)
        {
            if (!Plugin.Config.FullerItemDescriptions) return;

            ItemDef item = ItemCatalog.GetItemDef(__instance.itemIndex);
            if (item == null || __instance.tooltipProvider == null) return;

            __instance.tooltipProvider.overrideBodyText = GetCombinedDescription(item.descriptionToken, item.pickupToken);
        }

        [HarmonyPostfix, HarmonyPatch(typeof(RoR2.UI.EquipmentIcon), nameof(RoR2.UI.EquipmentIcon.SetDisplayData))]
        private static void EquipmentIcon_SetDisplayData(RoR2.UI.EquipmentIcon __instance)
        {
            if (!Plugin.Config.FullerEquipmentDescriptions) return;

            EquipmentDef item = __instance.currentDisplayData.equipmentDef;
            if (item == null || __instance.tooltipProvider == null) return;

            StringBuilder sb = new(GetCombinedDescription(item.descriptionToken, item.pickupToken));
            sb.AppendLine().AppendLine().Append(GetEquipmentCooldown(item, __instance.targetInventory));

            __instance.tooltipProvider.overrideBodyText = sb.ToString();
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

        internal static string GetEquipmentCooldown(EquipmentDef equipment, Inventory inventory)
        {
            if (inventory == null) return $"Cooldown: <style=cIsDamage>{(equipment.cooldown):0.###}s</style>";
            StringBuilder sb = new();

            float scaleFactor = inventory.CalculateEquipmentCooldownScale();
            sb.Append($"Cooldown: <style=cIsDamage>{(equipment.cooldown * scaleFactor):0.###}s</style>");
            if (scaleFactor < 1) sb.Append($" <style=cStack>({equipment.cooldown}×<style=cIsUtility>{scaleFactor:0.###%}</style>)</style>");

            return sb.ToString();
        }
    }
}
