using UnityEngine;

namespace MainMode
{
    public abstract class DeviceV2 : MonoBehaviour
    {
        [Header("Generate")]
        [SerializeField] private bool _playOnStart;

        protected event System.Action OnActivate;
        public event System.Action OnDeactivate;

        public bool IsActive { get; private set; }

        public bool IsShow { get; private set; }
        public virtual bool IsCompliteWork => IsActive;
        public abstract TrapType DeviceType { get; }


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
            gameObject.SetActive(true);
            if (_playOnStart)
                Activate();
        }
        public void Hide()
        {
            IsShow = false;
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
        #endregion
    }
}
