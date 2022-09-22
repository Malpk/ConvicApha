using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollowing : MonoBehaviour
{
    [SerializeField] private bool _playOnStart;
    [SerializeField] private float _speedFollowing;
    [SerializeField] private Camera _camera;
    [SerializeField] private Character _target;

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
            Vector2 velocity = Vector2.zero;
            Vector2 newPosition = Vector2.SmoothDamp(transform.position, _target.Position, ref velocity, Time.deltaTime, _speedFollowing);
            transform.position = new Vector3(newPosition.x, newPosition.y, transform.position.z);
            yield return new WaitForEndOfFrame();
        }
    }
    private void SetDefoutPosition()
    {
        transform.position =new Vector3(_target.transform.position.x, _target.transform.position.y, transform.position.z);
        Play();
    }
}
