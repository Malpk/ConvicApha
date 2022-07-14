using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollowing : MonoBehaviour
{
    [SerializeField] private float _speedFollowing;
    [SerializeField] private Character _target;

    private void Update()
    {
        if(_target)
            FollowingTheCamera();
    }

    public void SetTarget(Character target)
    {
        _target = target;
    }
    private void FollowingTheCamera()
    {
        Vector2 newPosition = Vector2.Lerp(transform.position, _target.Position, _speedFollowing * Time.deltaTime);
        transform.position = new Vector3(newPosition.x, newPosition.y, transform.position.z);
    }
}
