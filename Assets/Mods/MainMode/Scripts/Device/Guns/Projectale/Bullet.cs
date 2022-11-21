using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MainMode
{
    [RequireComponent(typeof(Rigidbody2D))]
    [RequireComponent(typeof(CapsuleCollider2D))]
    public class Bullet : MonoBehaviour,ISetAttack
    {
        [Min(1)]
        [SerializeField] private int _damage = 1;
        [Min(1)]
        [SerializeField] protected float _speed = 1;
        [Min(1)]
        [SerializeField] private float _delayDestroy = 1;
        
        private float _progress = 0f;
        private DamageInfo _attackInfo;
        private Rigidbody2D _rigidbody;

        public bool IsDestroy { get; private set; } = false;

        private void Awake()
        {
            _rigidbody = GetComponent<Rigidbody2D>();
        }
        private void Update()
        {
            _progress += Time.deltaTime / _delayDestroy;
            if (_progress >= 1f)
                Delete();
        }
        public void Shoot()
        {
            _progress = 0f;
            IsDestroy = false;
            gameObject.SetActive(true);
            _rigidbody.velocity = _speed * transform.up;
        }
        public void Delete()
        {
            IsDestroy = true;
            gameObject.SetActive(false);
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
                Delete();
            }
        }


    }
}