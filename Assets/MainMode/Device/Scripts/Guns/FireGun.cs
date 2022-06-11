using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MainMode
{
    public class FireGun : Gun
    {
        [Header("Reqired component")]
        [SerializeField] private Animator _animator;
        [SerializeField] private Transform _signalHolder;

        private SignalTile[] _signals;

        public override TrapType DeviceType => TrapType.FireGun;

        private void OnEnable()
        {
            foreach (var signal in _signals)
            {
                signal.SingnalAction += Run;
            }
        }

        private void OnDisable()
        {
            foreach (var signal in _signals)
            {
                signal.SingnalAction -= Run;
            }
        }

        private void Run(Collider2D collision)
        {
            _animator.SetTrigger("rotate");
        }

        protected override void Intilizate()
        {
            _signals = _signalHolder.GetComponentsInChildren<SignalTile>();
        }
    }
}