﻿using RoR2;

namespace LootObjectives
{
    public class Scanner
    {
        public record Interactables
        {
            public int chests = 0;
            public int chestsAvailable = 0;
            public int terminals = 0;
            public int terminalsAvailable = 0;
            public int shrineChances = 0;
            public int shrineChancesAvailable = 0;
            public int lockboxes = 0;
            public int lockboxesAvailable = 0;
            public int adaptiveChests = 0;
            public int adaptiveChestsAvailable = 0;
            public int voids = 0;
            public int voidsAvailable = 0;
            public int cloakedChests = 0;
            public int cloakedChestsAvailable = 0;
            public int equipment = 0;
            public int equipmentAvailable = 0;

            public bool scrappers = false;
        }

        private RoR2.UI.TooltipProvider tooltip;

        public Interactables interactables;

        public void Hook()
        {
            Stage.onStageStartGlobal += Scan;
            GlobalEventManager.OnInteractionsGlobal += Scan;
            TeleporterInteraction.onTeleporterChargedGlobal += Scan;
            RoR2.UI.HUD.shouldHudDisplay += UpdateHUD;
        }

        public void Unhook()
        {
            Stage.onStageStartGlobal -= Scan;
            GlobalEventManager.OnInteractionsGlobal -= Scan;
            TeleporterInteraction.onTeleporterChargedGlobal -= Scan;
            RoR2.UI.HUD.shouldHudDisplay -= UpdateHUD;
        }

        public void Scan(Stage _) => Scan();
        public void Scan(Interactor _, IInteractable __, UnityEngine.GameObject ___) => Scan();
        public void Scan(TeleporterInteraction _) => Scan();
        public Interactables Scan()
        {
            interactables = new Interactables();

            var interactors = InstanceTracker.GetInstancesList<PurchaseInteraction>();
            for (int i = 0; i < interactors.Count; i++) {
                switch (interactors[i].displayNameToken) {
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
                        interactables.chests++;
                        if (interactors[i].available) interactables.chestsAvailable++;
                        break;
                    case "EQUIPMENTBARREL_NAME":
                        interactables.equipment++;
                        if (interactors[i].available) interactables.equipmentAvailable++;
                        break;
                    case "MULTISHOP_TERMINAL_NAME":
                        {
                            ShopTerminalBehavior terminal = interactors[i].GetComponent<ShopTerminalBehavior>();
                            if (PickupCatalog.GetPickupDef(terminal.CurrentPickupIndex()).equipmentIndex != EquipmentIndex.None) {
                                interactables.equipment++;
                                if (interactors[i].available) interactables.equipmentAvailable++;
                            }
                            else {
                                interactables.terminals++;
                                if (interactors[i].available) interactables.terminalsAvailable++;
                            }
                            break;
                        }
                    case "SHRINE_CHANCE_NAME":
                        {
                            ShrineChanceBehavior shrine = interactors[i].GetComponent<ShrineChanceBehavior>();
                            interactables.shrineChances += shrine.maxPurchaseCount;
                            interactables.shrineChancesAvailable += (shrine.maxPurchaseCount - shrine.successfulPurchaseCount);
                            break;
                        }
                    case "LOCKBOX_NAME":
                    case "VOIDLOCKBOX_NAME":
                        interactables.lockboxes++;
                        if (interactors[i].available) interactables.lockboxesAvailable++;
                        break;
                    case "CASINOCHEST_NAME":
                        interactables.adaptiveChests++;
                        //todo: need to account for roulette time
                        if (interactors[i].available) interactables.adaptiveChestsAvailable++;
                        break;
                    case "VOID_CHEST_NAME":
                    case "VOID_TRIPLE_NAME":
                        interactables.voids++;
                        if (interactors[i].available) interactables.voidsAvailable++;
                        break;
                    case "CHEST1_STEALTHED_NAME":
                        interactables.cloakedChests++;
                        if (interactors[i].available) interactables.cloakedChestsAvailable++;
                        break;
                }
            }
            interactables.scrappers = (UnityEngine.Object.FindObjectsOfType<ScrapperController>() != null);

            if (tooltip) UpdateTooltip();

            Log.Debug("Executed scan.");
            return interactables;
        }

        public void UpdateTooltip()
        {
            string equip = ColorCatalog.GetColorHexString(ColorCatalog.ColorIndex.Equipment);
            System.Text.StringBuilder sb = new();
            if (interactables.terminals > 0)        sb.AppendLine($"{FormatLabel("<style=cIsUtility>" + Language.GetString("MULTISHOP_TERMINAL_NAME") + "</style>")}{FormatCounter(interactables.terminalsAvailable, interactables.terminals)}");
            if (interactables.chests > 0)           sb.AppendLine($"{FormatLabel("<style=cIsDamage>" + Language.GetString("CHEST1_NAME") + "</style>")}{FormatCounter(interactables.chestsAvailable,interactables.chests)}");
            if (interactables.adaptiveChests > 0)   sb.AppendLine($"{FormatLabel("<style=cArtifact>" + Language.GetString("CASINOCHEST_NAME") + "</style>")}{FormatCounter(interactables.adaptiveChestsAvailable, interactables.adaptiveChests)}");
            if (interactables.shrineChances > 0)    sb.AppendLine($"{FormatLabel("<style=cShrine>" + Language.GetString("SHRINE_CHANCE_NAME") + "</style>")}{FormatCounter(interactables.shrineChancesAvailable, interactables.shrineChances)}");
            if (interactables.equipment > 0)        sb.AppendLine($"{FormatLabel("<color=#FF7F7F>" + Language.GetString("SCOREBOARD_HEADER_EQUIPMENT") + "</color>")}{FormatCounter(interactables.equipmentAvailable, interactables.equipment)}");
            if (interactables.lockboxes > 0)        sb.AppendLine($"{FormatLabel("<style=cHumanObjective>" + Language.GetString("LOCKBOX_NAME") + "</style>")}{FormatCounter(interactables.chestsAvailable, interactables.chests)}");
            if (interactables.voids > 0)            sb.AppendLine($"{FormatLabel("<style=cIsVoid>" + Language.GetString("VOID_CHEST_NAME") + "</style>")}{FormatCounter(interactables.voidsAvailable, interactables.voids)}");
            if (TeleporterInteraction.instance != null && TeleporterInteraction.instance.isCharged) {
                sb.AppendLine($"{FormatLabel("<style=cSub>" + Language.GetString("SCRAPPER_NAME") + "</style>")}{(interactables.scrappers ? "Yes" : "No")}");
                if (interactables.cloakedChests > 0) sb.AppendLine($"{FormatLabel("<style=cLunarObjective>" + Language.GetString("CHEST1_STEALTHED_NAME") + "</style>")}{FormatCounter(interactables.cloakedChestsAvailable, interactables.cloakedChests)}");
            }
            
            tooltip.bodyToken = sb.ToString();
        }

        private static string FormatLabel(string label)
        {
            return $"<style=cStack>> </style>{label}<style=cStack>:</style> ";
        }

        private static string FormatCounter(int available, int total)
        {
            return $"{available}<style=cStack>/{total}</style>";
        }


        private void UpdateHUD(RoR2.UI.HUD hud, ref bool _)
        {
            if (tooltip != null) return;

            var objectivePanel = hud.GetComponentInChildren<RoR2.UI.ObjectivePanelController>();
            var label = objectivePanel?.GetComponentInChildren<RoR2.UI.HGTextMeshProUGUI>();
            tooltip = Utils.AddTooltipProvider(label);

            if (tooltip != null) {
                tooltip.titleColor = DifficultyCatalog.GetDifficultyDef(Run.instance.selectedDifficulty).color;
                tooltip.titleToken = "Loot";
                Log.Debug("Tooltip intialized.");
                Scan();
            }
            else {
                Log.Warning("Waiting for tooltip to be initialized.");
            }
        }
    }
}
