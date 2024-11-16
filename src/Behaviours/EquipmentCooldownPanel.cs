using RoR2;
using RoR2.UI;
using UnityEngine;
using UnityEngine.UI;

namespace HUDdleUP.Behaviours
{
    internal class EquipmentCooldownPanel : MonoBehaviour
    {
        private static float LastLogTime; // To prevent spam (Starstorm2 instantiates and destroys 16 times for Composite Injector on HUD.Awake)

        private EquipmentIcon parent;
        private RawImage cooldownRemapPanel;
        private float cooldownTimerMax;

        internal static void Init(EquipmentIcon parent, GameObject target)
        {
            GameObject clone = Instantiate(target, parent.transform);
            EquipmentCooldownPanel instance = clone.AddComponent<EquipmentCooldownPanel>();
            instance.parent = parent;
            instance.cooldownRemapPanel = clone.GetComponent<RawImage>();
            ((RectTransform)instance.cooldownRemapPanel.transform).sizeDelta = ((RectTransform)parent.transform).sizeDelta;
        }

        private void Update()
        {
            if (parent == null) {
                // TeamMoonstorm-Starstorm2 has a red item "Composite Injector" that instantiates additional EquipmentIcons (from prefab), then destroys and replaces them with custom component "EquipmentIconButEpic"(??)
                float time = Time.time;
                if (!Mathf.Approximately(LastLogTime, time)) {
                    Plugin.Logger.LogWarning($"{nameof(EquipmentCooldownPanel)}> Component instantiated but parent became null, removing self. This warning can safely be ignored if Starstorm 2 is installed.");
                    LastLogTime = time;
                }
                Destroy(this.gameObject);
                return;
            }

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
