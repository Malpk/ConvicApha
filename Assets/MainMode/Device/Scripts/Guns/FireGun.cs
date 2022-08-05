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

        public override TrapType DeviceType => TrapType.FireGun;

        protected override void Intilizate()
        {
            base.Intilizate();
            _fire.SetAttack(attackInfo);
        }

        public override void Run(Collider2D collision)
        {
            gunAnimator.SetTrigger("Rotate");
        }

    }
}