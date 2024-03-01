namespace LootObjectives
{
    public static class Utils
    {
        public static RoR2.UI.TooltipProvider AddTooltipProvider(UnityEngine.UI.Graphic target)
        {
            AddGraphicRaycasterToParentCanvas(target);
            target.raycastTarget = true;
            return AddComponentIfMissing<RoR2.UI.TooltipProvider>(target.gameObject);
        }

        public static bool AddGraphicRaycasterToParentCanvas(UnityEngine.Component child)
        {
            UnityEngine.GameObject parentCanvas = child.GetComponentInParent<UnityEngine.Canvas>()?.gameObject;
            return AddComponentIfMissing<UnityEngine.UI.GraphicRaycaster>(parentCanvas);
        }

        public static T AddComponentIfMissing<T>(UnityEngine.GameObject target) where T : UnityEngine.Component
        {
            if (target?.GetComponent<T>() == null) {
#if DEBUG
                UnityEngine.Debug.Log($"Added {typeof(T).FullName} to {target.name}");
#endif
                return target.AddComponent<T>();
            }
            return null;
        }
    }
}
