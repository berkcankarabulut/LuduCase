using CaseA.Damage.Runtime;
using UnityEngine;

namespace CaseA.Character.Runtime
{
    public interface IDamageable : ITargetable
    {
        void TakeDamage(float amount, IDamageType damageType = null, GameObject source = null);
        void Kill(GameObject source = null);
    }
}