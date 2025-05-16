using System; 
using UnityEngine;
using CaseA.Damage.Runtime;
using CaseA.Health.Runtime;
using CaseA.Status.Runtime;

namespace CaseA.Character.Runtime
{
    [RequireComponent(typeof(HealthSystem))]
    public abstract class Targetable : MonoBehaviour, IDamageable, IHealable, IResistable, IEffectable
    {
        [SerializeField] private HealthSystem _healthSystem;
        [SerializeField] private string _lastAction = "None";
       
        private RegenerationSystem _regenerationSystem;
        private ResistanceSystem _resistanceSystem;
        private CharacterEffectService _effectService; 
        
        public HealthSystem HealthSystem => _healthSystem;
        public RegenerationSystem RegenerationSystem => _regenerationSystem;
        public CharacterEffectService Effects => _effectService;
        public bool IsAlive => HealthSystem != null && HealthSystem.IsAlive;
        public float HealthPercentage => HealthSystem != null ? HealthSystem.HealthPercentage : 0f;


        private void Awake()
        {
            _regenerationSystem = new RegenerationSystem(_healthSystem);
            _resistanceSystem = new ResistanceSystem();
            _effectService = new CharacterEffectService(this);
        }

        protected virtual void OnEnable()
        {
            SubscribeToEvents();
            RegenerationSystem?.StartRegeneration();
        }

        protected virtual void OnDisable()
        {
            UnsubscribeFromEvents();
            RegenerationSystem?.StopRegeneration();
        }

        private void SubscribeToEvents()
        {
            if (HealthSystem == null) return;
            HealthSystem.OnHealthChanged += OnHealthChangedHandler;
            HealthSystem.OnDeath += OnDeathHandler;
            HealthSystem.OnCriticalHealth += OnCriticalHealthHandler;
        }

        private void UnsubscribeFromEvents()
        {
            if (HealthSystem == null) return;
            HealthSystem.OnHealthChanged -= OnHealthChangedHandler;
            HealthSystem.OnDeath -= OnDeathHandler;
            HealthSystem.OnCriticalHealth -= OnCriticalHealthHandler;
        }

        #region IDamageable Implementation

        public void TakeDamage(float amount, IDamageType damageType = null, GameObject source = null)
        {
            if (!IsAlive || amount <= 0) return;

            float actualDamage = amount;
            bool isCritical = false;

            if (damageType != null && _resistanceSystem != null)
            {
                float resistance = _resistanceSystem.GetResistance(damageType);
                actualDamage = amount * (1f - resistance);
            }

            if (UnityEngine.Random.value < 0.15f)
            {
                isCritical = true;
            }

            if (HealthSystem != null)
            {
                HealthSystem.TakeDamage(actualDamage, isCritical, source);
            }
        }

        public void Kill(GameObject source = null)
        {
            if (HealthSystem != null)
            {
                HealthSystem.Kill(source);
            }
        }

        #endregion

        #region IHealable Implementation

        public void Heal(float amount, GameObject source = null)
        {
            if (HealthSystem != null)
            {
                HealthSystem.Heal(amount, source);
            }
        }

        #endregion

        #region IResistable Implementation

        public float GetResistance(IDamageType damageType)
        {
            return _resistanceSystem != null ? _resistanceSystem.GetResistance(damageType) : 0f;
        }

        public void ModifyResistance(IDamageType damageType, float amount)
        {
            if (_resistanceSystem != null)
            {
                _resistanceSystem.ModifyResistance(damageType, amount);
            }
        }

        #endregion

        #region IEffectable Implementation

        public void AddStatusEffect(IStatusEffect effect)
        {
            _effectService?.AddEffect(effect);
        }

        public void RemoveStatusEffect(string effectName)
        {
            _effectService?.RemoveEffect(effectName);
        }

        public void RemoveAllStatusEffects()
        {
            _effectService?.RemoveAllEffects();
        }

        #endregion

        #region EventHandlers
 
        private void OnHealthChangedHandler(object sender, HealthChangedEventArgs e)
        { 
            string changeType = e.ChangeAmount > 0 ? "Healed" : "Damaged";
            string criticalText = e.IsCritical ? " (CRITICAL!)" : "";
            _lastAction = $"{changeType} {Mathf.Abs(e.ChangeAmount):F1}{criticalText}"; 
        }
 
        private void OnCriticalHealthHandler(object sender, EventArgs e)
        {
            _lastAction = "CRITICAL HEALTH WARNING!"; 
        }
 
        private void OnDeathHandler(object sender, EventArgs e)
        {
            _lastAction = "CHARACTER DIED"; 
        }

        #endregion
    }
}