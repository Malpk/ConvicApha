using UnityEngine;

namespace MainMode
{
    public class AimShootSet : MonoBehaviour
    {
        [SerializeField] private bool _playOnStart;
        [Header("Настройки Аима")]
        [Min(1)]
        [SerializeField] private float _returnTime = 1f;
        [Min(1)]
        [SerializeField] private float _maxTimeAiming = 1;
        [Min(0)]
        [SerializeField] private float _speedAimingDelta = 1f;
        [Header("Настройка урона")]
        [SerializeField] private DamageInfo _damageType;
        [Header("Ссылки на компоненты")]
        [SerializeField] private Pool _pool;
        [SerializeField] private Player _target;
        [SerializeField] private Bullet _projectile;
        [SerializeField] private Transform _shootPoint;

        private bool _isPlay;
        private float _progress = 0f;
        private System.Action State;
        private Player _curretTarget;

        private void Awake()
        {
            Stop();
        }

        private void FixedUpdate()
        {
            State();
        }

        public void SetTarget(Player target)
        {
            _target = target;
            _curretTarget = target;
            enabled = _isPlay && _curretTarget;
        }

        public void Play()
        {
            _curretTarget = _target;
            _isPlay = true;
            _progress = 0f;
            enabled = _isPlay && _curretTarget;
            State = AimingState;
        }

        public void Stop()
        {
            _isPlay = false;
            enabled = false;
        }

        private void AimingState()
        {
            _progress += Time.deltaTime / _maxTimeAiming;
            transform.up = GetDirection();
            if (_progress >= 1f)
            {
                Shoot();
                _progress = 0f;
                _curretTarget = null;
                if (!_target)
                {
                    State = ReturnState;
                }
                else
                {
                    Play();
                }
            }

        }

        private Vector2 GetDirection()
        {
            var localPosition = (Vector2)(_curretTarget.transform.position - transform.position);
            var curretTarget = transform.up *
                Vector2.Distance(transform.position, _curretTarget.transform.position);
            curretTarget = Vector2.MoveTowards(curretTarget, localPosition,
                _speedAimingDelta * Time.fixedDeltaTime);
            return curretTarget;
        }

        private void ReturnState()
        {
            _progress += Time.fixedDeltaTime / _returnTime;
            transform.up = Vector3.Lerp(transform.up, Vector2.up, _progress);
            if (_progress >= 1f)
            {
                enabled = false;
            }
            else if(_target)
            {
                _progress = 0f;
                State = AimingState;
            }
        }

        private void Shoot()
        {
            if (_pool.Create(_projectile).TryGetComponent(out Bullet projectile))
            {
                projectile.transform.up = _shootPoint.up;
                projectile.transform.position = _shootPoint.position;
                projectile.SetAttack(_damageType);
                projectile.Shoot();
            }
        }
    }
}