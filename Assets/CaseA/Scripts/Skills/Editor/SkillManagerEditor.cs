#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;
using CaseA.Skills.Runtime;
using CaseA.Character.Runtime;

namespace CaseA.Skills.Editor
{ 
    [CustomEditor(typeof(SkillManager))]
    public class SkillManagerEditor : UnityEditor.Editor
    {
        private SkillManager _skillManager;

        private void OnEnable()
        {
            _skillManager = (SkillManager)target;
        }

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            if (_skillManager == null) return;

            EditorGUILayout.Space(10); 
            Targetable currentTarget = _skillManager.GetCurrentTarget();
            bool hasTarget = currentTarget != null;
 
            GUILayout.Label("Target Information", EditorStyles.boldLabel);
            if (hasTarget)
            {
                EditorGUILayout.LabelField("Current Target:", currentTarget.gameObject.name);
                EditorGUILayout.LabelField("Target Status:", currentTarget.IsAlive ? "Alive" : "Dead");
                EditorGUILayout.LabelField($"Target Health: {currentTarget.HealthPercentage * 100:F1}%");
                 
                Rect rect = EditorGUILayout.GetControlRect(false, 20);
                EditorGUI.DrawRect(rect, new Color(0.2f, 0.2f, 0.2f));

                if (currentTarget.IsAlive)
                {
                    float width = rect.width * currentTarget.HealthPercentage;
                    rect.width = width;
                    
                    Color healthColor = Color.green;
                    if (currentTarget.HealthPercentage < 0.3f)
                        healthColor = Color.red;
                    else if (currentTarget.HealthPercentage < 0.6f)
                        healthColor = Color.yellow;

                    EditorGUI.DrawRect(rect, healthColor);
                }
            }
            else
            {
                EditorGUILayout.LabelField("No target selected", EditorStyles.boldLabel);
                EditorGUILayout.HelpBox("Assign a target to use skills", MessageType.Info);
                
                if (GUILayout.Button("Find Target in Scene"))
                {
                    FindTargetInScene();
                }
                return;
            }
 
            string lastAction = _skillManager.GetLastAction();
            if (!string.IsNullOrEmpty(lastAction))
            {
                EditorGUILayout.Space(5);
                GUILayout.Label("Last Action", EditorStyles.boldLabel);
                
                GUIStyle style = new GUIStyle(EditorStyles.boldLabel);
                
                if (lastAction.Contains("damage") || lastAction.Contains("Damage") || lastAction.Contains("Kill"))
                {
                    style.normal.textColor = Color.red;
                }
                else if (lastAction.Contains("heal") || lastAction.Contains("Heal") || lastAction.Contains("Revive"))
                {
                    style.normal.textColor = Color.green;
                }
                
                EditorGUILayout.LabelField(lastAction, style);
            }

            EditorGUILayout.Space(10);
            GUILayout.Label("Available Skills", EditorStyles.boldLabel);
 
            var availableSkillsField = typeof(SkillManager).GetField("_availableSkills", 
                System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
            var availableSkills = availableSkillsField?.GetValue(_skillManager) as System.Collections.IList;

            if (availableSkills != null && availableSkills.Count > 0)
            {
                for (int i = 0; i < availableSkills.Count; i++)
                {
                    var skill = availableSkills[i] as ScriptableObject;
                    if (skill == null) continue;

                    EditorGUILayout.BeginHorizontal();
                     
                    Texture2D icon = null;
                    var iconProperty = skill.GetType().GetProperty("Icon");
                    if (iconProperty != null)
                    {
                        icon = iconProperty.GetValue(skill) as Texture2D;
                    }
                     
                    string skillName = skill.name;
                    var nameProperty = skill.GetType().GetProperty("SkillName");
                    if (nameProperty != null)
                    {
                        var name = nameProperty.GetValue(skill) as string;
                        if (!string.IsNullOrEmpty(name))
                        {
                            skillName = name;
                        }
                    }
                     
                    GUIContent buttonContent = new GUIContent(skillName, icon, $"Use skill: {skillName}");
                    
                    if (GUILayout.Button(buttonContent, GUILayout.Height(40)))
                    {
                        _skillManager.UseSkill(i);
                    }
                    
                    EditorGUILayout.EndHorizontal();
                }
            }
            else
            {
                EditorGUILayout.HelpBox("No skills available. Add skills in the inspector.", MessageType.Warning);
            }
 
            EditorGUILayout.Space(15);
            GUILayout.Label("Target Selection", EditorStyles.boldLabel);

            if (GUILayout.Button("Find New Target"))
            {
                FindTargetInScene();
            }

            if (EditorApplication.isPlaying)
            {
                Repaint();
            }
        }
 
        private void FindTargetInScene()
        {  
            Targetable[] targets = Object.FindObjectsOfType<Targetable>();
            
            if (targets.Length > 0)
            {
                GenericMenu menu = new GenericMenu();
                 
                foreach (var target in targets)
                {
                    menu.AddItem(new GUIContent(target.gameObject.name), false, () => { 
                        var method = typeof(SkillManager).GetMethod("SetTarget", 
                            System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Instance);
                        method?.Invoke(_skillManager, new object[] { target });
                        Repaint();
                    });
                }
                 
                menu.ShowAsContext();
            }
            else
            { 
                EditorUtility.DisplayDialog("No Targets Found", 
                    "No objects with Targetable component found in the scene.", "OK");
            }
        }
    }
}
#endif