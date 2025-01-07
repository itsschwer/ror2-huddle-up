using RoR2;
using RoR2.Skills;

namespace HUDdleUP.Bandit
{
    /// <summary>
    /// My code is gross but this mostly mirrors RoR2.Achievements.Bandit2.Bandit2ConsecutiveResetAchievement
    /// </summary>
    internal sealed class ConsecutiveReset
    {
        private const string banditSkullColour = "#40C5E4";

        private static readonly SkillDef requiredSkillDef = SkillCatalog.GetSkillDef(SkillCatalog.FindSkillIndexByName("Bandit2.ResetRevolver"));

        private readonly LocalUser user;
        private CharacterBody _trackedBody;
        private CharacterBody trackedBody {
            get { return _trackedBody; }
            set {
                if (value != _trackedBody) {
                    if (_trackedBody != null) {
                        // Achievement is server-side — client/authority-side may not be accurate?
                        _trackedBody.onSkillActivatedAuthority -= Tracker_Start;
                    }

                    _trackedBody = value;

                    if (_trackedBody != null) {
                        // Achievement is server-side — client/authority-side may not be accurate?
                        _trackedBody.onSkillActivatedAuthority += Tracker_Start;
                        resets = 0;
                        waitingForKill = false;
                    }
                }
            }
        }
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
                }

                consecutive = resets;
            }
        }

        private int consecutive;
        private int consecutiveBest;

        public ConsecutiveReset(LocalUser user)
        {
            this.user = user;
        }

        public void Hook()
        {
            GlobalEventManager.onCharacterDeathGlobal += Tracker_End;
        }

        public void Unhook()
        {
            GlobalEventManager.onCharacterDeathGlobal -= Tracker_End;
            trackedBody = null;
        }

        private void Tracker_Start(GenericSkill skillSlot)
        {
            Plugin.Logger.LogWarning("ts");
            if (skillSlot.skillDef == requiredSkillDef && requiredSkillDef != null) {
                Plugin.Logger.LogWarning("skill match");
                if (waitingForKill) {
                    Plugin.Logger.LogWarning("reset");
                    resets = 0;
                }
                waitingForKill = true;
            }
        }

        private void Tracker_End(DamageReport damageReport)
        {
            Plugin.Logger.LogWarning("te");
            if (damageReport.attackerBody == null) return;
            if (damageReport.attackerBody != trackedBody) return;

            Plugin.Logger.LogWarning("attacker match");
            if ((damageReport.damageInfo.damageType & DamageType.ResetCooldownsOnKill) == DamageType.ResetCooldownsOnKill) {
                waitingForKill = false;
                resets++;
                Plugin.Logger.LogWarning("damage match");
            }
        }

        internal void FixedUpdate()
        {
            trackedBody = user.currentNetworkUser.GetCurrentBody();
        }

        public override string ToString()
        {
            System.Text.StringBuilder sb = new();

            sb.AppendLine($"<style=cStack>> </style><color={banditSkullColour}>Consecutive Resets</color><style=cStack>: </style>");
            sb.Append($"<style=cStack>   > consecutive: </style>{consecutive}<style=cStack> ({consecutiveBest})</style>");

            sb.AppendLine();
            sb.AppendLine(trackedBody == null ? "null" : trackedBody.name);

            return sb.ToString();
        }
    }
}
