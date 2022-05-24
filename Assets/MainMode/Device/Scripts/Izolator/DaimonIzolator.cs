using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MainMode
{
    public class DaimonIzolator : Izolator
    {
        public override EffectType TypeEffect => EffectType.None;

        public override TrapType DeviceType => TrapType.DaimondIzolator;

        protected override void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.TryGetComponent<Player>(out Player target))
            {
                ActivateDevice();
            }
        }

        protected override void SetMode(bool mode)
        {
        }
    }
}