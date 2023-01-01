using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MainMode
{
    public class FireGun : Gun
    {
        [SerializeField] private float _speedRotation;
        [SerializeField] private Laser _fire;
        [SerializeField] private Rigidbody2D _fireGun;
        [SerializeField] private ParticleSystem _fireParticale;

        private bool _particaleMode;

        private int[] _directions = new int[] { -1, 1 };

        public override TrapType DeviceType => TrapType.FireGun;

        protected void Awake()
        {
            _fireParticale.Pause();
            _fire.SetAttack(attackInfo);
        }

        private IEnumerator Rotate()
        {
            yield return new WaitWhile(() => !IsShow);
            _fireParticale.Play();
            var progress = 0f;
            var direction = _directions[Random.Range(0, _directions.Length)];
            _fireGun.rotation = 181;
            while (progress <= 1f && IsActive)
            {
                //progress += Time.deltaTime / timeActive;
                _fireGun.MoveRotation(_fireGun.rotation + direction * _speedRotation * Time.deltaTime);
                yield return null;
            }
            yield return ReturnState();
            _fire.SetMode(false);
            _fireParticale.Pause();
            _fireParticale.Clear();
            Deactivate();
        }
        private IEnumerator ReturnState()
        {
            while (Mathf.Abs(_fireGun.rotation) > 0.1f)
            {
                _fireGun.MoveRotation(Mathf.LerpAngle(_fireGun.rotation, 0, Time.fixedDeltaTime * _speedRotation));
                yield return new WaitForFixedUpdate();
            }
        }
        protected override void ActivateDevice()
        {
            base.ActivateDevice();
            _fire.SetMode(true);
        }
    }
}