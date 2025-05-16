using System;
using UnityEngine;
using GuidSystem.Runtime;

namespace CaseA.Damage.Runtime
{ 
    [CreateAssetMenu(fileName = "NewDamageType", menuName = "Game/Damage Type")]
    public class DamageTypeSO : ScriptableObject, IDamageType
    {
        [SerializeField] private string _damageTypeName;
        [SerializeField] private Color _damageColor = Color.white;
        [SerializeField] private Sprite _damageIcon;
        [SerializeField] private SerializableGuid _typeId;

        public string Name => _damageTypeName;
        public Guid TypeId => _typeId;
        public Color DamageColor => _damageColor;
        public Sprite Icon => _damageIcon;

        private void OnValidate()
        {
            if (_typeId == SerializableGuid.Empty)
            {
                _typeId = SerializableGuid.NewGuid();
            }
        }
    }
}