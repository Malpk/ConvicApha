using UnityEngine;
using MainMode.GameInteface;

namespace PlayerComponent
{
    public abstract class AbilityPassiveSet : PlayerBaseAbillitySet
    {
        public bool IsActive { get; private set; } = false;

        protected float progress;

        private void Awake()
        {
            State = Reloading;
            enabled = false;
        }
        private void Start()
        {
            UpdateIcon(baseIcon, false);
            UpdateState(false);
            State = Reloading;
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
                UpdateState(false);
                State = Reloading;
            }
        }
        protected override void ComplteReload()
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
                State = Reloading;
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
