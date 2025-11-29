using RoR2;
using RoR2.UI;

namespace HUDdleUP.Loot
{
    internal sealed class LootPanel : UnityEngine.MonoBehaviour
    {
        private static HUD hud;

        public static void Hook() => HUD.shouldHudDisplay += Init;
        public static void Unhook() => HUD.shouldHudDisplay -= Init;

        private static void Init(HUD hud, ref bool _)
        {
            if (!Plugin.Config.LootPanel) return;
            if (LootPanel.hud != null) return;

            var objectivePanel = hud.GetComponentInChildren<ObjectivePanelController>();
            if (!objectivePanel) {
#if DEBUG
                Plugin.Logger.LogDebug($"Waiting to initialize {nameof(LootPanel)}.");
#endif
                return;
            }

            LootPanel.hud = hud;

            HUDPanel panel = HUDPanel.ClonePanel(objectivePanel, nameof(LootPanel));
            hud.gameObject.AddComponent<LootPanel>().panel = panel;
            Plugin.Logger.LogDebug($"Initialized {nameof(LootPanel)}.");
        }




        private HUDPanel panel;
        private TMPro.TextMeshProUGUI display;
        private InteractablesTracker tracker;

        private const float scrapperSearchFrequency = 1f/4;
        private float lastScrapperSearchTimestamp;
        private bool scrapperPresent;

        private void Start()
        {
            panel.label.text = "Loot:";
            display = panel.AddTextComponent("Loot Tracker");
            tracker = hud.GetComponent<InteractablesTracker>();
        }

        private void Update()
        {
            bool visible = hud.scoreboardPanel.activeSelf;
            panel.gameObject.SetActive(visible);
            if (!visible) return;

            float deltaTime = UnityEngine.Time.unscaledTime - lastScrapperSearchTimestamp;
            if (deltaTime >= scrapperSearchFrequency) {
                lastScrapperSearchTimestamp = UnityEngine.Time.unscaledTime;
                scrapperPresent = FindObjectOfType<ScrapperController>() != null;
            }

            display.text = GenerateText();
        }

        public string GenerateText()
        {
            if (tracker == null) return "<style=cDeath>error: missing interactables tracker</style>";
            if (tracker.interactables == null) return "<style=cDeath>error: missing interactables</style>";
            Interactables interactables = tracker.interactables;

            string equip = $"#{ColorCatalog.GetColorHexString(ColorCatalog.ColorIndex.Equipment)}";
            System.Text.StringBuilder sb = new();

            if (interactables.terminals > 0)      sb.AppendLine(FormatLine("style", "cIsUtility", "MULTISHOP_TERMINAL_NAME", interactables.terminalsAvailable, interactables.terminals));
            if (interactables.chests > 0)         sb.AppendLine(FormatLine("style", "cIsDamage", "CHEST1_NAME", interactables.chestsAvailable, interactables.chests));
            if (interactables.adaptiveChests > 0) sb.AppendLine(FormatLine("style", "cArtifact", "CASINOCHEST_NAME", interactables.adaptiveChestsAvailable, interactables.adaptiveChests));
            if (interactables.chanceShrines > 0)  sb.AppendLine(UnityEngine.Networking.NetworkServer.active
                                                                ? FormatLine("style", "cShrine", "SHRINE_CHANCE_NAME", interactables.shrineChancesAvailable, interactables.shrineChances, interactables.chanceShrinesAvailable)
                                                                : FormatLine("style", "cShrine", "SHRINE_CHANCE_NAME", interactables.chanceShrinesAvailable, interactables.chanceShrines));
            if (interactables.equipment > 0)      sb.AppendLine(FormatLine("color", equip, "EQUIPMENTBARREL_NAME", interactables.equipmentAvailable, interactables.equipment));
            if (interactables.lockboxes > 0)      sb.AppendLine(FormatLine("style", "cHumanObjective", "LOCKBOX_NAME", interactables.lockboxesAvailable, interactables.lockboxes));

            if (TeleporterInteraction.instance != null) {
                if (TeleporterInteraction.instance.monstersCleared) {
                    string cleansingPool = interactables.cleansingPoolPresent ? " · <style=cLunarObjective>@</style>" : "";
                    sb.AppendLine().AppendLine($"{FormatLabel("<style=cSub>" + Language.GetString("SCRAPPER_NAME") + "</style>")}{(scrapperPresent ? "@" : "×")}{cleansingPool}");
                    AppendFabricators(interactables, sb);
                    if (interactables.voids > 0) sb.AppendLine(FormatLine("style", "cIsVoid", "VOID_CHEST_NAME", interactables.voidsAvailable, interactables.voids));
                    if (interactables.lunarPods > 0) sb.AppendLine(FormatLine("style", "cLunarObjective", "LUNAR_CHEST_NAME", interactables.lunarPodsAvailable, interactables.lunarPods));
                }
                if (TeleporterInteraction.instance.isCharged) {
                    if (interactables.cloakedChests > 0) sb.AppendLine().AppendLine(FormatLine("style", "cLunarObjective", "CHEST1_STEALTHED_NAME", interactables.cloakedChestsAvailable, interactables.cloakedChests));
                }
            }

            sb.AppendLine().AppendLine(FormatEnemies());
            if (TeleporterInteraction.instance != null && TeleporterInteraction.instance.shrineBonusStacks > 0)
                sb.AppendLine($"<style=cStack>Invitations: <style=cIsUtility>{TeleporterInteraction.instance.shrineBonusStacks}</style></style>");

            if (Stage.instance && Stage.instance.sceneDef)
                sb.AppendLine().AppendLine($"<style=cStack>-- {Language.GetString(Stage.instance.sceneDef.nameToken)} --</style>");

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
        internal static string FormatLine(string tagKey, string tagValue, string token, int available, int total)
            => $"{FormatToken(token, tagKey, tagValue)}{FormatCounter(available, total)}";
        private static string FormatToken(string token, string tagKey, string tagValue)
            => FormatLabel($"<{tagKey}={tagValue}>{Language.GetString(token)}</{tagKey}>");
        internal static string FormatLabel(string label)
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
