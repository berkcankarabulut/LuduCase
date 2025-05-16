using UnityEngine;

namespace CaseA.Character.Runtime
{
    public interface IHealable : ITargetable
    {
        void Heal(float amount, GameObject source = null);
    }
}