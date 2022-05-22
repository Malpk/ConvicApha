using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BaseMode
{
    public class TileTI81 : Trap
    {
        [SerializeField] private int _damage;
        [SerializeField] private float _screenEffectDuration;

        public override EffectType Type => EffectType.Venom;

        private void OnTriggerEnter2D(Collider2D collision)
        {
            SetScreen(collision, _screenEffectDuration);
            if (collision.TryGetComponent<IDamage>(out IDamage target))
            {
                target.TakeDamage(_damage);
            }
        }
    }
}