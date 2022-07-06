using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MainMode
{
    public class TileN7 : Trap
    {
        public override TrapType DeviceType => TrapType.N7;

        private void OnTriggerEnter2D(Collider2D collision)
        {
            SetScreen(collision, attackInfo);
            if (collision.TryGetComponent<IMoveEffect>(out IMoveEffect target))
            {
                target.StopMove(attackInfo.TimeEffect, attackInfo.Effect);
            }
        }
    }
}