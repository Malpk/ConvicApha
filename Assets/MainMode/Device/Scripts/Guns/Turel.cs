using System.Collections;
using UnityEngine;

namespace MainMode
{
    [RequireComponent(typeof(CapsuleCollider2D))]
    public class Turel : Device
    {

        [Header("Attacks properties")]
        [SerializeField]
        protected float _activeTime = 6f;
        [SerializeField]
        protected float _activationTime = 1f;
        [SerializeField]
        protected float _firingRateOnSeconds = 1f;
        [SerializeField]
        protected GameObject _bulletPrefab;
        [SerializeField]
        protected Transform _spawnTransform;

        [Header("Move properties")]
        [SerializeField]
        protected float _rotationAngleOnSeconds;
        [SerializeField]
        protected Animator _animator;
        [SerializeField] protected Rigidbody2D _rigidbody;
        [SerializeField] private Transform _signalHolder;

        protected bool _isActive;
        private float _time;

        private SignalTile[] _signals;

        public override TrapType DeviceType => TrapType.Turel;

        private void Awake()
        {
            _signals = _signalHolder.GetComponentsInChildren<SignalTile>();
        }
        private void OnEnable()
        {
            foreach (var signal in _signals)
            {
                signal.SingnalAction += ActivateGun;
            }
        }
        private void OnDisable()
        {
            foreach (var signal in _signals)
            {
                signal.SingnalAction -= ActivateGun;
            }
        }


        protected virtual IEnumerator Rotate()
        {
            _time = _firingRateOnSeconds;
            float startAngle = _rigidbody.rotation;
            for (float f = 0; f < _activeTime; f += Time.deltaTime)
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
                _animator.SetTrigger("Shoot");
                Bullet bullet = Instantiate(_bulletPrefab, _spawnTransform.position, _spawnTransform.rotation).GetComponent<Bullet>();
            }
        }

        public virtual void ActivateGun(Collider2D collision)
        {
            if (_isActive)
            {
                return;
            }
            _isActive = true;
            StartCoroutine(Rotate());
        }
    }
}