using UnityEngine; 
using CaseA.Damage.Runtime;
using CaseA.Status.Runtime;

namespace CaseA.Character.Runtime
{ 
    public interface ITargetable
    {
        GameObject gameObject { get; }
        bool IsAlive { get; }
        float HealthPercentage { get; }
    }

    public interface IResistable : ITargetable
    {
        float GetResistance(IDamageType damageType);
        void ModifyResistance(IDamageType damageType, float amount);
    }
    public interface IEffectable : ITargetable
    {
        void AddStatusEffect(IStatusEffect effect);
        void RemoveStatusEffect(string effectName);
        void RemoveAllStatusEffects();
    } 
}  