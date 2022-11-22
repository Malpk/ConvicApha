using UnityEngine;

namespace MainMode
{
    public class LaserGun : Gun
    {
        [Header("LaserGun setting")]
        [Min(1)]
        [SerializeField] private int _countShoot = 1;
        [Range(0.1f, 1f)]
        [SerializeField] private float _timeShoot = 1f;
        [Range(0.1f, 1f)]
        [Min(1)]
        [SerializeField] private float _timeReload = 1f;
        [SerializeField] private float _speedRotation = 0;
        [Header("Reference")]
        [SerializeField] private Laser _laser;
        [SerializeField] private Rigidbody2D _rotateBody;

        private int _count;
        private float _rotate;
        private float _progress = 0f;

        private System.Action State;

        public override TrapType DeviceType => TrapType.LaserGun;

        protected void Awake()
        {
            _laser.SetAttack(attackInfo);
        }

        protected override void Launch()
        {
            _count = 0;
            _progress = 0f;
            var directions = new int[] { -1, 1 };
            _rotate = _speedRotation * directions[Random.Range(0, directions.Length)];
            State = Reloading;
        }
        private void FixedUpdate()
        {
            State();
        }
        private void Reloading()
        {
            _progress += Time.deltaTime / _timeReload;
            _rotateBody.MoveRotation(_rotateBody.rotation + _rotate * Time.fixedDeltaTime);
            if (_progress >= 1)
            {
                SwitchState(true);
                State = Shootiong;
            }
        }
        private void Shootiong()
        {
            _progress += Time.deltaTime / _timeShoot;
            _rotateBody.MoveRotation(_rotateBody.rotation + _rotate * Time.fixedDeltaTime);
            if (_progress >= 1f)
            {
                _count++;
                SwitchState(false);
                if (_count < _countShoot)
                    State = Reloading;
                else if (target == null)
                    State = Compliting;
                else
                    Activate();
            }
        }

        private void Compliting()
        {
            var angle = _rotateBody.rotation + _rotate * Time.fixedDeltaTime;
            if (1 - Mathf.Cos(angle * Mathf.Deg2Rad) < 0.001f)
                Deactivate();
            _rotateBody.MoveRotation(angle);
        }
        private void SwitchState(bool mode)
        {
            _progress = 0f;
            _laser.SetMode(mode);
            gunAnimator.SetBool("mode", mode);
        }

    }
}