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

        internal static void Init(EquipmentIcon parent, GameObject target)
        {
            GameObject clone = Instantiate(target, parent.transform);
            CooldownPanel instance = clone.AddComponent<CooldownPanel>();
            instance.parent = parent;
            instance.cooldownRemapPanel = clone.GetComponent<RawImage>();
        }

        private void Update()
        {
            float alpha = 1;
            if (parent.hasEquipment) {
                float total = parent.targetEquipmentSlot._rechargeTime.t;
                if (total >= Mathf.Epsilon) {
                    alpha = 1 - parent.targetEquipmentSlot.cooldownTimer / total;
                }
            }
            cooldownRemapPanel.enabled = alpha < 1;
            cooldownRemapPanel.color = new Color(1, 1, 1, alpha);
        }
    }
}
