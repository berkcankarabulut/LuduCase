#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;
using System.Collections.Generic;
using CaseA.Skills.Runtime;

namespace CaseA.CombatLog.Editor
{
    public class CombatLogWindow : EditorWindow
    {
        private Vector2 _scrollPosition;
        private List<LogEntry> _logEntries = new List<LogEntry>();
        private int _maxEntries = 100;
         
        private enum LogType
        {
            Skill,
            Damage,
            Healing,
            Effect,
            Tick,
            Completion
        }
         
        private class LogEntry
        {
            public string Message;
            public System.DateTime Timestamp;
            public LogType Type;
            
            public LogEntry(string message, LogType type)
            {
                Message = message;
                Timestamp = System.DateTime.Now;
                Type = type;
            }
        }
        
        [MenuItem("Game/Combat Log")]
        public static void ShowWindow()
        {
            GetWindow<CombatLogWindow>("Combat Log");
        }
        
        private void OnEnable()
        {
            SkillEvents.OnSkillExecuted += HandleSkillExecuted;
            SkillEvents.OnEffectTick += HandleEffectTick; 
            Application.logMessageReceived += OnLogMessageReceived;
        }

        private void OnDisable()
        {
            SkillEvents.OnSkillExecuted -= HandleSkillExecuted;
            SkillEvents.OnEffectTick -= HandleEffectTick;
             
            Application.logMessageReceived -= OnLogMessageReceived;
        } 

        private void OnLogMessageReceived(string condition, string stacktrace, UnityEngine.LogType type)
        { 
            if (condition.StartsWith("[Finished]"))
            {
                var entry = new LogEntry(condition, LogType.Completion);
                _logEntries.Add(entry);
                
                if (_logEntries.Count > _maxEntries)
                {
                    _logEntries.RemoveAt(0);
                } 
                Repaint();
            }
        }
        
        private void HandleSkillExecuted(object sender, SkillEventArgs e)
        { 
            string logMessage = CreateLogMessageFromSkillEvent(e);
            
            LogType logType = LogType.Skill;
            if (e.Result.DamageDealt > 0)
            {
                logType = LogType.Damage;
            }
            else if (e.Result.HealingDone > 0)
            {
                logType = LogType.Healing;
            }
            else if (!string.IsNullOrEmpty(e.Result.EffectApplied))
            {
                logType = LogType.Effect;
            }
            
            _logEntries.Add(new LogEntry(logMessage, logType));
             
            if (_logEntries.Count > _maxEntries)
            {
                _logEntries.RemoveAt(0);
            } 
            Repaint();
        }
        
        private void HandleEffectTick(object sender, EffectTickEventArgs e)
        {
            string logMessage = $"[Tick] {e.EffectName} → {e.Target?.gameObject.name ?? "Unknown"}: ";
            
            if (e.IsHealing)
            {
                logMessage += $"Healed for {e.ActualAmount:F1}";
            }
            else
            {
                string damageTypeText = e.DamageType != null ? e.DamageType.Name + " damage" : "damage";
                if (e.ResistanceValue > 0)
                {
                    logMessage += $"{e.ActualAmount:F1} {damageTypeText} (Resisted {e.ResistanceValue * 100:F0}%)";
                }
                else
                {
                    logMessage += $"{e.ActualAmount:F1} {damageTypeText}";
                }
            }
            
            _logEntries.Add(new LogEntry(logMessage, LogType.Tick));
            
            if (_logEntries.Count > _maxEntries)
            {
                _logEntries.RemoveAt(0);
            }
            Repaint();
        }
        
        private string CreateLogMessageFromSkillEvent(SkillEventArgs e)
        {
            var skill = e.Skill;
            var result = e.Result;
            var target = e.Target?.gameObject.name ?? "Unknown";
            
            if (result.DamageDealt > 0)
            {
                string criticalText = result.IsCritical ? " (CRITICAL)" : "";
                
                if (result.ResistanceValue > 0)
                {
                    return $"{skill.SkillName}{criticalText} → {target}: {result.DamageDealt:F1} damage " +
                           $"(Resisted {result.ResistanceValue * 100:F0}%, Reduced by {result.DamageReduction:F1})";
                }
                else
                {
                    return $"{skill.SkillName}{criticalText} → {target}: {result.DamageDealt:F1} damage";
                }
            }
            else if (result.HealingDone > 0)
            {
                return $"{skill.SkillName} → {target}: +{result.HealingDone:F1} healing";
            }
            else if (!string.IsNullOrEmpty(result.EffectApplied))
            {
                return $"{skill.SkillName} → {target}: Applied {result.EffectApplied} for {result.EffectDuration:F1}s";
            }
            
            return $"{skill.SkillName} → {target}: Used skill";
        }
        
        private void OnGUI()
        {
            EditorGUILayout.BeginHorizontal(EditorStyles.toolbar);
            
            if (GUILayout.Button("Clear", EditorStyles.toolbarButton))
            {
                _logEntries.Clear();
            }
            
            EditorGUILayout.EndHorizontal();
            
            _scrollPosition = EditorGUILayout.BeginScrollView(_scrollPosition);
            
            foreach (var entry in _logEntries)
            {
                GUIStyle style = new GUIStyle(EditorStyles.label);
                
                switch (entry.Type)
                {
                    case LogType.Damage:
                        style.normal.textColor = Color.red;
                        break;
                    case LogType.Healing:
                        style.normal.textColor = Color.green;
                        break;
                    case LogType.Effect:
                        style.normal.textColor = new Color(0.7f, 0.4f, 1f);  
                        break;
                    case LogType.Tick:
                        style.normal.textColor = new Color(0.7f, 0.7f, 0.3f); 
                        break;
                    case LogType.Completion:
                        style.normal.textColor = new Color(0.3f, 0.6f, 1f); 
                        style.fontStyle = FontStyle.Bold;
                        break;
                }
                
                EditorGUILayout.LabelField($"[{entry.Timestamp.ToString("HH:mm:ss")}] {entry.Message}", style);
            }
            
            EditorGUILayout.EndScrollView();
        }
    }
}
#endif