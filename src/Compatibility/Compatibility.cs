﻿using RoR2;

namespace HUDdleUP.Compatibility
{
    internal static class Compatibility
    {
        private static System.Reflection.FieldInfo _DamageType_Field;
        internal static System.Reflection.FieldInfo DamageType_Field {
            get {
                if (_DamageType_Field == null) {
                    Plugin.Logger.LogWarning($"{nameof(System.MissingFieldException)}: Using Seekers of the Storm version of {nameof(DamageInfo)}.{nameof(DamageInfo.damageType)}");
                    // DamageInfo -> DamageTypeCombo -> DamageType
                    _DamageType_Field = DamageTypeCombo_Field.FieldType.GetField(nameof(DamageInfo.damageType));
                }
                return _DamageType_Field;
            }
        }

        private static System.Reflection.FieldInfo _DamageTypeCombo_Field;
        internal static System.Reflection.FieldInfo DamageTypeCombo_Field {
            get {
                if (_DamageTypeCombo_Field == null) {
                    _DamageTypeCombo_Field = typeof(DamageInfo).GetField(nameof(DamageInfo.damageType));
                }
                return _DamageTypeCombo_Field;
            }
        }



        internal static DamageType ExtractDamageType(DamageInfo damageInfo)
        {
            try {
                return ExtractDamageType_Old(damageInfo);
            }
            catch (System.MissingFieldException) {
                object combo = DamageTypeCombo_Field.GetValue(damageInfo);
                return (DamageType)DamageType_Field.GetValue(combo);
            }
        }

        // https://stackoverflow.com/questions/3546580/why-is-it-not-possible-to-catch-missingmethodexception/3546611#3546611
        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.NoInlining)]
        private static DamageType ExtractDamageType_Old(DamageInfo damageInfo)
        {
            return damageInfo.damageType;
        }
    }
}
