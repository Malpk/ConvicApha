using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollowing : MonoBehaviour
{
    [Header("Genral setting")]
    [SerializeField] private bool _playOnStart;
    [Min(0)]
    [SerializeField] private float _maxOffset = 1;
    [Min(0)]
    [SerializeField] private float _timeSmooth;
    [Min(1)]
    [SerializeField] private float _speedFollowing;
    [Header("Reference")]
    [SerializeField] private Camera _camera;
    [SerializeField] private Character _target;

    private Vector2 _velocity = Vector2.zero;

    public bool IsPlay { private set; get; }
    public Camera Camera => _camera;



    private void OnEnable()
    {
        if(_target)
            _target.RespawnAction += SetDefoutPosition;
    }
    private void OnDisable()
    {
        if (_target)
            _target.RespawnAction -= SetDefoutPosition;
    }

    private void Start()
    {
        if (_playOnStart)
            Play();
    }

    public void SetTarget(Character target)
    {
        if (_target)
            _target.RespawnAction -= SetDefoutPosition;
        _target = target;
        _target.RespawnAction += SetDefoutPosition;
    }

    public void Play()
    {
        if (!IsPlay)
        {
            IsPlay = true;
            StartCoroutine(FollowingTheCamera());
        }
    }

    public void Stop()
    {
        IsPlay = false; 
    }

    private IEnumerator FollowingTheCamera()
    {
        yield return new WaitWhile(() => !_target);
        while (IsPlay)
        {
            if (Vector2.Distance(transform.position, _target.Position) <= _maxOffset)
            {
                Vector2 newPosition = Vector2.SmoothDamp(transform.position, _target.Position, ref _velocity, _timeSmooth, _speedFollowing);
                transform.position = new Vector3(newPosition.x, newPosition.y, transform.position.z);
            }
            else
            {
                transform.position = (Vector3)_target.Position + Vector3.forward * transform.position.z;
            }
            yield return null;
        }
    }
    private void SetDefoutPosition()
    {
        transform.position =new Vector3(_target.transform.position.x, _target.transform.position.y, transform.position.z);
        Play();
    }
}
