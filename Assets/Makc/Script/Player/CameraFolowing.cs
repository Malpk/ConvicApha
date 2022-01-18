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
        var x = Mathf.Clamp(_target.position.x, -_offset.x, _offset.x);
        var y = Mathf.Clamp(_target.position.y, -_offset.y, _offset.y);
        transform.position = new Vector3(x, y,transform.position.z);
    }
}
