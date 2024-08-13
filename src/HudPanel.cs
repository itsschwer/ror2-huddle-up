using RoR2.UI;
using UnityEngine;

namespace LootTip
{
    public sealed class HUDPanel
    {
        public readonly GameObject gameObject;
        public readonly HGTextMeshProUGUI label;

        internal HUDPanel(GameObject panel, HGTextMeshProUGUI label)
        {
            this.gameObject = panel;
            this.label = label;
        }

        public HGTextMeshProUGUI AddTextComponent(string name)
        {
            HGTextMeshProUGUI text = Object.Instantiate(label, label.transform.parent);
            var unwanted = text.GetComponent<UnityEngine.UI.LayoutElement>();
            if (unwanted) Object.Destroy(unwanted);
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




        public static HUDPanel ClonePanel(ObjectivePanelController objectivePanel, string name)
        {
            var clone = Object.Instantiate(objectivePanel, objectivePanel.transform.parent);
            var panel = DeleteObjectiveComponents(clone);

            var label = panel.GetComponentInChildren<HGTextMeshProUGUI>();
            var unwanted = label.GetComponent<LanguageTextMeshController>();
            if (unwanted) Object.DestroyImmediate(unwanted); // DestroyImmediate in case calling AddTextComponent in same frame

            panel.name = name;
            return new HUDPanel(panel, label);
        }

        private static GameObject DeleteObjectiveComponents(ObjectivePanelController objectivePanel)
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
