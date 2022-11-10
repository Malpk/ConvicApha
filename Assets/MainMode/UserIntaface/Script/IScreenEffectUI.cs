using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MainMode.GameInteface
{
    public interface IScreenEffectUI 
    {
        public EffectType Type { get; }
        public void Show();
        public void Hide();
    }
}