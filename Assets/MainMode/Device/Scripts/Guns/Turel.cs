using System.Collections;
using UnityEngine;

namespace MainMode
{
    public class Turel : Gun
    {
        [Header("Attacks properties")]
        [SerializeField] private bool _activateOnStart;
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
        [SerializeField] protected Collider2D _colliderBody;

        public override TrapType DeviceType => TrapType.Turel;

        protected override void OnEnable()
        {
            base.OnEnable();
            if (_activateOnStart)
                CompliteUpAnimation += Activate;
        }
        protected override void OnDisable()
        {
            base.OnDisable();
            if (_activateOnStart)
                CompliteUpAnimation -= Activate;
        }
        private void Start()
        {
            if (showOnStart)
                ShowItem();
        }
        public override void Activate()
        {
            base.Activate();
            StartCoroutine(Rotate());
            StartCoroutine(Shoot());
        }

        protected IEnumerator Rotate()
        {
            yield return new WaitWhile(() => !IsShow);
            var progress = 0f;
            while (progress <= 1f && IsActive)
            {
                _rigidbody.rotation = Mathf.Lerp(_rigidbody.rotation, _rigidbody.rotation + _rotationAngleOnSeconds, Time.deltaTime);
                progress += Time.deltaTime / durationWork;
                yield return null;
            }
            if (IsActive)
            {
                Deactivate();
                if (destroyMode)
                    HideItem();
            }
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