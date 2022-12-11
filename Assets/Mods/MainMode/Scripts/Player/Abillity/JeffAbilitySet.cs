using UnityEngine;

namespace PlayerComponent
{
    public class JeffAbilitySet : AbilityPassiveSet
    {
        [SerializeField] private int _healValue;

        protected override bool UseAbility()
        {
            return user.Heal(_healValue);
        }
    }
}
