using BepInEx.Bootstrap;
using Mono.Cecil.Cil;
using MonoMod.Cil;
using MonoMod.RuntimeDetour;
using System;
using System.Linq;
using System.Reflection;
using UnityEngine;

namespace HUDdleUP.Compatibility
{
    internal static class MiniMapMod
    {
        internal const string PLUGIN_GUID = "MiniMap";

        private const string TARGET_ASSEMBLY = "MiniMapMod";
        private const string TARGET_TYPE = "MiniMapMod.MiniMapPlugin";
        private const string TARGET_METHOD = "TryCreateMinimap";

        internal static void TryPatch()
        {
            if (!Chainloader.PluginInfos.ContainsKey(PLUGIN_GUID)) return;

            try {
                Assembly targetAssembly = AppDomain.CurrentDomain.GetAssemblies().SingleOrDefault(a => a.GetName().Name == TARGET_ASSEMBLY);
                Type targetType = targetAssembly?.GetType(TARGET_TYPE);
                MethodBase targetMethod = targetType?.GetMethod(TARGET_METHOD, BindingFlags.Instance | BindingFlags.NonPublic);

                ILHook hook = new(targetMethod, TryCreateMinimap_InSeparatePanel);
            }
            catch (Exception e) {
                Plugin.Logger.LogError(e);
            }
        }

        private static void TryCreateMinimap_InSeparatePanel(ILContext il)
        {
            ILCursor c = new(il);

            // GameObject val = GameObject.Find("ObjectivePanel");
            Func<Instruction, bool>[] match = {
                x => x.MatchLdstr("ObjectivePanel"),
                x => x.MatchCallOrCallvirt<GameObject>(nameof(GameObject.Find)),
                x => x.MatchStloc(0)
            };

            if (c.TryGotoNext(MoveType.After, match)) {
                c.Index--; // move to before stloc.0
                // Create minimap in clone of objective panel, instead of modifying objective panel directly.
                c.EmitDelegate<Func<GameObject, GameObject>>((objectivePanelGameObject) => {
                    if (objectivePanelGameObject == null) return null;

                    var objectivePanel = objectivePanelGameObject.GetComponent<RoR2.UI.ObjectivePanelController>();
                    HUDPanel newPanel = HUDPanel.ClonePanel(objectivePanel, "MinimapPanel");
                    return newPanel.gameObject;
                });
#if DEBUG
                Plugin.Logger.LogDebug(il.ToString());
#endif
            }
            else Plugin.Logger.LogError($"{nameof(Compatibility)}: {nameof(MiniMapMod)}> Cannot hook {TARGET_METHOD}: failed to match IL instructions.");
        }
    }
}
