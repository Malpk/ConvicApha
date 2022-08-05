using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace MainMode.GameInteface
{
    public class ScreenDefaoutUI : MonoBehaviour, IScreenEffectUI
    {
        [SerializeField] private EffectType _type;
        [SerializeField] private Image _sceen;

        private int _orders;

        public EffectType Type => _type;

        private void Awake()
        {
            _sceen.enabled = false;
        }

        public void Show(float timeDeactive)
        {
            timeDeactive = Mathf.Abs(timeDeactive);
            Invoke("Hide", timeDeactive);
            Show();
        }
        public void Hide()
        {
            _orders--;
            if (_orders <= 0)
                _sceen.enabled = false;
        }

        public void Show()
        {
            if (_sceen.enabled)
            {
                _orders++;
                return;
            }
            _orders = 1;
            _sceen.enabled = true;
        }
    }
}
