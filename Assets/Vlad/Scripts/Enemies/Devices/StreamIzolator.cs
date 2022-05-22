using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BaseMode
{
    public class StreamIzolator : Izolator
    {
        public override EffectType TypeEffect => EffectType.None;

        protected override void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.GetComponent<PlayerMove>())
                ActivateDevice();
        }
    }
}