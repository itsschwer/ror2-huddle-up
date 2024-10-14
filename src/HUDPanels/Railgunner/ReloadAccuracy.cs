using MonoMod.RuntimeDetour;
using System;
using System.Reflection;
using RailgunnerReloading = EntityStates.Railgunner.Reload.Reloading;

namespace HUDdleUP.Railgunner
{
    internal sealed class ReloadAccuracy
    {
        private static readonly MethodInfo AttemptBoost = typeof(RailgunnerReloading).GetMethod(nameof(RailgunnerReloading.AttemptBoost), BindingFlags.Instance | BindingFlags.Public);
        private readonly string labelColor = $"#{UnityEngine.ColorUtility.ToHtmlStringRGB(RoR2.DamageColor.FindColor(RoR2.DamageColorIndex.Sniper))}"; // #FF888B

        private int totalReloads;
        private int perfectReloads;
        private int totalReloadsStage;
        private int perfectReloadsStage;

        private int consecutive;
        private int consecutiveBest;


        private Hook OnReloadAttemptBoost;

        public void Hook()
        {
            MethodInfo hook = typeof(ReloadAccuracy).GetMethod(nameof(ReloadAccuracy.RecordReload), BindingFlags.Instance | BindingFlags.NonPublic);
            OnReloadAttemptBoost = new Hook(AttemptBoost, hook, this);
            RoR2.Stage.onStageStartGlobal += OnStageStart;
        }

        public void Unhook()
        {
            OnReloadAttemptBoost.Undo();
            RoR2.Stage.onStageStartGlobal -= OnStageStart;
        }

        private bool RecordReload(Func<RailgunnerReloading, bool> orig, RailgunnerReloading self)
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

        private void OnStageStart(RoR2.Stage _)
        {
            totalReloadsStage = 0;
            perfectReloadsStage = 0;
        }

        public override string ToString()
        {
            System.Text.StringBuilder sb = new();

            sb.Append($"<style=cStack>> </style><color={labelColor}>Perfect Reloads</color><style=cStack>: </style>");
            if (totalReloads == 0) sb.Append("<style=cStack>-.--%</style>");
            else sb.Append($"{((float)perfectReloads / totalReloads):0.00%}");
            sb.AppendLine();

            sb.Append($"<style=cStack>   > this stage: </style>");
            if (totalReloadsStage == 0) sb.Append("<style=cStack>-.--%</style>");
            else sb.Append($"{((float)perfectReloadsStage / totalReloadsStage):0.00%}");
            sb.AppendLine($"<style=cStack> ({perfectReloadsStage}/{totalReloadsStage})</style>");

            sb.Append($"<style=cStack>   > consecutive: </style>{consecutive}<style=cStack> ({consecutiveBest})</style>");

            return sb.ToString();
        }
    }
}
