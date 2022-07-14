using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MainMode;

namespace MainMode.Mode1921
{
    [RequireComponent(typeof(CircleCollider2D),typeof(Animator))]
    public class VenomMine : MonoBehaviour, IMode
    {
        [Header("Setting General")]
        [Min(1)]
        [SerializeField] private float _dangerDistace;
        [Min(0.05f)]
        [SerializeField] private float _trigerDistance = 0.1f;
        [SerializeField] private DamageInfo _attackInfo;
        [Header("Reference requred")]
        [SerializeField] private GameObject _explosion;
        [SerializeField] private Collider2D _bodyCollider;
        [SerializeField] private SpriteRenderer _bodySprite;

        private Animator _animator;
        private Transform _targetTransform;
        private CircleCollider2D _trigerArea;

        private void Awake()
        {
            _animator = GetComponent<Animator>();
            _trigerArea = GetComponent<CircleCollider2D>();
            _trigerArea.isTrigger = true;
        }
        public void TurnOff()
        {
            SetMode(false);
        }

        private void SetMode(bool mode)
        {
            _bodySprite.enabled = mode;
            _trigerArea.enabled = mode;
            _bodyCollider.enabled = mode;
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
                if (Vector2.Distance(_targetTransform.position, transform.position) <= _trigerDistance)
                {
                    _targetTransform = null;
                    Instantiate(_explosion, transform.position, Quaternion.identity).
                        GetComponent<ISetAttack>().SetAttack(_attackInfo);
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