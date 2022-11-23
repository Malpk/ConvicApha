using PlayerComponent;
using MainMode.Items;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MainMode
{
    public class EvilRobot : SmartItem, IDamage,IAddEffects, IExplosion
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
        [SerializeField] private SpriteRenderer _spritebody;
        [SerializeField] private EnemyAnimator _enemyAnimator;

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
            ShowItemAction += () => SetMode(true);
            HideItemAction += () => SetMode(false);
        }
        private void OnDisable()
        {
            ShowItemAction -= () => SetMode(true);
            HideItemAction -= () => SetMode(false);
        }
        private void Start()
        {
            if (_playOnStart)
            {
                ShowItem();
                Activate();
            }
            else
            {
                enabled = false;
                SetMode(false);
            }
        }

        public void SetTarget(Player target)
        {
            _target = target;
        }
        private void Update()
        {
            Rotate();
            MoveToTarget();
        }

        public void Activate()
        {
#if UNITY_EDITOR
            if (!IsShow)
                throw new System.Exception("you can't activate an object while it's hidden");
#endif
            IsActive = true;
            enabled = true;
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
            _enemyAnimator.Effect(2);
        }

        public void TakeDamage(int damage, DamageInfo type)
        {
            if (IsActive && type.Attack != AttackType.Venom)
            {
                _health--;
                if (_health <= 0)
                    Explosion();
                else
                    _enemyAnimator.Effect(1);
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
        private void SetMode(bool mode)
        {
            _readyExlosion = mode;
            _rigidBody.simulated = mode;
            _spritebody.enabled = mode;
            _colliderBody.enabled = mode;
            _enemyAnimator.Effect(0);
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
