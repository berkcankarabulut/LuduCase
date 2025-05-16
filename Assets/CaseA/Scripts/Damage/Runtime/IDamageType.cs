using System;
using UnityEngine;

namespace CaseA.Damage.Runtime
{
    public interface IDamageType
    {
        string Name { get; }
        Guid TypeId { get; }
        Color DamageColor { get; }
        Sprite Icon { get; }
    }
}