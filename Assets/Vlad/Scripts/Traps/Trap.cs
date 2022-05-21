using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace BaseMode
{
    public abstract class Trap : MonoBehaviour
    {
        [SerializeField] private bool _destroyMode = true;
        [SerializeField] private float _timeDestroy = 1;

        [SerializeField] protected LayerMask playerLayer;
        
        public abstract TrapType Type { get; }

        private void Awake()
        {
            if(_destroyMode)
                Destroy(gameObject, _timeDestroy);
        }

        protected void SetScreen(Collider2D collision, float duration)
        {
            if (collision.TryGetComponent<PlayerEffect>(out PlayerEffect effect))
            {
                effect.SetEffect(Type, duration);
            }
        }
    }
}