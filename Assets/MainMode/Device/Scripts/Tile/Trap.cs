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
        [SerializeField] protected SpriteRenderer _body;

        private Collider2D _collider;

        protected override void Intilizate()
        {
            _collider = GetComponent<Collider2D>();
        }
        private void Start()
        {
            Show();
        }
        protected override void SetState(bool mode)
        {
            _body.enabled = mode;
            _collider.enabled = mode;
        }

        protected void SetScreen(Collider2D collision, DamageInfo attack)
        {
            if (collision.TryGetComponent<PlayerScreen>(out PlayerScreen screen))
            {
                screen.ShowEffect(attackInfo);
            }
        }
    }
}