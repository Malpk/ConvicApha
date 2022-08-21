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

        protected override void Intilizate()
        {
            colider = GetComponent<Collider2D>();
            jets = GetComponentsInChildren<IJet>();
            foreach (var jet in jets)
            {
                jet.SetAttack(attackInfo);
            }
            SetState(false);
            OnDeactivateJet();
        }
        protected override void Activate()
        {
            if (!isActiveDevice)
            {
                isActiveDevice = true;
                if (destroyMode)
                    StartCoroutine(DeactivateDevice());
                base.Activate();
            }
        }
        public void OnActivateJet()
        {
            if (isActiveDevice)
            {
                SetDeviceMode(true);
                Invoke(nameof(OnDeactivateJet), _activateTime);
            }
        }

        private void OnDeactivateJet()
        {
            SetDeviceMode(false);
        }
        protected virtual void SetDeviceMode(bool mode)
        {
            SetJetMode(mode);
        }
        private IEnumerator DeactivateDevice()
        {
            yield return new WaitWhile(() => !IsShow);
            var progress = 0f;
            while (progress <= 1f && IsShow)
            {
                progress += Time.deltaTime / durationWork;
                yield return null;
            }
            isActiveDevice = false;
            yield return new WaitWhile(() => jets[jets.Length-1].IsActive && IsShow);
            if (isActiveDevice)
                throw new System.Exception("Error");
            SetMode(false);
        }
        protected virtual void OnTriggerEnter2D(Collider2D collision)
        {
            if (attackInfo.Effect == EffectType.None)
                return;
            if (collision.TryGetComponent<PlayerScreen>(out PlayerScreen screen))
            {
                screen.ShowEffect(attackInfo);
            }
        }
        protected virtual void OnTriggerExit2D(Collider2D collision)
        {
            if (attackInfo.Effect == EffectType.None)
                return;
            if (collision.TryGetComponent<PlayerScreen>(out PlayerScreen screen))
            {
                screen.ShowEffect(attackInfo);
            }
        }
        protected override void SetState(bool mode)
        {
            isActiveDevice = mode;
            _body.enabled = mode;
            colider.enabled = mode;
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