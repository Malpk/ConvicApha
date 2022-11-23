using System.Collections.Generic;
using UnityEngine;
using MainMode;
using MainMode.GameInteface;

namespace PlayerComponent
{
    public class RobotManAbilitySet : PlayerAbillityUseSet
    {
        [SerializeField] private Animator _amimator;
        [Header("Damage")]
        [SerializeField] private int _damage;
        [SerializeField] private Sprite _abillityIcon;
        [SerializeField] private Transform _hitLight;
        [SerializeField] private DamageInfo _damageInfo;

        private Transform _parent;

        private List<DeviceV2> _devics = new List<DeviceV2>();

        private void Awake()
        {
            _parent = transform.parent;
        }

        public override void SetHud(HUDUI hud)
        {
            base.SetHud(hud);
            hud.SetAbilityIcon(_abillityIcon);
        }

        private void Update()
        {
            ReloadUpdate();
        }
        protected override void UseAbility()
        {
            SetReloadState(true);
            _hitLight.parent = null;
            _amimator.SetTrigger("Hit");
            user.TakeDamage(_damage, _damageInfo);
            foreach (var device in _devics)
            {
                device.Deactivate();
                device.CompliteWork();
            }
            enabled = true;
        }
        private void ReloadAnimationEvent()
        {
            _hitLight.parent = _parent;
            _hitLight.localPosition = Vector3.zero;
            _hitLight.localRotation = Quaternion.Euler(Vector3.zero);
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.TryGetComponent(out DeviceV2 device))
            {
                _devics.Add(device);
            }
        }
        private void OnTriggerExit2D(Collider2D collision)
        {
            if (collision.TryGetComponent(out DeviceV2 device))
            {
                _devics.Remove(device);
            }
        }
    }
}