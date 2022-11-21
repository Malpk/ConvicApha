using System.Collections;
using UnityEngine;
using MainMode;
using MainMode.GameInteface;

namespace PlayerComponent
{
    public class RobotManAbilitySet : PlayerAbillityUseSet
    {
        [SerializeField] private int _timeReload;
        [SerializeField] private float _radiusAttack;
        [SerializeField] private float _distanceAttack;
        [SerializeField] private Animator _amimator;
        [SerializeField] private LayerMask _deviceLayer;
        [Header("Damage")]
        [SerializeField] private int _damage;
        [SerializeField] private Sprite _abillityIcon;
        [SerializeField] private Transform _hitLight;
        [SerializeField] private DamageInfo _damageInfo;

        private Transform _parent;

        private void Awake()
        {
            _parent = transform.parent;
        }

        public override void SetHud(HUDUI hud)
        {
            base.SetHud(hud);
            hud.SetAbilityIcon(_abillityIcon);
        }

        protected override void UseAbility()
        {
            SetReloadState(true);
            _hitLight.parent = null;
            _amimator.SetTrigger("Hit");
            user.TakeDamage(_damage, _damageInfo);
            if (TrakingDevice(out DeviceV2 device))
            {
                device.Deactivate();
                device.Hide();
            }
        }
        private void ReloadAnimationEvent()
        {
            _hitLight.parent = _parent;
            _hitLight.localPosition = Vector3.zero;
            _hitLight.localRotation = Quaternion.Euler(Vector3.zero);
            StartCoroutine(Reload());
        }
        private bool TrakingDevice(out DeviceV2 device)
        {
            RaycastHit2D hit = new RaycastHit2D();
            var offset = transform.right * _radiusAttack / 2;
            var cheakPosition = new Vector3[] { transform.position - offset, transform.position, transform.position + offset };
            for (int i = 0; i < cheakPosition.Length && !hit; i++)
            {
                hit = Physics2D.Raycast(cheakPosition[i], transform.up, _distanceAttack, _deviceLayer);
#if UNITY_EDITOR
                Debug.DrawLine(cheakPosition[i], cheakPosition[i] + transform.up * _distanceAttack, Color.green);
#endif
            }
            device = hit ? hit.transform.GetComponent<DeviceV2>() : null;
            return device;
        }
        private IEnumerator Reload()
        {
            int progress = _timeReload;
            while (progress > 0)
            {
                hud.UpdateAbillityKdTimer(progress);
                yield return new WaitForSeconds(1f);
                progress--;
            }
            SetReloadState(false);
        }
    }
}