using RoR2;
using RoR2.UI;
using UnityEngine;
using UnityEngine.UI;

namespace HUDdleUP.Behaviours
{
    internal class CooldownPanel : MonoBehaviour
    {
        private EquipmentIcon parent;
        private RawImage cooldownRemapPanel;
        private float cooldownTimerMax;

        internal static void Init(EquipmentIcon parent, GameObject target)
        {
            GameObject clone = Instantiate(target, parent.transform);
            CooldownPanel instance = clone.AddComponent<CooldownPanel>();
            instance.parent = parent;
            instance.cooldownRemapPanel = clone.GetComponent<RawImage>();
            ((RectTransform)instance.cooldownRemapPanel.transform).sizeDelta = ((RectTransform)parent.transform).sizeDelta;
        }

        private void Update()
        {
            float alpha = 1;
            if (parent.targetInventory) {
                EquipmentState state = (parent.displayAlternateEquipment ? parent.targetInventory.alternateEquipmentState : parent.targetInventory.currentEquipmentState);

                float cooldownTimer = state.chargeFinishTime.timeUntilClamped;
                if (cooldownTimer > cooldownTimerMax || float.IsInfinity(cooldownTimerMax)) cooldownTimerMax = cooldownTimer;
                if (cooldownTimerMax >= Mathf.Epsilon) {
                    alpha = 1 - (cooldownTimer / cooldownTimerMax);
                }
            }
            cooldownRemapPanel.enabled = alpha < 1;
            cooldownRemapPanel.color = new Color(1, 1, 1, alpha);
        }
    }
}
