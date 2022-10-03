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
            shield.transform.SetParent(user.transform, false);        
            user.ApplyEffect(_itemEffect);
            user.AddResistAttack(attackResist,resistDuration);
            Destroy(shield, _itemEffect.Duration);
        }

    }
}
