using System;
using UnityEngine;
using UnityEngine.Events;

namespace MainMode
{
    [RequireComponent(typeof(Rigidbody2D))]
    [RequireComponent(typeof(CapsuleCollider2D))]
    public class Bullet : MonoBehaviour,ISetAttack,IPoolItem
    {
        [Min(1)]
        [SerializeField] private int _damage = 1;
        [Min(1)]
        [SerializeField] protected float _speed = 1;
        [Min(1)]
        [SerializeField] private float _delayDestroy = 1;
        [SerializeField] private UnityEvent _onHit;

        private float _progress = 0f;
        private DamageInfo _attackInfo;
        private Rigidbody2D _rigidbody;

        public event Action<IPoolItem> OnDelete;

        public bool IsDestroy { get; private set; } = false;

        public GameObject PoolItem => gameObject;

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
            _rigidbody.velocity = _speed * transform.up;
        }
        public void Delete()
        {
            IsDestroy = true;
            gameObject.SetActive(false);
            _rigidbody.velocity = Vector2.zero;
            _onHit.Invoke();
            OnDelete?.Invoke(this);
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