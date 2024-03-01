using BepInEx;
using HarmonyLib;
using RoR2;

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

        internal static event System.Action OnInteractionAttempted;
        private Scanner scanner;

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
            Run.onRunStartGlobal += OnRunStart;
            Run.onRunDestroyGlobal += OnRunDestroy;

            Log.Message("~enabled.");
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("CodeQuality", "IDE0051:Remove unused private members", Justification = "Unity Message")]
        private void OnDisable()
        {
            Run.onRunStartGlobal -= OnRunStart;
            Run.onRunDestroyGlobal -= OnRunDestroy;

            Log.Message("~disabled.");
        }


        private void OnRunStart(Run _) {
            scanner = new Scanner();
            scanner.Hook();
        }

        private void OnRunDestroy(Run _) {
            scanner.Unhook();
            scanner = null;
        }




        /// <remarks>
        /// Need to use a patch since <see cref="GlobalEventManager.OnInteractionsGlobal"/> is not called on clients.
        /// </remarks>
        [HarmonyPostfix, HarmonyPatch(typeof(Interactor), nameof(Interactor.AttemptInteraction))]
        private static void Interactor_AttemptInteraction() => OnInteractionAttempted?.Invoke();
    }
}
