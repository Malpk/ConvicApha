using UnityEngine;

namespace MainMode
{
    public class RocketLauncher : Gun
    {
        [Header("Geneeral Setting")]
        [SerializeField] private bool _smartRocket;
        [SerializeField] private float _aimTime = 1f;
        [SerializeField] private float _speedRotation;
        [Header("Reference")]
        [SerializeField] private SmartProjectale _rocket;
        [SerializeField] private FireWave _wave;
        [SerializeField] private Transform _spawnProjectelePosition;
        [SerializeField] private ShootPoint _shoot;
        [SerializeField] private Rigidbody2D _rotateBody;

        private float _progress = 0f;
        private Vector3 _lostTargetPosition;
        private Transform _target;

        private System.Action State;

        public override TrapType DeviceType => TrapType.RocketLauncher;

        protected void Awake()
        {
            _wave.SetAttack(attackInfo);
            _rocket.SetAttack(attackInfo);
        }
        private void OnEnable()
        {
            _shoot.FireAction += Fire;
        }
        private void OnDisable()
        {
            _shoot.FireAction -= Fire;
        }
        private void Update()
        {
            State();
        }

        protected override void ActivateDevice()
        {
            base.ActivateDevice();
            Launch();
        }

        private void Launch()
        {
            _progress = 0f;
            _target = target.transform;
            State = AimingState;
        }

        private void AimingState()
        {
            _progress += Time.deltaTime / _aimTime;
            var localPOsition = new Vector2(_target.position.x - transform.position.x, _target.position.y - transform.position.y);
            var direction = localPOsition.y > 0 ? 1 : -1;
            _rotateBody.MoveRotation(Quaternion.Lerp(_rotateBody.transform.rotation,
                Quaternion.Euler(Vector3.forward * (-90 + Vector2.Angle(Vector2.right, localPOsition) *
                      direction)), Time.deltaTime * _speedRotation));
            if (_progress >= 1f)
            {
                Shoot();
                _progress = 0f;
                if (target ? !target.IsPlay : true)
                    State = DelayState;
                else
                    Launch();
            }
        }
        private void DelayState()
        {
            _progress += Time.deltaTime;
            if (_progress >= 1f)
            {
                State = ReturnigState;
            }
        }
        private void ReturnigState()
        {
            _rotateBody.MoveRotation(Mathf.MoveTowards(_rotateBody.rotation, 0, _speedRotation));
            if (Mathf.Abs(_rotateBody.rotation) <= 0.01f)
            {
                Deactivate();
            }
        }

        private void Shoot()
        {
            _lostTargetPosition = _rotateBody.transform.position + _rotateBody.transform.up * Vector3.Distance(transform.position, _target.position);
            gunAnimator.SetTrigger("Shoot");
        }
        private void Fire()
        {
            _rocket.transform.position = _spawnProjectelePosition.position;
            _rocket.transform.rotation = Quaternion.Euler(Vector3.forward * _rotateBody.rotation);
            _rocket.Activate(target ? target : lastTarget);
        }
    }
}