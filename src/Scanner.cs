using RoR2;
using RoR2.UI;

namespace LootObjectives
{
    public sealed class Scanner
    {
        public sealed record Interactables
        {
            public readonly int chests = 0;
            public readonly int chestsAvailable = 0;
            public readonly int terminals = 0;
            public readonly int terminalsAvailable = 0;
            public readonly int chanceShrines = 0;
            public readonly int chanceShrinesAvailable = 0;
            public readonly int shrineChances = 0;
            public readonly int shrineChancesAvailable = 0;
            public readonly int lockboxes = 0;
            public readonly int lockboxesAvailable = 0;
            public readonly int adaptiveChests = 0;
            public readonly int adaptiveChestsAvailable = 0;
            public readonly int voids = 0;
            public readonly int voidsAvailable = 0;
            public readonly int cloakedChests = 0;
            public readonly int cloakedChestsAvailable = 0;
            public readonly int equipment = 0;
            public readonly int equipmentAvailable = 0;

            public readonly bool scrapperPresent = false;

            public Interactables(System.Collections.Generic.List<PurchaseInteraction> interactions, bool scrapperPresent)
            {
                this.scrapperPresent = scrapperPresent;

                for (int i = 0; i < interactions.Count; i++) {
                    switch (interactions[i].displayNameToken) {
                        default: break;
                        case "CHEST1_NAME":
                        case "CHEST2_NAME":
                        case "GOLDCHEST_NAME":
                        case "CATEGORYCHEST_HEALING_NAME":
                        case "CATEGORYCHEST_DAMAGE_NAME":
                        case "CATEGORYCHEST_UTILITY_NAME":
                        case "CATEGORYCHEST2_HEALING_NAME":
                        case "CATEGORYCHEST2_DAMAGE_NAME":
                        case "CATEGORYCHEST2_UTILITY_NAME":
                            chests++;
                            if (interactions[i].available) chestsAvailable++;
                            break;
                        case "EQUIPMENTBARREL_NAME":
                            equipment++;
                            if (interactions[i].available) equipmentAvailable++;
                            break;
                        case "MULTISHOP_TERMINAL_NAME":
                            terminals++;
                            if (interactions[i].available) terminalsAvailable++;
                            break;
                        case "SHRINE_CHANCE_NAME":
                            chanceShrines++;
                            if (interactions[i].available) chanceShrinesAvailable++;
                            // Only count potential successes on server ∵ purchase counts are not networked
                            if (UnityEngine.Networking.NetworkServer.active) {
                                ShrineChanceBehavior shrine = interactions[i].GetComponent<ShrineChanceBehavior>();
                                shrineChances = shrine.maxPurchaseCount;
                                shrineChancesAvailable = shrine.maxPurchaseCount - shrine.successfulPurchaseCount;
                            }
                            break;
                        case "LOCKBOX_NAME":
                        case "VOIDLOCKBOX_NAME":
                            lockboxes++;
                            if (interactions[i].available) lockboxesAvailable++;
                            break;
                        case "CASINOCHEST_NAME":
                            adaptiveChests++;
                            if (interactions[i].available) adaptiveChestsAvailable++;
                            break;
                        case "VOID_CHEST_NAME":
                        case "VOID_TRIPLE_NAME":
                            voids++;
                            if (interactions[i].available) voidsAvailable++;
                            break;
                        case "CHEST1_STEALTHED_NAME":
                            cloakedChests++;
                            if (interactions[i].available) cloakedChestsAvailable++;
                            break;
                    }
                }
            }
        }

        public Interactables interactables { get; private set; }

        private TooltipProvider tooltip;
        internal const string TOOLTIP_TITLE_TOKEN = "ITSSCHWER_LOOT_OBJECTIVE";
        internal const string TOOLTIP_TITLE = "Loot Remaining";

        public void Hook() => HUD.shouldHudDisplay += InitTooltip;
        public void Unhook() => HUD.shouldHudDisplay -= InitTooltip;

        private void InitTooltip(HUD hud, ref bool _)
        {
            if (tooltip != null) return;

            var objectivePanel = hud.GetComponentInChildren<ObjectivePanelController>();
            var label = objectivePanel?.GetComponentInChildren<HGTextMeshProUGUI>();
            tooltip = Utils.AddTooltipProvider(label);

            if (tooltip != null) {
                tooltip.titleColor = DifficultyCatalog.GetDifficultyDef(Run.instance.selectedDifficulty).color;
                tooltip.titleToken = TOOLTIP_TITLE_TOKEN;
                Log.Debug("Tooltip intialized.");
                Scan();
            }
            else {
                Log.Warning("Waiting for tooltip to be initialized.");
            }
        }

        public Scanner Scan()
        {
            interactables = new Interactables(
                InstanceTracker.GetInstancesList<PurchaseInteraction>(),
                (UnityEngine.Object.FindObjectOfType<ScrapperController>() != null)
            );

            Log.Debug("Scanned interactables.");
            return this;
        }

        internal string GetTooltipString()
        {
            string equip = $"#{ColorCatalog.GetColorHexString(ColorCatalog.ColorIndex.Equipment)}";
            System.Text.StringBuilder sb = new();
            if (interactables.terminals > 0)        sb.AppendLine(FormatLine("style", "cIsUtility", "MULTISHOP_TERMINAL_NAME", interactables.terminalsAvailable, interactables.terminals));
            if (interactables.chests > 0)           sb.AppendLine(FormatLine("style", "cIsDamage", "CHEST1_NAME", interactables.chestsAvailable,interactables.chests));
            if (interactables.adaptiveChests > 0)   sb.AppendLine(FormatLine("style", "cArtifact", "CASINOCHEST_NAME", interactables.adaptiveChestsAvailable, interactables.adaptiveChests));
            if (interactables.shrineChances > 0)    sb.AppendLine(UnityEngine.Networking.NetworkServer.active
                                                                ? FormatLine("style", "cShrine", "SHRINE_CHANCE_NAME", interactables.shrineChancesAvailable, interactables.shrineChances, interactables.chanceShrinesAvailable)
                                                                : FormatLine("style", "cShrine", "SHRINE_CHANCE_NAME", interactables.chanceShrinesAvailable, interactables.chanceShrines));
            if (interactables.equipment > 0)        sb.AppendLine(FormatLine("color", equip, "EQUIPMENTBARREL_NAME", interactables.equipmentAvailable, interactables.equipment));
            if (interactables.lockboxes > 0)        sb.AppendLine(FormatLine("style", "cHumanObjective", "LOCKBOX_NAME", interactables.lockboxesAvailable, interactables.lockboxes));

            if (TeleporterInteraction.instance != null) {
                if (TeleporterInteraction.instance.monstersCleared) {
                    sb.AppendLine().AppendLine($"{FormatLabel("<style=cSub>" + Language.GetString("SCRAPPER_NAME") + "</style>")}{(interactables.scrapperPresent ? "Yes" : "No")}");
                    if (interactables.voids > 0) sb.AppendLine(FormatLine("style", "cIsVoid", "VOID_CHEST_NAME", interactables.voidsAvailable, interactables.voids));
                }
                if (TeleporterInteraction.instance.isCharged) {
                    if (interactables.cloakedChests > 0) sb.AppendLine().AppendLine(FormatLine("style", "cLunarObjective", "CHEST1_STEALTHED_NAME", interactables.cloakedChestsAvailable, interactables.cloakedChests));
                }
            }

            sb.AppendLine().AppendLine(FormatEnemies());

            return sb.ToString();
        }

        private static string FormatEnemies()
        {
            int monsters = TeamComponent.GetTeamMembers(TeamIndex.Monster).Count;
            int lunars = TeamComponent.GetTeamMembers(TeamIndex.Lunar).Count;
            int voids = TeamComponent.GetTeamMembers(TeamIndex.Void).Count;

            System.Collections.Generic.List<string> strings = new();
            if (monsters > 0) strings.Add($"<style=cSub>{monsters}</style>");
            if (lunars > 0) strings.Add($"<style=cLunarObjective>{lunars}</style>");
            if (voids > 0) strings.Add($"<style=cIsVoid>{voids}</style>");

            return $"<style=cStack>Enemies: {string.Join("/", strings)}</style>";
        }

        private static string FormatLine(string tagKey, string tagValue, string token, int available, int total, int meta)
            => $"{FormatToken(token, tagKey, tagValue)}{FormatCounter(available, total, meta)}";
        private static string FormatLine(string tagKey, string tagValue, string token, int available, int total)
            => $"{FormatToken(token, tagKey, tagValue)}{FormatCounter(available, total)}";
        private static string FormatToken(string token, string tagKey, string tagValue)
            => FormatLabel($"<{tagKey}={tagValue}>{Language.GetString(token)}</{tagKey}>");
        private static string FormatLabel(string label)
            => $"<style=cStack>> </style>{label}<style=cStack>:</style> ";
        private static string FormatCounter(int available, int total, int meta)
            => $"{FormatCounter(available, total)} <style=cStack><size=60%>({meta})</size></style>";
        private static string FormatCounter(int available, int total)
        {
            if (available != 0) return $"{available}<style=cStack>/{total}</style>";
            else return $"<style=cSub>{available}</style><style=cStack>/{total}</style>";
        }
    }
}
