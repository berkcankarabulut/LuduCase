using System;
using UnityEngine;

namespace CaseA.Health.Runtime
{ 
    [Serializable]
    public class HealthData
    {
        [SerializeField] private float _maxHealth = 100f;
        [SerializeField] private float _currentHealth = 100f;
        [SerializeField] private float _criticalHealthThreshold = 0.2f;
        [SerializeField] private float _regenerationRate = 0f; 
 
        public float MaxHealth
        {
            get => _maxHealth;
            set => _maxHealth = Mathf.Max(1f, value);
        }
 
        public float CurrentHealth
        {
            get => _currentHealth;
            set => _currentHealth = Mathf.Clamp(value, 0f, _maxHealth);
        }
 
        public float HealthPercentage => _currentHealth / _maxHealth;
         
        public bool IsAlive => _currentHealth > 0;
         
        public bool IsInCriticalState => IsAlive && HealthPercentage <= _criticalHealthThreshold;
 
        public float CriticalHealthThreshold
        {
            get => _criticalHealthThreshold;
            set => _criticalHealthThreshold = Mathf.Clamp01(value);
        }
 
        public float RegenerationRate
        {
            get => _regenerationRate;
            set => _regenerationRate = Mathf.Max(0f, value);
        } 
 
        public HealthData()
        {
            _currentHealth = _maxHealth;
        }
         
        public HealthData(float maxHealth, float criticalThreshold = 0.2f, float regenRate = 0f, bool canRegen = false)
        {
            _maxHealth = Mathf.Max(1f, maxHealth);
            _currentHealth = _maxHealth;
            _criticalHealthThreshold = Mathf.Clamp01(criticalThreshold);
            _regenerationRate = Mathf.Max(0f, regenRate); 
        }
 
        public void ModifyMaxHealth(float amount)
        {
            float previousMaxHealth = _maxHealth;
            _maxHealth = Mathf.Max(1f, _maxHealth + amount);
 
            if (_maxHealth > previousMaxHealth)
            {
                _currentHealth += (_maxHealth - previousMaxHealth);
            } 
            else if (_currentHealth > _maxHealth)
            {
                _currentHealth = _maxHealth;
            }
        }
 
        public void ModifyRegenerationRate(float amount)
        {
            _regenerationRate = Mathf.Max(0f, _regenerationRate + amount);
        }
         
        public void RestoreFullHealth()
        {
            _currentHealth = _maxHealth;
        }
         
        public void SetHealthPercentage(float percentage)
        {
            _currentHealth = _maxHealth * Mathf.Clamp01(percentage);
        }
    }
}