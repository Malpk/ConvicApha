using System.Collections;
using UnityEngine;

namespace MainMode
{
    [RequireComponent(typeof(CapsuleCollider2D))]
    public class Turel : Gun
    {
        [Header("Attacks properties")]
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

        public override TrapType DeviceType => TrapType.Turel;

        private void Start()
        {
            if (playOnStart)
                Run();
        }

        public override void Run(Collider2D collision = null)
        {
            if (!isActiveDevice)
            {
                isActiveDevice = true;
                StartCoroutine(Rotate());
                StartCoroutine(Shoot());
            }
        }

        protected IEnumerator Rotate()
        {
            yield return new WaitWhile(() => !IsShow);
            float startAngle = _rigidbody.rotation;
            var progress = 0f;
            while (progress <= 1f && IsShow)
            {
                _rigidbody.rotation = Mathf.Lerp(_rigidbody.rotation, _rigidbody.rotation + _rotationAngleOnSeconds, Time.deltaTime);
                progress += Time.deltaTime / durationWork;
                yield return null;
            }
            _rigidbody.rotation = startAngle;
            isActiveDevice = false;
            if(destroyMode)
                SetMode(false);
        }
        protected IEnumerator Shoot()
        {
            yield return new WaitWhile(() => !IsShow);
            while (isActiveDevice)
            {
                var progress = 0f;
                while (progress <= 1f && isActiveAndEnabled)
                {
                    progress += Time.deltaTime * _firingRateOnSeconds;
                    yield return null;
                }
                gunAnimator.SetTrigger("Shoot");
                var bullet = Instantiate(_bulletPrefab.gameObject, _spawnTransform.position, _spawnTransform.rotation).GetComponent<Bullet>();
                bullet.SetAttack(attackInfo);
            }
        }
    }
}