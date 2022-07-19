using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace MainMode.GameInteface
{
    [System.Serializable]
    public class ScreenInfo
    {
        [SerializeField] private Sprite _screen;
        [SerializeField] private RuntimeAnimatorController _controller;

        private EffectType _type;

        public void SetType(EffectType type)
        {
            _type = type;
        }

        public EffectType Type => _type;
        public Sprite Screen => _screen;
        public RuntimeAnimatorController Controller => _controller;
    }
}