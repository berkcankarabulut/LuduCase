using System.Collections.Generic;
using UnityEngine; 

namespace CaseA.Damage.Runtime
{ 
    [CreateAssetMenu(fileName = "DamageTypeDatabase", menuName = "Game/Damage Type Database")]
    public class DamageTypeDatabase : ScriptableObject
    {  
        [Header("All Registered Types")]
        [SerializeField] private List<DamageTypeSO> _registeredTypes = new List<DamageTypeSO>();
  
        public IReadOnlyList<DamageTypeSO> GetAllDamageTypes()
        {
            return _registeredTypes.AsReadOnly();
        }
        
        #if UNITY_EDITOR 
        [ContextMenu("Update Registered Types List")]
        private void UpdateRegisteredTypesList()
        { 
            _registeredTypes.Clear();
             
            var fields = GetType().GetFields(System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
            
            foreach (var field in fields)
            {
                if (field.FieldType == typeof(DamageTypeSO))
                {
                    var value = field.GetValue(this) as DamageTypeSO;
                    if (value != null && !_registeredTypes.Contains(value))
                    {
                        _registeredTypes.Add(value);
                        Debug.Log($"Added damage type: {value}");
                    }
                }
            }
             
            UnityEditor.EditorUtility.SetDirty(this);
        }
         
        public void CreateBasicTypes()
        { 
        }
        #endif
    }
}