using UnityEngine;
using UnityEngine.Events;

namespace MainMode
{
    [RequireComponent(typeof(CircleCollider2D))]
    public class DetectDeviceSet : MonoBehaviour
    {
        [SerializeField] private UnityEvent<Player> _onDetect;

        private CircleCollider2D _collider;

        protected void Awake()
        {
            _collider = GetComponent<CircleCollider2D>();
            _collider.isTrigger = true;
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.TryGetComponent(out Player player))
            {
                _onDetect.Invoke(player);
            }
        }

        private void OnTriggerExit2D(Collider2D collision)
        {
            if (collision.TryGetComponent(out Player player))
            {
                _onDetect.Invoke(null);
            }
        }
    }
}
