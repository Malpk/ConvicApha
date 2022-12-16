using UnityEngine;
using MainMode.GameInteface;

namespace PlayerComponent
{
    public abstract class AbilityPassiveSet : PlayerBaseAbillitySet
    {
        public bool IsActive { get; private set; } = false;

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
        protected abstract bool UseAbility();
        public override void ResetState()
        {
            base.ResetState();
            State = () => Reloading(ComplteReload);
        }

    }
}
