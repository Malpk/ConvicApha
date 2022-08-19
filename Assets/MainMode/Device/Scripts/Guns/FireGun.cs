using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MainMode
{
    public class FireGun : Gun
    {
        [SerializeField] private float _speedRotation;
        [Header("Reference")]
        [SerializeField] private Laser _fire;
        [SerializeField] private Rigidbody2D _fireGun;
        [SerializeField] private ParticleSystem _fireParticale;

        
        private int[] _directions = new int[] { -1, 1 };

        public override TrapType DeviceType => TrapType.FireGun;

        protected override void Intilizate()
        {
            base.Intilizate();
            _fireParticale.Pause();
            _fire.SetAttack(attackInfo);
        }

        private void Start()
        {
            if(playOnStart)
                Run();
        }

        public override void Run(Collider2D collision = null)
        {
            if (!isActiveDevice)
            {
                _fire.SetMode(true);
                StartCoroutine(Rotate());
            }
        }
        private IEnumerator Rotate()
        {
            yield return new WaitWhile(() => !IsShow);
            _fireParticale.Play();
            var progress = 0f;
            isActiveDevice = true;
            var direction = _directions[Random.Range(0, _directions.Length)];
            _fireGun.rotation = 181;
            while (progress <= 1f)
            {
                progress += Time.deltaTime / durationWork;
                _fireGun.transform.localRotation *= Quaternion.Euler(Vector3.forward * direction * _speedRotation * Time.deltaTime);
                yield return null;
            }
            _fire.SetMode(false);
            _fireParticale.Pause();
            _fireParticale.Clear();
            isActiveDevice = false;
            SetMode(false);
        }
    }
}