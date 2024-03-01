using RoR2;

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
            public int scrappers = 0;
        }

        private RoR2.UI.TooltipProvider tooltip;

        public Interactables interactables;

        public void Hook()
        {
            Stage.onStageStartGlobal += Scan;
            GlobalEventManager.OnInteractionsGlobal += Scan;
            RoR2.UI.HUD.shouldHudDisplay += UpdateHUD;
        }

        public void Unhook()
        {
            Stage.onStageStartGlobal -= Scan;
            GlobalEventManager.OnInteractionsGlobal -= Scan;
            RoR2.UI.HUD.shouldHudDisplay -= UpdateHUD;
        }

        public void Scan(Stage _) => Scan();
        public void Scan(Interactor _, IInteractable __, UnityEngine.GameObject ___) => Scan();
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
                    case "MULTISHOP_TERMINAL_NAME":
                        interactables.terminals++;
                        if (interactors[i].available) interactables.terminalsAvailable++;
                        break;
                    case "SHRINE_CHANCE_NAME":
                        {
                            ShrineChanceBehavior shrine = interactors[i].GetComponent<ShrineChanceBehavior>();
                            interactables.shrineChances += shrine.maxPurchaseCount;
                            if (interactors[i].available) interactables.shrineChancesAvailable += (shrine.maxPurchaseCount - shrine.successfulPurchaseCount);
                            break;
                        }
                    case "LOCKBOX_NAME":
                    case "VOIDLOCKBOX_NAME":
                        interactables.lockboxes++;
                        if (interactors[i].available) interactables.lockboxesAvailable++;
                        break;
                    case "CASINOCHEST_NAME":
                        interactables.adaptiveChests++;
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
                    case "SCRAPPER_NAME":
                        interactables.scrappers++;
                        break;
                }
            }

            if (tooltip) UpdateTooltip();

            return interactables;
        }

        public void UpdateTooltip()
        {
            System.Text.StringBuilder sb = new();
            if (interactables.terminals > 0)        sb.AppendLine($"{Language.GetString("MULTISHOP_TERMINAL_NAME")}: {interactables.terminalsAvailable}/{interactables.terminals}");
            if (interactables.chests > 0)           sb.AppendLine($"{Language.GetString("CHEST1_NAME")}: {interactables.chestsAvailable}/{interactables.chests}");
            if (interactables.adaptiveChests > 0)   sb.AppendLine($"{Language.GetString("CASINOCHEST_NAME")}: {interactables.adaptiveChestsAvailable}/{interactables.adaptiveChests}");
            if (interactables.shrineChances > 0)    sb.AppendLine($"{Language.GetString("SHRINE_CHANCE_NAME")}: {interactables.shrineChancesAvailable}/{interactables.shrineChances}");
            if (interactables.lockboxes > 0)        sb.AppendLine($"{Language.GetString("LOCKBOX_NAME")}: {interactables.chestsAvailable}/{interactables.chests}");
            if (interactables.voids > 0)            sb.AppendLine($"{Language.GetString("VOID_CHEST_NAME")}: {interactables.voidsAvailable}/{interactables.voids}");
            if (TeleporterInteraction.instance != null && TeleporterInteraction.instance.isCharged) {
                if (interactables.scrappers > 0)    sb.AppendLine($"{Language.GetString("SCRAPPER_NAME")}: {interactables.scrappers}");
                if (interactables.cloakedChests > 0) sb.AppendLine($"{Language.GetString("CHEST1_STEALTHED_NAME")}: {interactables.cloakedChestsAvailable}/{interactables.cloakedChests}");
            }
        }


        private void UpdateHUD(RoR2.UI.HUD hud, ref bool _)
        {
            var target = hud.GetComponentInChildren<RoR2.UI.ObjectivePanelController>().GetComponentInChildren<RoR2.UI.HGTextMeshProUGUI>();
            tooltip = Utils.AddTooltipProvider(target);
            tooltip.titleToken = "Loot";
        }
    }
}
