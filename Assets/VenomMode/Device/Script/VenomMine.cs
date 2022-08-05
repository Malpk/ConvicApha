using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MainMode;

namespace MainMode.Mode1921
{
    [RequireComponent(typeof(CircleCollider2D),typeof(Animator))]
    public class VenomMine : Device
    {
        [Min(0.05f)]
        [SerializeField] private float _trigerDistance = 0.1f;
        [SerializeField] private DamageInfo _attackInfo;
        [Header("Reference requred")]
        [SerializeField] private FireWave _explosion;
        [SerializeField] private SpriteRenderer _bodySprite;
        [SerializeField] private CircleCollider2D _bodyCollider;

        private Animator _animator;
        private Transform _targetTransform;
        private CircleCollider2D _trigerArea;

        public override TrapType DeviceType => throw new System.NotImplementedException();

        protected override void Intilizate()
        {
            _animator = GetComponent<Animator>();
            _trigerArea = GetComponent<CircleCollider2D>();
            _explosion.SetAttack(_attackInfo);
            _trigerArea.isTrigger = true;
        }

        public void TurnOff()
        {
            SetMode(false);
        }
        protected override void SetState(bool mode)
        {
            _bodySprite.enabled = mode;
            _trigerArea.enabled = mode;
            _bodyCollider.enabled = mode;
            _animator.SetBool("Mode", false);
            if (mode)
                _explosion.SetMode(false);
        }
        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.TryGetComponent<Player>(out Player player))
            {
                _animator.SetBool("Mode", true);
                _targetTransform = player.transform;
            }
        }
        private void OnTriggerStay2D(Collider2D collision)
        {
            if (_targetTransform != null)
            {
                if (Vector2.Distance(_targetTransform.position, transform.position) <= _trigerDistance + _bodyCollider.radius / 2)
                {
                    _targetTransform = null;
                    _explosion.Explosion();
                    SetMode(false);
                }
            }
        }
        private void OnTriggerExit2D(Collider2D collision)
        {
            if (collision.TryGetComponent<Player>(out Player player))
            {
                _animator.SetBool("Mode", false);
                _targetTransform = null;
            }
        }
    }
}