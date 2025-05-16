using UnityEngine;
using CaseA.Damage.Runtime;
using CaseA.Character.Runtime;
using System;
using System.Threading;
using Cysharp.Threading.Tasks;

namespace CaseA.Status.Runtime
{
    public class OverTimeEffect : StatusEffectBase
    {
        public float AmountPerTick { get; private set; }
        public float TickInterval { get; private set; }
        public Guid? DamageTypeId { get; private set; }
        public IDamageType DamageType { get; private set; }
        public bool IsHealing { get; private set; }

        private CancellationTokenSource _tickCts;
        private Targetable _targetObject;

        public OverTimeEffect(string name, float duration, float amountPerTick, float tickInterval,
            bool isHealing, Guid? damageTypeId = null, IDamageType damageType = null,
            GameObject source = null)
            : base(name, duration, source)
        {
            AmountPerTick = amountPerTick;
            TickInterval = tickInterval;
            DamageTypeId = damageTypeId;
            DamageType = damageType;
            IsHealing = isHealing;
        }

        public override void Apply(IEffectable target)
        {
            base.Apply(target);

            if (target is Targetable targetable)
            {
                _targetObject = targetable;

                if (IsPermanent || Duration > 0)
                {
                    _tickCts = new CancellationTokenSource();
                    TickRoutine(_tickCts.Token).Forget();
                }
            }
        }

        public override void Remove(IEffectable target)
        {
            base.Remove(target);

            if (_tickCts != null)
            {
                _tickCts.Cancel();
                _tickCts.Dispose();
                _tickCts = null;
            }
        }

        private async UniTaskVoid TickRoutine(CancellationToken cancellationToken)
        {
            ApplyTick();

            try
            {
                while (IsActive && _targetObject != null && !cancellationToken.IsCancellationRequested)
                {
                    await UniTask.Delay(System.TimeSpan.FromSeconds(TickInterval),
                        cancellationToken: cancellationToken);

                    if (IsActive)
                    {
                        ApplyTick();
                    }
                }
            }
            catch (OperationCanceledException)
            {
                
            }
        }

        private void ApplyTick()
        {
            if (_targetObject == null) return;

            if (IsHealing)
            {
                _targetObject.Heal(AmountPerTick, Source);
            }
            else
            {
                _targetObject.TakeDamage(AmountPerTick, DamageType, Source);
            }
        }
    }
}