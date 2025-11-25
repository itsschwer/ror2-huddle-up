using RoR2;

namespace HUDdleUP.Loot
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

        public readonly int whiteTakers = 0;
        public readonly int greenTakers = 0;
        public readonly int redTakers = 0;
        public readonly int yellowTakers = 0;

        // "Broken "
        public readonly int gunnerTurrets = 0;

        public readonly int gunnerDrones = 0;
        public readonly int healingDrones = 0;
        public readonly int missileDrones = 0;
        public readonly int incineratorDrones = 0;
        public readonly int emergencyDrones = 0;
        public readonly int tc280Drones = 0;

        public readonly int equipmentDrones = 0;

        public readonly int transportDrones = 0;
        public readonly int junkDrones = 0;
        public readonly int barrierDrones = 0;
        public readonly int cleanupDrones = 0;
        public readonly int jailerDrones = 0;
        public readonly int bombardmentDrones = 0;
        public readonly int freezeDrones = 0;

        public int drones => gunnerDrones + healingDrones + missileDrones
            + incineratorDrones + emergencyDrones + tc280Drones
            + equipmentDrones + transportDrones + junkDrones + barrierDrones
            + cleanupDrones + jailerDrones + bombardmentDrones + freezeDrones;

        public int t1Drones => gunnerDrones + healingDrones + transportDrones + junkDrones;
        public int t2Drones => missileDrones + incineratorDrones + emergencyDrones + barrierDrones + cleanupDrones + jailerDrones; // technically equipment drones too
        public int t3Drones => tc280Drones + bombardmentDrones + freezeDrones;

        public readonly int droneTerminals = 0;
        public readonly int droneTerminalsAvailable = 0;
        public readonly bool droneCombinerPresent = false; // "DRONE_COMBINER_NAME"
        public readonly bool droneScrapperPresent = false; // "DRONE_SCRAPPER_NAME"


        public Interactables() : this(InstanceTracker.GetInstancesList<PurchaseInteraction>()) { }
        public Interactables(System.Collections.Generic.List<PurchaseInteraction> interactions)
        {
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
                        if (interactions[i].name.Contains("Equipment")) { // can't seem to find another client-friendly way
                            equipment++;
                            if (interactions[i].available) equipmentAvailable++;
                        }
                        else {
                            terminals++;
                            if (interactions[i].available) terminalsAvailable++;
                        }
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
                    // drones
                    case "TURRET1_INTERACTABLE_NAME":
                        gunnerTurrets++;
                        break;
                    case "DRONE_GUNNER_INTERACTABLE_NAME":
                        gunnerDrones++;
                        break;
                    case "DRONE_HEALING_INTERACTABLE_NAME":
                        healingDrones++;
                        break;
                    case "DRONE_MISSILE_INTERACTABLE_NAME":
                        missileDrones++;
                        break;
                    case "FLAMEDRONE_INTERACTABLE_NAME":
                        incineratorDrones++;
                        break;
                    case "EMERGENCYDRONE_INTERACTABLE_NAME":
                        emergencyDrones++;
                        break;
                    case "DRONE_MEGA_INTERACTABLE_NAME":
                        tc280Drones++;
                        break;
                    case "EQUIPMENTDRONE_INTERACTABLE_NAME":
                        equipmentDrones++;
                        break;
                    case "DRONE_HAULER_INTERACTABLE_NAME":
                        transportDrones++;
                        break;
                    case "DRONE_JUNK_INTERACTABLE_NAME":
                        junkDrones++;
                        break;
                    case "DRONE_RECHARGE_INTERACTABLE_NAME":
                        barrierDrones++;
                        break;
                    case "DRONE_CLEANUP_INTERACTABLE_NAME":
                        cleanupDrones++;
                        break;
                    case "DRONE_JAILER_INTERACTABLE_NAME":
                        jailerDrones++;
                        break;
                    case "DRONE_BOMBARDMENT_INTERACTABLE_NAME":
                        bombardmentDrones++;
                        break;
                    case "DRONE_COPYCAT_INTERACTABLE_NAME":
                        freezeDrones++;
                        break;
                    case "DRONE_VENDOR_TERMINAL_NAME":
                        droneTerminals++;
                        if (interactions[i].available) droneTerminalsAvailable++;
                        break;
                    case "DRONE_COMBINER_NAME":
                        droneCombinerPresent = true;
                        break;
                    case "DRONE_SCRAPPER_NAME":
                        droneScrapperPresent = true;
                        break;
                }
            }
        }
    }
}
