using System.Collections;
using UnityEngine;

namespace MainMode
{
    [RequireComponent(typeof(CapsuleCollider2D))]
    public class Turel : Gun
    {

        [Header("Attacks properties")]
        [SerializeField]
        protected float _activeTime = 6f;
        [SerializeField]
        protected float _activationTime = 1f;
        [SerializeField]
        protected float _firingRateOnSeconds = 1f;
        [SerializeField]
        protected Bullet _bulletPrefab;
        [SerializeField]
        protected Transform _spawnTransform;

        [Header("Move properties")]
        [SerializeField] protected float _rotationAngleOnSeconds;
        [SerializeField] protected Rigidbody2D _rigidbody;

        protected bool _isActive;
        
        private float _time;
        private Coroutine _rotate;

        public override TrapType DeviceType => TrapType.Turel;

        public override void Run(Collider2D collision)
        {
            if(_rotate == null)
                _rotate = StartCoroutine(Rotate());
        }

        protected IEnumerator Rotate()
        {
            _time = _firingRateOnSeconds;
            float startAngle = _rigidbody.rotation;
            for (float f = 0; f < _activeTime && IsShow; f += Time.deltaTime)
            {
                _rigidbody.rotation = Mathf.Lerp(_rigidbody.rotation, _rigidbody.rotation + _rotationAngleOnSeconds, Time.deltaTime);
                _time += Time.deltaTime;
                Shoot();
                yield return null;
            }
            _rigidbody.rotation = startAngle;
            _isActive = false;
        }
        protected virtual void Shoot()
        {
            if (_time >= _firingRateOnSeconds)
            {
                _time = 0;
                gunAnimator.SetTrigger("Shoot");
                var bullet = Instantiate(_bulletPrefab.gameObject, _spawnTransform.position, _spawnTransform.rotation).GetComponent<Bullet>();
                bullet.SetAttack(attackInfo);
            }
        }
    }
}