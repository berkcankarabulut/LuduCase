using System.Collections.Generic; 
using CaseA.Character.Runtime;
using UnityEngine;

namespace CaseA.Skills.Runtime
{ 
    public class SkillManager : MonoBehaviour
    {
        [Header("Skills Configuration")]
        [SerializeField] private List<SkillBase> _availableSkills = new List<SkillBase>();
        
        [Header("Target")]
        [SerializeField] private GameObject _defaultTarget;
        
        [Header("Debug Info")]
        [SerializeField] private bool _showDebugInfo = true;
        [SerializeField] private string _lastAction = "None";
        
        private Targetable _currentTarget;
        private Dictionary<SkillBase, float> _skillCooldowns = new Dictionary<SkillBase, float>();
        
        private void Start()
        {
            if (_defaultTarget != null)
            {
                SetTarget(_defaultTarget.GetComponent<Targetable>());
            }
               
            foreach (var skill in _availableSkills)
            {
                _skillCooldowns[skill] = -999f;  
            }
        } 
         
        public void SetTarget(Targetable target)
        {
            _currentTarget = target;
        }
         
        public Targetable GetCurrentTarget()
        {
            return _currentTarget;
        }
         
        public bool IsSkillReady(int skillIndex)
        {
            if (skillIndex < 0 || skillIndex >= _availableSkills.Count) return false;
            
            var skill = _availableSkills[skillIndex];
            float lastUseTime = _skillCooldowns[skill];
            return Time.time >= lastUseTime + skill.Cooldown;
        }
         
        public void UseSkill(int skillIndex)
        {
            if (skillIndex < 0 || skillIndex >= _availableSkills.Count) return;
            if (_currentTarget == null) return;
            
            var skill = _availableSkills[skillIndex];
              
            if (!IsSkillReady(skillIndex))
            {
                if (_showDebugInfo)
                {
                    Debug.Log($"Skill {skill.SkillName} is on cooldown");
                }
                return;
            }
              
            var result = skill.Execute(_currentTarget, gameObject);
            
            if (result.Success)
            {  
                _skillCooldowns[skill] = Time.time;
                 
                UpdateLastActionFromResult(skill, result);
                
                if (_showDebugInfo)
                {
                    Debug.Log($"Used skill {skill.SkillName} on {_currentTarget.gameObject.name}: {_lastAction}");
                }
            }
            else if (_showDebugInfo)
            {
                Debug.LogWarning($"Failed to use skill {skill.SkillName} on {_currentTarget?.gameObject.name}");
            }
        }
         
        private void UpdateLastActionFromResult(SkillBase skill, SkillResult result)
        {
            if (result.DamageDealt > 0)
            {
                _lastAction = $"Used {skill.SkillName}: {result.DamageDealt:F1} damage" + 
                             (result.IsCritical ? " (CRITICAL!)" : "");
            }
            else if (result.HealingDone > 0)
            {
                _lastAction = $"Used {skill.SkillName}: Healed for {result.HealingDone:F1}";
            }
            else if (!string.IsNullOrEmpty(result.EffectApplied))
            {
                _lastAction = $"Applied {result.EffectApplied} for {result.EffectDuration:F1}s";
            }
            else
            {
                _lastAction = $"Used {skill.SkillName}";
            }
        }
         
        public string GetLastAction()
        {
            return _lastAction;
        }
    }
}