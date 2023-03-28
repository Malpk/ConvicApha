using System;
using UnityEngine;

namespace MainMode
{
    public class DevicePlatform : MonoBehaviour
    {
        [SerializeField] private bool _showOnStart;
        [SerializeField] private bool _destroyMode;
        [Min(1)]
        [SerializeField] private float _timeActive = 1f;
        [Header("Reference")]
        [SerializeField] private Device _upDevice;
        [SerializeField] private Animator _animator;

        private float _progress = 0f;

        public event Action<DevicePlatform> OnDelete;

        public bool IsShow { get; private set;}

        private void OnValidate()
        {
            if (_upDevice)
            {
                _upDevice.transform.parent = transform;
                _upDevice.transform.localPosition = Vector3.zero;
            }
            name = "UpPlatform" + (_upDevice ? _upDevice.name : "");
        }

        private void Awake()
        {
            _upDevice.gameObject.SetActive(false);
        }

        public void Start()
        {
            if (_showOnStart)
            {
                Show();
            }
            enabled = false;
        }

        private void Update()
        {
            _progress += Time.deltaTime / _timeActive;
            if (_progress >= 1f)
            {
                Hide();
                _progress = 0f;
                enabled = false;
            }
        }

        public void Show()
        {
            SetMode(true);
            IsShow = true;
        }

        public void Hide()
        {
            SetMode(false);
        }
        private void SetMode(bool mode)
        {
            _animator.SetBool("Show", mode);
        }

        private void ShowAnimationEvent()
        {
            _upDevice.gameObject.SetActive(true);
            enabled = _destroyMode;
        }

        private void HideAnimationEvent()
        {
            _upDevice.gameObject.SetActive(false);
            OnDelete?.Invoke(this);
            IsShow = false;
        }
        private void CompliteUpAnimationEvent()
        {
            _upDevice.Play();
        }
    }
}