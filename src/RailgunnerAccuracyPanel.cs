using RoR2.UI;

using Snipe = On.EntityStates.Railgunner.Weapon.BaseFireSnipe;
using Reload = On.EntityStates.Railgunner.Reload.Reloading;

namespace LootTip
{
    internal sealed class RailgunnerAccuracyPanel : UnityEngine.MonoBehaviour
    {
        private static HUD hud;
        private static RailgunnerSnipeAccuracy snipeAccuracy;
        private static RailgunnerReloadAccuracy reloadAccuracy;

        public static void Hook()
        {
            HUD.shouldHudDisplay += Init;

            snipeAccuracy = new RailgunnerSnipeAccuracy();
            reloadAccuracy = new RailgunnerReloadAccuracy();
            Snipe.OnExit += snipeAccuracy.RecordSnipe;
            Reload.AttemptBoost += reloadAccuracy.RecordReload;

            RoR2.Stage.onStageStartGlobal += reloadAccuracy.StageClear;
        }

        public static void Unhook()
        {
            HUD.shouldHudDisplay -= Init;

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
            System.Text.StringBuilder sb = new();
            sb.AppendLine(snipeAccuracy.ToString());
            sb.AppendLine(reloadAccuracy.ToString());
            display.text = sb.ToString();
        }




        private class RailgunnerSnipeAccuracy
        {
            private const string WeakPointDamageColor = "#EF8434";

            private bool hitWeakPoint;
            // todo: convert into tracking streaks and both regular and critical hits
            internal void RecordSnipe(Snipe.orig_OnExit orig, EntityStates.Railgunner.Weapon.BaseFireSnipe self)
            {
                hitWeakPoint = (!self.wasMiss && self.wasAtLeastOneWeakpoint);
                orig(self);
            }

            public override string ToString()
                => $"<color={WeakPointDamageColor}>Hit Weak Point?</color>: {(hitWeakPoint ? "@" : "×")}";
        }

        private class RailgunnerReloadAccuracy
        {
            private const string SniperDamageColor = "#FF888B";

            private int totalReloads;
            private int perfectReloads;
            private int totalReloadsStage;
            private int perfectReloadsStage;

            private int consecutive;
            private int consecutiveBest;

            internal bool RecordReload(Reload.orig_AttemptBoost orig, EntityStates.Railgunner.Reload.Reloading self)
            {
                bool successful = orig(self);
                if (successful) {
                    perfectReloads++;
                    perfectReloadsStage++;
                    consecutive++;
                }
                else {
                    if (consecutive > consecutiveBest)
                        consecutiveBest = consecutive;
                    consecutive = 0;
                }
                totalReloads++;
                totalReloadsStage++;
                return successful;
            }

            internal void StageClear(RoR2.Stage _)
            {
                totalReloadsStage = 0;
                perfectReloadsStage = 0;
            }

            // color derived from DamageColorIndex.Weakpoint
            public override string ToString()
                => $"<color={SniperDamageColor}>Perfect Reloads</color>: {Format(perfectReloadsStage, totalReloadsStage)} [{consecutive}] <align=\"right\">{Format(perfectReloads, totalReloads)} [{consecutiveBest}]</align>";
            private string Format(int perfect, int total)
            {
                if (total == 0) return "-/- (-)";
                return $"{perfect} / {total} ({((float)perfect/total):0.0%})";
            }
        }
    }
}
