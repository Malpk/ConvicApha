using System.Collections;
using System;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using MainMode.Items;

public class ItemView : MonoBehaviour
{
    protected float _duration = 1f;
    protected Coroutine _coroutine;
    protected RectTransform _rectTransform;
    public GameObject itemPrefab;
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