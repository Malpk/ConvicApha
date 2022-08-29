using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MainMode
{
    [RequireComponent(typeof(Collider2D))]
    public abstract class Izolator : Device
    {
        [SerializeField] private float _activateTime;
        [Header("Reference")]
        [SerializeField] protected Transform _jetHolder;
        [SerializeField] protected DamageInfo attackInfo;
        [SerializeField] private SpriteRenderer _body;

        protected Collider2D colider;
        
        protected IJet[] jets;

        protected override void Awake()
        {
            base.Awake();
            colider = GetComponent<Collider2D>();
            jets = GetComponentsInChildren<IJet>();
            foreach (var jet in jets)
            {
                jet.SetAttack(attackInfo);
            }
            SetState(false);
        }
        private void Start()
        {
            if(showOnStart)
                ShowItem();
        }
        public override void Activate()
        {
            if (isActiveDevice)
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
            yield return new WaitForSeconds(timeAcive);
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
            if (!isActiveDevice)
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
            if (isActiveDevice)
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