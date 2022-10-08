using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MainMode
{
    [RequireComponent(typeof(BoxCollider2D))]
    public class CrystalSheild : SmartItem, IDamage
    {
        [Header("Setting")]
        [SerializeField] private int _endurance;
        [Min(1)]
        [SerializeField] private float _timeDestroy =1f;
        [SerializeField] private SpriteRenderer _sprite;

        private int _curretEndurance;
        private bool _isDeactive;
        private Coroutine _corotine;
        private BoxCollider2D _collider;

        public bool IsDeactive => _isDeactive;
        private void Awake()
        {
            _collider = GetComponent<BoxCollider2D>();
            _collider.isTrigger = false;
            _curretEndurance = _endurance;
        }
        private void OnEnable()
        {
            ShowItemAction += ShowShield;
            HideItemAction += HideShield;
        }
        private void OnDisable()
        {
            HideItemAction -= HideShield;
        }
        private void OnValidate()
        {
            _curretEndurance = _endurance;
        }
        public void Explosion()
        {
            if (IsShow)
                HideItem();
        }
        private void ShowShield()
        {
            if (_corotine == null)
            {
                SetMode(true);
                _curretEndurance = _endurance;
                _corotine = StartCoroutine(Deactivete(_timeDestroy));
            }
        }
        private void HideShield()
        {
            SetMode(false);
            _curretEndurance = 0;
        }

        public void TakeDamage(int damage, DamageInfo type)
        {
            if (_curretEndurance - damage > 0 && _isDeactive)
                _curretEndurance -= damage;
            else
                HideShield();
        }

        private void SetMode(bool mode)
        {
            _isDeactive = mode;
            _sprite.enabled = mode;
            _collider.enabled = mode;
        }
        private IEnumerator Deactivete(float timeDeactivete)
        {
            var progress = 0f;
            while (progress < 1f && IsShow)
            {
                progress += Time.deltaTime / timeDeactivete;
                yield return null;
            }
            Explosion();
            _corotine = null;
        }


    }
}