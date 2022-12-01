using UnityEngine;
using System;
using TMPro;

namespace MainMode
{
    public class TimerCountRecord : MonoBehaviour
    {
        [SerializeField] private TimerDisplay _timer;

        private float _progress;

        private event Action<float> OnCompliteTimer;

        private void Awake()
        {
            enabled = false;
        }

        public void Play()
        {
            enabled = true;
            _progress = 0;
  
        }

        public void Stop()
        {
            enabled = false;
            OnCompliteTimer?.Invoke(_progress);
        }

        private void Update()
        {
            _progress += Time.deltaTime;
            _timer.Output((int)_progress);
        }

    }
}