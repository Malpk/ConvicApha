using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MainMode
{
    public class VenomIzolator : Izolator
    {
        [SerializeField] private SpriteRenderer _sprite;

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