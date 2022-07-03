using System;
using UnityEngine;


namespace MainMode.Items
{
    public class FirePill : ConsumablesItem
    {
        [SerializeField] private GameObject _fireShield;     

        public override void Pick(Player player)
        {         
            _ownerPlayer = player;
            gameObject.SetActive(false);
        }

        public override void Use()
        {
            var shield = Instantiate(_fireShield);
            shield.transform.SetParent(_ownerPlayer.transform, false);        
            _ownerPlayer.ApplyEffect(_itemEffect);
            _ownerPlayer.AddResistAttack(attackResist,resistDuration);
            Destroy(shield, _itemEffect.Duration);
        }

    }
}
