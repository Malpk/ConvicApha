using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BaseMode
{
    [RequireComponent(typeof(Rigidbody2D))]
    [RequireComponent(typeof(CapsuleCollider2D))]
    public class Bullet : MonoBehaviour
    {
        [Min(1)]
        [SerializeField] private int _damage = 1;
        [Min(1)]
        [SerializeField] protected float _speed = 1;

        private Rigidbody2D _rigidbody;
        protected virtual void Start()
        {
            _rigidbody = GetComponent<Rigidbody2D>();
            _rigidbody.velocity = _speed * transform.up;
        }
        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.TryGetComponent<IDamage>(out IDamage target))
            {
                target.TakeDamage(_damage);
            }
        }
    }
}