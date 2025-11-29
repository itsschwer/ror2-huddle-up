using RoR2;
using RoR2.UI;

namespace HUDdleUP.Loot
{
    // PurchaseInteraction.displayNameToken and .contextToken
    // appear to be resolved into language string if drone is upgraded
    // e.g. "Broken Emergency Drone (Tier 2)"
    // which kind of seems like a massive pain to check for
    //
    // spawn_prefab
    // "RoR2/DLC3/DroneCombinerStation/DroneCombinerStation.prefab"
    // "RoR2/Base/Drones/EmergencyDroneBroken.prefab"
    internal sealed class DronePanel : UnityEngine.MonoBehaviour
    {
        private static HUD hud;

        public static void Hook() => HUD.shouldHudDisplay += Init;
        public static void Unhook() => HUD.shouldHudDisplay -= Init;

        private static void Init(HUD hud, ref bool _)
        {
            if (!Plugin.Config.DronePanel) return;
            if (DronePanel.hud != null) return;

            var objectivePanel = hud.GetComponentInChildren<ObjectivePanelController>();
            if (!objectivePanel) {
#if DEBUG
                Plugin.Logger.LogDebug($"Waiting to initialize {nameof(DronePanel)}.");
#endif
                return;
            }

            DronePanel.hud = hud;

            HUDPanel panel = HUDPanel.ClonePanel(objectivePanel, nameof(DronePanel));
            hud.gameObject.AddComponent<DronePanel>().panel = panel;
            Plugin.Logger.LogDebug($"Initialized {nameof(DronePanel)}.");
        }




        private HUDPanel panel;
        private TMPro.TextMeshProUGUI display;
        private InteractablesTracker tracker;

        private void Start()
        {
            panel.label.text = "Drones:";
            display = panel.AddTextComponent("Drone Tracker");
            tracker = hud.GetComponent<InteractablesTracker>();
        }

        private void Update()
        {
            bool visible = hud.scoreboardPanel.activeSelf;
            panel.gameObject.SetActive(visible);
            if (!visible) return;

            display.text = GenerateText();
        }

        public string GenerateText()
        {
            if (tracker == null) return "<style=cDeath>error: missing interactables tracker</style>";
            if (tracker.interactables == null) return "<style=cDeath>error: missing interactables</style>";
            Interactables interactables = tracker.interactables;

            bool teleporterBossDefeated = false;
            bool teleporterFullyCharged = false;
            if (TeleporterInteraction.instance != null) {
                teleporterBossDefeated = TeleporterInteraction.instance.monstersCleared;
                teleporterFullyCharged = TeleporterInteraction.instance.isCharged;
            }

            System.Text.StringBuilder sb = new();

            if (interactables.gunnerTurrets > 0) sb.AppendLine(FormatLine("style", "cIsUtility", Language.GetString("TURRET1_BODY_NAME"), interactables.gunnerTurrets));
            GenerateDronesText(interactables, teleporterBossDefeated, teleporterFullyCharged, sb);
            if (interactables.droneTerminals > 0) sb.AppendLine(LootPanel.FormatLine("style", "cIsUtility", "DRONE_VENDOR_TERMINAL_NAME", interactables.droneTerminalsAvailable, interactables.droneTerminals));

#if ALLOYED_COLLECTIVE
            if (teleporterBossDefeated) {
                bool droneCombinerPresent = InstanceTracker.GetInstancesList<DroneCombinerController>().Count > 0;
                sb.AppendLine();
                sb.AppendLine($"{LootPanel.FormatLabel("<style=cSub>" + Language.GetString("DRONE_SCRAPPER_NAME") + "</style>")}{(interactables.droneScrapperPresent ? "@" : "×")}");
                sb.AppendLine($"{LootPanel.FormatLabel("<style=cSub>" + Language.GetString("DRONE_COMBINER_NAME") + "</style>")}{(droneCombinerPresent ? "@" : "×")}");
            }
#endif

            return sb.ToString();
        }

        private static System.Text.StringBuilder GenerateDronesText(Interactables interactables, bool teleporterBossDefeated, bool teleporterFullyCharged, System.Text.StringBuilder sb)
        {
            if (interactables.drones > 0) {
                if (teleporterFullyCharged) {
                    AppendDrone("DRONE_GUNNER_BODY_NAME", ColorCatalog.ColorIndex.Tier1Item, interactables.gunnerDrones, sb);
                    AppendDrone("DRONE_HEALING_BODY_NAME", ColorCatalog.ColorIndex.Tier1Item, interactables.healingDrones, sb);
                    AppendDrone("DRONE_HAULER_BODY_NAME", ColorCatalog.ColorIndex.Tier1Item, interactables.transportDrones, sb);
                    AppendDrone("DRONE_JUNK_BODY_NAME", ColorCatalog.ColorIndex.Tier1Item, interactables.junkDrones, sb);
                    AppendDrone("DRONE_MISSILE_BODY_NAME", ColorCatalog.ColorIndex.Tier2Item, interactables.missileDrones, sb);
                    AppendDrone("FLAMEDRONE_BODY_NAME", ColorCatalog.ColorIndex.Tier2Item, interactables.incineratorDrones, sb);
                    AppendDrone("EMERGENCYDRONE_BODY_NAME", ColorCatalog.ColorIndex.Tier2Item, interactables.emergencyDrones, sb);
                    AppendDrone("DRONE_RECHARGE_BODY_NAME", ColorCatalog.ColorIndex.Tier2Item, interactables.barrierDrones, sb);
                    AppendDrone("DRONE_CLEANUP_BODY_NAME", ColorCatalog.ColorIndex.Tier2Item, interactables.cleanupDrones, sb);
                    AppendDrone("DRONE_JAILER_BODY_NAME", ColorCatalog.ColorIndex.Tier2Item, interactables.jailerDrones, sb);
                    AppendDrone("DRONE_BOMBARDMENT_BODY_NAME", ColorCatalog.ColorIndex.Tier3Item, interactables.bombardmentDrones, sb);
                    AppendDrone("DRONE_COPYCAT_BODY_NAME", ColorCatalog.ColorIndex.Tier3Item, interactables.freezeDrones, sb);
                    AppendDrone("DRONE_MEGA_BODY_NAME", ColorCatalog.ColorIndex.Tier3Item, interactables.tc280Drones, sb);
                    AppendDrone("EQUIPMENTDRONE_BODY_NAME", ColorCatalog.ColorIndex.Equipment, interactables.equipmentDrones, sb);

                    return sb;
                }
                else if (teleporterBossDefeated) {
                    // other potential idea is to split by drone category (healing [#77FF75], combat [#FF4B32],utility [#AC68F8]; names from DroneType, colours from Operator UI assets..?)
                    System.Collections.Generic.List<string> strings = new();
                    if (interactables.t1Drones > 0) strings.Add(Util.GenerateColoredString(interactables.t1Drones.ToString(), ColorCatalog.GetColor(ColorCatalog.ColorIndex.Tier1Item)));
                    if (interactables.t2Drones > 0) strings.Add(Util.GenerateColoredString(interactables.t2Drones.ToString(), ColorCatalog.GetColor(ColorCatalog.ColorIndex.Tier2Item)));
                    if (interactables.t3Drones > 0) strings.Add(Util.GenerateColoredString(interactables.t3Drones.ToString(), ColorCatalog.GetColor(ColorCatalog.ColorIndex.Tier3Item)));
                    if (interactables.equipmentDrones > 0) strings.Add(Util.GenerateColoredString(interactables.equipmentDrones.ToString(), ColorCatalog.GetColor(ColorCatalog.ColorIndex.Equipment)));
                    string result = string.Join(" · ", strings);

                    if (!string.IsNullOrEmpty(result)) sb.AppendLine($"{LootPanel.FormatLabel("<style=cIsUtility>Drones</style>")}<style=cStack>{result}</style>");
                    return sb;
                }
            }

            return sb.AppendLine(FormatLine("style", "cIsUtility", "Drones", interactables.drones));
        }

        private static System.Text.StringBuilder AppendDrone(string nameToken, ColorCatalog.ColorIndex color, int count, System.Text.StringBuilder sb)
        {
            if (count > 0) sb.AppendLine($"{LootPanel.FormatLabel(Util.GenerateColoredString(Language.GetString(nameToken), ColorCatalog.GetColor(color)))}{count}");
            return sb;
        }

        private static string FormatLine(string tagKey, string tagValue, string label, int count)
            => $"{LootPanel.FormatLabel($"<{tagKey}={tagValue}>{label}</{tagKey}>")}{count}";
    }
}
