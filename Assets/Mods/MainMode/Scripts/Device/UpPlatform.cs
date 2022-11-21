using UnityEngine;

namespace MainMode
{
    public class UpPlatform : MonoBehaviour, IExplosion
    {
        [SerializeField] private bool _showOnStart;
        [SerializeField] private bool _playOnStart;
        [SerializeField] private DeviceV2 _upDevice;

        private Animator _animator;

        public TrapType DeviceType => _upDevice.DeviceType;
        public bool IsReadyExplosion { get; private set; } = true;

        protected virtual void Awake()
        {
            _upDevice.gameObject.SetActive(false);
            _animator = GetComponent<Animator>();
        }

        private void Start()
        {
            enabled = false;
            if (_showOnStart)
                UpDevice();
        }
        public void UpDevice()
        {
            IsReadyExplosion = true;
            _animator.SetBool("Show", true);
        }
        public void DownDevice()
        {
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
        private void OnTriggerStay2D(Collider2D collision)
        {
            if (!_upDevice.IsActive)
            {
                if (collision.gameObject.GetComponent<Player>() != null)
                {
                    _upDevice.Activate();
                }
            }
        }
        private void ShowAnimationEvent()
        {
            enabled = true;
            _upDevice.Show();
        }
        private void HideAnimationEvent()
        {
            enabled = false; 
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
