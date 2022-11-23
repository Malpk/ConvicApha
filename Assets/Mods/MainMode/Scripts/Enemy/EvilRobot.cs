using PlayerComponent;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MainMode
{
    public class EvilRobot : MonoBehaviour, IDamage,IAddEffects, IExplosion
    {
        [Header("General")]
        [SerializeField] private bool _playOnStart;
        [Min(0)]
        [SerializeField] private int _startHealth;
        [Range(0.1f, 1)]
        [SerializeField] private float _minSpeedMovement = 0.2f;
        [SerializeField] private float _speedMovement;
        [SerializeField] private float _minDistanceToTarget;
        [Header("Reference")]
        [SerializeField] private Player _target;
        [SerializeField] private Collider2D _colliderBody;
        [SerializeField] private Rigidbody2D _rigidBody;
        [SerializeField] private EnemyAnimator _enemyAnimator;
        [SerializeField] private EffectAnimator _effectAnimator;

        private int _direction = 1;
        private int _health;
        private bool _readyExlosion;
        private float angle = 0;
        public Dictionary<MovementEffect, int> _debafList = new Dictionary<MovementEffect, int>();

        public bool IsActive { get; private set; }

        private IDamage _hitTarget;

        public bool IsReadyExplosion => _readyExlosion;

        private void OnEnable()
        {
            _enemyAnimator.OnHit += Hit;
            _effectAnimator.OnExplosion +=()=> SetMode(false);
        }
        private void OnDisable()
        {
            _enemyAnimator.OnHit -= Hit;
            _effectAnimator.OnExplosion -=()=> SetMode(false);
        }
        private void Start()
        {
            if (_playOnStart && _target)
            {
                SetMode(true);
                Activate();
            }
            else
            {
                SetMode(false);
            }
        }

        public void SetTarget(Player target)
        {
            _target = target;
            if (_playOnStart)
            {
                SetMode(true);
                Activate();
            }
            else
            {
                SetMode(false);
            }
        }
        private void Update()
        {
            Rotate();
            MoveToTarget();
        }

        public void Activate()
        {
            IsActive = true;
            _health = _startHealth;
        }
        public void Deactivate()
        {
            IsActive = false;
            enabled = false;
        }
        public void Explosion()
        {
            _readyExlosion = false;
            Deactivate();
            _effectAnimator.Effect(2);
        }

        public void TakeDamage(int damage, DamageInfo type)
        {
            if (IsActive && type.Attack != AttackType.Venom)
            {
                _health--;
                if (_health <= 0)
                    Explosion();
                else
                    _effectAnimator.Effect(1);
            }
        }

        private void MoveToTarget()
        {
            if (Vector2.Distance(_rigidBody.position, _target.Position) > _minDistanceToTarget)
            {
                var move = (Vector2)transform.right * _speedMovement * Time.fixedDeltaTime * GetEffects();
                _rigidBody.MovePosition(_rigidBody.position + move);
            }
        }

        private void Rotate()
        {
            var localPosition = (Vector3)_target.Position - transform.position;
            _direction = localPosition.y > 0 ? 1 : -1;
            angle = Vector3.Angle(Vector3.right, localPosition)  * _direction ;
            _rigidBody.MoveRotation(angle);
        }
        private float GetEffects()
        {
            var amount = 1f;
            if (_debafList.Count > 0)
            {
                foreach (var debaf in _debafList)
                {
                    amount *= debaf.Key.Value;
                }
            }
            return amount > _minSpeedMovement ? amount : _minSpeedMovement;
        }
        public void AddEffects(MovementEffect effect, float timeActive)
        {
            if (effect.Effect == EffectType.Freez && !_debafList.ContainsKey(effect))
            {
                _debafList.Add(effect, 1);
                StartCoroutine(DeleteMovementEffect(timeActive, effect));
            }
            else if (_debafList.ContainsKey(effect))
            {
                _debafList[effect]++;
            }
        }
        public void SetMode(bool mode)
        {
            enabled = mode;
            _readyExlosion = mode;
            _rigidBody.simulated = mode;
            _colliderBody.enabled = mode;
            _effectAnimator.Effect(0);
            _enemyAnimator.gameObject.SetActive(mode);
        }
        private IEnumerator DeleteMovementEffect(float timeActive, MovementEffect effect)
        {
            yield return new WaitForSeconds(timeActive);
            _debafList[effect]--;
            if (_debafList[effect] <= 0)
                _debafList.Remove(effect);
        }
        public void Hit()
        {
            if(_hitTarget != default(IDamage))
                _hitTarget.Explosion();
        }
        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (IsActive)
            {
                if (collision.TryGetComponent(out IDamage target))
                {
                    _enemyAnimator.Hit();
                    _hitTarget = target;
                }
                else if (collision.TryGetComponent(out IExplosion device))
                {
                    if(device.IsReadyExplosion)
                        device.Explosion();
                }
            }
        }
        private void OnTriggerExit2D(Collider2D collision)
        {
            if (collision.TryGetComponent(out IDamage target))
            {
                _hitTarget = default(IDamage);
            }
        }

    }
}
