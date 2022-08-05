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

        public void Show(EffectType type, float duration)
        {
            foreach (var screen in _screens)
            {
                if (screen.Type == type)
                {
                    screen.Show(duration);
                }
            }
        }

        public void Show(EffectType type)
        {
            foreach (var screen in _screens)
            {
                if (screen.Type == type)
                {
                    screen.Show();
                }
            }
        }
        public void Hide(EffectType type)
        {
            foreach (var screen in _screens)
            {
                if (screen.Type == type)
                {
                    screen.Hide();
                }
            }
        }
    }
}