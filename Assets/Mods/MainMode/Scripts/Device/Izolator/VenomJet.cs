using UnityEngine;

namespace MainMode
{
    public class VenomJet : JetPoint
    {
        [Header("General Setting")]
        [Min(1)]
        [SerializeField] private int _damageValue = 1;

        private DamageInfo _attackInfo;

        public override void SetAttack(DamageInfo info)
        {
            _attackInfo = info;
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.TryGetComponent<IDamage>(out IDamage target))
            {
                target.TakeDamage(_damageValue, _attackInfo);
            }
        }
    }
}