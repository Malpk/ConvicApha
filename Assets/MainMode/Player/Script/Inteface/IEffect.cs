using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MainMode
{
    public interface IEffect
    {
        public void ShowEffect(EffectType type);
        public void ShowEffect(AttackInfo attack);
        public void ScreenHide(EffectType type);
    }
}