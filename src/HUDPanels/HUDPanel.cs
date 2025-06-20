﻿using RoR2.UI;
using TMPro;
using UnityEngine;

namespace HUDdleUP
{
    /// <summary>
    /// 
    /// <code>
    /// HUDSimple(Clone)
    ///  │ HUD
    ///  └─ MainContainer/MainUIArea/SpringCanvas/UpperRightCluster/ClassicRunInfoHudPanel(Clone)/RightInfoBar
    ///      └─ this [game object]
    ///          └─ StripContainer
    ///              │ Image
    ///              │ RoR2.UI.SkinControllers.PanelSkinController
    ///              │ VerticalLayoutGroup
    ///              │ LayoutElement
    ///              │ RoR2.UI.Juice
    ///              └─ text component [game object]
    ///                  │ RoR2.UI.HGTextMeshProUGUI
    ///                  │ RoR2.UI.SkinControllers.LabelSkinController
    /// </code>
    /// </summary>
    public sealed class HUDPanel
    {
        public readonly GameObject gameObject;
        public readonly TextMeshProUGUI label;
        public readonly Transform stripContainer;

        private HUDPanel(GameObject panel, TextMeshProUGUI label, Transform stripContainer)
        {
            this.gameObject = panel;
            this.label = label;
            this.stripContainer = stripContainer;
        }

        public TextMeshProUGUI AddTextComponent(string name)
        {
            TextMeshProUGUI text = Object.Instantiate(label, stripContainer.transform);
            text.name = name;

            if (text.TryGetComponent<UnityEngine.UI.LayoutElement>(out var layout))
                Object.Destroy(layout);
            if (text.TryGetComponent<RoR2.UI.SkinControllers.LabelSkinController>(out var skin)) // Using TryGetComponent ∵ LabelSkinController is removed in RiskUI
                skin.labelType = RoR2.UI.SkinControllers.LabelSkinController.LabelType.Default; // Based on in-game objectives text component

            // Based on in-game objectives text component
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
            GameObject panel = clone.gameObject;
            panel.name = name;
            Object.DestroyImmediate(clone); // DestroyImmediate in case a panel becomes ordered above the real objective panel in the hierarchy (which is accessed via GetComponentInChildren as a template for HUDdleUP panels)

            // Destroy unwanted children (e.g. existing objectives)
            Transform stripContainer = panel.transform.GetChild(panel.transform.childCount - 1); // or .transform.Find("StripContainer")
            for (int i = stripContainer.transform.childCount - 1; i >= 0; i--)
                Object.Destroy(stripContainer.GetChild(i).gameObject);

            if (panel.TryGetComponent<HudObjectiveTargetSetter>(out var hudObjectiveTargetSetter))
                Object.Destroy(hudObjectiveTargetSetter);

            TextMeshProUGUI label = panel.GetComponentInChildren<HGTextMeshProUGUI>() ?? panel.GetComponentInChildren<TextMeshProUGUI>();
            if (label == null)
                Plugin.Logger.LogWarning($"New HUD panel \"{panel.name}\" is missing a label!");
            else if (label.TryGetComponent<LanguageTextMeshController>(out var localiser))
                Object.DestroyImmediate(localiser); // DestroyImmediate in case calling AddTextComponent in same frame

            return new HUDPanel(panel, label, stripContainer);
        }
    }
}
