using System.Collections.Generic;
using UnityEngine;
using CaseA.Damage.Runtime;

namespace CaseA.Status.Runtime
{
    public class StatusEffectFactory
    {
        public HealthModifierEffect CreateHealthModifier(
            string name,
            float duration,
            float maxHealthModifier,
            float regenerationRateModifier,
            GameObject source = null)
        {
            return new HealthModifierEffect(
                name,
                duration,
                maxHealthModifier,
                regenerationRateModifier,
                source);
        }

        public OverTimeEffect CreateDamageOverTime(
            string name,
            float duration,
            float damagePerTick,
            float tickInterval,
            IDamageType damageType,
            GameObject source = null)
        {
            return new OverTimeEffect(
                name,
                duration,
                damagePerTick,
                tickInterval,
                false,
                damageType?.TypeId,
                damageType,
                source);
        }

        public OverTimeEffect CreateHealOverTime(
            string name,
            float duration,
            float healPerTick,
            float tickInterval,
            GameObject source = null)
        {
            return new OverTimeEffect(
                name,
                duration,
                healPerTick,
                tickInterval,
                true,
                null,
                null,
                source);
        }

        public ResistanceModifierEffect CreateSingleResistance(
            string name,
            float duration,
            IDamageType damageType,
            float resistanceAmount,
            GameObject source = null)
        {
            Dictionary<IDamageType, float> resistances = new Dictionary<IDamageType, float>
            {
                { damageType, resistanceAmount }
            };

            return new ResistanceModifierEffect(
                name,
                duration,
                resistances,
                source);
        }

        public ResistanceModifierEffect CreateMultiResistance(
            string name,
            float duration,
            Dictionary<IDamageType, float> resistances,
            GameObject source = null)
        {
            return new ResistanceModifierEffect(
                name,
                duration,
                resistances,
                source);
        }
    }
}