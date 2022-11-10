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

        public void Hide()
        {
            _sceen.enabled = false;
        }

        public void Show()
        {
            _sceen.enabled = true;
        }
    }
}
