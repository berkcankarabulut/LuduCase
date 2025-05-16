using System.Collections.Generic;
using CaseA.Character.Runtime;
using CaseA.Damage.Runtime;
using UnityEngine;

namespace CaseA.Skills.Runtime
{ 
    [CreateAssetMenu(fileName = "NewResistanceSkill", menuName = "Game/Combat/Skills/Resistance Skill")]
    public class ResistanceSkill : SkillBase
    {
        [SerializeField] private List<DamageTypeSO> _damageTypes = new List<DamageTypeSO>();
        [SerializeField, Range(0f, 1f)] private float _resistanceAmount = 0.5f;
        [SerializeField] private float _effectDuration = 5f;
         
        public override SkillResult Execute(Targetable target, GameObject source)
        {
            if (!CanExecute(target)) return SkillResult.Failed;
              
            if (_damageTypes.Count == 1)
            {  
                target.Effects.ApplySingleResistance(
                    _damageTypes[0], 
                    _resistanceAmount, 
                    _effectDuration, 
                    _skillName, 
                    source
                );
            }
            else if (_damageTypes.Count > 1)
            {  
                Dictionary<IDamageType, float> resistances = new Dictionary<IDamageType, float>();
                foreach (var damageType in _damageTypes)
                {
                    resistances.Add(damageType, _resistanceAmount);
                }
                
                target.Effects.ApplyMultipleResistances(
                    resistances, 
                    _effectDuration, 
                    _skillName, 
                    source
                );
            }
            else
            {
                return SkillResult.Failed;
            } 
              
            var result = new SkillResult
            {
                Success = true,
                Target = target,
                EffectApplied = _skillName,
                EffectDuration = _effectDuration
            };
              
            SkillEvents.RaiseSkillExecuted(this, target, source, result);
            
            return result;
        }
    }
}