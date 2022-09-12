using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MainMode
{
    [RequireComponent(typeof(Animator))]
    public abstract class DeviceV2 : SmartItem, IPause
    {
        [Header("Generate")]
        [SerializeField] protected bool destroyMode;
        [SerializeField] protected bool showOnStart;
        [SerializeField] protected float durationWork;

        private Animator _animator;

        protected event System.Action PauseAction;
        protected event System.Action UnPauseAction;
        protected event System.Action CompliteUpAnimation;

        public bool IsPause { get; private set; }

        public abstract bool IsActive { get; }
        public abstract TrapType DeviceType { get; }

        protected virtual void Awake()
        {
            _animator = GetComponent<Animator>();
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
            if (IsActive)
                throw new System.Exception("you can't hide a device that is active");
#endif
            DownDevice();
        }
        private void DownDevice()
        {
            _animator.SetBool("Show", false);
        }
        private void UpDevice()
        {
            _animator.SetBool("Show", true);
        }
        #endregion
        #region Controllers
        public abstract void Activate();
        public abstract void Deactivate();
        public void Pause()
        {
            IsPause = true;
            if (PauseAction != null)
                PauseAction();
        }

        public void UnPause()
        {
            IsPause = false;
            if (UnPauseAction != null)
                UnPauseAction();
        }
        public void Explosion()
        {
            _animator.SetTrigger("Explosion");
        }
        #endregion
        protected abstract void ShowDeviceAnimationEvent();
        protected abstract void HideDeviceAnimationEvent();

        private void DestroyDeviceAnimationEvent()
        {
            if(IsActive)
                Deactivate();
            HideItem();
            DownDevice();
            HideDeviceAnimationEvent();
        }
        protected void CompliteUpAnimationEvent()
        {
            if (CompliteUpAnimation != null)
                CompliteUpAnimation();
        }

    }
}
