using System.Collections.Generic;
using System.Threading;
using CaseA.Character.Runtime;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace CaseA.Status.Runtime
{
    public class StatusEffectHandler
    {
        private Targetable _targetable;

        private class EffectEntry
        {
            public IStatusEffect Effect;
            public CancellationTokenSource CancellationTokenSource;

            public EffectEntry(IStatusEffect effect, CancellationTokenSource cts)
            {
                Effect = effect;
                CancellationTokenSource = cts;
            }
        }

        private List<EffectEntry> _activeEffects = new List<EffectEntry>();

        public StatusEffectHandler(Targetable targetable)
        {
            _targetable = targetable;
        }

        public void AddStatusEffect(IStatusEffect effect)
        {
            if (effect == null || _targetable == null) return;

            RemoveStatusEffect(effect.Name);

            effect.Apply(_targetable);

            var cts = new CancellationTokenSource();
            EffectRoutine(effect, cts.Token).Forget();

            _activeEffects.Add(new EffectEntry(effect, cts));

            Debug.Log($"[StatusEffectManager] Added effect: {effect.Name}");
        }

        private async UniTaskVoid EffectRoutine(IStatusEffect effect, CancellationToken cancellationToken)
        {
            float updateInterval = 0.1f;
            float duration = effect.Duration;
            float elapsed = 0f;

            bool hasDuration = duration > 0;

            effect.Tick(0);

            try
            {
                while (!cancellationToken.IsCancellationRequested)
                {
                    await UniTask.Delay(System.TimeSpan.FromSeconds(updateInterval),
                        cancellationToken: cancellationToken);

                    if (hasDuration)
                    {
                        elapsed += updateInterval;

                        if (elapsed >= duration)
                        {
                            effect.Remove(_targetable);
                            RemoveEffectEntry(effect);
                            Debug.Log($"[StatusEffectManager] Effect duration ended: {effect.Name}");
                            return;
                        }
                    }

                    effect.Tick(updateInterval);
                    if (!effect.IsActive)
                    {
                        effect.Remove(_targetable);
                        RemoveEffectEntry(effect);
                        Debug.Log($"[StatusEffectManager] Effect is no longer active: {effect.Name}");
                        return;
                    }
                }
            }
            catch (System.OperationCanceledException)
            { 
            }
        }

        public void RemoveStatusEffect(string effectName)
        {
            if (_targetable == null) return;

            for (int i = _activeEffects.Count - 1; i >= 0; i--)
            {
                var entry = _activeEffects[i];
                if (entry.Effect.Name == effectName)
                {
                    entry.Effect.Remove(_targetable);
                    if (entry.CancellationTokenSource != null)
                    {
                        entry.CancellationTokenSource.Cancel();
                        entry.CancellationTokenSource.Dispose();
                    }

                    _activeEffects.RemoveAt(i);

                    Debug.Log($"[StatusEffectManager] Removed effect: {effectName}");
                    return;
                }
            }
        }

        private void RemoveEffectEntry(IStatusEffect effect)
        {
            for (int i = _activeEffects.Count - 1; i >= 0; i--)
            {
                if (_activeEffects[i].Effect == effect)
                {
                    if (_activeEffects[i].CancellationTokenSource != null)
                    {
                        _activeEffects[i].CancellationTokenSource.Cancel();
                        _activeEffects[i].CancellationTokenSource.Dispose();
                    }

                    _activeEffects.RemoveAt(i);
                    return;
                }
            }
        }

        public void RemoveAllStatusEffects()
        {
            if (_targetable == null) return;

            foreach (var entry in _activeEffects)
            {
                entry.Effect.Remove(_targetable);

                if (entry.CancellationTokenSource != null)
                {
                    entry.CancellationTokenSource.Cancel();
                    entry.CancellationTokenSource.Dispose();
                }
            }

            _activeEffects.Clear();

            Debug.Log("[StatusEffectManager] Removed all effects");
        }
    }
} 