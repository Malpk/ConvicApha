using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace MainMode
{
    [RequireComponent(typeof(Collider2D))]
    public abstract class Trap : Device
    {
        [SerializeField] protected LayerMask playerLayer;
        [SerializeField] protected AttackInfo attackInfo;
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
                effect.SetEffect(attackInfo.Effect, duration);
            }
        }
        protected void SetScreen(Collider2D collision)
        {
            if (collision.TryGetComponent<PlayerScreen>(out PlayerScreen effect))
            {
                effect.SetEffect(attackInfo.Effect);
            }
        }
    }
}