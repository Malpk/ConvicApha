using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFolowing : MonoBehaviour
{
    [Header("Scene Setting")]
    [SerializeField] private Transform _target;
    [SerializeField] private Vector2 _offset;
    
    void LateUpdate()
    {
        transform.position = new Vector3(_target.position.x, _target.position.y, transform.position.z);
    }
}
