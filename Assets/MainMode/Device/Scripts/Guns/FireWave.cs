using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MainMode
{
    public class FireWave : MonoBehaviour, ISetAttack
    {
        [Min(1)]
        [SerializeField] private int _damage;
        [Min(0)]
        [SerializeField] private float _fireEffect;
        [SerializeField] private SpriteRenderer _sprite;
        [SerializeField] private Collider2D _colider;
        [SerializeField] private Animator _animator;

        private DamageInfo _attackInfo;

        private void Awake()
        {
            _animator = GetComponent<Animator>();
            DestroyObject();
        }
        public void Explosion()
        {
            SetMode(true);
        }
        public void DestroyObject()
        {
            SetMode(false);
        }
        public void SetMode(bool mode)
        {
            _sprite.enabled = mode;
            _colider.enabled = mode;
            _animator.SetBool("Explosion", mode);
        }
        private void OnCollisionEnter2D(Collision2D collision)
        {
            if (collision.transform.TryGetComponent<IDamage>(out IDamage target))
            {
                target.TakeDamage(_damage, _attackInfo);
            }
        }

        public void SetAttack(DamageInfo info)
        {
            _attackInfo = info;
        }
    }
}