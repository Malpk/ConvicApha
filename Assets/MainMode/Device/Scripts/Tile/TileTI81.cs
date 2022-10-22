using UnityEngine;

namespace MainMode
{
    public class TileTI81 : Trap
    {
        [SerializeField] private int _damage;

        public override TrapType DeviceType => TrapType.TI81;

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.TryGetComponent<IDamage>(out IDamage target))
            {
                target.TakeDamage(_damage, attackInfo);
            }
        }
    }
}