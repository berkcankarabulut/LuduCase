using CaseA.Character.Runtime;
using UnityEngine;

namespace CaseA.Skills.Runtime
{ 
    [CreateAssetMenu(fileName = "NewHealOverTimeSkill", menuName = "Game/Combat/Skills/Heal Over Time Skill")]
    public class HealOverTimeSkill : SkillBase
    {
        [Header("Healing Properties")]
        [SerializeField, Range(0.1f, 50f)] private float _healPerTick = 3f;
        
        [Header("Effect Properties")]
        [SerializeField, Range(0.1f, 30f)] private float _effectDuration = 5f;
        [SerializeField, Range(0.1f, 5f)] private float _tickInterval = 0.5f;
         
        public override SkillResult Execute(Targetable target, GameObject source)
        {
            if (!CanExecute(target)) return SkillResult.Failed;
      
            target.Effects.ApplyHealOverTime(
                _healPerTick, 
                _effectDuration, 
                _tickInterval, 
                _skillName,   
                source
            );
      
            float totalHealing = (_effectDuration / _tickInterval) * _healPerTick;
      
            var result = new SkillResult
            {
                Success = true,
                Target = target,
                EffectApplied = _skillName,
                EffectDuration = _effectDuration,
                HealingDone = totalHealing   
            };
      
            SkillEvents.RaiseSkillExecuted(this, target, source, result);
    
            return result;
        }
    }
}