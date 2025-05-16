using CaseA.Character.Runtime; 
using UnityEngine;

namespace CaseA.Skills.Runtime
{
    public abstract class SkillBase : ScriptableObject
    {
        [SerializeField] protected string _skillName = "Unnamed Skill";
        [SerializeField] protected float _cooldown = 1;
        [SerializeField] protected Sprite _icon;
        
        public string SkillName => _skillName;
        public Sprite Icon => _icon;
        public float Cooldown => _cooldown;
        
        public abstract SkillResult Execute(Targetable target, GameObject source);
        public virtual bool CanExecute(Targetable target) 
        {
            return target != null && target.IsAlive;
        }
    }
}