using CaseA.Character.Runtime;
using UnityEngine;

namespace CaseA.Skills.Runtime
{ 
    [CreateAssetMenu(fileName = "NewHealingSkill", menuName = "Game/Combat/Skills/Healing Skill")]
    public class HealingSkill : SkillBase
    {
        [SerializeField, Range(1f, 100f)] private float _healAmount = 10f;
         
        public override SkillResult Execute(Targetable target, GameObject source)
        {
            if (!CanExecute(target)) return SkillResult.Failed;
             
            target.Heal(_healAmount, source);
             
            return new SkillResult
            {
                Success = true,
                Target = target,
                HealingDone = _healAmount
            };
        }
    }
}