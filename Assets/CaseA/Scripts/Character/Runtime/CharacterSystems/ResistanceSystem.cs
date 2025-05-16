using System;
using System.Collections.Generic;
using UnityEngine;
using CaseA.Damage.Runtime;

namespace CaseA.Character.Runtime
{ 
    public class ResistanceSystem 
    {
        private Dictionary<Guid, float> _resistances = new Dictionary<Guid, float>();
        private Dictionary<IDamageType, float> _cachedResistances = new Dictionary<IDamageType, float>();
 
        public float GetResistance(IDamageType damageType)
        {
            if (damageType == null) return 0f;
             
            if (_cachedResistances.TryGetValue(damageType, out float value))
            {
                return value;
            }
             
            if (_resistances.TryGetValue(damageType.TypeId, out value))
            { 
                _cachedResistances[damageType] = value;
                return value;
            }
            
            return 0f;
        }
 
        public void ModifyResistance(IDamageType damageType, float amount)
        {
            if (damageType == null) return;
            
            Guid typeId = damageType.TypeId;
            
            if (!_resistances.ContainsKey(typeId))
            {
                _resistances[typeId] = 0f;
            }
            
            _resistances[typeId] += amount;
             
            _resistances[typeId] = Mathf.Clamp01(_resistances[typeId]);
             
            if (_cachedResistances.ContainsKey(damageType))
            {
                _cachedResistances[damageType] = _resistances[typeId];
            }
        }
 
        public void SetResistance(IDamageType damageType, float value)
        {
            if (damageType == null) return;
            
            Guid typeId = damageType.TypeId;
            value = Mathf.Clamp01(value);
            
            _resistances[typeId] = value;
             
            if (_cachedResistances.ContainsKey(damageType))
            {
                _cachedResistances[damageType] = value;
            }
        }
 
        public void ClearAllResistances()
        {
            _resistances.Clear();
            _cachedResistances.Clear();
        }
    }
}