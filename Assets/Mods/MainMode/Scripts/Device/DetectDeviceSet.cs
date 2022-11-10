using UnityEngine;
using UnityEngine.Events;

namespace MainMode
{
    [RequireComponent(typeof(CircleCollider2D))]
    public class DetectDeviceSet : MonoBehaviour
    {
        [SerializeField] protected Izolator izolator;

        [SerializeField] private UnityEvent _detect;

        private CircleCollider2D _collider;

        protected virtual void Awake()
        {
            _collider = GetComponent<CircleCollider2D>();
            _collider.isTrigger = true;
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.TryGetComponent(out Player player))
            {
                SendMessange(player);
            }
        }
        protected virtual void SendMessange(Player player)
        {
            if (izolator.IsShow && !izolator.IsActive)
            {
                _detect.Invoke();
            }
        }
    }
}
