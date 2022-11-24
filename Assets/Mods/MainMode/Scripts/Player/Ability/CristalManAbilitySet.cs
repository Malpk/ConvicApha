using MainMode.GameInteface;
using UnityEngine;

namespace PlayerComponent
{
    public class CristalManAbilitySet : PlayerAbillityUseSet
    {
        [Header("Abillity")]
        [SerializeField] private ReturnPoint _returnPoint;
        [SerializeField] private DamageInfo _returnDamage;
        [SerializeField] private Sprite _teleportIcon;
        [SerializeField] private Sprite _setCristalPointIcon;


        public override void SetHud(HUDUI hud)
        {
            base.SetHud(hud);
            hud.SetAbilityIcon(_setCristalPointIcon);
        }
        private void Update()
        {
            ReloadUpdate();
        }
        protected override void UseAbility()
        {
            if (!_returnPoint.IsActive)
            {
                hud.SetAbilityIcon(_teleportIcon);
                _returnPoint.ActiveMode(user.transform.position);
            }
            else
            {
                enabled = true;
                user.TakeDamage(1, _returnDamage);
                user.transform.position = _returnPoint.Position;
                _returnPoint.Deactive(user);
                hud.SetAbilityIcon(_setCristalPointIcon);
            }
        }

    }
}