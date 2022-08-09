using System;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using System.Collections;

namespace UserIntaface.MainMenu
{
    public class BaseScrollElementView :MonoBehaviour
    {
        protected float _duration = 1f;
        protected Coroutine _coroutine;
        [SerializeField]protected RectTransform _rectTransform;

        protected void Awake()
        {            
           _rectTransform = GetComponent<RectTransform>();
            DOTween.Init(); 
        }
        public void MoveTo(Vector3 endPosition, Action OnComplete)
        {
            if (_coroutine == null)
            {
                _coroutine = StartCoroutine(Move(endPosition, OnComplete));
            }
        }
        public void MoveTo(Vector3 endPosition)
        {
            StartCoroutine(Move(endPosition));
        }

        public void JumpTo(RectTransform transformPlace)
        {
           _rectTransform.localPosition = transformPlace.localPosition;
        }
        protected IEnumerator Move(Vector3 endValue, Action OnComplete)
        {
            transform.DOLocalMove(endValue, _duration);
            yield return new WaitForSeconds(_duration);
            _coroutine = null;
            OnComplete?.Invoke();
        }
        protected IEnumerator Move(Vector3 endValue)
        {
            transform.DOLocalMove(endValue, _duration);
            yield return new WaitForSeconds(_duration);

        }

    }
}
