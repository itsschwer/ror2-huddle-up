namespace LootTip
{
    public static class Utils
    {
        public static RoR2.UI.TooltipProvider AddTooltipProvider(UnityEngine.UI.Graphic target)
        {
            if (target == null) return null;

            AddGraphicRaycasterToParentCanvas(target);
            target.raycastTarget = true;
            return AddComponentIfMissing<RoR2.UI.TooltipProvider>(target.gameObject);
        }

        public static bool AddGraphicRaycasterToParentCanvas(UnityEngine.Component child)
        {
            // Unity 2019.4 only allows finding components on active game objects...
            UnityEngine.GameObject parentCanvas = child.GetComponentInParent<UnityEngine.Canvas>().gameObject;
            Log.Debug($"Found parent canvas {parentCanvas.name} for {child.name}");
            return AddComponentIfMissing<UnityEngine.UI.GraphicRaycaster>(parentCanvas);
        }

        public static T AddComponentIfMissing<T>(UnityEngine.GameObject target) where T : UnityEngine.Component
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
