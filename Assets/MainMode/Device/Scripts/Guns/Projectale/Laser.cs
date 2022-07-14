using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MainMode
{
    public class Laser : MonoBehaviour,ISetAttack
    {
        [Min(1)]
        [SerializeField] private int _damage = 1;
        [Min(10)]
        [SerializeField] private float _timeEffect = 1;
        
        private DamageInfo _attackInfo;

        public void SetAttack(DamageInfo info)
        {
            _attackInfo = info;
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.TryGetComponent<IDamage>(out IDamage target))
            {
                target.TakeDamage(_damage, _attackInfo);
            }
        }
    }
}