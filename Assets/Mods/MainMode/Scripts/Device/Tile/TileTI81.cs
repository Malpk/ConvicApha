using UnityEngine;

namespace MainMode
{
    public class TileTI81 : DeviceV2
    {
        [SerializeField] private int _damage;
        [SerializeField] private DamageInfo _damageInfo;

        public override TrapType DeviceType => TrapType.TI81;

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.TryGetComponent(out IDamage target))
            {
                target.TakeDamage(_damage, _damageInfo);
            }
        }
    }
}