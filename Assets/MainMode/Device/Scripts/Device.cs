using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MainMode
{
    [RequireComponent(typeof(Animator))]
    public abstract class Device : SmartItem,IPause
    {
        [Header("General")]
        [SerializeField] protected bool destroyMode = false;
        [SerializeField] protected bool showOnStart = true;
        [Min(1)]
        [SerializeField] protected float durationWork = 1f;

        protected bool _isPause;
        protected bool isActiveDevice = false;
        protected Animator animator;

        public bool IsActive => isActiveDevice;
        public bool IsPause => _isPause;

        public abstract TrapType DeviceType { get; }

        protected event System.Action CompliteUpAnimation;

        protected virtual void Awake()
        {
            animator = GetComponent<Animator>();
        }
        protected virtual void OnEnable()
        {
            ShowItemAction += ShowDevice;
            HideItemAction += HideDevice;
        }
        protected virtual void OnDisable()
        {
            ShowItemAction -= ShowDevice;
            HideItemAction -= HideDevice;
        }
        #region Dispaly Device
        private void ShowDevice()
        {
            UpDevice();
        }
        private void HideDevice()
        {
#if UNITY_EDITOR
            if (isActiveDevice)
                throw new System.Exception("you can't hide a device that is active");
#endif
            DownDevice();
        }
        private void DownDevice()
        {
            animator.SetBool("Show", false);
        }
        private void UpDevice()
        {
            animator.SetBool("Show", true);
        }
        #endregion
        #region Work Device
        public abstract void Activate();
        public abstract void Deactivate();
        public virtual void UnPause()
        {
#if UNITY_EDITOR
            if (!_isPause)
                throw new System.Exception("Device is already play");
#endif
            _isPause = false;
        }
        public virtual void Pause()
        {
#if UNITY_EDITOR
            if (_isPause)
                throw new System.Exception("Device is already pause");
#endif
            _isPause = true;
        }
        #endregion
        protected abstract void ShowDeviceAnimationEvent();
        protected abstract void HideDeviceAnimationEvent();
        protected void CompliteUpAnimationEvent()
        {
            if (CompliteUpAnimation != null)
                CompliteUpAnimation();
        }
    }
}