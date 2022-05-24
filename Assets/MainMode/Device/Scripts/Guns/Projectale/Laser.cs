using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MainMode
{
    public class Laser : MonoBehaviour
    {
        [Min(1)]
        [SerializeField] private int _damage = 1;
        [Min(10)]
        [SerializeField] private float _timeEffect = 1;



        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.TryGetComponent<IDamage>(out IDamage target))
            {
                target.TakeDamage(_damage);
            }
            if (collision.TryGetComponent<PlayerScreen>(out PlayerScreen screen))
            {
                screen.SetEffect(EffectType.Fire, _timeEffect);
            }
        }
    }
}