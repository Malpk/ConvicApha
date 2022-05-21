using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace BaseMode
{
    [System.Serializable]
    public class ScreenEffect
    {
        [SerializeField] private TrapType _type;
        [SerializeField] private Sprite _screen;

        public TrapType Type => _type;
        public Sprite Screen => _screen;
    }
}