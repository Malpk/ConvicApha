using UnityEngine;

namespace MainMode.Items
{
    public class FirePill : ConsumablesItem
    {
        [SerializeField] private float _timeActivate;
        [SerializeField] private GameObject _fireShield;
        [SerializeField] private DamageInfo _attackResist;

        public override string Name => "Огнезин";
        protected override void UseConsumable()
        {
            var shield = Instantiate(_fireShield);
            shield.transform.SetParent(user.transform, false);
            user.AddResistAttack(_attackResist, _timeActivate);
            Destroy(shield, _timeActivate);
        }
    }
}
