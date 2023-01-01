using UnityEngine;

namespace MainMode
{
    public class UpPlatform : MonoBehaviour
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
        public bool IsShow { get; private set; }

        protected virtual void Awake()
        {
            _upDevice.gameObject.SetActive(false);
            _animator = GetComponent<Animator>();
            _upDevice.OnExlosion += Explosion;
            _upDevice.OnCompliteWork += DownDevice;
        }
        private void OnDestroy()
        {
            _upDevice.OnExlosion -= Explosion;
            _upDevice.OnCompliteWork -= DownDevice;
        }
        private void Start()
        {
            if (_showOnStart)
                UpDevice();
        }
        private void Update()
        {
            _progress += Time.deltaTime / _workTime;
            if (_progress >= 1 && _upDevice.IsCompliteWork)
                DownDevice();
        }
        public void UpDevice()
        {
            _animator.enabled = true;
            gameObject.SetActive(true);
            _progress = 0f;
            IsShow = true;
            enabled = _destroyMode;
            IsReadyExplosion = true;
            _animator.SetBool("Show", true);
            if (!_animator.enabled)
                _upDevice.Show();
        }
        public void DownDevice()
        {
            enabled = false;
            IsReadyExplosion = false;
            _animator.SetBool("Show", false);
            if (!_animator.enabled)
                HideAnimationEvent();
        }
        public void ActivateDevices()
        {
            _upDevice.Activate();
        }
        public void DeactivateDevice()
        {
            _upDevice.Deactivate();
        }
        private void Explosion()
        {
            if(_animator.enabled)
                _animator.SetTrigger("Explosion");
            DownDevice();
        }
        private void ShowAnimationEvent()
        {
            _upDevice.Show();
        }
        private void HideAnimationEvent()
        {
            _upDevice.Hide();
            _animator.enabled = false;
            gameObject.SetActive(false);
            IsShow = false;
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
            _animator.enabled = false;
            gameObject.SetActive(false);
            _upDevice.Hide();
        }
        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.TryGetComponent(out Player player))
            {
                if(_upDevice.IsShow)
                    _upDevice.Activate();
            }
        }
    }
}
