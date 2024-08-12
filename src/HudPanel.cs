using RoR2.UI;
using UnityEngine;

namespace LootTip
{
    public sealed class HudPanel
    {
        public readonly GameObject gameObject;
        public readonly HGTextMeshProUGUI label;

        internal HudPanel(GameObject panel, HGTextMeshProUGUI label)
        {
            this.gameObject = panel;
            this.label = label;
        }

        public HGTextMeshProUGUI AddTextComponent(string name)
        {
            HGTextMeshProUGUI text = Object.Instantiate(label, label.transform.parent);
            text.name = name;
            // Based on in-game objectives text component
            text.GetComponent<RoR2.UI.SkinControllers.LabelSkinController>().labelType = RoR2.UI.SkinControllers.LabelSkinController.LabelType.Default;
            text.fontSize = 12;
            text.fontSizeMax = 12;
            text.fontSizeMin = 6;
            // Own defaults
            text.alignment = TMPro.TextAlignmentOptions.TopLeft;
            text.text = "hello world.";
            return text;
        }
    }

    public static class HudPanelUtil
    {
        public static HudPanel ClonePanel(this ObjectivePanelController objectivePanel, string name)
        {
            var clone = Object.Instantiate(objectivePanel, objectivePanel.transform.parent);
            var panel = DeleteObjectiveComponents(clone);

            var label = panel.GetComponentInChildren<HGTextMeshProUGUI>();
            var component = label.GetComponent<LanguageTextMeshController>();
            if (component) Object.Destroy(component);

            panel.name = name;
            return new HudPanel(panel, label);
        }

        private static GameObject DeleteObjectiveComponents(this ObjectivePanelController objectivePanel)
        {
            GameObject stripContainer = objectivePanel.transform.GetChild(objectivePanel.transform.childCount - 1).gameObject;
            if (stripContainer) Object.Destroy(stripContainer);
            var component = objectivePanel.GetComponent<HudObjectiveTargetSetter>();
            if (component) Object.Destroy(component);

            GameObject panel = objectivePanel.gameObject;
            Object.Destroy(objectivePanel);
            return panel;
        }
    }
}
