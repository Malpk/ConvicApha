using UnityEngine;
using UnityEngine.Events;

namespace MainMode
{
    public class DetectDeviceSet : MonoBehaviour
    {
        [SerializeField] private bool _playOnAwake;
        [Header("Events")]
        [SerializeField] private UnityEvent<Player> _onDetect;
        [Header("Reference")]
        [SerializeField] private Collider2D _collider;

        protected void Awake()
        {
            _collider.isTrigger = true;
            if (_playOnAwake)
                Play();
            else
                Stop();
        }
        public void Play()
        {
            _collider.enabled = true;
        }
        public void Stop()
        {
            _collider.enabled = false;
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
