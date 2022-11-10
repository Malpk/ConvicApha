using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace MainMode.Mode1921
{
    [RequireComponent(typeof(Animator),typeof(CanvasGroup))]
    public class Triger : MonoBehaviour,IPause
    {
        [SerializeField] private bool _onStart = true;
        [SerializeField] private Image _triger;
        [SerializeField] private MessangeRepairTest[] _messange;

        private Animator _animator;
        private CanvasGroup _canvas;

        private void Awake()
        {
            _canvas = GetComponent<CanvasGroup>();
            _animator = GetComponent<Animator>();
            _canvas.alpha = 0;
            SortBubble();
        }
        public void SortBubble()
        {
            for (int i = 0; i < _messange.Length; i++)
            {
                for (int j = 0; j < _messange.Length - i - 1; j++)
                {
                    if (_messange[i].Widht > _messange[i + 1].Widht)
                    {
                        var temp = _messange[i];
                        _messange[i] = _messange[i + 1];
                        _messange[i + 1] = temp;
                    }

                }
            }
        }
        private void Start()
        {
            if(_onStart)
                Run();
        }
        #region Pause
        public void Pause()
        {
            _animator.speed = 0f;
        }

        public void UnPause()
        {
            _animator.speed = 1f;
        }
        #endregion
        public void Run()
        {
            _canvas.alpha = 1f;
            _animator.SetBool("Mode", true);
        }
        public void TurnOff()
        {
            _canvas.alpha = 0f;
            _animator.SetBool("Mode", false);
        }
        public MessangeRepairTest GetMessange()
        {
            var curretPosition = Mathf.Abs(_triger.rectTransform.localPosition.x);
            if (curretPosition <= Mathf.Abs(_messange[0].Widht))
                return _messange[0];
            for (int i = 1; i < _messange.Length; i++)
            {
                if (Mathf.Abs(_messange[i - 1].Widht) < curretPosition && curretPosition <= Mathf.Abs(_messange[i].Widht))
                    return _messange[i];
            }
            return null;
        }
    }
}