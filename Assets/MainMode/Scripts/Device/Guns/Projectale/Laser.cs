using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MainMode
{
    [RequireComponent(typeof(BoxCollider2D))]
    public class Laser : MonoBehaviour,ISetAttack
    {
        [Min(1)]
        [SerializeField] private int _damage = 1;
        [SerializeField] private SpriteRenderer _laserGun;

        private DamageInfo _attackInfo;
        private BoxCollider2D _collider;
        private void Awake()
        {
            _collider = GetComponent<BoxCollider2D>();
            SetMode(false);
        }
        public void SetMode(bool mode)
        {
            _collider.enabled = mode;
            _laserGun.enabled = mode;
        }
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