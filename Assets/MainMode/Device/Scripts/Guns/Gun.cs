using MainMode;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MainMode
{
    [RequireComponent(typeof(Collider2D))]
    public abstract class Gun : Device
    {
        [Header("TrigerSetting")]
        [SerializeField] private bool _isShowTriger;
        [SerializeField] private SpriteRenderer _trigerArean;
        [Header("Reference")]
        [SerializeField] protected DamageInfo attackInfo;
        [SerializeField] protected Animator gunAnimator;
        [SerializeField] private Collider2D _bodyCollider;
        [SerializeField] private SpriteRenderer _spriteRender;

        protected Transform _target;
        

        protected override void Awake()
        {
            base.Awake();
            SetState(false);
            _trigerArean.enabled = false;
        }
        protected override void OnEnable()
        {
            base.OnEnable();
            ShowItemAction += () => _trigerArean.enabled = _isShowTriger;
            HideItemAction += () => _trigerArean.enabled = false;
        }
        protected override void OnDisable()
        {
            base.OnDisable();
            ShowItemAction -= () => _trigerArean.enabled = _isShowTriger;
            HideItemAction -= () => _trigerArean.enabled = false;
        }
        private void Start()
        {
            if (showOnStart)
                ShowItem();
        }
        public override void Activate()
        {
#if UNITY_EDITOR
            if (isActiveDevice)
                throw new System.Exception("gun is already active");
            else if (!IsShow)
                throw new System.Exception("you can't activate a gun that is hide");
#endif
            isActiveDevice = true;
        }
        public override void Deactivate()
        {
#if UNITY_EDITOR
            if(!isActiveDevice)
                throw new System.Exception("gun is already deactive");
#endif
            isActiveDevice = false;
        }
        protected override void ShowDeviceAnimationEvent()
        {
            SetState(true);
            _trigerArean.enabled = _isShowTriger;
        }
        protected override void HideDeviceAnimationEvent()
        {
            SetState(false);
            _trigerArean.enabled = false;
        }
        private void SetState(bool mode)
        {
            _spriteRender.enabled = mode;
            _bodyCollider.enabled = mode;
        }
        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (!isActiveDevice)
            {
                if (collision.TryGetComponent(out Player player))
                {
                    _target = player.transform;
                    Activate();
                }
            }
        }

    }
}