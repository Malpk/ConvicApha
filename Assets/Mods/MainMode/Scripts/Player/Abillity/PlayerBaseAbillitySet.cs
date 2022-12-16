using UnityEngine;

namespace PlayerComponent
{
    public abstract class PlayerBaseAbillitySet : MonoBehaviour, IPlayerAbillitySet, IPlayerComponent
    {
        [SerializeField] private float _timeReload;

        [SerializeField] protected float progress;
        [Header("Reference")]
        [SerializeField] protected Player user;
        [SerializeField] protected Sprite baseIcon;

        protected System.Action State;

        public event System.Action<bool> OnUpdateState;
        public event System.Action<float> OnReloading;
        public event System.Action<Sprite, bool> OnUpdateIcon;

        private void Start()
        {
            ResetState();
            enabled = user;
        }

        private void FixedUpdate()
        {
            State();
        }

        public void Play()
        {
            enabled = true;
        }

        public void Stop()
        {
            enabled = false;
        }
        public void SetUser(Player user)
        {
            this.user = user;
            ResetState();
        }
        protected void Reloading(System.Action next)
        {
            progress += Time.fixedDeltaTime / _timeReload;
            OnReloading?.Invoke(_timeReload - _timeReload * progress);
            if (progress >= 1)
            {
                progress = 0f;
                next();
            }
        }

        protected void UpdateState(bool mode)
        {
            OnUpdateState?.Invoke(mode);
        }

        protected void UpdateIcon(Sprite sprite, bool active)
        {
            OnUpdateIcon?.Invoke(sprite, active);
        }

        public virtual void ResetState()
        {
            enabled = false;
            UpdateIcon(baseIcon, false);
            UpdateState(false);
            progress = 0f;
        }
    }
}
