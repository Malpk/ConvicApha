using System.Collections;
using UnityEngine;

namespace PlayerComponent
{
    public class CristalManAbilitySet : PlayerAbillityUseSet
    {
        [Header("Abillity")]
        [SerializeField] private float _timeReload;
        [SerializeField] private ReturnPoint _returnPoint;
        [SerializeField] private DamageInfo _returnDamage;

        private bool _isReload = false;

        public override bool IsReload => _isReload;

        protected override void UseAbility()
        {
            if (_returnPoint.IsActive)
            {
                StartCoroutine(Reload());
                _returnPoint.Deactive(user);
                user.TakeDamage(1, _returnDamage);
                user.transform.position = _returnPoint.Position;
            }
            else
            {
                _returnPoint.ActiveMode(user.transform.position);
            }
        }

        private IEnumerator Reload()
        {
            _isReload = true;
            yield return new WaitForSeconds(_timeReload);
            _isReload = false;
        }
    }
}