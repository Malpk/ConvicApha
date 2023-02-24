using UnityEngine;
using UnityEngine.Events;

namespace MainMode
{
    public class GroupController : MonoBehaviour
    {
        [SerializeField] private bool _playOnStart;
        [Min(1)]
        [SerializeField] private float _timeActive = 1f;
        [SerializeField] private Device[] _devices;
        [Header("Events")]
        [SerializeField] private UnityEvent _onStart;
        [SerializeField] private UnityEvent _onStop;

        private float _progress = 0f;


        public bool IsWork { get; private set; } = false;


        public void Play()
        {
            enabled = true;
            foreach (var device in _devices)
            {
                device.Play();
            }
            _progress = 0f;
            _onStart.Invoke();
        }

        public void Stop()
        {
            enabled = false;
            foreach (var device in _devices)
            {
                device.Stop();
            }
            _onStop.Invoke();
        }

        private void FixedUpdate()
        {
            _progress += Time.deltaTime / _timeActive;
            if (_progress > 1f)
            {
                Stop();
            }
        }

    }
}