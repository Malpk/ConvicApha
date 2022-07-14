using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace MainMode
{
    [RequireComponent(typeof(Collider2D))]
    public abstract class Trap : Device
    {
        [SerializeField] protected LayerMask playerLayer;
        [SerializeField] protected DamageInfo attackInfo;
        protected override void Intilizate()
        {
            base.Intilizate();  
            if (destroyMode)
                Destroy(gameObject, timeDestroy);
        }
        protected void SetScreen(Collider2D collision, DamageInfo attack)
        {
            if (collision.TryGetComponent<PlayerScreen>(out PlayerScreen screen))
            {
                screen.ShowEffect(attackInfo);
            }
        }
        protected void SetScreen(Collider2D collision, EffectType effect)
        {
            if (collision.TryGetComponent<PlayerScreen>(out PlayerScreen screen))
            {
                screen.ShowEffect(effect);
            }
        }
    }
}