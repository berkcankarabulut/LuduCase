using System;
using CaseA.Character.Runtime;
using CaseA.Damage.Runtime;
using UnityEngine;

namespace CaseA.Skills.Runtime
{  
    public class EffectTickEventArgs : EventArgs
    { 
        public string EffectName { get; }
        public Targetable Target { get; }
        public GameObject Source { get; }
        public float RawAmount { get; }
        public float ActualAmount { get; }
        public float ResistanceValue { get; }
        public bool IsHealing { get; }
        public IDamageType DamageType { get; }
        
        public EffectTickEventArgs(string effectName, Targetable target, GameObject source, 
            float rawAmount, float actualAmount, float resistanceValue, 
            bool isHealing, IDamageType damageType)
        {
            EffectName = effectName;
            Target = target;
            Source = source;
            RawAmount = rawAmount;
            ActualAmount = actualAmount;
            ResistanceValue = resistanceValue;
            IsHealing = isHealing;
            DamageType = damageType;
        }
    }
}