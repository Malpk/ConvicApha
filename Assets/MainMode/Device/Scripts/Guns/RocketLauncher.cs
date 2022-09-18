using System.Collections;
using UnityEngine;

namespace MainMode
{
    public class RocketLauncher : Gun, ISetTarget
    {
        [Header("Geneeral Setting")]
        [SerializeField] private float _aimTime = 1f;
        [SerializeField] private float _speedRotation;
        [Header("Reference")]
        [SerializeField] private Rocket _rocket;
        [SerializeField] private FireWave _wave;
        [SerializeField] private Transform _spawnProjectelePosition;
        [SerializeField] private ShootPoint _shoot;
        [SerializeField] private Rigidbody2D _rotateBody;

        private Vector3 _lostTargetPosition;

        private Transform target;

        private Coroutine _delete;
        private Coroutine _coroutine;

        public override TrapType DeviceType => TrapType.RocketLauncher;

        protected override void Awake()
        {
            base.Awake();
            _wave.SetAttack(attackInfo);
            _rocket.SetAttack(attackInfo);
        }
        protected override void OnEnable()
        {
            base.OnEnable();
            _shoot.FireAction += Fire;
            ActivateAction += Launch;
            if (destroyMode)
                ShowItemAction += DeleteGun;
        }
        protected override void OnDisable()
        {
            base.OnDisable();
            _shoot.FireAction -= Fire;
            ActivateAction -= Launch;
            if(destroyMode)
                ShowItemAction -= DeleteGun;
        }
        protected override void Launch()
        {
#if UNITY_EDITOR
            if(!IsActive)
            throw new System.Exception("you can't launch to gun while he is deactivate");
#endif
            if (_coroutine == null)
            {
                _coroutine = StartCoroutine(Rotate(target));
            }
        }
        public void SetTarget(Transform target)
        {
            this.target = target;
        }
        private void DeleteGun()
        {
            if (_delete == null)
            {
                _delete = StartCoroutine(Delete());
            }
        }
        private IEnumerator Delete()
        {
            var progress = 0f;
            while (progress < 1f)
            {
                progress += Time.deltaTime / durationWork;
                yield return null;
            }
            yield return new WaitWhile(() => IsActive);
            if (IsShow)
                HideItem();
            _delete = null;
        }
        private IEnumerator Rotate(Transform target)
        {
            float progress = 0f;
            while (progress < 1f)
            {
                var localPOsition = new Vector2(target.position.x - transform.position.x, target.position.y - transform.position.y);
                var direction = localPOsition.y > 0 ? 1 : -1;
                _rotateBody.MoveRotation(Quaternion.Lerp(_rotateBody.transform.rotation,
                    Quaternion.Euler(Vector3.forward * (-90 + Vector2.Angle(Vector2.right, localPOsition) *
                          direction)), Time.deltaTime * _speedRotation));
                progress += Time.deltaTime / _aimTime;
                yield return null;
            }
            if (IsShow)
            {
                _lostTargetPosition = _rotateBody.transform.position + _rotateBody.transform.up * Vector3.Distance(transform.position, target.position);
                Shoot();
            }
            yield return new WaitForSeconds(1f);
            yield return StartCoroutine(ReturnState());
            _coroutine = null;
        }
        private IEnumerator ReturnState()
        {
            while (Mathf.Abs(_rotateBody.rotation) > 0.01f)
            {
                _rotateBody.MoveRotation(Mathf.MoveTowards(_rotateBody.rotation, 0, _speedRotation));
                yield return null;
            }
        }
        private void Shoot()
        {
            gunAnimator.SetTrigger("Shoot");
        }
        private void Fire()
        {
            if (_wave != null)
                _wave.Explosion();
#if UNITY_EDITOR
            else
                Debug.LogWarning("_wave = null");
#endif
            _rocket.transform.position = _spawnProjectelePosition.position;
            _rocket.transform.rotation = Quaternion.Euler(Vector3.forward * _rotateBody.rotation);
            _rocket.SetMode(true);
            _rocket.SetTarget(_lostTargetPosition);
        }
    }
}