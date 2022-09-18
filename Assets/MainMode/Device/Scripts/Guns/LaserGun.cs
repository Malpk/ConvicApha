using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MainMode
{
    public class LaserGun : AutoGun
    {
        [Header("LaserGun setting")]
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

        private Coroutine _coroutine = null;

        public override TrapType DeviceType => TrapType.LaserGun;
        private void Start()
        {
            if (showOnStart)
                ShowItem();
        }
        protected override void Launch()
        {
            _coroutine = StartCoroutine(Rotate());
            StartCoroutine(ShootLaser());
        }
        private IEnumerator Rotate()
        {
            yield return new WaitWhile(() => !IsShow);
            var direction = new int[] { -1, 1 };
            int index = Random.Range(0, direction.Length);
            float progress = 0f;
            while (progress < 1f && IsActive)
            {
                while (Mathf.Abs(_rotateBody.rotation) <= 360 && IsActive)
                {
                    _rotateBody.MoveRotation(_rotateBody.rotation + _speedRotation * direction[index]);
                    progress += Time.deltaTime / durationWork;
                    yield return null;
                }
                _rotateBody.rotation -= 360 * direction[index];
            }
            _coroutine = null;
            if (IsActive)
            {
                Deactivate();
                yield return ReturnState();
                if (destroyMode)
                    HideItem();
            }
        }
        private IEnumerator ReturnState()
        {
            while (Mathf.Abs(_rotateBody.rotation) != 0)
            {
                _rotateBody.MoveRotation(Mathf.MoveTowards(_rotateBody.rotation, 0, Time.fixedDeltaTime * _speedRotation));
                yield return new WaitForFixedUpdate();
            }
        }

        private IEnumerator ShootLaser()
        {
            yield return new WaitWhile(() => !IsShow);
            float progress = 0f;
            while (progress < 1f && IsActive)
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
                    return localProgress < 1f && IsActive;
                });
                _laser.SetMode(false);
                gunAnimator.SetBool("mode", false);
                progress += (_timeReload + _shootDuration) / durationWork;
                yield return null;
            }
            _laser.SetMode(false);
            gunAnimator.SetBool("mode", false);
        }
    }
}