using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace MainMode.GameInteface
{
    [RequireComponent(typeof(Animator))]
    public class ScreenHandAnimationUI : MonoBehaviour, IScreenEffectUI
    {
        [SerializeField] private EffectType _type;
        [SerializeField] private Image _screen;


        private int _orders;
        private Animator _animator;

        public EffectType Type => _type;

        private void Awake()
        {
            _screen.enabled = false;
            _animator = GetComponent<Animator>();
        }

        public void Show(float timeDeactive = 0)
        {
            Invoke("Hide", timeDeactive);
            if (_screen.enabled)
            {
                _orders++;
                return;
            }
            _orders = 1;
            SetMode(true);
        }
        private void Hide()
        {
            _orders--;
            if (_orders <= 0)
                SetMode(false);
        }

        private void SetMode(bool mode)
        {
            _screen.enabled = mode;
            _animator.SetBool("Show", mode);
        }
    }
}