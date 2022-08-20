using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MainMode
{
    [RequireComponent(typeof(Collider2D))]
    public abstract class Izolator : Device
    {
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
        }
        private void Start()
        {
            SetDeviceMode(playOnStart);
        }
        public void OnActivateJet()
        {
            if (IsShow)
            {
                SetDeviceMode(true);
                Invoke(nameof(OnDeactivateJet), durationWork);
            }
        }

        private void OnDeactivateJet()
        {
            SetDeviceMode(false);
            if(destroyMode)
                StartCoroutine(DeactivateDevice());
        }
        protected virtual void SetDeviceMode(bool mode)
        {
            isActiveDevice = mode;
            SetJetMode(mode);
        }
        private IEnumerator DeactivateDevice()
        {
            yield return new WaitWhile(() => jets[jets.Length-1].IsActive);
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