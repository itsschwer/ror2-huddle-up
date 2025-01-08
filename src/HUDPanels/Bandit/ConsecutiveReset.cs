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

        private int totalShots;
        private int resetShots;
        private int totalShotsStage;
        private int resetShotsStage;


        public ConsecutiveReset(LocalUser user)
        {
            this.user = user;
        }

        public void Hook()
        {
            // OnCharacterDeath is server-side only :/
            GlobalEventManager.onCharacterDeathGlobal += Tracker_End;
            Stage.onStageStartGlobal += OnStageStart;
        }

        public void Unhook()
        {
            GlobalEventManager.onCharacterDeathGlobal -= Tracker_End;
            trackedBody = null;
            Stage.onStageStartGlobal -= OnStageStart;
        }

        private void OnStageStart(Stage _)
        {
            totalShotsStage = 0;
            resetShotsStage = 0;
        }

        private void Tracker_Start(GenericSkill skillSlot)
        {
            // This means that the consecutive counter will only reset to zero on next use of special after miss
            // -- is there any way to have it accurately reflect zero sooner?
            if (skillSlot.skillDef == requiredSkillDef && requiredSkillDef != null) {
                if (waitingForKill) {
                    resets = 0;
                }
                waitingForKill = true;

                totalShots++;
                totalShotsStage++;
            }
        }

        private void Tracker_End(DamageReport damageReport)
        {
            if (damageReport.attackerBody == null) return;
            if (damageReport.attackerBody != trackedBody) return;

            DamageType damageType = Compatibility.Compatibility.ExtractDamageType(damageReport.damageInfo);
            if ((damageType & DamageType.ResetCooldownsOnKill) == DamageType.ResetCooldownsOnKill) {
                waitingForKill = false;
                resets++;

                resetShots++;
                resetShotsStage++;
            }
        }

        private bool HasRequiredSkill() => trackedBody.skillLocator.FindSkillByDef(requiredSkillDef);




        internal void FixedUpdate()
        {
            trackedBody = user.currentNetworkUser.GetCurrentBody();
        }

        public override string ToString()
        {
            System.Text.StringBuilder sb = new();

            sb.Append($"<style=cStack>> </style><color={banditSkullColour}>Consecutive Resets</color><style=cStack>: </style>");
            if (totalShots == 0) sb.Append("<style=cStack>-.--%</style>");
            else sb.Append($"{((float)resetShots / totalShots):0.00%}");
            sb.AppendLine();

            sb.Append($"<style=cStack>   > this stage: </style>");
            if (totalShotsStage == 0) sb.Append("<style=cStack>-.--%</style>");
            else sb.Append($"{((float)resetShotsStage / totalShotsStage):0.00%}");
            sb.AppendLine($"<style=cStack> ({resetShotsStage}/{totalShotsStage})</style>");

            sb.Append($"<style=cStack>   > consecutive: </style>{consecutive}<style=cStack> ({consecutiveBest})</style>");


            bool lightsOutNotSelected = !HasRequiredSkill();
            bool notHost = !UnityEngine.Networking.NetworkServer.active;
            if (lightsOutNotSelected || notHost) {
                sb.AppendLine().AppendLine();

                if (lightsOutNotSelected) {
                    sb.AppendLine("<size=80%><style=cDeath>WARN: nothing to track</style></size>");
                    sb.Append($"<size=80%><style=cStack>    × <style=cDeath><color={banditSkullColour}>{Language.GetString(requiredSkillDef.skillNameToken)}</color> not selected.</style></style></size>");
                }

                if (lightsOutNotSelected && notHost) sb.AppendLine();

                if (notHost) {
                    sb.AppendLine("<size=80%><style=cDeath>WARN: sorry, this feature only works").Append("  on host.</style></size>");
                    sb.AppendLine().Append("<size=80%><style=cStack>    × <style=cDeath>report on <style=cStack>GitHub</style> if you know a fix!</style></style></size>");
                }
            }

            return sb.ToString();
        }
    }
}
