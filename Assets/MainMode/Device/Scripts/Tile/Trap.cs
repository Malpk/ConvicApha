using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace MainMode
{
    [RequireComponent(typeof(Collider2D))]
    public abstract class Trap : Device
    {
        [SerializeField] protected LayerMask playerLayer;
        public abstract EffectType Type { get; }

        protected override void Intilizate()
        {
            base.Intilizate();  
            if (destroyMode)
                Destroy(gameObject, timeDestroy);
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