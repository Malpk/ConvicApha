using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using MainMode;
using MainMode.GameInteface;

namespace PlayerComponent
{
    public class SwitchScreenEffect : Receiver
    {
        private IScreenEffectUI[] _screens;

        public override TypeDisplay DisplayType => TypeDisplay.ScreenUI;

        private void Awake()
        {
            _screens = GetComponentsInChildren<IScreenEffectUI>();
        }

        public void Show(EffectType type, float duration = 0.5f)
        {
            foreach (var screen in _screens)
            {
                if (screen.Type == type)
                {
                    screen.Show(duration);
                }
            }
        }
    }
}