using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MainMode
{
    public class TileTI81 : Trap
    {
        [SerializeField] private int _damage;

        public override TrapType DeviceType => TrapType.TI81;

        protected override void Intilizate()
        {
            base.Intilizate();
            OffItem();
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            SetScreen(collision, attackInfo);
            if (collision.TryGetComponent<IDamage>(out IDamage target))
            {
                target.TakeDamage(_damage, attackInfo);
            }
        }
    }
}