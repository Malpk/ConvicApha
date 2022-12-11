using UnityEngine;

namespace PlayerComponent
{
    public abstract class AbillityActiveSet : PlayerBaseAbillitySet
    {
        [SerializeField] private bool _isUseRotation = false;

        protected float _progress = 0f;

        public bool IsActive { get; protected set; }
        public bool IsReload { get; private set; }
        public bool IsUseRotation => _isUseRotation;
        private void Start()
        {
            UpdateIcon(baseIcon, true);
            UpdateState(false);
            State = ()=> Reloading(ComplteReload);
        }
        public void Use()
        {
            if (!IsReload)
            {
                IsActive = true;
                _progress = 0f;
                UseAbility();
            }
        }
        protected abstract void UseAbility();

        protected void SetReloadState(bool reload)
        {
            UpdateState(!reload);
            IsReload = reload;
        }
        protected void ComplteReload()
        {
            enabled = false;
            SetReloadState(false);
        }
    }
}
