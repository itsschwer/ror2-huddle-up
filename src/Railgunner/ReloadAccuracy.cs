using Reload = On.EntityStates.Railgunner.Reload.Reloading;

namespace LootTip.Railgunner
{
    internal sealed class ReloadAccuracy
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

        internal void OnStageStart(RoR2.Stage _)
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
            return $"{perfect} / {total} ({((float)perfect / total):0.0%})";
        }
    }
}
