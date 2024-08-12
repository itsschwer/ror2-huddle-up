﻿namespace LootTip
{
    public static class Utils
    {
        public static RoR2.UI.TooltipProvider AddTooltipProvider(UnityEngine.UI.Graphic target)
        {
            if (target == null) return null;

            target.AddGraphicRaycasterToParentCanvas();
            target.raycastTarget = true;
            return target.gameObject.AddComponentIfMissing<RoR2.UI.TooltipProvider>();
        }

        private static bool AddGraphicRaycasterToParentCanvas(this UnityEngine.Component child)
        {
            // Unity 2019.4 only allows finding components on active game objects...
            UnityEngine.GameObject parentCanvas = child.GetComponentInParent<UnityEngine.Canvas>().gameObject;
            Log.Debug($"Found parent canvas {parentCanvas.name} for {child.name}");
            return parentCanvas.AddComponentIfMissing<UnityEngine.UI.GraphicRaycaster>();
        }

        private static T AddComponentIfMissing<T>(this UnityEngine.GameObject target) where T : UnityEngine.Component
        {
            T current = target.GetComponent<T>();
            if (current == null) {
                current = target.AddComponent<T>();
                Log.Debug($"Added {typeof(T).FullName} to {target.name}");
            }
            return current;
        }
    }
}
