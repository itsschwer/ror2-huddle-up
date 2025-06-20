using BepInEx;
using HarmonyLib;

namespace HUDdleUP
{
    // hack; would prefer changing original mod but no public repository
    [BepInDependency(Compatibility.MiniMapMod.PLUGIN_GUID, BepInDependency.DependencyFlags.SoftDependency)]

    [BepInPlugin(GUID, Name, Version)]
    public sealed class Plugin : BaseUnityPlugin
    {
        public const string GUID = Author + "." + Name;
        public const string Author = "itsschwer";
        public const string Name = "HUDdleUP";
        public const string Version = "1.1.1";

        internal static new BepInEx.Logging.ManualLogSource Logger { get; private set; }

        public static new Config Config { get; private set; }

        private void Awake()
        {
            // Use Plugin.GUID instead of Plugin.Name as source name
            BepInEx.Logging.Logger.Sources.Remove(base.Logger);
            Logger = BepInEx.Logging.Logger.CreateLogSource(Plugin.GUID);

            Config = new Config(base.Config);

            new Harmony(Info.Metadata.GUID).PatchAll();

            Compatibility.MiniMapMod.TryPatch();

            Logger.LogMessage("~awake.");
        }

        private void OnEnable()
        {
            RoR2.Run.onRunStartGlobal += OnRunStart;
            RoR2.Run.onRunDestroyGlobal += OnRunDestroy;

            Logger.LogMessage("~enabled.");
        }

        private void OnDisable()
        {
            RoR2.Run.onRunStartGlobal -= OnRunStart;
            RoR2.Run.onRunDestroyGlobal -= OnRunDestroy;

            Logger.LogMessage("~disabled.");
        }


        private void OnRunStart(RoR2.Run _) {
            Loot.LootPanel.Hook();
            Multiplayer.ConnectionPanel.Hook();
            Railgunner.RailgunnerAccuracyPanel.Hook();
            Bandit.BanditComboPanel.Hook();
        }

        private void OnRunDestroy(RoR2.Run _) {
            Loot.LootPanel.Unhook();
            Multiplayer.ConnectionPanel.Unhook();
            Railgunner.RailgunnerAccuracyPanel.Unhook();
            Bandit.BanditComboPanel.Unhook();
        }
    }
}
