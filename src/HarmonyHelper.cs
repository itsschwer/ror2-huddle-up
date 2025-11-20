using HarmonyLib;
using System;
using System.Reflection;

namespace HUDdleUP
{
    internal static class HarmonyHelper
    {
        internal static void TryPatchAll(this Harmony harmony)
        {
            // https://github.com/BepInEx/HarmonyX/blob/v2.9.0/Harmony/Public/Harmony.cs#L143-L146
            Assembly assembly = Assembly.GetExecutingAssembly();
            foreach (Type type in AccessTools.GetTypesFromAssembly(assembly)) {
                try {
                    harmony.CreateClassProcessor(type).Patch();
                }
                catch (Exception e) {
                    Plugin.Logger.LogError(e);
                }
            }
        }
    }
}
