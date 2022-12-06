using UnityEngine;

namespace PlayerComponent
{
    public abstract class PlayerBaseAbillitySet : MonoBehaviour, IPlayerAbillitySet
    {
        [SerializeField] private float _timeReload;
        [Header("Reference")]
        [SerializeField] protected Player user;
        [SerializeField] protected Sprite baseIcon;

        protected System.Action State;
        
        private float _progress = 0f;

        public event System.Action<bool> OnUpdateState;
        public event System.Action<float> OnReloading;
        public event System.Action<Sprite, bool> OnUpdateIcon;


        private void FixedUpdate()
        {
            State();
        }
        public void SetUser(Player user)
        {
            this.user = user;
        }
        protected void Reloading()
        {
            _progress += Time.fixedDeltaTime / _timeReload;
            OnReloading?.Invoke(_timeReload - _timeReload * _progress);
            if (_progress >= 1)
            {
                _progress = 0f;
                ComplteReload();
            }
        }
        protected abstract void ComplteReload();

        protected void UpdateState(bool mode)
        {
            OnUpdateState?.Invoke(mode);
        }

        protected void UpdateIcon(Sprite sprite, bool active)
        {
            OnUpdateIcon?.Invoke(sprite, active);
        }
    }
}
