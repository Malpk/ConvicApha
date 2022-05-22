using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BaseMode
{
    public class VenomIzolator : Izolator
    {
        public override EffectType TypeEffect => EffectType.Venom;

        protected override void OnTriggerEnter2D(Collider2D collision)
        {
            base.OnTriggerEnter2D(collision);
            if (collision.TryGetComponent<IDamage>(out IDamage target))
            {
                ActivateDevice();
            }
        }
    }
}