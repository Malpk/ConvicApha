using System.Collections.Generic;
using UnityEngine;

namespace PlayerComponent
{
    public class CristalManBehaviour : PlayerBehaviour
    {
        [SerializeField] private List<AttackType> _dangersAttack = new List<AttackType>() { AttackType.Explosion, AttackType.Kinetic };

        //public override void TakeDamage(int damage, DamageInfo type)
        //{
        //    damage *= _dangersAttack.Contains(type.Attack) ? 2 : 1;
        //    base.TakeDamage(damage, type);
        //}
    }
}
