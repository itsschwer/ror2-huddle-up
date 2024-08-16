using Snipe = EntityStates.Railgunner.Weapon.BaseFireSnipe;

namespace LootTip.Railgunner
{
    internal sealed class SnipeAccuracy
    {
        private Snipe currentShot;

        private int shots;
        private int misses;
        // private int missedWeakPointShots;
        private int weakPointHits; // Can have multiple hits from a single shot

        private int consecutive;
        private int consecutiveBest;

        public void Hook()
        {
            Snipe.onFireSnipe += Snipe_onFireSnipe;
            Snipe.onWeakPointHit += Snipe_onWeakPointHit;
            Snipe.onWeakPointMissed += Snipe_onWeakPointMissed;
        }

        public void Unhook()
        {
            Snipe.onFireSnipe -= Snipe_onFireSnipe;
            Snipe.onWeakPointHit -= Snipe_onWeakPointHit;
            Snipe.onWeakPointMissed -= Snipe_onWeakPointMissed;
        }

        private void Snipe_onFireSnipe(Snipe snipe)
        {
            shots++;
            currentShot = snipe;
        }

        private void Snipe_onWeakPointHit(RoR2.DamageInfo _)
        {
            weakPointHits++;
            consecutive++;
        }

        private void Snipe_onWeakPointMissed()
        {
            // missedWeakPointShots++;

            if (consecutive > consecutiveBest)
                consecutiveBest = consecutive;
            consecutive = 0;

            if (currentShot == null) { Log.Warning("Missed a Weak Point without firing a shot??"); return; }
            if (currentShot.wasMiss) {
                misses++;
            }
            currentShot = null;
        }

        private string styleAlt = "cWorldEvent";
        private string style = "cStack";
        public override string ToString()
        {
            System.Text.StringBuilder sb = new();
            sb.Append($"<style={style}>");
            sb.AppendLine($"> shots: {shots}");
            sb.AppendLine($"> crits: {weakPointHits}");
            sb.AppendLine($"> norms: {shots - misses - weakPointHits}");
            sb.AppendLine($"> misses: {misses}");
            sb.Append("</style>");
            // (styleAlt, style) = (style, styleAlt);
            return sb.ToString();
        }

        /* RailgunnerSnipeAccuracy [to[re]do]
         * - aim to track total shots, total hits, and weak point hits (consecutive + percentage)
         * - check RoR2.Achievements.Railgunner.RailgunnerConsecutiveWeakPointsAchievement
         * - check EntityStates.Railgunner.Weapon.BaseFireSnipe (OnExit, ModifyBullet)
         *    - OnExit call seems to be delayed (maybe animation timings?)
         */
    }
}
