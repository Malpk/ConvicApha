using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class DeadlinePoint : MonoBehaviour
{
    private bool _isStart;
    private SpriteRenderer _deadLinaSprite;

    private int[] _offsetDirection = new int[] { 1, -1 };

    public bool isBusy => _isStart;

    private void Awake()
    {
        _deadLinaSprite = GetComponent<SpriteRenderer>();
    }

    public bool TurnOn(TridentSetting trident,int angel,float instateDelay)
    {
        if (!_isStart)
        {
            _isStart = true;
            StartCoroutine(Indicator(trident,angel,instateDelay));
            return true;
        }
        else 
        {
            return false;
        }
    }
    private IEnumerator Indicator(TridentSetting trident, int angel,float instateDelay)
    {
        yield return StartCoroutine(Indicator(trident.startDelay));
        foreach (var direction in _offsetDirection)
        {
            Quaternion rotation = Quaternion.Euler(Vector3.forward * angel);
            var offset = new Vector3(trident.OffSet.x * direction, trident.OffSet.y);
            var instatePosition = transform.position + RotateVector(offset, angel);
            Instantiate(trident.InstateObject, instatePosition, rotation);
            yield return new WaitForSeconds(instateDelay);
        }
        _isStart = false;
        yield return null;
    }
    private IEnumerator Indicator(float duration)
    {
        float progress = 0;
        Color color = _deadLinaSprite.color;
        while (progress < 1)
        {
            progress += Time.deltaTime / duration;
            _deadLinaSprite.color = new Color(color.r, color.g, color.b, 0.4f * progress);
            yield return null;
        }
        _deadLinaSprite.color = new Color(color.r, color.g, color.b, 0f);
        yield return null;
    }
    private Vector3 RotateVector(Vector3 offset, float angel)
    {
        angel *= Mathf.Deg2Rad;
        float xOffset = offset.x * Mathf.Cos(angel) - offset.y * Mathf.Sin(angel);
        float yOffset = offset.x * Mathf.Sin(angel) + offset.y * Mathf.Cos(angel);
        return new Vector3(xOffset, yOffset);
    }

}

