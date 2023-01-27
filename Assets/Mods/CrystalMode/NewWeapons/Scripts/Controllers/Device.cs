using UnityEngine;
using UnityEngine.Events;

namespace MainMode
{
    public class Device : MonoBehaviour
    {
        [SerializeField] private bool _playOnStart;
        [SerializeField] private TrapType _deviceType;
        [Header("Events")]
        [SerializeField] private UnityEvent _onPlay;
        [SerializeField] private UnityEvent _onStop;

        public TrapType DeviceType => _deviceType;

        private void Start()
        {
            if (_playOnStart)
                Play();
            else
                Stop();
        }

        public void Play()
        {
            _onPlay.Invoke();
        }
        public void Stop()
        {
            _onStop.Invoke();
        }
    }
}