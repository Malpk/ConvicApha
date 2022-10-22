using UnityEngine;

namespace PlayerComponent
{
    public class JeffAbilitySet : PlayerAbilityPassiveSet
    {
        [SerializeField] private int _healValue;

        protected override void UseAbility()
        {
            user.Heal(_healValue);
        }
    }
}
