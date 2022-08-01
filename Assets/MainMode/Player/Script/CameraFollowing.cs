using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class CameraFollowing : MonoBehaviour
{
    [SerializeField] private float _speedFollowing;
    [SerializeField] private Character _target;

    void Update()
    {
        FollowingTheCamera();
    }
    private void FollowingTheCamera()
    {
        Vector2 newPosition = Vector2.Lerp(transform.position, _target.Position, _speedFollowing * Time.deltaTime);
        transform.position = new Vector3(newPosition.x, newPosition.y, transform.position.z);
    }
}
