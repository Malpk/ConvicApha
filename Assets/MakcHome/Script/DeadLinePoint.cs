using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeadLinePoint : MonoBehaviour
{
    [SerializeField] private float _instateDelay;
    [SerializeField] private float _duration;
    [SerializeField] private Vector3 _offset;
    [SerializeField] private SpriteRenderer _deadLinaSprite;

    private int [] _direction = new int[] { 1, -1 }; 
    private bool _isStart;
    private Coroutine _curretCorotine;

    public bool IsStart => _isStart;

    public bool InstateTrap(GameObject createObject, int direction)
    {
        if (_isStart)
            return false;
        _isStart = true;
        _curretCorotine = StartCoroutine(CreateObject(createObject, direction));
        return true;
    }

    private IEnumerator CreateObject(GameObject instateObject, int direction)
    {
        float progress = 0;
        Color color = _deadLinaSprite.color;
        while (progress < 1)
        {
            progress += Time.deltaTime / _duration;
            _deadLinaSprite.color = new Color(color.r, color.g, color.b, 0.4f * progress);
            yield return null;
        }
        _deadLinaSprite.color = new Color(color.r, color.g, color.b,0f);
        int offsetDirection = 0;
        for (int i = 0; i < _direction.Length; i++)
        {
            if (offsetDirection == 0)
            {
                int index = Random.Range(0, _direction.Length);
                offsetDirection = _direction[index];
            }
            else
            {
                offsetDirection *= -1;
            }
            Quaternion rotation = GetRotation(instateObject,direction);
            Instantiate(instateObject, GetInstatePosition(direction,offsetDirection), rotation);
            yield return new WaitForSeconds(_instateDelay);
        }
        _isStart = false;
        yield return null;
    }
    private Vector3 GetInstatePosition(int offsetDirectionX, int offsetDirectionY)
    {
        float x = transform.position.x + _offset.x * offsetDirectionX;
        float y = transform.position.y + _offset.y * offsetDirectionY;
        return Vector3.right *x + Vector3.up * y;
    }
    private Quaternion GetRotation(GameObject instateObject, int direction)
    {
        if (direction < 0)
            return Quaternion.Inverse(instateObject.transform.rotation);
        else
            return instateObject.transform.rotation;
    }
}
