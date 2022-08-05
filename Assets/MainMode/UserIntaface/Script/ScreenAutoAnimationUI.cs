using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace MainMode.GameInteface
{
    [RequireComponent(typeof(Animator))]
    public class ScreenAutoAnimationUI : MonoBehaviour, IScreenEffectUI
    {
        [SerializeField] private EffectType _type;
        [SerializeField] private Image _screen;

        private Animator _animator;

        public EffectType Type => _type;

        private void Awake()
        {
            _screen.enabled = false;
            _animator = GetComponent<Animator>();
        }

        public void Show(float timeDeactive)
        {
            if(!_screen.enabled)
                SetMode(true);
        }
        public void Hide()
        {
            SetMode(false);
        }

        private void SetMode(bool mode)
        {
            _screen.enabled = mode;
            _animator.SetBool("Show", mode);
        }

        public void Show()
        {
            Show(0);
        }
    }
}