using RoR2.UI;
using TMPro;
using UnityEngine;

namespace HUDdleUP
{
    public sealed class HUDPanel
    {
        public readonly GameObject gameObject;
        public readonly TextMeshProUGUI label;
        private readonly GameObject stripContainer;

        private HUDPanel(GameObject panel, TextMeshProUGUI label, GameObject stripContainer)
        {
            this.gameObject = panel;
            this.label = label;
            this.stripContainer = stripContainer;
        }

        public TextMeshProUGUI AddTextComponent(string name)
        {
            TextMeshProUGUI text = Object.Instantiate(label, stripContainer.transform);
            text.name = name;
            var layout = text.GetComponent<UnityEngine.UI.LayoutElement>();
            if (layout) Object.Destroy(layout);
            // Based on in-game objectives text component
            if (text.TryGetComponent<RoR2.UI.SkinControllers.LabelSkinController>(out var skin)) {
                // Using TryGetComponent ∵ LabelSkinController is removed in RiskUI
                skin.labelType = RoR2.UI.SkinControllers.LabelSkinController.LabelType.Default;
            }
            text.fontSize = 12;
            text.fontSizeMax = 12;
            text.fontSizeMin = 6;
            // Own defaults
            text.alignment = TextAlignmentOptions.TopLeft;
            text.text = "hello world.";
            return text;
        }




        public static HUDPanel ClonePanel(ObjectivePanelController objectivePanel, string name)
        {
            ObjectivePanelController clone = Object.Instantiate(objectivePanel, objectivePanel.transform.parent);
            GameObject stripContainer = clone.transform.GetChild(clone.transform.childCount - 1).gameObject;
            GameObject panel = clone.gameObject;
            panel.name = name;
            Object.Destroy(clone);

            TextMeshProUGUI label = panel.GetComponentInChildren<HGTextMeshProUGUI>() ?? panel.GetComponentInChildren<TextMeshProUGUI>();

            if (panel.TryGetComponent<HudObjectiveTargetSetter>(out var hudObjectiveTargetSetter))
                Object.Destroy(hudObjectiveTargetSetter);
            if (label.TryGetComponent<LanguageTextMeshController>(out var translator))
                Object.DestroyImmediate(translator); // DestroyImmediate in case calling AddTextComponent in same frame

            return new HUDPanel(panel, label, stripContainer);
        }
    }
}
