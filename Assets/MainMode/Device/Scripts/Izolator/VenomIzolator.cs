using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MainMode
{
    public class VenomIzolator : Izolator
    {
        [SerializeField] private SpriteRenderer _sprite;

        public override TrapType DeviceType => TrapType.VenomIsolator;

        protected override void Intilizate()
        {
            var jets = GetComponentsInChildren<ISetAttack>();
            foreach (var jet in jets)
            {
                jet.SetAttack(attackInfo);
            }
            base.Intilizate();
        }

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