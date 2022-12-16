using UnityEngine;

namespace PlayerComponent
{
    public abstract class AbillityActiveSet : PlayerBaseAbillitySet
    {
        [SerializeField] private bool _isUseRotation = false;

        protected float _progress = 0f;

        public bool IsActive { get; protected set; }
        public bool IsReload { get; private set; } = true;
        public bool IsUseRotation => _isUseRotation;

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
        public override void ResetState()
        {
            base.ResetState();
            State = () => Reloading(ComplteReload);
        }
    }
}
