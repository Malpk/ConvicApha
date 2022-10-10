using MainMode.Effects;
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
        [SerializeField] private Animator _effects;
        [SerializeField] private Collider2D _colliderBody;
        [SerializeField] private Rigidbody2D _rigidBody;
        [SerializeField] private SpriteRenderer _spritebody;

        private int _direction = 1;
        private int _health;
        private bool _readyExlosion;
        private float angle = 0;
        public Dictionary<MovementEffect, int> _debafList = new Dictionary<MovementEffect, int>();

        public bool IsActive { get; private set; }
        public bool ReadyExplosion => _readyExlosion;

        private void OnEnable()
        {
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
                SetMode(false);
            }
        }
        public void Activate()
        {
#if UNITY_EDITOR
            if (IsActive)
                throw new System.Exception("enemy is already activate");
            else if (!IsShow)
                throw new System.Exception("you can't activate an object while it's hidden");
#endif
            IsActive = true;
            _health = _startHealth;
        }
        public void SetTarget(Player target)
        {
            _target = target;
        }
        public void Deactivate()
        {
            if (IsActive)
                IsActive = false;
        }
        public void Explosion()
        {
            _readyExlosion = false;
            Deactivate();
            _effects.SetInteger("State", 2);
        }

        public void TakeDamage(int damage, DamageInfo type)
        {
            if(IsActive && type.Attack != AttackType.Venom)
            {
                _health--;
                if (_health <= 0)
                    Explosion();
                else
                    _effects.SetInteger("State", 1);
            }
        }
        public void AddEffects(MovementEffect effect, float timeActive)
        {
            if (effect.Effect == EffectType.Freez && !_debafList.ContainsKey(effect))
            {
                _debafList.Add(effect, 1);
                StartCoroutine(DeleteMovementEffect(timeActive, effect));
            }
            else if(_debafList.ContainsKey(effect))
            {
                _debafList[effect]++;
            }
        }

        public bool AddEffects(ITransport movement, float timeActive)
        {
            return false;
        }
        private void Update()
        {
            if (IsActive)
            {
                Rotate();
                MoveToTarget();
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
        private void Rotate()
        {
            var localPosition = (Vector3)_target.Position - transform.position;
            _direction = localPosition.y > 0 ? 1 : -1;
            angle = Vector3.Angle(Vector3.right, localPosition) * _direction;
            _rigidBody.MoveRotation(angle);
        }

        private void SetMode(bool mode)
        {
            _readyExlosion = mode;
            _rigidBody.simulated = mode;
            _spritebody.enabled = mode;
            _colliderBody.enabled = mode;
            _effects.SetInteger("State", 0);
        }
        private IEnumerator DeleteMovementEffect(float timeActive, MovementEffect effect)
        {
            yield return new WaitForSeconds(timeActive);
            _debafList[effect]--;
            if (_debafList[effect] <= 0)
                _debafList.Remove(effect);
        }
        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (IsActive)
            {
                if (collision.TryGetComponent(out IDamage target))
                {
                    target.Explosion();
                }
                else if (collision.TryGetComponent(out DeviceV2 device))
                {
                    if(device.IsShow)
                        device.Explosion();
                }
            }
        }


    }
}
