using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace MainMode
{
    [System.Serializable]
    public class ScreenEffect
    {
        [SerializeField] private EffectType _type;
        [SerializeField] private Sprite _screen;

        public EffectType Type => _type;
        public Sprite Screen => _screen;

    }
}