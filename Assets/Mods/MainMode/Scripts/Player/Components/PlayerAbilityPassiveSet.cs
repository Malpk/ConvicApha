using UnityEngine;
using System.Collections;
using MainMode.GameInteface;

namespace PlayerComponent
{
    public abstract class PlayerAbilityPassiveSet : MonoBehaviour, IPlayerAbillitySet
    {
        [Header("Ability Setting")]
        [Min(1)]
        [SerializeField] private int _timeReload = 1;
        [SerializeField] protected Player user;
        [SerializeField] protected Sprite _abillityIcon;

        public bool IsActive { get; private set; } = false;

        protected float progress;
        protected HUDUI hud;

        private System.Action State;

        public void SetHud(HUDUI hud)
        {
            this.hud = hud;
            hud.SetAbilityIcon(_abillityIcon, false);
        }
        private void Start()
        {
            hud.DisplayStateAbillity(false);
            State = ReloadUpdate;
        }

        private void Update()
        {
            State();
        }
        private void Dealy()
        {
            progress += Time.fixedDeltaTime / 0.1f;
            if (progress >= 1f)
            {
                progress = 0f;
                hud.DisplayStateAbillity(false);
                State = ReloadUpdate;
            }
        }
        private void ReloadUpdate()
        {
            progress += Time.fixedDeltaTime / _timeReload;
            hud.UpdateAbillityKdTimer(_timeReload - _timeReload * progress);
            if (progress >= 1f)
            {
                hud.DisplayStateAbillity(true);
                if (UseAbility())
                {
                    State = Dealy;
                }
                else
                {
                    State = WaitUseAbility;
                }
            }
        }
        private void WaitUseAbility()
        {
            if (UseAbility())
            {
                progress = 0f;
                State = ReloadUpdate;
            }
        }
        public void Activate()
        {
            enabled = true;
        }

        public void Deactivate()
        {
            enabled = false;
        }
        public void SetUser(Player player)
        {
            user = player;
        }

        protected abstract bool UseAbility();
    }
}
