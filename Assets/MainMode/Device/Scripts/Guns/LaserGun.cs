using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MainMode
{
    public class LaserGun : Gun
    {
        [Header("LaserGun setting")]
        [SerializeField] private bool _activateOnStart;
        [Min(1)]
        [SerializeField] private float _timeReload = 1f;
        [Min(1)]
        [SerializeField] private float _shootDuration = 1f;
        [Header("Movement setting")]
        [Min(1)]
        [SerializeField] private float _speedRotation = 1f;
        [Header("Reference")]
        [SerializeField] private Laser _laser;
        [SerializeField] private Rigidbody2D _rotateBody;
        [SerializeField] private Collider2D _collider;

        private Coroutine _coroutine = null;

        public override TrapType DeviceType => TrapType.LaserGun;

        protected override void Awake()
        {
            base.Awake();
            _collider.enabled = !_activateOnStart;
            _laser.SetAttack(attackInfo);
        }
        protected override void OnEnable()
        {
            base.OnEnable();
            if(_activateOnStart)
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
            _coroutine = StartCoroutine(Rotate());
            StartCoroutine(ShootLaser());
        }
        private IEnumerator Rotate()
        {
            yield return new WaitWhile(() => !IsShow);
            var direction = new int[] { -1, 1 };
            int index = Random.Range(0, direction.Length);
            float progress = 0f;
            while (progress < 1f && IsShow)
            {
                _rotateBody.MoveRotation(_rotateBody.rotation + _speedRotation * direction[index]);
                progress += Time.deltaTime / durationWork;
                yield return null;
            }
            _coroutine = null;
            yield return StartCoroutine(ReturnState());
            Deactivate();
            if (destroyMode)
                HideItem();
        }
        private IEnumerator ReturnState()
        {
            while (Mathf.Abs(_rotateBody.rotation) > 0.1f)
            {
                _rotateBody.MoveRotation(Mathf.LerpAngle(_rotateBody.rotation,0,Time.fixedDeltaTime * _speedRotation));
                yield return new WaitForFixedUpdate();
            }
        }

        private IEnumerator ShootLaser()
        {
            yield return new WaitWhile(() => !IsShow);
            float progress = 0f;
            while (progress < 1f && _coroutine != null)
            {
                float localProgress = 0f;
                yield return new WaitWhile(() => 
                {
                    localProgress += Time.deltaTime/ _timeReload;
                    return  localProgress < 1f && _coroutine != null;
                });
                localProgress = 0f;
                _laser.SetMode(true);
                gunAnimator.SetBool("mode", true);
                yield return new WaitWhile(() =>
                {
                    localProgress += Time.deltaTime / _shootDuration;
                    return localProgress < 1f && _coroutine != null;
                });
                _laser.SetMode(false);
                gunAnimator.SetBool("mode", false);
                progress += (_timeReload + _shootDuration) / durationWork;
                yield return null;
            }
        }
    }
}