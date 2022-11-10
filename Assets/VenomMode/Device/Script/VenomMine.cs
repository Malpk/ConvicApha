using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace MainMode.Mode1921
{
    [RequireComponent(typeof(CircleCollider2D))]
    public class VenomMine : SmartItem,IPause
    {
        [SerializeField] private bool _showOnStart;
        [Min(0.05f)]
        [SerializeField] private float _trigerDistance = 0.1f;
        [SerializeField] private DamageInfo _attackInfo;
        [Header("Reference requred")]
        [SerializeField] private FireWave _explosion;
        [SerializeField] private Animator _animator;
        [SerializeField] private SpriteRenderer _bodySprite;
        [SerializeField] private CircleCollider2D _bodyCollider;

        private bool _isPause = false;

        private Transform _targetTransform;
        private CircleCollider2D _trigerArea;

        protected void Awake()
        {
            _trigerArea = GetComponent<CircleCollider2D>();
            _explosion.SetAttack(_attackInfo);
            _trigerArea.isTrigger = true;
            HideMine();
        }
        private void OnEnable()
        {
            ShowItemAction += ShowMine;
            HideItemAction += HideMine;
        }
        private void OnDisable()
        {
            ShowItemAction -= ShowMine;
            HideItemAction -= HideMine;
        }
        private void Start()
        {
            if (_showOnStart)
                ShowItem();
        }
        #region Display Mine
        private void ShowMine()
        {
            _explosion.SetMode(false);
            DisplayMine(true);
        }
        private void HideMine()
        {
            DisplayMine(false);
        }
        protected void DisplayMine(bool mode)
        {
            _bodySprite.enabled = mode;
            _trigerArea.enabled = mode;
            _bodyCollider.enabled = mode;
            _animator.SetBool("Mode", false);
        }
        #endregion
        #region Mode
        public void Pause()
        {
#if UNITY_EDITOR
            if (_isPause)
                throw new System.Exception("device is already pause");
#endif
            _isPause = true;
        }

        public void UnPause()
        {
#if UNITY_EDITOR
            if (_isPause)
                throw new System.Exception("device is already play");
#endif
            _isPause = false;
        }
        #endregion
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
                if (Vector2.Distance(_targetTransform.position, transform.position) < _trigerDistance)
                {
                    _targetTransform = null;
                    _explosion.Explosion();
                    HideItem();
                }
            }
        }
        private void OnTriggerExit2D(Collider2D collision)
        {
            if (_isPause)
                return;
            if (collision.TryGetComponent<Player>(out Player player))
            {
                _animator.SetBool("Mode", false);
                _targetTransform = null;
            }
        }
    }
}