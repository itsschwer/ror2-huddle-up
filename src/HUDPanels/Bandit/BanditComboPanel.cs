using RoR2.UI;

namespace HUDdleUP.Bandit
{
    internal sealed class BanditComboPanel : UnityEngine.MonoBehaviour
    {
        private static HUD hud;

        public static void Hook()
        {
            HUD.shouldHudDisplay += Init;
        }

        public static void Unhook()
        {
            HUD.shouldHudDisplay -= Init;
        }

        public static void Init(HUD hud, ref bool _)
        {
            if (!Plugin.Config.BanditComboPanel) return;
            if (BanditComboPanel.hud != null) return;

            var objectivePanel = hud.GetComponentInChildren<ObjectivePanelController>();
            if (!objectivePanel || hud.localUserViewer.cachedBody == null) {
#if DEBUG
                Plugin.Logger.LogDebug($"Waiting to initialize {nameof(BanditComboPanel)}.");
#endif
                return;
            }

            BanditComboPanel.hud = hud;
            if (hud.localUserViewer.cachedBody.bodyIndex != RoR2.BodyCatalog.FindBodyIndex("Bandit2Body")) {
                Plugin.Logger.LogDebug($"Local user is not Bandit, skipping {nameof(BanditComboPanel)} initialization.");
                return;
            }

            HUDPanel panel = HUDPanel.ClonePanel(objectivePanel, nameof(BanditComboPanel));
            hud.gameObject.AddComponent<BanditComboPanel>().panel = panel;
            Plugin.Logger.LogDebug($"Initialized {nameof(BanditComboPanel)}.");
        }




        private HUDPanel panel;
        private TMPro.TextMeshProUGUI display;
        private ConsecutiveReset tracker = new ConsecutiveReset(hud.localUserViewer.cachedBody);

        private void Start()
        {
            panel.label.text = "Combo";
            display = panel.AddTextComponent("Combo Tracker");
        }

        private void Update()
        {
             display.text = tracker.ToString();
        }
    }
}
