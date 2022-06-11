using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MainMode
{
    public class TileTI81 : Trap
    {
        [SerializeField] private int _damage;
        [SerializeField] private float _screenEffectDuration;

        private void OnTriggerEnter2D(Collider2D collision)
        {
            SetScreen(collision, _screenEffectDuration);
            if (collision.TryGetComponent<IDamage>(out IDamage target))
            {
                target.TakeDamage(_damage, attackInfo);
            }
        }
    }
}