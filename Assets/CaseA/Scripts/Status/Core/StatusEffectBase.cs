using UnityEngine;
using CaseA.Character.Runtime;

namespace CaseA.Status.Runtime
{ 
    public abstract class StatusEffectBase : IStatusEffect
    {
        public string Name { get; protected set; }
        public float Duration { get; protected set; }
        public float RemainingTime { get; protected set; }
        public bool IsActive => RemainingTime > 0 || IsPermanent;
        public bool IsPermanent { get; protected set; }
        public GameObject Source { get; protected set; }
        
        protected IEffectable CurrentTarget { get; private set; }

        protected StatusEffectBase(string name, float duration, GameObject source = null)
        {
            Name = name;
            Duration = duration;
            RemainingTime = duration;
            IsPermanent = duration <= 0;
            Source = source;
        }

        public virtual void Apply(IEffectable target)
        {
            CurrentTarget = target; 
        }

        public virtual void Remove(IEffectable target)
        {
            CurrentTarget = null;
        }

        public virtual void Tick(float deltaTime)
        {
            if (!IsPermanent)
            {
                RemainingTime -= deltaTime;
                if (RemainingTime <= 0 && CurrentTarget != null)
                {
                    CurrentTarget.RemoveStatusEffect(Name);
                }
            }
        }
    }
}