using CaseA.Character.Runtime;
using CaseA.Damage.Runtime; 
using UnityEngine;

namespace CaseA.Skills.Runtime
{ 
    [CreateAssetMenu(fileName = "NewDamageSkill", menuName = "Game/Combat/Skills/Direct Damage Skill")]
    public class DamageSkill : SkillBase
    {
        [Header("Damage Properties")]
        [SerializeField] private DamageTypeSO _damageType;
        [SerializeField, Range(1f, 100f)] private float _damageAmount = 10f;
        [SerializeField] private bool _canCrit = true;
        [SerializeField, Range(0f, 1f)] private float _critChance = 0.15f;
        [SerializeField, Range(1f, 5f)] private float _critMultiplier = 2f;
        
        public DamageTypeSO DamageType => _damageType;
         
        public override SkillResult Execute(Targetable target, GameObject source)
        {
            if (!CanExecute(target)) return SkillResult.Failed;
             
            float initialDamage = _damageAmount;
              
            bool isCritical = false;
            if (_canCrit && Random.value < _critChance)
            {
                initialDamage *= _critMultiplier;
                isCritical = true;
            }
              
            float resistanceValue = 0f;
            if (_damageType != null)
            {
                resistanceValue = target.GetResistance(_damageType);
            }
             
            float actualDamage = initialDamage * (1f - resistanceValue);
            float damageReduction = initialDamage - actualDamage;
              
            target.TakeDamage(actualDamage, _damageType, source);
              
            var result = new SkillResult
            {
                Success = true,
                Target = target,
                DamageDealt = actualDamage,
                IsCritical = isCritical,
                InitialDamage = initialDamage,
                ResistanceValue = resistanceValue,
                DamageReduction = damageReduction
            };
              
            SkillEvents.RaiseSkillExecuted(this, target, source, result);
            
            return result;
        }
    }
}