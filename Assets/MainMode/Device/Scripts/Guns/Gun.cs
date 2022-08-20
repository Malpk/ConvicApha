using MainMode;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MainMode
{
    [RequireComponent(typeof(Collider2D))]
    public abstract class Gun : Device
    {
        [Header("Reference")]
        [SerializeField] protected DamageInfo attackInfo;
        [SerializeField] protected Animator gunAnimator;
        [SerializeField] protected SignalTile triger;

        [SerializeField] private SpriteRenderer _spriteRender;

        private Collider2D _collider;

        protected override void Intilizate()
        {
            _collider = GetComponent<Collider2D>();
        }
        protected virtual void OnEnable()
        {
            if (!playOnStart)
            {
                triger.SetMode(true);
                triger.SingnalAction += Run;
            }
        }
        protected virtual void OnDisable()
        {
            if (!playOnStart)
            {
                triger.SetMode(false);
                triger.SingnalAction -= Run;
            }
        }
        public abstract void Run(Collider2D collision);

        protected override void SetState(bool mode)
        {
            _spriteRender.enabled = mode;
            _collider.enabled = mode;
            triger.SetMode(mode);
        }
    }
}