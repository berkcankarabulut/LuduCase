using CaseA.Character.Runtime;
using UnityEngine;

namespace CaseA.Skills.Runtime
{ 
    [CreateAssetMenu(fileName = "NewHealthModifierSkill", menuName = "Game/Combat/Skills/Health Modifier Skill")]
    public class HealthModifierSkill : SkillBase
    {
        [SerializeField] private float _maxHealthModifier = 0f;
        [SerializeField] private float _regenerationRateModifier = 0f;
        [SerializeField] private float _effectDuration = 5f;
 
        public override SkillResult Execute(Targetable target, GameObject source)
        {
            if (!CanExecute(target)) return SkillResult.Failed;
              
            string effectDescription = "";
            if (_maxHealthModifier != 0)
                effectDescription += $"Health {(_maxHealthModifier > 0 ? "+" : "")}{_maxHealthModifier} ";
            if (_regenerationRateModifier != 0)
                effectDescription += $"Regen {(_regenerationRateModifier > 0 ? "+" : "")}{_regenerationRateModifier}/s";
              
            target.Effects.ApplyHealthModifier(
                _maxHealthModifier,
                _regenerationRateModifier,
                _effectDuration,
                _skillName,
                source
            ); 
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