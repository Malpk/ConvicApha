using System.Collections;
using UnityEngine;

namespace MainMode
{
    public class VenomEffect : MonoBehaviour,ISetAttack
    {
        [Min(1)]
        [SerializeField] private int _damageValue = 1;
        
        private AttackInfo _attackInfo;

        public void SetAttack(AttackInfo info)
        {
            _attackInfo = info;
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.TryGetComponent<IDamage>(out IDamage target))
            {
                target.TakeDamage(_damageValue, _attackInfo);
            }
        }
    }
}