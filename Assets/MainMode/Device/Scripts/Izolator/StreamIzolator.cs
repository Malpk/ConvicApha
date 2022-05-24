using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MainMode
{
    public class StreamIzolator : Izolator
    {
        public override EffectType TypeEffect => EffectType.None;

        public override TrapType DeviceType => TrapType.SteamIsolator;

        protected override void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.GetComponent<IMoveEffect>() != null)
                ActivateDevice();
        }

        protected override void SetMode(bool mode)
        {
        }
    }
}