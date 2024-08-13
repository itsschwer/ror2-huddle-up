using RoR2;

namespace LootTip
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
}
