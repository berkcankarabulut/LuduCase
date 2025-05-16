using System.Collections.Generic;
using UnityEngine;
using CaseA.Damage.Runtime;
using CaseA.Status.Runtime;

namespace CaseA.Character.Runtime
{ 
    public class CharacterEffectService
    {
        private StatusEffectFactory _factory;
        private StatusEffectHandler _handler;
        private GameObject _owner;
        
        public CharacterEffectService(Targetable targetable)
        {
            _factory = new StatusEffectFactory();
            _handler = new StatusEffectHandler(targetable);
            _owner = targetable.gameObject;
        }
        
        #region Health Modifier Effects
         
        public void ApplyHealthModifier(
            float maxHealthModifier, 
            float regenerationRateModifier, 
            float duration,
            string effectName = "HealthBoost", 
            GameObject source = null)
        {
            var effect = _factory.CreateHealthModifier(
                effectName,
                duration,
                maxHealthModifier,
                regenerationRateModifier,
                source ?? _owner
            );
            
            _handler.AddStatusEffect(effect);
        }
        
        #endregion
        
        #region Over Time Effects
         
        public void ApplyDamageOverTime(
            float damagePerTick, 
            float duration, 
            float tickInterval,
            IDamageType damageType, 
            string effectName, 
            GameObject source = null)
        {
            if (damageType == null)
            {
                Debug.LogWarning("Damage type not specified. Effect could not be applied.");
                return;
            }
            
            var effect = _factory.CreateDamageOverTime(
                effectName,
                duration,
                damagePerTick,
                tickInterval,
                damageType,
                source ?? _owner
            );
            
            _handler.AddStatusEffect(effect);
        }
         
        public void ApplyHealOverTime(
            float healPerTick, 
            float duration, 
            float tickInterval = 0.5f,
            string effectName = "HealOverTime", 
            GameObject source = null)
        {
            var effect = _factory.CreateHealOverTime(
                effectName,
                duration,
                healPerTick,
                tickInterval,
                source ?? _owner
            );
            
            _handler.AddStatusEffect(effect);
        }
        
        #endregion
        
        #region Resistance Effects
         
        public void ApplySingleResistance(
            IDamageType damageType, 
            float resistanceAmount, 
            float duration,
            string effectName = null, 
            GameObject source = null)
        {
            if (damageType == null)
            {
                Debug.LogWarning("Damage type not specified. Resistance effect could not be applied.");
                return;
            }
            
            if (string.IsNullOrEmpty(effectName))
            {
                effectName = $"{damageType.Name}Resistance";
            }
            
            var effect = _factory.CreateSingleResistance(
                effectName,
                duration,
                damageType,
                resistanceAmount,
                source ?? _owner
            );
            
            _handler.AddStatusEffect(effect);
        }
         
        public void ApplyMultipleResistances(
            Dictionary<IDamageType, float> resistances, 
            float duration,
            string effectName = "MultiResistance", 
            GameObject source = null)
        {
            if (resistances == null || resistances.Count == 0)
            {
                Debug.LogWarning("Resistance table is empty or invalid. Resistance effect could not be applied.");
                return;
            }
            
            var effect = _factory.CreateMultiResistance(
                effectName,
                duration,
                resistances,
                source ?? _owner
            );
            
            _handler.AddStatusEffect(effect);
        }
        
        #endregion
        
        #region Effect Management
         
        public void RemoveEffect(string effectName)
        {
            _handler.RemoveStatusEffect(effectName);
        }
         
        public void RemoveAllEffects()
        {
            _handler.RemoveAllStatusEffects();
        }
         
        public void AddEffect(IStatusEffect effect)
        {
            _handler.AddStatusEffect(effect);
        }
        
        #endregion
    }
}