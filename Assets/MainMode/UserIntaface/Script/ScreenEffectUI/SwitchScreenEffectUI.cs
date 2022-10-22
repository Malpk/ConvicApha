using UnityEngine;

namespace MainMode.GameInteface
{
    public class SwitchScreenEffectUI : MonoBehaviour
    {
        private IScreenEffectUI[] _screens;

        private void Awake()
        {
            _screens = GetComponentsInChildren<IScreenEffectUI>();
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