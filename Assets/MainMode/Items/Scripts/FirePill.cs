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
        protected override void OnEnable()
        {
            base.OnEnable();
            UseAction += Actvate;
        }
        protected override void OnDisable()
        {
            base.OnDisable();
            UseAction -= Actvate;
        }
        private void Actvate()
        {
            var shield = Instantiate(_fireShield);
            shield.transform.SetParent(user.transform, false);        
            user.AddResistAttack(attackResist,resistDuration);
            Destroy(shield, _itemEffect.Duration);
        }

    }
}
