using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cleaner : MonoBehaviour
{
    [SerializeField] private float _speedRotation;

    private void Update()
    {
        transform.rotation *= Quaternion.Euler(Vector3.forward * _speedRotation * Time.deltaTime);
    }
}
