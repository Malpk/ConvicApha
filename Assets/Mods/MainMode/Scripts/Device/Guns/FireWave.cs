using System;
using UnityEngine;

namespace MainMode
{
    public class FireWave : MonoBehaviour, ISetAttack,IPoolItem
    {
        [Min(1)]
        [SerializeField] private int _damage;
        [SerializeField] private SpriteRenderer _sprite;
        [SerializeField] private Collider2D _colider;
        [SerializeField] private Animator _animator;

        [SerializeField] private DamageInfo _attackInfo;

        public GameObject PoolItem => gameObject;

        public event Action<IPoolItem> OnDelete;

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
            OnDelete?.Invoke(this);
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