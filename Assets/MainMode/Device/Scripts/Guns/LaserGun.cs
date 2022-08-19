using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MainMode
{
    public class LaserGun : Gun
    {
        [Header("Time setting")]
        [Min(1)]
        [SerializeField] private float _timeReload = 1f;
        [Min(1)]
        [SerializeField] private float _shootDuration = 1f;
        [Header("Movement setting")]
        [Min(1)]
        [SerializeField] private float _speedRotation = 1f;
        [Header("Reqired component")]
        [SerializeField] private Laser _laser;
        [SerializeField] private Rigidbody2D _rotateBody;

        private Coroutine _coroutine = null;

        public override TrapType DeviceType => TrapType.LaserGun;

        protected override void Intilizate()
        {
            base.Intilizate();
            _laser.SetAttack(attackInfo);
        }
        private void Start()
        {
            if (playOnStart)
            {
                Run();
            }
        }
        public override void Run(Collider2D target = null)
        {
            if (_coroutine == null)
            {
                isActiveDevice = true;
                _coroutine = StartCoroutine(Rotate());
                StartCoroutine(ShootLaser());
            }
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
            isActiveDevice = false;
            SetMode(false);
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
                gunAnimator.SetBool("mode", true);
                yield return new WaitWhile(() =>
                {
                    localProgress += Time.deltaTime / _shootDuration;
                    return localProgress < 1f && _coroutine != null;
                });
                gunAnimator.SetBool("mode", false);
                progress += (_timeReload + _shootDuration) / durationWork;
                yield return null;
            }
        }
    }
}