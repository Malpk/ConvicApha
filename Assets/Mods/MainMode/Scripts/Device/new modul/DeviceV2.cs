using UnityEngine;

namespace MainMode
{
    public abstract class DeviceV2 : MonoBehaviour, IExplosion
    {
        [Header("Generate")]
        [SerializeField] private bool _playOnStart;

        private bool _isReadyExlosion = true;

        public event System.Action OnExlosion;
        public event System.Action OnDeactivate;
        public event System.Action OnCompliteWork;

        protected event System.Action OnActivate;
        public bool IsActive { get; private set; }

        public bool IsShow { get; private set; }
        public virtual bool IsCompliteWork => IsActive;
        public abstract TrapType DeviceType { get; }

        public bool IsReadyExplosion => _isReadyExlosion;

        private void Start()
        {
            enabled = false;
            if (_playOnStart)
                Activate();
        }

        #region Controllers
        public void Show()
        {
            IsShow = true;
            _isReadyExlosion = true;
            gameObject.SetActive(true);
            if (_playOnStart)
                Activate();
        }
        public void Hide()
        {
            IsShow = false;
            _isReadyExlosion = false;
            gameObject.SetActive(false);
        }

        public void Activate()
        {
#if UNITY_EDITOR
            if (!IsShow)
                throw new System.Exception("you con't a device that deactive");
#endif
            IsActive = true;
            enabled = true;
            OnActivate?.Invoke();
        }
        public void Deactivate()
        {
            OnDeactivate?.Invoke();
            IsActive = false;
            enabled = false;
        }
        public void CompliteWork()
        {
            OnCompliteWork?.Invoke();
        }
        public void Explosion()
        {
            if (_isReadyExlosion)
            {
                _isReadyExlosion = false;
                OnExlosion?.Invoke();
            }
        }
        #endregion
    }
}
