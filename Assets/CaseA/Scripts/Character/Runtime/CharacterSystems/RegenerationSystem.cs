using System; 
using System.Threading;
using CaseA.Health.Runtime;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace CaseA.Character.Runtime
{
    public class RegenerationSystem
    {
        private float _regenerationRate = 0f;

        private HealthSystem _healthSystem;
        private CancellationTokenSource _regenerationCts;
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
                _regenerationCts = new CancellationTokenSource();
                RegenerationRoutine(_regenerationCts.Token).Forget();
            }
        }

        public void StopRegeneration()
        {
            if (_regenerationCts == null) return;
            _regenerationCts.Cancel();
            _regenerationCts.Dispose();
            _regenerationCts = null;
        }

        public void ModifyRegenerationRate(float amount)
        {
            _regenerationRate = Mathf.Max(0f, _regenerationRate + amount);
            StartRegeneration();
        }

        private async UniTaskVoid RegenerationRoutine(CancellationToken cancellationToken)
        {
            try
            {
                while (_regenerationRate > 0 && _healthSystem != null && !cancellationToken.IsCancellationRequested)
                {
                    await UniTask.Delay(System.TimeSpan.FromSeconds(REGEN_TICK_INTERVAL), cancellationToken: cancellationToken);

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
            }
            catch (OperationCanceledException)
            {
                
            }
        }
    }
}