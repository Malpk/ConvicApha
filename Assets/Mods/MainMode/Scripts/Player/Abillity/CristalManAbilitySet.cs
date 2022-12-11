using UnityEngine;

namespace PlayerComponent
{
    public class CristalManAbilitySet : AbillityActiveSet
    {
        [Header("Abillity")]
        [SerializeField] private ReturnPoint _returnPoint;
        [SerializeField] private DamageInfo _returnDamage;
        [SerializeField] private Sprite _setCristalPointIcon;

        protected override void UseAbility()
        {
            if (!_returnPoint.IsActive)
            {
                UpdateIcon(_setCristalPointIcon, true);
                _returnPoint.ActiveMode(user.transform.position);
            }
            else
            {
                enabled = true;
                user.TakeDamage(1, _returnDamage);
                user.transform.position = _returnPoint.Position;
                _returnPoint.Deactive(user);
                UpdateIcon(baseIcon,true);
            }
        }

    }
}