using UnityEngine;
using System;

namespace Underworld
{
    public class TImerCount : MonoBehaviour
    {
        [Range(1, 99)]
        [SerializeField] private float _countMinute = 1;
        [SerializeField] private Player _player;
        [SerializeField] private TimerDisplay _timer;

        private int _minuteSeconds = 60;
        private float _progress = 0f;

        public event Action OnCompliteTimer;

        private void Awake()
        {
            enabled = false;
        }

        public void Play()
        {
            enabled = true;
            _progress = _minuteSeconds * _countMinute;
        }
        public void Stop()
        {
            enabled = false;
        }

        private void Update()
        {
            _progress = Mathf.Clamp(_progress - Time.deltaTime, 0, _progress);
            _timer.Output((int)_progress);
            if (_progress == 0 && _player.IsPlay)
            {
                OnCompliteTimer?.Invoke();
            }
        }
    }
}