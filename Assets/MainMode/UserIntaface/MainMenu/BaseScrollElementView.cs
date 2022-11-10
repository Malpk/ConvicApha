using System;
using System.Collections.Generic;
using UnityEngine;

using System.Collections;

namespace UserIntaface.MainMenu
{
    public class BaseScrollElementView :MonoBehaviour
    {
        protected float _duration = 1f;
        protected Coroutine _coroutine;
        protected RectTransform _rectTransform;

        protected void Awake()
        {
            _rectTransform = GetComponent<RectTransform>();
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
            yield return new WaitForSeconds(_duration);
            _coroutine = null;
            OnComplete?.Invoke();
        }
        protected IEnumerator Move(Vector3 endValue)
        {
            yield return new WaitForSeconds(_duration);
        }

    }
}
