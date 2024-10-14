using BepInEx;
using HarmonyLib;

namespace HUDdleUP
{
    [HarmonyPatch]
    [BepInPlugin(GUID, Name, Version)]
    public sealed class Plugin : BaseUnityPlugin
    {
        public const string GUID = Author + "." + Name;
        public const string Author = "itsschwer";
        public const string Name = "HUDdleUP";
        public const string Version = "1.0.0";

        internal static new BepInEx.Logging.ManualLogSource Logger { get; private set; }

        public static new Config Config { get; private set; }

        private void Awake()
        {
            // Use Plugin.GUID instead of Plugin.Name as source name
            BepInEx.Logging.Logger.Sources.Remove(base.Logger);
            Logger = BepInEx.Logging.Logger.CreateLogSource(Plugin.GUID);

            Config = new Config(base.Config);

            new Harmony(Info.Metadata.GUID).PatchAll();

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
            Railgunner.RailgunnerAccuracyPanel.Hook();
        }

        private void OnRunDestroy(RoR2.Run _) {
            Loot.LootPanel.Unhook();
            Railgunner.RailgunnerAccuracyPanel.Unhook();
        }
    }
}
