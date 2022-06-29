using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MainMode
{
    public class FireGun : Gun
    {
        [Header("Reqired component")]
        [SerializeField] private Laser _fire;
        [SerializeField] private Animator _animator;
        [SerializeField] private Animator _fireAnimator;
        [SerializeField] private Transform _signalHolder;

        private SignalTile[] _signals;

        protected override void Intilizate()
        {
            _signals = _signalHolder.GetComponentsInChildren<SignalTile>();
            _fire.SetAttack(AttackInfo);
        }

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


    }
}