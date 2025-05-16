using CaseA.Character.Runtime;
using UnityEngine;

namespace CaseA.Status.Runtime
{ 
    public interface IStatusEffect
    {
        string Name { get; }
        float Duration { get; }
        float RemainingTime { get; }
        bool IsActive { get; }
        GameObject Source { get; }
        
        void Apply(IEffectable target);
        void Remove(IEffectable target);
        void Tick(float deltaTime);
    }
}