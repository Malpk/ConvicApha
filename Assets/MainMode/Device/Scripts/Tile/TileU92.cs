using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MainMode
{
    public class TileU92 : Trap
    {
        public override TrapType DeviceType => TrapType.U92;

        protected override void Intilizate()
        {
            base.Intilizate();
            Deactivate();
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            SetScreen(collision, attackInfo);
        }
    }
}