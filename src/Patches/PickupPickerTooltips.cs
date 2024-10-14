using HarmonyLib;
using RoR2;

namespace HUDdleUP.Patches
{
    [HarmonyPatch]
    internal static class PickupPickerTooltips
    {
        [HarmonyPostfix, HarmonyPatch(typeof(RoR2.UI.PickupPickerPanel), nameof(RoR2.UI.PickupPickerPanel.SetPickupOptions))]
        private static void PickupPickerPanel_SetPickupOptions(RoR2.UI.PickupPickerPanel __instance, PickupPickerController.Option[] options)
        {
            if (!Plugin.Config.CommandMenuItemTooltips) return;

            for (int i = 0; i < options.Length; i++) {
                RoR2.UI.TooltipProvider tooltip = TooltipHelper.AddTooltipProvider(
                    __instance.buttonAllocator.elements[i].GetComponent<UnityEngine.UI.Graphic>(),
                    true
                );

                PickupDef pickup = PickupCatalog.GetPickupDef(options[i].pickupIndex);
                ItemDef item = ItemCatalog.GetItemDef(pickup.itemIndex);
                EquipmentDef equipment = EquipmentCatalog.GetEquipmentDef(pickup.equipmentIndex);
                bool isItem = (item != null);

                tooltip.titleColor = pickup.darkColor;
                tooltip.titleToken = isItem ? item.nameToken : equipment.nameToken;
                tooltip.bodyToken = isItem ? item.descriptionToken : equipment.descriptionToken;
                if (isItem) tooltip.overrideBodyText = FullerDescriptions.GetCombinedDescription(tooltip.bodyToken, item.pickupToken);
                else tooltip.overrideBodyText = FullerDescriptions.GetCombinedDescription(tooltip.bodyToken, equipment.pickupToken) + "\n\n" + FullerDescriptions.GetEquipmentCooldown(equipment, null);
            }
        }
    }
}
