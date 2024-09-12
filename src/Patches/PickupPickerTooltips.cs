using HarmonyLib;
using RoR2;

namespace HUDdleUP.Patches
{
    [HarmonyPatch]
    internal static class PickupPickerTooltips
    {
        [HarmonyPostfix, HarmonyPatch(typeof(RoR2.UI.PickupPickerPanel), nameof(RoR2.UI.PickupPickerPanel.SetPickupOptions))]
        private static void PickupPickerPanel_SetPickupOptions(RoR2.UI.PickupPickerPanel __instance, RoR2.PickupPickerController.Option[] options)
        {
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
                tooltip.overrideBodyText = FullerDescriptions.GetCombinedDescription(tooltip.bodyToken, (isItem ? item.pickupToken : equipment.pickupToken));
            }
        }
    }
}
