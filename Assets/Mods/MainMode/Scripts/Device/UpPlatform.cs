using UnityEngine;

namespace MainMode
{
    public class UpPlatform : MonoBehaviour, IExplosion
    {
        [SerializeField] private bool _showOnStart;
        [SerializeField] private bool _playOnStart;
        [SerializeField] private bool _destroyMode;
        [Min(1)]
        [SerializeField] private float _workTime = 1f;
        [SerializeField] private DeviceV2 _upDevice;

        private float _progress = 0f;
        private Animator _animator;

        public TrapType DeviceType => _upDevice.DeviceType;
        public bool IsReadyExplosion { get; private set; } = true;
        public bool IsShow => _upDevice.IsShow;

        protected virtual void Awake()
        {
            _upDevice.gameObject.SetActive(false);
            _animator = GetComponent<Animator>();
        }

        private void Start()
        {
            if (_showOnStart)
                UpDevice();
        }
        private void Update()
        {
            _progress += Time.deltaTime / _workTime;
            if (_progress >= 1 && !_upDevice.IsCompliteWork)
                DownDevice();
        }
        public void UpDevice()
        {
            _progress = 0f;
            enabled = _destroyMode;
            IsReadyExplosion = true;
            _animator.SetBool("Show", true);
        }
        public void DownDevice()
        {
            enabled = false;
            IsReadyExplosion = false;
            _animator.SetBool("Show", false);
        }

        public void Explosion()
        {
            if (IsReadyExplosion)
            {
                IsReadyExplosion = false;
                _animator.SetTrigger("Explosion");
            }
        }
        private void ShowAnimationEvent()
        {
            _upDevice.Show();
        }
        private void HideAnimationEvent()
        {
            _upDevice.Hide();
        }
        private void CompliteUpAnimationEvent()
        {
            if (_playOnStart)
                _upDevice.Activate();
        }
        private void DestroyDeviceAnimationEvent()
        {
            if (_upDevice.IsActive)
                _upDevice.Deactivate();
            _upDevice.Hide();
        }
    }
}
