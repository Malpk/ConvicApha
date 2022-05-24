using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MainMode
{
    public interface IEffect
    {
        public void SetEffect(EffectType type);
        public void SetEffect(EffectType type, float duration);
        public void ScreenOff(EffectType type);
    }
}