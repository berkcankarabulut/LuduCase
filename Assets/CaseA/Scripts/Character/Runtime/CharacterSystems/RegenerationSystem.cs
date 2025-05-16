using System.Collections;
using CaseA.Health.Runtime;
using UnityEngine;

namespace CaseA.Character.Runtime
{
    public class RegenerationSystem
    {
        private float _regenerationRate = 0f;

        private HealthSystem _healthSystem;
        private Coroutine _regenerationCoroutine; 
        private const float REGEN_TICK_INTERVAL = 0.5f; 

        public RegenerationSystem(HealthSystem healthSystem)
        {
            _healthSystem = healthSystem;
            _regenerationRate = _healthSystem.RegenerationRate;
        } 

        public void StartRegeneration()
        {
            StopRegeneration();
            if (_regenerationRate > 0 && _healthSystem != null)
            {
                _regenerationCoroutine = _healthSystem.StartCoroutine(RegenerationRoutine());
            }
        }

        public void StopRegeneration()
        {
            if (_regenerationCoroutine == null) return;
            _healthSystem.StopCoroutine(_regenerationCoroutine);
            _regenerationCoroutine = null;
        }

        public void ModifyRegenerationRate(float amount)
        {
            _regenerationRate = Mathf.Max(0f, _regenerationRate + amount);
            StartRegeneration();
        }

        private IEnumerator RegenerationRoutine()
        {
            while (_regenerationRate > 0 && _healthSystem != null)
            {
                yield return new WaitForSeconds(REGEN_TICK_INTERVAL);

                if (_healthSystem.CurrentHealth < _healthSystem.MaxHealth)
                {
                    float healAmount = _regenerationRate * REGEN_TICK_INTERVAL;
                    _healthSystem.Heal(healAmount, null);
                }

                if (_regenerationRate <= 0)
                {
                    break;
                }
            }

            _regenerationCoroutine = null;
        }
    }
}