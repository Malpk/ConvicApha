using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MainMode
{
    [RequireComponent(typeof(Collider2D), typeof(SpriteRenderer), typeof(Animator))]
    public class FireWave : MonoBehaviour,ISetAttack
    {
        [Min(1)]
        [SerializeField] private int _damage;
        [Min(0)]
        [SerializeField] private float _fireEffect;

        private Animator _animator;
        private Collider2D _colider;
        private AttackInfo _attackInfo;
        private SpriteRenderer _sprite;

        private void Awake()
        {
            _sprite = GetComponent<SpriteRenderer>();
            _colider = GetComponent<Collider2D>();
            _animator = GetComponent<Animator>();
        }
        public void Explosion()
        {
            _animator.SetTrigger("explosion");
        }
        private void DestroyObject()
        {
            Destroy(gameObject);
        }
        private void OnCollisionEnter2D(Collision2D collision)
        {
            if (collision.transform.TryGetComponent<IDamage>(out IDamage target))
            {
                target.TakeDamage(_damage, _attackInfo);
            }
        }

        public void SetAttack(AttackInfo info)
        {
            _attackInfo = info;
        }
    }
}