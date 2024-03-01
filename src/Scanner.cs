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

            public bool scrapperPresent = false;
        }

        public Interactables interactables;

        public void Hook()
        {
            Stage.onStageStartGlobal += Scan;
            GlobalEventManager.OnInteractionsGlobal += Scan;
        }

        public void Unhook()
        {
            Stage.onStageStartGlobal -= Scan;
            GlobalEventManager.OnInteractionsGlobal -= Scan;
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
                        interactables.scrapperPresent = true;
                        break;
                }
            }

            return interactables;
        }
    }
}
