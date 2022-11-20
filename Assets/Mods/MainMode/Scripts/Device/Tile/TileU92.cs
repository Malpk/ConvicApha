using UnityEngine;

namespace MainMode
{
    public class TileU92 : DeviceV2
    {
        [SerializeField] private DamageInfo _damageInfo;

        public override TrapType DeviceType => TrapType.U92;

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.TryGetComponent(out IDamage damage))
            {
                damage.TakeDamage(0, _damageInfo);
            }
        }
    }
}