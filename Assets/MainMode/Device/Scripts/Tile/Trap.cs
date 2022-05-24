using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace MainMode
{
    public abstract class Trap : Device
    {
        [SerializeField] private bool _destroyMode = true;
        [SerializeField] private float _timeDestroy = 1;

        [SerializeField] protected LayerMask playerLayer;
        
        public abstract EffectType Type { get; }

        private void Awake()
        {
            if(_destroyMode)
                Destroy(gameObject, _timeDestroy);
        }

        protected void SetScreen(Collider2D collision, float duration)
        {
            if (collision.TryGetComponent<PlayerScreen>(out PlayerScreen effect))
            {
                effect.SetEffect(Type, duration);
            }
        }
        protected void SetScreen(Collider2D collision)
        {
            if (collision.TryGetComponent<PlayerScreen>(out PlayerScreen effect))
            {
                effect.SetEffect(Type);
            }
        }
    }
}