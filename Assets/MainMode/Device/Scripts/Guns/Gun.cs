using MainMode;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MainMode
{
    [RequireComponent(typeof(Collider2D))]
    public abstract class Gun : Device
    {
        [Header("Help Setting")]
        [SerializeField] private bool _showTrigerTile;
        [Header("Reference")]
        [SerializeField] protected Transform signalHolder;
        [SerializeField] protected DamageInfo attackInfo;
        [SerializeField] protected Animator gunAnimator;
        [SerializeField] private SpriteRenderer _spriteRender;

        private Collider2D _collider;
        protected SignalTile[] signals;

        protected override void Intilizate()
        {
            _collider = GetComponent<Collider2D>();
            signals = signalHolder.GetComponentsInChildren<SignalTile>();
            if (_showTrigerTile)
            {
                foreach (var triger in signals)
                {
                    triger.IsShowTile = _showTrigerTile;
                }
            }
        }

        protected virtual void OnEnable()
        {
            foreach (var signal in signals)
            {
                signal.SingnalAction += Run;
            }
        }

        protected virtual void OnDisable()
        {
            foreach (var signal in signals)
            {
                signal.SingnalAction -= Run;
            }
        }

        public abstract void Run(Collider2D collision);

        protected override void SetState(bool mode)
        {
            _spriteRender.enabled = mode;
            _collider.enabled = mode;
            foreach (var tile in signals)
            {
                tile.SetMode(mode);
            }
        }
    }
}