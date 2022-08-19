using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MainMode
{
    [RequireComponent(typeof(Collider2D))]
    public abstract class Izolator : Device
    {
        [Header("General Setting")]
        [SerializeField] protected float activeTime = 10f;
        [Header("Reference")]
        [SerializeField] protected Transform _jetHolder;
        [SerializeField] protected DamageInfo attackInfo;
        [SerializeField] protected Animator[] animators;
        [SerializeField] private SpriteRenderer _body;

        protected Collider2D colider;
        
        protected IJet[] jets;

        protected override void Intilizate()
        {
            colider = GetComponent<Collider2D>();
            animators = _jetHolder.GetComponentsInChildren<Animator>();
            jets = GetComponentsInChildren<IJet>();
            foreach (var jet in jets)
            {
                jet.SetAttack(attackInfo);
            }
        }
        private void Start()
        {
            SetState(false);
        }
        public void OnActivateJet()
        {
            if (IsShow)
            {
                foreach (Animator animator in animators)
                {
                    animator.SetBool("Mode", true);
                }
                isActiveDevice = true;
                Invoke(nameof(OnDeactivateJet), activeTime);
            }
        }

        private void OnDeactivateJet()
        {
            isActiveDevice = false;
            foreach (Animator animator in animators)
            {
                animator.SetBool("Mode", false);
            }
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
            foreach (var jet in jets)
            {
                jet.SetMode(false);
            }
        }
    }
}