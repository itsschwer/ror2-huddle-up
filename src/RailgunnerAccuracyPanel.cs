using RoR2.UI;

using Reload = On.EntityStates.Railgunner.Reload.Reloading;

namespace LootTip
{
    internal sealed class RailgunnerAccuracyPanel : UnityEngine.MonoBehaviour
    {
        private static HUD hud;
        private static Railgunner.ReloadAccuracy reloadAccuracy;

        public static void Hook()
        {
            HUD.shouldHudDisplay += Init;

            reloadAccuracy = new Railgunner.ReloadAccuracy();
            Reload.AttemptBoost += reloadAccuracy.RecordReload;

            RoR2.Stage.onStageStartGlobal += reloadAccuracy.OnStageStart;
        }

        public static void Unhook()
        {
            HUD.shouldHudDisplay -= Init;

            RoR2.Stage.onStageStartGlobal -= reloadAccuracy.OnStageStart;

            Reload.AttemptBoost -= reloadAccuracy.RecordReload;
            reloadAccuracy = null;
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
            display.text = reloadAccuracy.ToString();

            /* RailgunnerSnipeAccuracy [to[re]do]
             * - aim to track total shots, total hits, and weak point hits (consecutive + percentage)
             * - check RoR2.Achievements.Railgunner.RailgunnerConsecutiveWeakPointsAchievement
             * - check EntityStates.Railgunner.Weapon.BaseFireSnipe (OnExit, ModifyBullet)
             *    - OnExit call seems to be delayed (maybe animation timings?)
             */
        }
    }
}
