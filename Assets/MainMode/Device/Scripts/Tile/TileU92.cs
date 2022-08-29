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
            if(isActiveDevice)
                SetScreen(collision, attackInfo);
        }
    }
}