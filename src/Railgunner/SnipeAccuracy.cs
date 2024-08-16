using Snipe = EntityStates.Railgunner.Weapon.BaseFireSnipe;

namespace LootTip.Railgunner
{
    internal sealed class SnipeAccuracy
    {
        private readonly string labelColor = $"#{UnityEngine.ColorUtility.ToHtmlStringRGB(RoR2.DamageColor.FindColor(RoR2.DamageColorIndex.WeakPoint))}";

        private int shots;
        private int weakPointHits; // Can have multiple hits from a single shot

        private int consecutive;
        private int consecutiveBest;

        public void Hook()
        {
            Snipe.onFireSnipe += Snipe_onFireSnipe;
            Snipe.onWeakPointHit += Snipe_onWeakPointHit;
            Snipe.onWeakPointMissed += Snipe_onWeakPointMissed;

            RoR2.Stage.onServerStageBegin += OnStageStart;
        }

        public void Unhook()
        {
            Snipe.onFireSnipe -= Snipe_onFireSnipe;
            Snipe.onWeakPointHit -= Snipe_onWeakPointHit;
            Snipe.onWeakPointMissed -= Snipe_onWeakPointMissed;

            RoR2.Stage.onServerStageBegin -= OnStageStart;
        }

        private void Snipe_onFireSnipe(Snipe snipe)
        {
            shots++;
        }

        private void Snipe_onWeakPointHit(RoR2.DamageInfo _)
        {
            weakPointHits++;
            consecutive++;
        }

        private void Snipe_onWeakPointMissed()
        {
            // onWeakPointMissed is invoked in OnExit, which seems to be delayed (maybe animation timing?)
            if (consecutive > consecutiveBest)
                consecutiveBest = consecutive;
            consecutive = 0;
            // Note: can't extract regular hits/misses by checking .wasMiss on its own,
            //       since hitting terrain sets .wasMiss to false :/
        }

        private void OnStageStart(RoR2.Stage _)
        {
            shots = 0;
            weakPointHits = 0;
        }

        public override string ToString()
        {
            System.Text.StringBuilder sb = new();

            sb.Append($"<style=cStack>> </style><color={labelColor}>Weak Points Hit</color><style=cStack>: </style>");
            sb.AppendLine($"{weakPointHits}<style=cStack> : {shots}</style>");
            sb.Append($"<style=cStack>   > consecutive: </style>{consecutive}<style=cStack> ({consecutiveBest})</style>");

            return sb.ToString();
        }
    }
}
