using UnityEngine;

namespace PlayerComponent
{
    public class JeffAbilitySet : PlayerAbilityPassiveSet
    {
        [SerializeField] private int _healValue;

        protected override bool UseAbility()
        {
            user.Heal(_healValue);
            return true;
        }
    }
}
