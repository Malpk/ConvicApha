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

        private bool _particaleMode;

        private int[] _directions = new int[] { -1, 1 };

        public override TrapType DeviceType => TrapType.FireGun;

        protected override void Awake()
        {
            base.Awake();
            _fireParticale.Pause();
            _fire.SetAttack(attackInfo);
        }
        private void Start()
        {
            if (showOnStart)
                Activate();
        }
        public override void Activate()
        {
            base.Activate();
            _fire.SetMode(true);
            StartCoroutine(Rotate());
        }
        private IEnumerator Rotate()
        {
            yield return new WaitWhile(() => !IsShow);
            _fireParticale.Play();
            var progress = 0f;
            isActiveDevice = true;
            var direction = _directions[Random.Range(0, _directions.Length)];
            _fireGun.rotation = 181;
            while (progress <= 1f && IsShow)
            {
                progress += Time.deltaTime / durationWork;
                _fireGun.MoveRotation(_fireGun.rotation + direction * _speedRotation * Time.deltaTime);
                yield return new WaitWhile(() => _isPause);
                yield return null;
            }
            yield return ReturnState();
            _fire.SetMode(false);
            _fireParticale.Pause();
            _fireParticale.Clear();
            isActiveDevice = false;
            if(destroyMode)
                HideItem();
        }
        private IEnumerator ReturnState()
        {
            while (Mathf.Abs(_fireGun.rotation) > 0.1f)
            {
                _fireGun.MoveRotation(Mathf.LerpAngle(_fireGun.rotation, 0, Time.fixedDeltaTime * _speedRotation));
                yield return new WaitForFixedUpdate();
            }
        }
        public override void UnPause()
        {
            base.UnPause();
            if (_particaleMode)
                _fireParticale.Play();
        }
        public override void Pause()
        {
            base.Pause();
            _particaleMode = _fireParticale.isPlaying;
            _fireParticale.Pause();
        }
    }
}