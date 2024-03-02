using BepInEx;
using HarmonyLib;

namespace LootObjectives
{
    [HarmonyPatch]
    [BepInPlugin(GUID, Name, Version)]

    public sealed class Plugin : BaseUnityPlugin
    {
        public const string GUID = Author + "." + Name;
        public const string Author = "itsschwer";
        public const string Name = "LootObjectives";
        public const string Version = "0.0.0";

        private static Scanner scanner;

        [System.Diagnostics.CodeAnalysis.SuppressMessage("CodeQuality", "IDE0051:Remove unused private members", Justification = "Unity Message")]
        private void Awake()
        {
            Log.Init(Logger);
            new Harmony(Info.Metadata.GUID).PatchAll();

            Log.Message("~awake.");
        }

        /// <summary>
        /// All plugins are attached to the
        /// <see href="https://github.com/BepInEx/BepInEx/blob/0d06996b52c0215a8327b8c69a747f425bbb0023/BepInEx/Bootstrap/Chainloader.cs#L88">same</see>
        /// <see cref="UnityEngine.GameObject"/>, so manually manage components instead of calling <see cref="UnityEngine.GameObject.SetActive"/>.
        /// </summary>
        public void SetActive(bool value)
        {
            this.enabled = value;
            Log.Message($"~{(value ? "active" : "inactive")}.");
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("CodeQuality", "IDE0051:Remove unused private members", Justification = "Unity Message")]
        private void OnEnable()
        {
            RoR2.Run.onRunStartGlobal += OnRunStart;
            RoR2.Run.onRunDestroyGlobal += OnRunDestroy;

            Log.Message("~enabled.");
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("CodeQuality", "IDE0051:Remove unused private members", Justification = "Unity Message")]
        private void OnDisable()
        {
            RoR2.Run.onRunStartGlobal -= OnRunStart;
            RoR2.Run.onRunDestroyGlobal -= OnRunDestroy;

            Log.Message("~disabled.");
        }


        private void OnRunStart(RoR2.Run _) {
            scanner = new Scanner();
            scanner.Hook();
        }

        private void OnRunDestroy(RoR2.Run _) {
            scanner.Unhook();
            scanner = null;
        }




        /// <remarks>
        /// Need to use a patch since <see cref="RoR2.GlobalEventManager.OnInteractionsGlobal"/> is not called on clients.
        /// </remarks>
        [HarmonyPostfix, HarmonyPatch(typeof(RoR2.UI.TooltipController), nameof(RoR2.UI.TooltipController.SetTooltipProvider))]
        private static void TooltipController_SetTooltipProvider(RoR2.UI.TooltipController __instance, RoR2.UI.TooltipProvider provider) {
            if (provider.titleToken != Scanner.TOOLTIP_TITLE_TOKEN) return;

            __instance.titleLabel.text = Scanner.TOOLTIP_TITLE;
            __instance.bodyLabel.text = scanner.Scan().GetTooltipString();
        }
    }
}
