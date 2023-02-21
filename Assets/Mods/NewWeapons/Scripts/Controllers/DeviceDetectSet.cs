using UnityEngine;
using UnityEngine.Events;

namespace MainMode
{
    public class DeviceDetectSet : MonoBehaviour
    {
        [SerializeField] private bool _playOnAwake;
        [Header("Events")]
        [SerializeField] private UnityEvent<Player> _onTrigerEnter;
        [SerializeField] private UnityEvent<Player> _onTrigerExit;
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
                _onTrigerEnter.Invoke(player);
            }
        }

        private void OnTriggerExit2D(Collider2D collision)
        {
            if (collision.TryGetComponent(out Player player))
            {
                _onTrigerExit.Invoke(null);
            }
        }
    }
}
