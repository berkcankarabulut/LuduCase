using System;
using CaseA.Character.Runtime;
using CaseA.Damage.Runtime;
using UnityEngine;

namespace CaseA.Skills.Runtime
{  
    public static class SkillEvents
    { 
        public static event EventHandler<SkillEventArgs> OnSkillExecuted; 
        public static event EventHandler<SkillEventArgs> OnSkillCompleted;  
        public static event EventHandler<EffectTickEventArgs> OnEffectTick;
          
        public static void RaiseSkillExecuted(SkillBase skill, Targetable target, GameObject source, SkillResult result)
        {
            OnSkillExecuted?.Invoke(null, new SkillEventArgs(skill, target, source, result));
        } 

        public static void RaiseSkillCompleted(SkillBase skill, Targetable target, GameObject source, SkillResult result)
        {
            OnSkillCompleted?.Invoke(null, new SkillEventArgs(skill, target, source, result));
        }
         
        public static void RaiseEffectTick(string effectName, Targetable target, GameObject source, 
            float rawAmount, float actualAmount, float resistanceValue, 
            bool isHealing, IDamageType damageType)
        {
            OnEffectTick?.Invoke(null, new EffectTickEventArgs(
                effectName, target, source, rawAmount, actualAmount, resistanceValue, isHealing, damageType));
        }
    }
}