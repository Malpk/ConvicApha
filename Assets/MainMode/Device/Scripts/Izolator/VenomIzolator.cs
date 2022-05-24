using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BaseMode
{
    public class VenomIzolator : Izolator
    {
        [SerializeField] private SpriteRenderer _sprite;
        public override EffectType TypeEffect => EffectType.Venom;

        public override TrapType DeviceType => TrapType.VenomIsolator;

        protected override void OnTriggerEnter2D(Collider2D collision)
        {
            base.OnTriggerEnter2D(collision);
            if (collision.TryGetComponent<IDamage>(out IDamage target))
            {
                ActivateDevice();
            }
        }

        protected override void SetMode(bool mode)
        {
            _sprite.enabled = mode;
        }
    }
}