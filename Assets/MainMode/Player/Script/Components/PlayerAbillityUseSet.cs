using UnityEngine;
using System.Collections;

namespace PlayerComponent
{
    public abstract class PlayerAbillityUseSet : MonoBehaviour, IPlayerAbillitySet
    {
        protected Player user;

        public abstract bool IsReload { get; }

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
    }
}
