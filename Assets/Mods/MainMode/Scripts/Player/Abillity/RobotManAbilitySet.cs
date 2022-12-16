using System.Collections.Generic;
using UnityEngine;
using MainMode;
using MainMode.GameInteface;

namespace PlayerComponent
{
    public class RobotManAbilitySet : AbillityActiveSet
    {
        [SerializeField] private Animator _amimator;
        [Header("Damage")]
        [SerializeField] private int _damage;
        [SerializeField] private Transform _hitLight;
        [SerializeField] private DamageInfo _damageInfo;

        private Transform _parent;

        private List<UpPlatform> _upPlatform = new List<UpPlatform>();

        private void Awake()
        {
            _parent = transform.parent;
        }
        protected override void UseAbility()
        {
            ResetAbillity();
            SetReloadState(true);
            _hitLight.parent = null;
            _amimator.SetTrigger("Hit");
            user.TakeDamage(_damage, _damageInfo);
            foreach (var platform in _upPlatform)
            {
                platform.DeactivateDevice();
                platform.DownDevice();
            }
            enabled = true;
        }
        private void ResetAbillity()
        {
            _hitLight.parent = _parent;
            _hitLight.localPosition = Vector3.zero;
            _hitLight.localRotation = Quaternion.Euler(Vector3.zero);
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.TryGetComponent(out UpPlatform device))
            {
                _upPlatform.Add(device);
            }
        }
        private void OnTriggerExit2D(Collider2D collision)
        {
            if (collision.TryGetComponent(out UpPlatform device))
            {
                _upPlatform.Remove(device);
            }
        }
    }
}