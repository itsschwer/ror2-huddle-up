using RoR2;
using RoR2.Skills;

namespace HUDdleUP.Bandit
{
    internal sealed class ConsecutiveReset
    {
        private const string banditSkullColour = "#40C5E4";

        private static readonly SkillDef requiredSkillDef = SkillCatalog.GetSkillDef(SkillCatalog.FindSkillIndexByName("Bandit2.ResetRevolver"));

        private readonly CharacterBody trackedBody;
        private bool waitingForKill;
        private int _resets;
        private int resets {
            get { return _resets; }
            set {
                _resets = value;

                if (resets < consecutive) {
                    if (consecutive > consecutiveBest) {
                        consecutiveBest = consecutive;
                    }
                    consecutive = resets;
                }
            }
        }

        private int consecutive;
        private int consecutiveBest;

        public ConsecutiveReset(CharacterBody trackedBody)
        {
            this.trackedBody = trackedBody;
        }

        public void Hook()
        {
            // Achievement is server-side — client/authority-side may not be accurate?
            trackedBody.onSkillActivatedAuthority += Tracker_Start;
            GlobalEventManager.onCharacterDeathGlobal += Tracker_End;
        }

        public void Unhook()
        {
            trackedBody.onSkillActivatedAuthority -= Tracker_Start;
            GlobalEventManager.onCharacterDeathGlobal -= Tracker_End;
        }

        private void Tracker_Start(GenericSkill skillSlot)
        {
            if (skillSlot.skillDef == requiredSkillDef && requiredSkillDef != null) {
                if (waitingForKill) {
                    resets = 0;
                }
                waitingForKill = true;
            }
        }

        private void Tracker_End(DamageReport damageReport)
        {
            if (damageReport.attackerBody == null) return;
            if (damageReport.attackerBody != trackedBody) return;

            if ((damageReport.damageInfo.damageType & DamageType.ResetCooldownsOnKill) == DamageType.ResetCooldownsOnKill) {
                waitingForKill = false;
                resets++;
            }
        }

        public override string ToString()
        {
            System.Text.StringBuilder sb = new();

            sb.Append($"<style=cStack>> </style><color={banditSkullColour}>Consecutive Resets</color><style=cStack>: </style>");
            sb.Append($"<style=cStack>   > consecutive: </style>{consecutive}<style=cStack> ({consecutiveBest})</style>");

            return sb.ToString();
        }
    }
}
