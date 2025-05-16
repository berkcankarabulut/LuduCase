using System;
using UnityEngine;

namespace CaseA.Health.Runtime
{
    public class HealthSystem : MonoBehaviour
    {
        [SerializeField] private HealthData _healthData;

        public event EventHandler<HealthChangedEventArgs> OnHealthChanged;
        public event EventHandler OnCriticalHealth;
        public event EventHandler OnDeath;

        public float MaxHealth => _healthData.MaxHealth;
        public float CurrentHealth => _healthData.CurrentHealth;
        public float HealthPercentage => _healthData.CurrentHealth / _healthData.MaxHealth;
        public bool IsAlive => _healthData.CurrentHealth > 0;
        public bool IsInCriticalState => IsAlive && HealthPercentage <= _healthData.CriticalHealthThreshold;
        public float RegenerationRate { get; set; }


        public void TakeDamage(float amount, bool isCritical = false, GameObject source = null)
        {
            if (!IsAlive || amount <= 0) return;

            ModifyHealth(-amount, source, isCritical);
        }

        public void Heal(float amount, GameObject source = null)
        {
            if (!IsAlive || amount <= 0) return;
            ModifyHealth(amount, source);
        }

        public void Kill(GameObject source = null)
        {
            if (!IsAlive) return;
            ModifyHealth(-_healthData.CurrentHealth, source);
        }

        public void Revive(float healthAmount = -1f, GameObject source = null)
        {
            if (healthAmount < 0)
            {
                healthAmount = _healthData.MaxHealth;
            }

            if (IsAlive)
            {
                Heal(healthAmount, source);
                return;
            }

            float previousHealth = _healthData.CurrentHealth;
            _healthData.CurrentHealth = Mathf.Clamp(healthAmount, 1f, _healthData.MaxHealth);

            HealthChangedEventArgs eventArgs = new HealthChangedEventArgs(
                _healthData.CurrentHealth, previousHealth, _healthData.MaxHealth,
                _healthData.CurrentHealth - previousHealth, source, false);

            OnHealthChanged?.Invoke(this, eventArgs);
        }

        public void ModifyMaxHealth(float amount)
        {
            float previousMaxHealth = _healthData.MaxHealth;
            _healthData.MaxHealth = Mathf.Max(1f, _healthData.MaxHealth + amount);

            if (_healthData.MaxHealth > previousMaxHealth)
            {
                _healthData.CurrentHealth += (_healthData.MaxHealth - previousMaxHealth);
            }
            else if (_healthData.CurrentHealth > _healthData.MaxHealth)
            {
                _healthData.CurrentHealth = _healthData.MaxHealth;
            }
        }

        private void ModifyHealth(float amount, GameObject source = null, bool isCritical = false)
        {
            float previousHealth = _healthData.CurrentHealth;
            _healthData.CurrentHealth += amount;
            _healthData.CurrentHealth = Mathf.Clamp(_healthData.CurrentHealth, 0f, _healthData.MaxHealth);
            float changeAmount = _healthData.CurrentHealth - previousHealth;

            if (changeAmount == 0) return;

            HealthChangedEventArgs eventArgs = new HealthChangedEventArgs(
                _healthData.CurrentHealth, previousHealth, _healthData.MaxHealth,
                changeAmount, source, isCritical);
            OnHealthChanged?.Invoke(this, eventArgs);

            if (!IsInCriticalState && previousHealth > 0 &&
                _healthData.CurrentHealth <= _healthData.MaxHealth * _healthData.CriticalHealthThreshold)
            {
                OnCriticalHealth?.Invoke(this, EventArgs.Empty);
            }

            if (previousHealth > 0 && _healthData.CurrentHealth <= 0)
            {
                OnDeath?.Invoke(this, EventArgs.Empty);
            }
        }
    }
}