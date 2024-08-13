using RoR2;
using RoR2.UI;

namespace LootTip
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

            public readonly int lunarPods = 0;
            public readonly int lunarPodsAvailable = 0;
            public readonly bool cleansingPoolPresent = false;

            public readonly bool scrapperPresent = false;

            public readonly int whiteTakers = 0;
            public readonly int greenTakers = 0;
            public readonly int redTakers = 0;
            public readonly int yellowTakers = 0;

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
                                shrineChances += shrine.maxPurchaseCount;
                                shrineChancesAvailable += shrine.maxPurchaseCount - shrine.successfulPurchaseCount;
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
                        case "DUPLICATOR_NAME":
                        case "DUPLICATOR_MILITARY_NAME":
                        case "DUPLICATOR_WILD_NAME":
                        case "BAZAAR_CAULDRON_NAME":
                            switch (interactions[i].costType) {
                                default: break;
                                case CostTypeIndex.WhiteItem:
                                    whiteTakers++;
                                    break;
                                case CostTypeIndex.GreenItem:
                                    greenTakers++;
                                    break;
                                case CostTypeIndex.RedItem:
                                    redTakers++;
                                    break;
                                case CostTypeIndex.BossItem:
                                    yellowTakers++;
                                    break;
                                
                            }
                            break;
                        case "SHRINE_CLEANSE_NAME":
                            cleansingPoolPresent = true;
                            break;
                        case "LUNAR_CHEST_NAME":
                            lunarPods++;
                            if (interactions[i].available) lunarPodsAvailable++;
                            break;
                    }
                }
            }
        }

        public Interactables interactables { get; private set; }

        private HUD hud;
        private HudPanel hudPanel;
        private HGTextMeshProUGUI display;
        private UnityEngine.UI.LayoutElement layout;

        public void Hook() => HUD.shouldHudDisplay += InitTooltip;
        public void Unhook() => HUD.shouldHudDisplay -= InitTooltip;

        private void InitTooltip(HUD hud, ref bool _)
        {
            if (display != null) return;

            var objectivePanel = hud.GetComponentInChildren<ObjectivePanelController>();
            if (!objectivePanel) {
                Log.Debug("Waiting for loot panel to be initialized.");
                return;
            }

            HudPanel panel = objectivePanel.ClonePanel("Loot Panel");
            panel.label.text = "Loot";
            display = panel.AddTextComponent("Loot Tracker");
            layout = display.GetComponent<UnityEngine.UI.LayoutElement>();
            this.hud = hud;
            hudPanel = panel;

            Log.Debug("Loot panel initialized.");
            Scan();
        }

        public void Scan()
        {
            if (!display) {
                Log.Warning("Attempted to scan but loot panel has not been initialized!");
                return;
            }

            // Scoreboard visibility logic from RoR2.UI.HUD.Update()
            bool visible = (hud.localUserViewer?.inputPlayer != null && hud.localUserViewer.inputPlayer.GetButton("info"));
            hudPanel.gameObject.SetActive(visible);
            if (!visible) return; //todo: maybe config

            interactables = new Interactables(
                InstanceTracker.GetInstancesList<PurchaseInteraction>(),
                (UnityEngine.Object.FindObjectOfType<ScrapperController>() != null)
            );

            display.text = GenerateText();
            layout.preferredHeight = display.renderedHeight;
        }

        internal string GenerateText()
        {
            string equip = $"#{ColorCatalog.GetColorHexString(ColorCatalog.ColorIndex.Equipment)}";
            System.Text.StringBuilder sb = new();
            if (interactables.terminals > 0)        sb.AppendLine(FormatLine("style", "cIsUtility", "MULTISHOP_TERMINAL_NAME", interactables.terminalsAvailable, interactables.terminals));
            if (interactables.chests > 0)           sb.AppendLine(FormatLine("style", "cIsDamage", "CHEST1_NAME", interactables.chestsAvailable,interactables.chests));
            if (interactables.adaptiveChests > 0)   sb.AppendLine(FormatLine("style", "cArtifact", "CASINOCHEST_NAME", interactables.adaptiveChestsAvailable, interactables.adaptiveChests));
            if (interactables.chanceShrines > 0)    sb.AppendLine(UnityEngine.Networking.NetworkServer.active
                                                                ? FormatLine("style", "cShrine", "SHRINE_CHANCE_NAME", interactables.shrineChancesAvailable, interactables.shrineChances, interactables.chanceShrinesAvailable)
                                                                : FormatLine("style", "cShrine", "SHRINE_CHANCE_NAME", interactables.chanceShrinesAvailable, interactables.chanceShrines));
            if (interactables.equipment > 0)        sb.AppendLine(FormatLine("color", equip, "EQUIPMENTBARREL_NAME", interactables.equipmentAvailable, interactables.equipment));
            if (interactables.lockboxes > 0)        sb.AppendLine(FormatLine("style", "cHumanObjective", "LOCKBOX_NAME", interactables.lockboxesAvailable, interactables.lockboxes));

            if (TeleporterInteraction.instance != null) {
                if (TeleporterInteraction.instance.monstersCleared) {
                    string cleansingPool = interactables.cleansingPoolPresent ? " · <style=cLunarObjective><sprite name=\"LunarCoin\" tint=1></style>" : "";
                    sb.AppendLine().AppendLine($"{FormatLabel("<style=cSub>" + Language.GetString("SCRAPPER_NAME") + "</style>")}{(interactables.scrapperPresent ? "<sprite name=\"LunarCoin\" tint=1>" : "×")}{cleansingPool}");
                    AppendFabricators(interactables, sb);
                    if (interactables.voids > 0) sb.AppendLine(FormatLine("style", "cIsVoid", "VOID_CHEST_NAME", interactables.voidsAvailable, interactables.voids));
                    if (interactables.lunarPods > 0) sb.AppendLine(FormatLine("style", "cLunarObjective", "LUNAR_CHEST_NAME", interactables.lunarPodsAvailable, interactables.lunarPods));
                }
                if (TeleporterInteraction.instance.isCharged) {
                    if (interactables.cloakedChests > 0) sb.AppendLine().AppendLine(FormatLine("style", "cLunarObjective", "CHEST1_STEALTHED_NAME", interactables.cloakedChestsAvailable, interactables.cloakedChests));
                }
            }

            sb.AppendLine().AppendLine(FormatEnemies());

            return sb.ToString();
        }

        private static System.Text.StringBuilder AppendFabricators(Interactables interactables, System.Text.StringBuilder sb)
        {
            System.Collections.Generic.List<string> strings = new();
            if (interactables.whiteTakers > 0)  strings.Add(Util.GenerateColoredString(interactables.whiteTakers.ToString(),  ColorCatalog.GetColor(ColorCatalog.ColorIndex.Tier1Item)));
            if (interactables.greenTakers > 0)  strings.Add(Util.GenerateColoredString(interactables.greenTakers.ToString(),  ColorCatalog.GetColor(ColorCatalog.ColorIndex.Tier2Item)));
            if (interactables.redTakers > 0)    strings.Add(Util.GenerateColoredString(interactables.redTakers.ToString(),    ColorCatalog.GetColor(ColorCatalog.ColorIndex.Tier3Item)));
            if (interactables.yellowTakers > 0) strings.Add(Util.GenerateColoredString(interactables.yellowTakers.ToString(), ColorCatalog.GetColor(ColorCatalog.ColorIndex.BossItem)));
            string result = string.Join(" · ", strings);

            if (!string.IsNullOrEmpty(result)) sb.AppendLine($"{FormatToken("DUPLICATOR_NAME", "style", "cStack")}<style=cStack>{result}</style>");
            return sb;
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
            string result = string.Join(" · ", strings);

            return $"<style=cStack>Enemies: {(string.IsNullOrEmpty(result) ? "0" : result)}</style>";
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
            => $"{FormatCounter(available, total)}<style=cStack><size=90%> ({meta})</size></style>";
        private static string FormatCounter(int available, int total)
        {
            if (available != 0) return $"{available}<style=cStack>/{total}</style>";
            else return $"<style=cSub>{available}</style><style=cStack>/{total}</style>";
        }
    }
}
