using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BaseMode
{
    public class TileU92 : Trap
    {
        public override EffectType Type => EffectType.Flash;

        public override TrapType DeviceType => TrapType.U92;

        private void OnTriggerEnter2D(Collider2D collision)
        {
            SetScreen(collision);
        }
    }
}