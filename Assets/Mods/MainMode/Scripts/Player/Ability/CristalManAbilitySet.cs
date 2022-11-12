using MainMode.GameInteface;
using System.Collections;
using UnityEngine;

namespace PlayerComponent
{
    public class CristalManAbilitySet : PlayerAbillityUseSet
    {
        [Header("Abillity")]
        [SerializeField] private int _timeReload;
        [SerializeField] private ReturnPoint _returnPoint;
        [SerializeField] private DamageInfo _returnDamage;
        [SerializeField] private Sprite _teleportIcon;
        [SerializeField] private Sprite _setCristalPointIcon;

        public override void SetHud(HUDUI hud)
        {
            base.SetHud(hud);
            hud.SetAbilityIcon(_setCristalPointIcon);
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
                StartCoroutine(Reload());
                _returnPoint.Deactive(user);
                user.TakeDamage(1, _returnDamage);
                user.transform.position = _returnPoint.Position;
                hud.SetAbilityIcon(_setCristalPointIcon);
            }
        }

        private IEnumerator Reload()
        {
            SetReloadState(true);
            hud.DisplayStateAbillity(false);
            int progress = _timeReload;
            while (progress > 0)
            {
                hud.UpdateAbillityKdTimer(progress);
                yield return new WaitForSeconds(1f);
                progress--;
            }
            SetReloadState(false);
            hud.DisplayStateAbillity(true);
        }
    }
}