using System;
using UnityEngine;

namespace CaseA.Health.Runtime
{ 
    public class HealthChangedEventArgs : EventArgs
    {
        public float CurrentHealth { get; }
        public float PreviousHealth { get; }
        public float MaxHealth { get; }
        public float ChangeAmount { get; }
        public GameObject Source { get; }
        public bool IsCritical { get; }

        public HealthChangedEventArgs(float currentHealth, float previousHealth, float maxHealth, float changeAmount, 
            GameObject source = null, bool isCritical = false)
        {
            CurrentHealth = currentHealth;
            PreviousHealth = previousHealth;
            MaxHealth = maxHealth;
            ChangeAmount = changeAmount;
            Source = source;
            IsCritical = isCritical;
        }
    }
}