using MainMode;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MainMode
{
    public abstract class Gun : DeviceV2
    {
        [Header("TrigerSetting")]
        [SerializeField] private bool _isShowTriger;
        [SerializeField] private bool _playOnStart;

        [SerializeField] protected SignalTile triger;
        [Header("Reference")]
        [SerializeField] protected DamageInfo attackInfo;
        [SerializeField] protected Animator gunAnimator;

        [SerializeField] private Collider2D _bodyCollider;
        [SerializeField] private SpriteRenderer _spriteRender;

        protected bool isActiveDevice;

        public override bool IsActive => isActiveDevice;

        public event System.Action ActivateAction;
        public event System.Action DeactivateAction;

        protected override void Awake()
        {
            base.Awake();
            HideDeviceAnimationEvent();
        }
        protected override void OnEnable()
        {
            base.OnEnable();
            if (_playOnStart)
                CompliteUpAnimation += Activate;
            else
                triger.SingnalAction += Activate;
        }
        protected override void OnDisable()
        {
            base.OnDisable();
            if (_playOnStart)
                CompliteUpAnimation -= Activate;
            else
                triger.SingnalAction -= Activate;
        }
        private void Start()
        {
            if (showOnStart)
                ShowItem();
        }
        public override void Activate()
        {
            if (isActiveDevice && !IsShow)
            {
#if UNITY_EDITOR
                Debug.LogError("you can't activate a gun that is hide");
#endif
                return;
            }
            isActiveDevice = true;
            if (ActivateAction != null)
                ActivateAction();
        }
        public override void Deactivate()
        {
#if UNITY_EDITOR
            if(!isActiveDevice)
                throw new System.Exception("gun is already deactive");
#endif
            isActiveDevice = false;
            if (DeactivateAction != null)
                DeactivateAction();
        }
        protected override void ShowDeviceAnimationEvent()
        {
            SetState(true);
            triger.SetMode(_isShowTriger);
        }
        protected override void HideDeviceAnimationEvent()
        {
            SetState(false);
            triger.SetMode(false);
        }
        private void SetState(bool mode)
        {
            transform.rotation = Quaternion.Euler(Vector3.zero);
            _spriteRender.enabled = mode;
            _bodyCollider.enabled = mode;
        }
    }
}