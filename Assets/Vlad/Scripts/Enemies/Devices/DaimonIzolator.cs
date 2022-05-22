using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BaseMode
{
    public class DaimonIzolator : Izolator
    {
        public override EffectType TypeEffect => EffectType.None;

        protected override void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.TryGetComponent<PlayerMove>(out PlayerMove target))
            {
                ActivateDevice();
            }
        }
    }
}