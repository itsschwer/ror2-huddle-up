using RoR2.UI;

using Reload = On.EntityStates.Railgunner.Reload.Reloading;

namespace LootTip
{
    internal sealed class RailgunnerAccuracyPanel : UnityEngine.MonoBehaviour
    {
        private static HUD hud;
        private static Railgunner.ReloadAccuracy reloadAccuracy;
        private static Railgunner.SnipeAccuracy snipeAccuracy;

        public static void Hook()
        {
            HUD.shouldHudDisplay += Init;

            reloadAccuracy = new Railgunner.ReloadAccuracy();
            reloadAccuracy.Hook();
            snipeAccuracy = new Railgunner.SnipeAccuracy();
            snipeAccuracy.Hook();
        }

        public static void Unhook()
        {
            HUD.shouldHudDisplay -= Init;

            reloadAccuracy.Unhook();
            reloadAccuracy = null;
            snipeAccuracy.Unhook();
            snipeAccuracy = null;
        }

        public static void Init(HUD hud, ref bool _)
        {
            if (RailgunnerAccuracyPanel.hud != null) return;

            var objectivePanel = hud.GetComponentInChildren<ObjectivePanelController>();
            if (!objectivePanel) {
                Log.Debug($"Waiting to initialize {nameof(RailgunnerAccuracyPanel)}.");
                return;
            }

            HUDPanel panel = HUDPanel.ClonePanel(objectivePanel, nameof(RailgunnerAccuracyPanel));
            hud.gameObject.AddComponent<RailgunnerAccuracyPanel>().panel = panel;
            RailgunnerAccuracyPanel.hud = hud;
            Log.Debug($"Initialized {nameof(RailgunnerAccuracyPanel)}.");
        }




        private HUDPanel panel;
        private HGTextMeshProUGUI display;
        // private readonly BodyIndex bodyRequirement = BodyCatalog.FindBodyIndex("RailgunnerBody");

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
            sb.AppendLine(snipeAccuracy.ToString());
            display.text = sb.ToString();
        }
    }
}
