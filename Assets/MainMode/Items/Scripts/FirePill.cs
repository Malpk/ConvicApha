using System;
using UnityEngine;


namespace MainMode.Items
{
    public class FirePill : ConsumablesItem
    {
        [SerializeField] private ItemEffect _itemEffect;
        [SerializeField] private GameObject _fireShield;

        protected override void Awake()
        {
            base.Awake();
            _itemEffect = GetComponent<ItemEffect>();
        }
        public override void Use()
        {
            var shield = Instantiate(_fireShield);
            shield.transform.SetParent(_target.transform, false);        
            _target.ApplyEffect(_itemEffect);
            _target.AddResistAttack(attackResist,resistDuration);
            Destroy(shield, _itemEffect.Duration);
        }

    }
}
