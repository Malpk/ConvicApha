using UnityEngine;
using MainMode.GameInteface;

namespace PlayerComponent
{
    public abstract class PlayerAbillityUseSet : MonoBehaviour, IPlayerAbillitySet
    {
        [Min(1)]
        [SerializeField] private int _timeReload = 1;

        protected Player user;
        protected HUDUI hud;

        protected float _progress = 0f;

        public bool IsActive { get; protected set; }
        public bool IsReload { get; private set; }

        private System.Action<bool> OnReloadUpdate;


        public virtual void SetHud(HUDUI hud)
        {
            this.hud = hud;
            hud.DisplayStateAbillity(!IsReload);
        }

        public void SetUser(Player player)
        {
            user = player;
        }

        public void Use()
        {
            if (!IsReload)
            {
                IsActive = true;
                UseAbility();
            }
        }
        protected abstract void UseAbility();

        protected void SetReloadState(bool reload)
        {
            IsReload = reload;
            OnReloadUpdate?.Invoke(reload);
            hud.DisplayStateAbillity(!reload);
        }
        protected void ReloadUpdate()
        {
            _progress += Time.fixedDeltaTime / _timeReload;
            hud.UpdateAbillityKdTimer(_timeReload - _timeReload * _progress);
            if (_progress >= 1f)
            {
                enabled = false;
                SetReloadState(false);
                hud.DisplayStateAbillity(true);
            }
        }
    }
}
