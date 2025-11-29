using RoR2.UI;
using UnityEngine;

namespace HUDdleUP.Loot
{
    internal sealed class InteractablesTracker : MonoBehaviour
    {
        private const float updateFrequency = 1f/30;
        private float lastUpdateTimestamp;


        private static HUD hud;

        public static void Hook() => HUD.shouldHudDisplay += Init;
        public static void Unhook() => HUD.shouldHudDisplay -= Init;

        private static void Init(HUD hud, ref bool _)
        {
            if (!Plugin.Config.TrackInteractables) return;
            if (InteractablesTracker.hud != null) return;

            InteractablesTracker.hud = hud;
            hud.gameObject.AddComponent<InteractablesTracker>();
            Plugin.Logger.LogDebug($"Initialized {nameof(InteractablesTracker)}.");
        }




        internal Interactables interactables;

        private void Update()
        {
            bool visible = hud.scoreboardPanel.activeSelf;
            if (!visible) return;

            float deltaTime = Time.unscaledTime - lastUpdateTimestamp;
            if (deltaTime < updateFrequency) return;
            lastUpdateTimestamp = Time.unscaledTime;

            interactables = new Interactables();
        }
    }
}
