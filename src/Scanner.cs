using RoR2;

namespace LootObjectives
{
    public static class Scanner
    {
        public struct Interactables
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

            public Interactables() { }
        }

        public static Interactables Scan()
        {
            Interactables interactables = new Interactables();

            var interactors = InstanceTracker.GetInstancesList<PurchaseInteraction>();
            for (int i = 0; i < interactors.Count; i++) {
                // if (!interactors[i].available) continue;
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

        public static bool IsLoot(PurchaseInteraction interactor)
        {
            return IsChest(interactor)
                || IsTerminal(interactor)
                || IsChanceShrine(interactor)
                || IsLockbox(interactor)
                || IsAdaptiveChest(interactor)
                || IsVoid(interactor)
                || IsCloakedChest(interactor);
        }

        public static bool IsLockbox(PurchaseInteraction interactor)
        {
            switch (interactor.displayNameToken) {
                default: return false;
                case "LOCKBOX_NAME":
                case "VOIDLOCKBOX_NAME":
                    return true;
            }
        }

        public static bool IsTerminal(PurchaseInteraction interactor)
        {
            return interactor.displayNameToken == "MULTISHOP_TERMINAL_NAME";
        }

        public static bool IsChanceShrine(PurchaseInteraction interactor)
        {
            return interactor.displayNameToken == "SHRINE_CHANCE_NAME";
        }

        public static bool IsAdaptiveChest(PurchaseInteraction interactor)
        {
            return interactor.displayNameToken == "CASINOCHEST_NAME";
        }

        public static bool IsCloakedChest(PurchaseInteraction interactor)
        {
            return interactor.displayNameToken == "CHEST1_STEALTHED_NAME";
        }

        public static bool IsChest(PurchaseInteraction interactor)
        {
            switch(interactor.displayNameToken) {
                default: return false;
                case "CHEST1_NAME":
                case "CHEST2_NAME":
                case "GOLDCHEST_NAME":
                case "CATEGORYCHEST_HEALING_NAME":
                case "CATEGORYCHEST_DAMAGE_NAME":
                case "CATEGORYCHEST_UTILITY_NAME":
                case "CATEGORYCHEST2_HEALING_NAME":
                case "CATEGORYCHEST2_DAMAGE_NAME":
                case "CATEGORYCHEST2_UTILITY_NAME":
                    return true;
            }
        }

        public static bool IsVoid(PurchaseInteraction interactor)
        {
            switch (interactor.displayNameToken) {
                default: return false;
                case "VOID_CHEST_NAME":
                case "VOID_TRIPLE_NAME":
                    return true;
            }
        }
    }
}
