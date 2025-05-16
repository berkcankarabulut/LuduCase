using CaseA.Character.Runtime;
using CaseA.Damage.Runtime; 
using UnityEngine;

namespace CaseA.Skills.Runtime
{ 
    [CreateAssetMenu(fileName = "NewDamageOverTimeSkill", menuName = "Game/Combat/Skills/Damage Over Time Skill")]
    public class DamageOverTimeSkill : SkillBase
    {
        [Header("Damage Properties")]
        [SerializeField] private DamageTypeSO _damageType;
        [SerializeField, Range(0.1f, 50f)] private float _damagePerTick = 3f;
        
        [Header("Effect Properties")]
        [SerializeField, Range(0.1f, 30f)] private float _effectDuration = 5f;
        [SerializeField, Range(0.1f, 5f)] private float _tickInterval = 0.5f;
        
        public DamageTypeSO DamageType => _damageType;
         
        public override SkillResult Execute(Targetable target, GameObject source)
        {
            if (!CanExecute(target)) return SkillResult.Failed;
              
            float resistanceValue = 0f;
            if (_damageType != null)
            {
                resistanceValue = target.GetResistance(_damageType);
            }
              
            target.Effects.ApplyDamageOverTime(
                _damagePerTick,
                _effectDuration,
                _tickInterval,
                _damageType,
                _skillName,
                source
            );
              
            float totalTickCount = _effectDuration / _tickInterval;
            float potentialTotalDamage = _damagePerTick * totalTickCount;
            float damageReduction = potentialTotalDamage * resistanceValue;
              
            var result = new SkillResult
            {
                Success = true,
                Target = target,
                EffectApplied = _skillName,
                EffectDuration = _effectDuration,
                DamageDealt = potentialTotalDamage,
                InitialDamage = potentialTotalDamage,
                ResistanceValue = resistanceValue,
                DamageReduction = damageReduction
            };
              
            SkillEvents.RaiseSkillExecuted(this, target, source, result);
            
            return result;
        }
    }
}