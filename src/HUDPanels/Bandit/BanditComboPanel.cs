using RoR2.UI;

namespace HUDdleUP.Bandit
{
    internal sealed class BanditComboPanel : UnityEngine.MonoBehaviour
    {
        private static HUD hud;
        private static ConsecutiveReset tracker;

        public static void Hook()
        {
            HUD.shouldHudDisplay += Init;
        }

        public static void Unhook()
        {
            HUD.shouldHudDisplay -= Init;

            if (tracker != null) {
                tracker.Unhook();
                tracker = null;
            }
        }

        private static void Init(HUD hud, ref bool _)
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

            if (tracker == null) {
                tracker = new ConsecutiveReset(hud.localUserViewer);
                tracker.Hook();
            }

            HUDPanel panel = HUDPanel.ClonePanel(objectivePanel, nameof(BanditComboPanel));
            hud.gameObject.AddComponent<BanditComboPanel>().panel = panel;
            Plugin.Logger.LogDebug($"Initialized {nameof(BanditComboPanel)}.");
        }




        private HUDPanel panel;
        private TMPro.TextMeshProUGUI display;

        private void Start()
        {
            panel.label.text = "Combo:";
            display = panel.AddTextComponent("Combo Tracker");
        }

        private void Update()
        {
            if (tracker == null) return;

             display.text = tracker.ToString();
        }

        private void FixedUpdate()
        {
            if (tracker != null) tracker.FixedUpdate();
        }
    }
}
