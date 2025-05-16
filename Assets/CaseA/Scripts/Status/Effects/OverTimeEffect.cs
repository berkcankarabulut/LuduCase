using System.Collections;
using UnityEngine;
using CaseA.Damage.Runtime;
using CaseA.Character.Runtime;
using System;

namespace CaseA.Status.Runtime
{ 
    public class OverTimeEffect : StatusEffectBase
    {
        public float AmountPerTick { get; private set; }
        public float TickInterval { get; private set; }
        public Guid? DamageTypeId { get; private set; }
        public IDamageType DamageType { get; private set; }
        public bool IsHealing { get; private set; }

        private Coroutine _tickCoroutine;
        private Targetable _targetObject;
 
        public OverTimeEffect(string name, float duration, float amountPerTick, float tickInterval,
            bool isHealing, Guid? damageTypeId = null, IDamageType damageType = null,
            GameObject source = null)
            : base(name, duration, source)
        {
            AmountPerTick = amountPerTick;
            TickInterval = tickInterval;
            DamageTypeId = damageTypeId;
            DamageType = damageType;
            IsHealing = isHealing;
        }
 
        public override void Apply(IEffectable target)
        {
            base.Apply(target);
             
            if (target is Targetable targetable)
            {
                _targetObject = targetable;
                
                if (IsPermanent || Duration > 0)
                {
                    _tickCoroutine = _targetObject.StartCoroutine(TickRoutine());
                }
            }
        }
 
        public override void Remove(IEffectable target)
        {
            base.Remove(target);
            
            if (_tickCoroutine != null && _targetObject != null)
            {
                _targetObject.StopCoroutine(_tickCoroutine);
                _tickCoroutine = null;
            }
        } 
        
        private IEnumerator TickRoutine()
        { 
            ApplyTick();
             
            while (IsActive && _targetObject != null)
            {
                yield return new WaitForSeconds(TickInterval);
                 
                if (IsActive)
                {
                    ApplyTick();
                }
            }
            
            _tickCoroutine = null;
        }
 
        private void ApplyTick()
        {
            if (_targetObject == null) return;
            
            if (IsHealing)
            { 
                _targetObject.Heal(AmountPerTick, Source);
            }
            else
            { 
                _targetObject.TakeDamage(AmountPerTick, DamageType, Source);
            }
        }
    }
}