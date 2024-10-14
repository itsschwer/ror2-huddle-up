using RoR2.UI;

namespace HUDdleUP.Railgunner
{
    internal sealed class RailgunnerAccuracyPanel : UnityEngine.MonoBehaviour
    {
        private static HUD hud;
        private static ReloadAccuracy reloadAccuracy;
        private static WeakPointAccuracy weakPointAccuracy;

        public static void Hook()
        {
            HUD.shouldHudDisplay += Init;
        }

        public static void Unhook()
        {
            HUD.shouldHudDisplay -= Init;

            if (reloadAccuracy != null) {
                reloadAccuracy.Unhook();
                reloadAccuracy = null;
            }
            if (weakPointAccuracy != null) {
                weakPointAccuracy.Unhook();
                weakPointAccuracy = null;
            }
        }

        public static void Init(HUD hud, ref bool _)
        {
            if (RailgunnerAccuracyPanel.hud != null) return;

            var objectivePanel = hud.GetComponentInChildren<ObjectivePanelController>();
            if (!objectivePanel) {
#if DEBUG
                Plugin.Logger.LogDebug($"Waiting to initialize {nameof(RailgunnerAccuracyPanel)}.");
#endif
                return;
            }

            RailgunnerAccuracyPanel.hud = hud;
            if (hud.localUserViewer.cachedBody.bodyIndex != RoR2.BodyCatalog.FindBodyIndex("RailgunnerBody")) {
                Plugin.Logger.LogDebug($"Local user is not Railgunner, skipping {nameof(RailgunnerAccuracyPanel)} initialization.");
                return;
            }

            if (reloadAccuracy == null) {
                reloadAccuracy = new ReloadAccuracy();
                reloadAccuracy.Hook();
            }
            if (weakPointAccuracy == null) {
                weakPointAccuracy = new WeakPointAccuracy();
                weakPointAccuracy.Hook();
            }

            HUDPanel panel = HUDPanel.ClonePanel(objectivePanel, nameof(RailgunnerAccuracyPanel));
            hud.gameObject.AddComponent<RailgunnerAccuracyPanel>().panel = panel;
            Plugin.Logger.LogDebug($"Initialized {nameof(RailgunnerAccuracyPanel)}.");
        }




        private HUDPanel panel;
        private HGTextMeshProUGUI display;

        private void Start()
        {
            panel.label.text = "Accuracy:";
            display = panel.AddTextComponent("Accuracy Tracker");
        }

        private void Update()
        {
            if (reloadAccuracy == null) return;

            System.Text.StringBuilder sb = new();
            sb.AppendLine(reloadAccuracy.ToString());
            sb.AppendLine();
            sb.AppendLine(weakPointAccuracy.ToString());
            display.text = sb.ToString();
        }
    }
}
