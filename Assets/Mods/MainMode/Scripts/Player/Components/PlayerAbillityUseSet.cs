using UnityEngine;
using MainMode.GameInteface;

namespace PlayerComponent
{
    public abstract class PlayerAbillityUseSet : MonoBehaviour, IPlayerAbillitySet
    {
        protected Player user;
        protected HUDUI hud;

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
    }
}
