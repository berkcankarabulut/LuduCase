using System.Collections;
using System.Collections.Generic;
using CaseA.Character.Runtime;
using UnityEngine; 

namespace CaseA.Status.Runtime
{ 
    public class StatusEffectHandler
    {
        private Targetable _targetable;
        
        private class EffectEntry
        {
            public IStatusEffect Effect;
            public Coroutine Coroutine;
            
            public EffectEntry(IStatusEffect effect, Coroutine coroutine)
            {
                Effect = effect;
                Coroutine = coroutine;
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
            
            Coroutine coroutine = _targetable.StartCoroutine(EffectRoutine(effect));
            
            _activeEffects.Add(new EffectEntry(effect, coroutine));
            
            Debug.Log($"[StatusEffectManager] Added effect: {effect.Name}");
        }
         
        private IEnumerator EffectRoutine(IStatusEffect effect)
        {
            float updateInterval = 0.1f;
            float duration = effect.Duration;
            float elapsed = 0f;
            
            bool hasDuration = duration > 0;
            
            effect.Tick(0);
            
            while (true)
            {
                yield return new WaitForSeconds(updateInterval);
                
                if (hasDuration)
                {
                    elapsed += updateInterval;
                    
                    if (elapsed >= duration)
                    {
                        effect.Remove(_targetable);
                        RemoveEffectEntry(effect);
                        Debug.Log($"[StatusEffectManager] Effect duration ended: {effect.Name}");
                        yield break;
                    }
                }
                
                effect.Tick(updateInterval);
                if (!effect.IsActive)
                {
                    effect.Remove(_targetable);
                    RemoveEffectEntry(effect);
                    Debug.Log($"[StatusEffectManager] Effect is no longer active: {effect.Name}");
                    yield break;
                }
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
                    if (entry.Coroutine != null)
                    {
                        _targetable.StopCoroutine(entry.Coroutine);
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
                
                if (entry.Coroutine != null)
                {
                    _targetable.StopCoroutine(entry.Coroutine);
                }
            }
            
            _activeEffects.Clear();
            
            Debug.Log("[StatusEffectManager] Removed all effects");
        }
    }
}