namespace HUDdleUP
{
    public static class TooltipHelper
    {
        public static RoR2.UI.TooltipProvider AddTooltipProvider(UnityEngine.UI.Graphic target, bool suppressLogs = false)
        {
            if (target == null) return null;

            target.AddGraphicRaycasterToParentCanvas(!suppressLogs);
            target.raycastTarget = true;
            return target.gameObject.AddComponentIfMissing<RoR2.UI.TooltipProvider>(!suppressLogs);
        }

        private static bool AddGraphicRaycasterToParentCanvas(this UnityEngine.Component child, bool emitLog)
        {
            // Unity 2019.4 only allows finding components on active game objects...
            UnityEngine.GameObject parentCanvas = child.GetComponentInParent<UnityEngine.Canvas>().gameObject;
            if (emitLog) Plugin.Logger.LogDebug($"Found parent canvas {parentCanvas.name} for {child.name}");
            return parentCanvas.AddComponentIfMissing<UnityEngine.UI.GraphicRaycaster>(emitLog);
        }

        private static T AddComponentIfMissing<T>(this UnityEngine.GameObject target, bool emitLog) where T : UnityEngine.Component
        {
            T current = target.GetComponent<T>();
            if (current == null) {
                current = target.AddComponent<T>();
                if (emitLog) Plugin.Logger.LogDebug($"Added {typeof(T).FullName} to {target.name}");
            }
            return current;
        }
    }
}
