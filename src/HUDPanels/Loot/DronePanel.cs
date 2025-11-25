using RoR2;
using RoR2.UI;

namespace HUDdleUP.Loot
{
    internal sealed class DronePanel : UnityEngine.MonoBehaviour
    {
        private static HUD hud;

        public static void Hook() => HUD.shouldHudDisplay += Init;
        public static void Unhook() => HUD.shouldHudDisplay -= Init;

        private static void Init(HUD hud, ref bool _)
        {
            // if (!Plugin.Config.DronePanel) return;
            if (DronePanel.hud != null) return;

            var objectivePanel = hud.GetComponentInChildren<ObjectivePanelController>();
            if (!objectivePanel) {
#if DEBUG
                Plugin.Logger.LogDebug($"Waiting to initialize {nameof(DronePanel)}.");
#endif
                return;
            }

            DronePanel.hud = hud;

            HUDPanel panel = HUDPanel.ClonePanel(objectivePanel, nameof(DronePanel));
            hud.gameObject.AddComponent<DronePanel>().panel = panel;
            Plugin.Logger.LogDebug($"Initialized {nameof(DronePanel)}.");
        }




        private HUDPanel panel;
        private TMPro.TextMeshProUGUI display;
        private Interactables interactables;
        
        private void Start()
        {
            panel.label.text = "Drones:";
            display = panel.AddTextComponent("Drone Tracker");
        }

        private void Update()
        {
            bool visible = hud.scoreboardPanel.activeSelf;
            panel.gameObject.SetActive(visible);
            if (!visible) return;

            // interactables

            display.text = GenerateText();
        }

        public string GenerateText()
        {
            System.Text.StringBuilder sb = new();

            return sb.ToString();
        }
    }
}
