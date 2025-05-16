using CaseA.Skills.Runtime; 
using UnityEngine;
using System.Collections.Generic;

namespace CaseA.CombatLog.Runtime
{ 
    public class CombatLogger : MonoBehaviour
    {
        [SerializeField] private bool _logToConsole = true; 
        private Dictionary<string, SkillTrackingInfo> _activeSkills = new Dictionary<string, SkillTrackingInfo>();
         
        private class SkillTrackingInfo
        {
            public string SkillName;
            public string TargetName;
            public float RemainingTime;
            public bool IsOverTimeEffect;
            
            public SkillTrackingInfo(string skillName, string targetName, float duration, bool isOverTimeEffect)
            {
                SkillName = skillName;
                TargetName = targetName;
                RemainingTime = duration;
                IsOverTimeEffect = isOverTimeEffect;
            }
        }
        
        private void OnEnable()
        {
            SkillEvents.OnSkillExecuted += HandleSkillExecuted;
            SkillEvents.OnEffectTick += HandleEffectTick;
        }
        
        private void OnDisable()
        {
            SkillEvents.OnSkillExecuted -= HandleSkillExecuted;
            SkillEvents.OnEffectTick -= HandleEffectTick;
        }
        
        private void Update()
        { 
            List<string> finishedSkills = new List<string>();
            
            foreach (var pair in _activeSkills)
            {
                var info = pair.Value;
                info.RemainingTime -= Time.deltaTime;
                
                if (info.RemainingTime <= 0)
                {
                    if (_logToConsole)
                    {
                        if (info.IsOverTimeEffect)
                        {
                            Debug.Log($"[Finished] {info.SkillName} - Over-time effect has expired on {info.TargetName}");
                        }
                        else
                        {
                            Debug.Log($"[Finished] {info.SkillName} - Skill execution completed");
                        }
                    }
                    finishedSkills.Add(pair.Key);
                }
            }
             
            foreach (var key in finishedSkills)
            {
                _activeSkills.Remove(key);
            }
        }
         
        private void HandleSkillExecuted(object sender, SkillEventArgs e)
        {
            if (!_logToConsole) return;
            
            var skill = e.Skill;
            var result = e.Result;
            var target = e.Target?.gameObject.name ?? "Unknown";
             
            if (result.DamageDealt > 0)
            {
                LogDamageSkill(skill, result, target);
            } 
            else if (result.HealingDone > 0)
            {
                LogHealingSkill(skill, result, target);
            } 
            else if (!string.IsNullOrEmpty(result.EffectApplied))
            {
                LogEffectSkill(skill, result, target);
            }
             
            if (!string.IsNullOrEmpty(result.EffectApplied) && result.DamageDealt > 0)
            {
                LogDamageOverTimeSkill(skill, result, target);
            }
             
            if (result.EffectDuration > 0 && !string.IsNullOrEmpty(result.EffectApplied))
            {
                string trackingKey = $"{result.EffectApplied}_{target}";
                _activeSkills[trackingKey] = new SkillTrackingInfo(
                    skill.SkillName, 
                    target, 
                    result.EffectDuration,
                    !string.IsNullOrEmpty(result.EffectApplied)
                );
            }
            else if (skill is DamageSkill || skill is HealingSkill)
            { 
                Debug.Log($"[Finished] {skill.SkillName} - Direct skill execution completed on {target}");
            }
        } 
        
        private void HandleEffectTick(object sender, EffectTickEventArgs e)
        {
            if (!_logToConsole) return;
            
            var effectName = e.EffectName;
            var target = e.Target?.gameObject.name ?? "Unknown";
            var isHealing = e.IsHealing;
            
            if (isHealing)
            {
                Debug.Log($"[Tick] {effectName} - Healed {target} for {e.ActualAmount:F1}");
            }
            else
            {
                string damageTypeText = e.DamageType != null ? e.DamageType.Name + " damage" : "damage";
                
                if (e.ResistanceValue > 0)
                {
                    Debug.Log($"[Tick] {effectName} - Raw: {e.RawAmount:F1}, Resist: {e.ResistanceValue * 100:F0}%, Final: {e.ActualAmount:F1} {damageTypeText}");
                }
                else
                {
                    Debug.Log($"[Tick] {effectName} - Damage: {e.ActualAmount:F1} {damageTypeText}");
                }
            }
             
            string trackingKey = $"{effectName}_{target}";
            if (_activeSkills.ContainsKey(trackingKey))
            { 
                var info = _activeSkills[trackingKey]; 
            }
        }
         
        private void LogDamageSkill(SkillBase skill, SkillResult result, string target)
        {
            string criticalText = result.IsCritical ? " (CRITICAL HIT!)" : "";
            string damageTypeText = "damage";
             
            if (skill is DamageSkill damageSkill && damageSkill.DamageType != null)
            {
                damageTypeText = damageSkill.DamageType.Name + " damage";
            }
            
            if (result.ResistanceValue > 0)
            {
                Debug.Log($"{skill.SkillName}{criticalText} - Initial damage: {result.InitialDamage:F1} {damageTypeText}");
                Debug.Log($"Target resisted {result.ResistanceValue * 100:F0}% - Damage reduced by {result.DamageReduction:F1} points");
                Debug.Log($"Final damage: {result.DamageDealt:F1} {damageTypeText}");
            }
            else
            {
                Debug.Log($"{skill.SkillName}{criticalText} - Damage: {result.DamageDealt:F1} {damageTypeText}");
            }
        }
         
        private void LogHealingSkill(SkillBase skill, SkillResult result, string target)
        {
            Debug.Log($"{skill.SkillName} - Healed {target} for {result.HealingDone:F1}");
        }
         
        private void LogEffectSkill(SkillBase skill, SkillResult result, string target)
        {
            Debug.Log($"{skill.SkillName} - Applied {result.EffectApplied} to {target} for {result.EffectDuration:F1}s");
        }
         
        private void LogDamageOverTimeSkill(SkillBase skill, SkillResult result, string target)
        {
            string resistanceText = "";
            if (result.ResistanceValue > 0)
            {
                resistanceText = $" (Current resistance: {result.ResistanceValue * 100:F0}%)";
            }

            Debug.Log($"{skill.SkillName} - Applied {result.EffectApplied} to {target} for {result.EffectDuration:F1}s");
            Debug.Log($"Potential damage: ~{result.DamageDealt:F1} over duration{resistanceText}");
            Debug.Log($"Note: Actual damage will be calculated per tick based on current resistances");
        }
    }
}