using System.Collections.Generic;
using UnityEngine;
using CaseA.Character.Runtime;
using CaseA.Damage.Runtime;

namespace CaseA.Status.Runtime
{ 
    public class ResistanceModifierEffect : StatusEffectBase
    {
        private Dictionary<IDamageType, float> _resistanceModifiers;
        private bool _applied;
        private IResistable _appliedTarget;
 
        public ResistanceModifierEffect(string name, float duration,
            Dictionary<IDamageType, float> resistanceModifiers, GameObject source = null)
            : base(name, duration, source)
        {
            _resistanceModifiers = new Dictionary<IDamageType, float>(resistanceModifiers);
            _applied = false;
            _appliedTarget = null;
        } 
        public override void Apply(IEffectable target)
        {
            base.Apply(target);
            
            if (_applied) return;
             
            if (target is IResistable resistable)
            {
                foreach (var modifier in _resistanceModifiers)
                {
                    if (modifier.Key != null)
                    {
                        resistable.ModifyResistance(modifier.Key, modifier.Value);
                    }
                }
                
                _applied = true;
                _appliedTarget = resistable;  
                
                Debug.Log($"[ResistanceModifierEffect] Applied {Name} to {target.gameObject.name}");
            }
        }
 
        public override void Remove(IEffectable target)
        {
            base.Remove(target);
             
            if (!_applied) return;
             
            if (target == null && _appliedTarget != null)
            { 
                target = _appliedTarget as IEffectable;
            }
             
            if (target == null) return;
             
            if (!(target is IResistable resistable)) return;
             
            foreach (var modifier in _resistanceModifiers)
            {
                if (modifier.Key != null)
                {
                    resistable.ModifyResistance(modifier.Key, -modifier.Value);
                }
            }
            
            Debug.Log($"[ResistanceModifierEffect] Removed {Name} from {target.gameObject.name}");
             
            _applied = false;
            _appliedTarget = null;
        }
    }
}