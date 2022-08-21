using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MainMode
{
    [RequireComponent(typeof(Animator))]
    public abstract class Device : SpawnItem
    {
        [Header("General")]
#if UNITY_EDITOR
        [SerializeField] protected bool isDebug = false;
#endif
        [SerializeField] protected bool playOnStart = true;

        protected bool isActiveDevice = false;
        protected Animator animator;

        private bool _isMode = false;
        private Coroutine _trakingActive = null;
        private Coroutine _destroy = null;

        public override bool IsShow => _isMode;

        public abstract TrapType DeviceType { get; }

        protected void Awake()
        {
            animator = GetComponent<Animator>();
            UpDevice();
            Intilizate();
        }
        protected abstract void Intilizate();

        public virtual void Show()
        {
            SetMode(true);
        }
        public override void SetMode(bool mode)
        {
            if (mode)
            {
#if UNITY_EDITOR
                if (isDebug)
                    Debug.Log("UpDevice");
#endif
                UpDevice();
            }
            else if (IsShow && _trakingActive == null)
            {
                if (!isActiveDevice)
                {
                    DownDevice();
                }
                else if (_trakingActive == null)
                {
                    _trakingActive = StartCoroutine(TrakingCompliteWorkDevice());
                }
            }
        }
        private IEnumerator TrakingCompliteWorkDevice()
        {
#if UNITY_EDITOR
            if (isDebug)
                Debug.Log("DeviceDownWait");
#endif
            yield return new WaitWhile(() => isActiveDevice);

            DownDevice();
            _trakingActive = null;
        }
        protected virtual void Activate()
        {
            SetState(true);
        }
        private void SetShowState()
        {
            _isMode = true;
        }
        public override void OffItem()
        {
            _destroy = null;
            SetState(false);
            animator.SetTrigger("Drop");
        }

        private void DownDevice()
        {
#if UNITY_EDITOR
            if (isDebug)
                Debug.Log("DownDevice");
#endif
            _isMode = false; 
            animator.SetBool("Show", false);
        }
        private void UpDevice()
        {
            animator.SetBool("Show",true );
        }
        protected abstract void SetState(bool mode);

        private void OnBecameInvisible()
        {;
            SetMode(false);
        }
        private void OnBecameVisible()
        {
            SetMode(true);
        }
    }
}