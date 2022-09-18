using MainMode;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MainMode
{
    public abstract class Gun : DeviceV2
    {
        [Header("Reference")]
        [SerializeField] protected DamageInfo attackInfo;
        [SerializeField] protected Animator gunAnimator;

        [SerializeField] private Collider2D _bodyCollider;
        [SerializeField] private SpriteRenderer _spriteRender;

        private bool _isActiveDevice;

        public override bool IsActive => _isActiveDevice;

        public event System.Action ActivateAction;
        public event System.Action DeactivateAction;

        protected override void Awake()
        {
            base.Awake();
            HideDeviceAnimationEvent();
        }
        private void Start()
        {
            if (showOnStart)
                ShowItem();
        }
        public void Intilizate(bool destroyMode)
        {
            this.destroyMode = destroyMode;
        }
        public override void Activate()
        {
            if (_isActiveDevice && !IsShow)
            {
#if UNITY_EDITOR
                Debug.LogError("you can't activate a gun that is hide");
#endif
                return;
            }
            _isActiveDevice = true;
            if (ActivateAction != null)
                ActivateAction();
        }
        protected abstract void Launch();
        public override void Deactivate()
        {
#if UNITY_EDITOR
            if(!_isActiveDevice)
                throw new System.Exception("gun is already deactive");
#endif
            _isActiveDevice = false;
            if (DeactivateAction != null)
                DeactivateAction();
        }
        protected override void ShowDeviceAnimationEvent()
        {
            SetState(true);
        }
        protected override void HideDeviceAnimationEvent()
        {
            SetState(false);
        }
        private void SetState(bool mode)
        {
            transform.rotation = Quaternion.Euler(Vector3.zero);
            _spriteRender.enabled = mode;
            _bodyCollider.enabled = mode;
        }
    }
}