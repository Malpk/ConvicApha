using System.Collections.Generic;
using UnityEngine;

public class GroupTransform : MonoBehaviour
{
    [SerializeField] private float _offset;
    [SerializeField] private float _duration;
    [SerializeField] private Rigidbody2D _rigidBody;

    private float _progress = 0f;
    private Vector2 _curretDiration = Vector2.zero;

    private Vector2 _startPosition;

    private Vector2[] _directions = new Vector2[]
    {
        Vector2.right, Vector2.left, Vector2.up, Vector2.down
    };

    private System.Action State;

    private void Awake()
    {
        enabled = false;
    }

    public void Play()
    {
        enabled = true;
        Return();
    }
    public void Stop()
    {
        enabled = false;
    }

    private void FixedUpdate()
    {
        _rigidBody.MovePosition(_startPosition + _curretDiration * _progress * _offset);
        _progress = Mathf.Clamp01(_progress + Time.fixedDeltaTime / _duration);
        if (_progress >= 1)
        {
            _progress = 0f;
            State();
        }
    }

    private void Move()
    {
        _startPosition = _startPosition + _curretDiration * _offset;
        _curretDiration = -_curretDiration;
        State = Return;
    }

    private void Return()
    {
        State = Move;
        _curretDiration = GetDiractions();
        _startPosition = Vector2.zero;
    }

    private Vector2 GetDiractions()
    {
        var list = new List<Vector2>();
        foreach (var direction in _directions)
        {
            if (_curretDiration != direction)
                list.Add(direction);
        }
        return list[Random.Range(0, list.Count)];
    }
}
