using UnityEngine;
using MainMode.GameInteface;

namespace PlayerComponent
{
    public abstract class AbilityPassiveSet : PlayerBaseAbillitySet
    {
        public bool IsActive { get; private set; } = false;

       [SerializeField] protected float progress;

        private void Awake()
        {
            enabled = false;
        }
        private void Start()
        {
            UpdateIcon(baseIcon, false);
            UpdateState(false);
            State = () => Reloading(ComplteReload);
        }
        private void Dealy()
        {
            progress += Time.fixedDeltaTime / 0.5f;
            if (progress >= 1f)
            {
                progress = 0f;
                UpdateState(false);
                State = () => Reloading(ComplteReload);
            }
        }
        protected void ComplteReload()
        {
            UpdateState(true);
            if (UseAbility())
            {
                State = Dealy;
            }
            else
            {
                State = WaitUseAbility;
            }
        }
        private void WaitUseAbility()
        {
            if (UseAbility())
            {
                progress = 0f;
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
        protected abstract bool UseAbility();
    }
}
