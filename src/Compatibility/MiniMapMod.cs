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
    // Alternatively could just Destroy(transform.Find("Minimap")?.gameObject) in HUDPanel.ClonePanel() but that feels too vague.
    internal static class MiniMapMod
    {
        internal const string PLUGIN_GUID = "MiniMap";
        internal const string PLUGIN_GUID_2 = "com.minimapmod.rebalance"; // :(

        private const string TARGET_ASSEMBLY = "MiniMapMod";
        private const string TARGET_TYPE = "MiniMapMod.MiniMapPlugin";
        private const string TARGET_METHOD = "TryCreateMinimap";

        internal static void TryPatch()
        {
            if (!Chainloader.PluginInfos.ContainsKey(PLUGIN_GUID)
                && !Chainloader.PluginInfos.ContainsKey(PLUGIN_GUID_2)) return;

            try {
                var matches = AppDomain.CurrentDomain.GetAssemblies().Where(a => a.GetName().Name == TARGET_ASSEMBLY);
                foreach (Assembly targetAssembly in matches) {
#if DEBUG || true
                    string path = System.IO.Path.GetRelativePath(BepInEx.Paths.PluginPath, targetAssembly.Location);
                    Plugin.Logger.LogDebug($"{nameof(Compatibility)}: {nameof(MiniMapMod)}> Found assembly at {path}.");
#endif
                    Type targetType = targetAssembly?.GetType(TARGET_TYPE);
                    MethodBase targetMethod = targetType?.GetMethod(TARGET_METHOD, BindingFlags.Instance | BindingFlags.NonPublic);
                    ILHook hook = new(targetMethod, TryCreateMinimap_InSeparatePanel);
                }
            }
            catch (Exception e) {
                Plugin.Logger.LogError(e);
            }
        }

        private static void TryCreateMinimap_InSeparatePanel(ILContext il)
        {
            ILCursor c = new(il);

            // GameObject val = GameObject.Find("ObjectivePanel"); | GameObject val = ResolveMinimapParent();
            // if ((Object)(object)val == (Object)null || SpriteManager == null)
            int loc = -1;
            Func<Instruction, bool>[] match = {
                x => x.MatchStloc(out loc),
                // end of GameObject val assignment
                x => x.MatchLdloc(loc),
                x => x.MatchLdnull(),
                x => x.MatchCallOrCallvirt<UnityEngine.Object>("op_Equality")
            };

            if (c.TryGotoNext(MoveType.Before, match)) {
                // Create minimap in clone of objective panel, instead of modifying objective panel directly.
                c.EmitDelegate<Func<GameObject, GameObject>>((objectivePanelGameObject) => {
                    if (objectivePanelGameObject == null) return null;

                    Plugin.Logger.LogDebug($"{nameof(Compatibility)}: {nameof(MiniMapMod)}> Attempting to redirect minimap creation from {objectivePanelGameObject.name} to separate panel.");
                    try {
                        var objectivePanel = objectivePanelGameObject.GetComponent<RoR2.UI.ObjectivePanelController>();
                        HUDPanel newPanel = HUDPanel.ClonePanel(objectivePanel, "MinimapPanel");
                        if (newPanel.label)
                            GameObject.Destroy(newPanel.label.gameObject);
                        newPanel.gameObject.transform.SetAsFirstSibling();
                        return newPanel.gameObject;
                    }
                    catch (Exception e) {
                        Plugin.Logger.LogError(e);
                        Plugin.Logger.LogWarning($"{nameof(Compatibility)}: {nameof(MiniMapMod)}> Attempt to redirect minimap creation failed! Using original target instead. Other HUD panels may have non-functional copies of the minimap.");
                        return objectivePanelGameObject;
                    }
                });
#if DEBUG
                Plugin.Logger.LogDebug(il.ToString());
#endif
            }
            else Plugin.Logger.LogError($"{nameof(Compatibility)}: {nameof(MiniMapMod)}> Cannot hook {TARGET_METHOD}: failed to match IL instructions.");
        }
    }
}
