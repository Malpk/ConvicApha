using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MainMode
{
    public class TileU92 : Trap
    {
        public override TrapType DeviceType => TrapType.U92;

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.TryGetComponent(out IDamage damage))
            {
                damage.TakeDamage(0, attackInfo);
            }
        }
    }
}