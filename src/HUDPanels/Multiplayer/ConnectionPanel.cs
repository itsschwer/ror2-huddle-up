using RoR2;
using RoR2.Networking;
using RoR2.UI;
using UnityEngine;
using UnityEngine.Networking;

namespace HUDdleUP.Multiplayer
{
    internal sealed class ConnectionPanel : MonoBehaviour
    {
        private const float updateFrequency = 1/5;
        private float lastUpdateTimestamp;


        private static HUD hud;

        public static void Hook() => HUD.shouldHudDisplay += Init;
        public static void Unhook() => HUD.shouldHudDisplay -= Init;

        public static void Init(HUD hud, ref bool _)
        {
            if (!Plugin.Config.MultiplayerConnectionPanel) return;
            if (NetworkUser.readOnlyInstancesList.Count <= 1) return;
            if (ConnectionPanel.hud != null) return;

            var objectivePanel = hud.GetComponentInChildren<ObjectivePanelController>();
            if (!objectivePanel) {
#if DEBUG
                Plugin.Logger.LogDebug($"Waiting to initialize {nameof(ConnectionPanel)}.");
#endif
                return;
            }

            ConnectionPanel.hud = hud;

            HUDPanel panel = HUDPanel.ClonePanel(objectivePanel, nameof(ConnectionPanel));
            hud.gameObject.AddComponent<ConnectionPanel>().panel = panel;
            Plugin.Logger.LogDebug($"Initialized {nameof(ConnectionPanel)}");
        }




        private HUDPanel panel;
        private TMPro.TextMeshProUGUI display;

        private void Start()
        {
            panel.label.text = "Connection:";
            display = panel.AddTextComponent("Multiplayer Connection");
        }

        private void Update()
        {
            // Scoreboard visibility logic from RoR2.UI.HUD.Update()
            bool visible = (hud.localUserViewer?.inputPlayer != null && hud.localUserViewer.inputPlayer.GetButton("info"));
            panel.gameObject.SetActive(visible);
            if (!visible) return;

            float deltaTime = Time.unscaledTime - lastUpdateTimestamp;
            if (deltaTime < updateFrequency) return;

            lastUpdateTimestamp = Time.unscaledTime;
            display.text = $"<style=cStack>> <style=cIsUtility>Ping (round-trip time)</style>:</style>\n{(NetworkServer.active ? GetPingHost() : GetPingClient())}";
        }

        private string GetPingClient()
        {
            int rttMs = -1;

            if (NetworkClient.active && NetworkManagerSystem.singleton) {
                NetworkConnection connection = NetworkManagerSystem.singleton.client.connection;
                if (connection != null) {
                    rttMs = (int)RttManager.GetConnectionRTTInMilliseconds(connection);
                }
            }

            return $"<style=cStack>   > You are client</style>\n<style=cStack>      > <style=cSub>{rttMs}</style> ms</style>";
        }

        private string GetPingHost()
        {
            System.Text.StringBuilder sb = new("<style=cStack>   > You are host</style>");

            foreach (NetworkUser user in NetworkUser.readOnlyInstancesList) {
                if (user && !user.hasAuthority) {
                    int rttMs = (user.connectionToClient != null) ? (int)RttManager.GetConnectionRTTInMilliseconds(user.connectionToClient) : -1;
                    sb.AppendLine().Append($"<style=cStack>   > <style=cUserSetting>{user.userName}</style>: <style=cSub>{rttMs}</style> ms</style>");
                }
            }

            return sb.ToString();
        }
    }
}
