using System.Collections.Generic;
using UnityEngine;

namespace PlayerComponent
{
    [System.Serializable]
    public class PlayerResistContainer : PlayerContainer<DamageInfo>
    {
        [SerializeField] private List<AttackType> _defoutResistAttacks;
        [SerializeField] private List<EffectType> _defoutResistEffects;

        public bool ContainResistAttack(AttackType attact)
        {
            foreach (var reisit in contents)
            {
                if (reisit.content.Attack == attact)
                    return true;
            }
            return _defoutResistAttacks.Contains(attact);
        }
        public bool ContainResistEffect(EffectType effect)
        {
            foreach (var reisit in contents)
            {
                if (reisit.content.Effect == effect)
                    return true;
            }
            return _defoutResistEffects.Contains(effect);
        }
    }
}
