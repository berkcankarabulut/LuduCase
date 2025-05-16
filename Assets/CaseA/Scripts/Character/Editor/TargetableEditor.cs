#if UNITY_EDITOR 
using CaseA.Health.Runtime;
using UnityEditor;
using UnityEngine;

namespace CaseA.Character.Editor
{
    [CustomEditor(typeof(Runtime.Targetable), true)] 
    public class TargetableEditor : UnityEditor.Editor
    {
        private Runtime.Targetable _controller;

        private void OnEnable()
        {
            _controller = (Runtime.Targetable)target;
        }

        public override void OnInspectorGUI()
        { 
            base.OnInspectorGUI();

            if (_controller == null) return;

            EditorGUILayout.Space(10);
             
            DrawCharacterHealthInfo(); 
            DrawActiveEffects(); 
            DrawLastAction();
 
            if (EditorApplication.isPlaying)
            {
                Repaint();
            }
        }

        private void DrawCharacterHealthInfo()
        {
            var healthController = FindHealthController();
            if (healthController == null) return;
 
            EditorGUILayout.Space(5);
            GUILayout.Label("Character Health", EditorStyles.boldLabel);

            float currentHealth = healthController.CurrentHealth;
            float maxHealth = healthController.MaxHealth;
            float healthPercentage = healthController.HealthPercentage;
 
            EditorGUILayout.LabelField($"Health: {currentHealth:F1} / {maxHealth:F1} ({healthPercentage * 100:F1}%)");
            EditorGUILayout.LabelField(
                $"Status: {(healthController.IsAlive ? "Alive" : "Dead")}{(healthController.IsInCriticalState ? " (Critical)" : "")}");
 
            Rect rect = EditorGUILayout.GetControlRect(false, 20);
            EditorGUI.DrawRect(rect, new Color(0.2f, 0.2f, 0.2f));

            if (!healthController.IsAlive) return;
            float width = rect.width * healthPercentage;
            rect.width = width;
 
            Color healthColor = Color.green;
            if (healthPercentage < 0.3f)
                healthColor = Color.red;
            else if (healthPercentage < 0.6f)
                healthColor = Color.yellow;

            EditorGUI.DrawRect(rect, healthColor);
        }

        private void DrawActiveEffects()
        {
            SerializedProperty activeEffectsProp = serializedObject.FindProperty("_activeEffects");
            if (activeEffectsProp == null || activeEffectsProp.arraySize <= 0) return;
            EditorGUILayout.Space(10);
            GUILayout.Label("Active Effects", EditorStyles.boldLabel);

            for (int i = 0; i < activeEffectsProp.arraySize; i++)
            {
                SerializedProperty effectProp = activeEffectsProp.GetArrayElementAtIndex(i);
                EditorGUILayout.LabelField($"• {effectProp.stringValue}");
            }
        }

        private void DrawLastAction()
        {
            SerializedProperty lastActionProp = serializedObject.FindProperty("_lastAction");
            if (lastActionProp == null || string.IsNullOrEmpty(lastActionProp.stringValue)) return;
            EditorGUILayout.Space(10);
            GUILayout.Label("Last Action", EditorStyles.boldLabel);

            GUIStyle style = new GUIStyle(EditorStyles.boldLabel);
 
            string action = lastActionProp.stringValue;
            if (action.Contains("CRITICAL") || action.Contains("DIED"))
            {
                style.normal.textColor = Color.red;
            }
            else if (action.Contains("Healed") || action.Contains("REVIVED"))
            {
                style.normal.textColor = Color.green;
            }

            EditorGUILayout.LabelField(action, style);
        }

        private HealthSystem FindHealthController()
        {
            return _controller.HealthSystem;
        }
    }
} 

#endif