using System;
using UnityEngine;

namespace CaseA.Health.Runtime
{
    public interface IHealth
    {
        float MaxHealth { get; }
        float CurrentHealth { get; }
        float HealthPercentage { get; }
        bool IsAlive { get; }
        bool IsInCriticalState { get; }
        
        event EventHandler<HealthChangedEventArgs> OnHealthChanged;
        event EventHandler OnCriticalHealth;
        event EventHandler OnDeath;
        
        void TakeDamage(float amount, bool isCritical = false, GameObject source = null);
        void Heal(float amount, GameObject source = null);
        void Kill(GameObject source = null);
        void Revive(float healthAmount = -1f, GameObject source = null);
        void ModifyMaxHealth(float amount);
    }
}