using System.Collections;
using System;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using System.Threading.Tasks;

public class ItemView : MonoBehaviour
{    
    private float _duration = 1f;
    private Coroutine _coroutine;
    private RectTransform _rectTransform;
    private void Awake()
    {
        DOTween.RewindAll();
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


    private IEnumerator Move(Vector3 endValue, Action OnComplete)
    {
        transform.DOLocalMove(endValue, _duration);
        yield return new WaitForSeconds(_duration);
        _coroutine = null;
        OnComplete?.Invoke();
    }
    private IEnumerator Move(Vector3 endValue)
    {
        transform.DOLocalMove(endValue, _duration);
        yield return new WaitForSeconds(_duration);

    }

}