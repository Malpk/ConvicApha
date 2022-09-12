using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MainMode
{
    [RequireComponent(typeof(Collider2D))]
    public abstract class Izolator : DeviceV2
    {
        [SerializeField] private float _activateTime;
        [Header("Reference")]
        [SerializeField] protected Transform _jetHolder;
        [SerializeField] protected DamageInfo attackInfo;
        [SerializeField] private SpriteRenderer _body;

        protected bool isActiveDevice;
        protected Collider2D colider;
        
        protected IJet[] jets;
        public override bool IsActive => isActiveDevice;


        protected override void Awake()
        {
            base.Awake();
            colider = GetComponent<Collider2D>();
            jets = GetComponentsInChildren<IJet>();
            foreach (var jet in jets)
            {
                jet.SetAttack(attackInfo);
            }
            HideDeviceAnimationEvent();
        }
        private void Start()
        {
            if(showOnStart)
                ShowItem();
        }
        public override void Activate()
        {
            if (IsActive)
                return;
#if UNITY_EDITOR
             if (!IsShow)
                throw new System.Exception("you can't activate a Izolator that is hide");
#endif
            if (destroyMode)
                StartCoroutine(Delete());
            SetDeviceMode(true);
            StartCoroutine(Deactivate(_activateTime));
        }
        private IEnumerator Deactivate(float timeAcive)
        {
            var progress = 0f;
            while (progress < 1f && IsActive)
            {
                progress += Time.deltaTime/ timeAcive;
                yield return null;
            }
            if(IsActive)
                Deactivate();
        }
        private IEnumerator WaitJet()
        {
            var jetActive = new List<IJet>();
            jetActive.AddRange(jets);
            while (jetActive.Count > 0)
            {
                yield return new WaitForSeconds(0.1f);
                var list = new List<IJet>();
                for (int i = 0; i < jetActive.Count; i++)
                {
                    if (jetActive[i].IsActive)
                        list.Add(jetActive[i]);
                }
                jetActive.Clear();
                jetActive = list;
            }
        }
        public override void Deactivate()
        {
#if UNITY_EDITOR
            if (!IsActive)
                throw new System.Exception("Izolator is already deactive");
#endif
            SetDeviceMode(false);
        }

        private IEnumerator Delete()
        {
            yield return new WaitWhile(() => !IsShow);
            var progress = 0f;
            while (progress <= 1f && IsShow)
            {
                progress += Time.deltaTime / durationWork;
                yield return null;
            }
            yield return WaitJet();
            if (IsActive)
                throw new System.Exception("Error");
            HideItem();
        }
        protected virtual void SetDeviceMode(bool mode)
        {
            isActiveDevice = mode;
            SetJetMode(mode);
        }
        #region Display Izolator
        protected override void ShowDeviceAnimationEvent()
        {
            SetState(true);
        }
        protected override void HideDeviceAnimationEvent()
        {
            SetState(false);
        }

        protected void SetState(bool mode)
        {
            _body.enabled = mode;
            colider.enabled = mode;
        }
        #endregion

        protected virtual void OnTriggerEnter2D(Collider2D collision)
        {
            if (attackInfo.Effect == EffectType.None)
                return;
            if (collision.TryGetComponent<PlayerScreen>(out PlayerScreen screen) && isActiveDevice)
            {
                screen.ShowEffect(attackInfo);
            }
        }
        protected virtual void OnTriggerExit2D(Collider2D collision)
        {
            if (attackInfo.Effect == EffectType.None)
                return;
            if (collision.TryGetComponent<PlayerScreen>(out PlayerScreen screen) && isActiveDevice)
            {
                screen.ShowEffect(attackInfo);
            }
        }

        private void SetJetMode(bool mode)
        {
            foreach (var jet in jets)
            {
                jet.SetMode(mode);
            }
        }

    }
}