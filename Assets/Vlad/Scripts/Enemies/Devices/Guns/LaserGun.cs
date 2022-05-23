using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BaseMode
{
    public class LaserGun : Device
    {
        [Header("Time setting")]
        [Min(1)]
        [SerializeField] private float _durationWork = 1f;
        [Min(1)]
        [SerializeField] private float _timeReload = 1f;
        [Min(1)]
        [SerializeField] private float _shootDuration = 1f;
        [Header("Movement setting")]
        [Min(1)]
        [SerializeField] private float _speedRotation = 1f;
        [Header("Reqired component")]
        [SerializeField] private Laser _laser;
        [SerializeField] private Animator _animator;
        [SerializeField] private Rigidbody2D _rotateBody;
        [SerializeField] private Transform _signalHolder;

        private Coroutine _coroutine = null;
        private SignalTile[] _signals;

        public override TrapType DeviceType => TrapType.LaserGun;

        private void Awake()
        {
            _signals = _signalHolder.GetComponentsInChildren<SignalTile>();
        }

        private void OnEnable()
        {
            foreach (var signal in _signals)
            {
                signal.SingnalAction += Run;
            }
        }

        private void OnDisable()
        {
            foreach (var signal in _signals)
            {
                signal.SingnalAction -= Run;
            }
        }

        private void Run(Collider2D collision)
        {
            if (_coroutine == null)
            {
                _coroutine = StartCoroutine(Rotate());
                StartCoroutine(ShootLaser());
            }
        }

        private IEnumerator Rotate()
        {
            var direction = new int[] { -1, 1 };
            int index = Random.Range(0, direction.Length);
            float progress = 0f;
            while (progress < 1f)
            {
                _rotateBody.MoveRotation(_rotateBody.rotation + _speedRotation * direction[index]);
                progress += Time.deltaTime / _durationWork;
                yield return new WaitForFixedUpdate();
            }
            _coroutine = null;
            yield return StartCoroutine(ReturnState());
        }
        private IEnumerator ReturnState()
        {
            while (Mathf.Abs(_rotateBody.rotation) > 0.1f && _coroutine == null)
            {
                _rotateBody.MoveRotation(Mathf.LerpAngle(_rotateBody.rotation,0,Time.fixedDeltaTime * _speedRotation));
                yield return new WaitForFixedUpdate();
            }
        }

        private IEnumerator ShootLaser()
        {
            float progress = 0f;
            while (progress < 1f && _coroutine != null)
            {
                float localProgress = 0f;
                yield return new WaitWhile(() => 
                {
                    localProgress += Time.deltaTime/ _timeReload;
                    return  localProgress < 1f && _coroutine != null;
                });
                if(_coroutine !=null)
                    SetMode(true);
                localProgress = 0f;
                yield return new WaitWhile(() =>
                {
                    localProgress += Time.deltaTime / _shootDuration;
                    return localProgress < 1f && _coroutine != null;
                });
                SetMode(false);
                progress += (_timeReload + _shootDuration) / _durationWork;
                yield return null;
            }
        }
        private void SetMode(bool mode)
        {
            _animator.SetBool("mode", mode);
        }
    }
}