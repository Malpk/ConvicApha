using System.Collections.Generic;
using UnityEngine;

namespace PlayerComponent
{
    public class CristalManBehaviour : PlayerBaseBehaviour
    {
        [SerializeField] private List<AttackType> _dangersAttack = new List<AttackType>() { AttackType.Explosion, AttackType.Kinetic };

        public override bool TakeDamage(int damage, DamageInfo type)
        {
            damage *= _dangersAttack.Contains(type.Attack) ? 2 : 1;
            return base.TakeDamage(damage, type);
        }
    }
}
