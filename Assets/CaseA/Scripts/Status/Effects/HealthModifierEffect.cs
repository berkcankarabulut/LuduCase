using UnityEngine;
using CaseA.Character.Runtime; 

namespace CaseA.Status.Runtime
{
    public class HealthModifierEffect : StatusEffectBase
    {
        public float MaxHealthModifier { get; private set; }
        public float RegenerationRateModifier { get; private set; }

        private bool _applied;

        public HealthModifierEffect(string name, float duration, float maxHealthModifier,
            float regenerationRateModifier, GameObject source = null)
            : base(name, duration, source)
        {
            MaxHealthModifier = maxHealthModifier;
            RegenerationRateModifier = regenerationRateModifier;
            _applied = false;
        }

        public override void Apply(IEffectable target)
        {
            base.Apply(target);

            if (_applied) return;

            if (target is not Targetable targetable) return;
            var healthSystem = targetable.HealthSystem;
            var regenerationSystem = targetable.RegenerationSystem;

            if (healthSystem == null || regenerationSystem == null) return;
                
            healthSystem.ModifyMaxHealth(MaxHealthModifier);
            regenerationSystem.ModifyRegenerationRate(RegenerationRateModifier); 
            _applied = true;
        }

        public override void Remove(IEffectable target)
        {
            base.Remove(target);

            if (!_applied) return;

            if (target is not Targetable targetable) return;
            var healthSystem = targetable.HealthSystem;
            var regenerationSystem = targetable.RegenerationSystem;

            if (healthSystem == null || regenerationSystem == null) return;
            
            healthSystem.ModifyMaxHealth(-MaxHealthModifier); 
            regenerationSystem.ModifyRegenerationRate(-RegenerationRateModifier); 
            _applied = false;
        }
    }
}