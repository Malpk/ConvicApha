using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotatingWeapon : MonoBehaviour
{
    [Header("If want rotate to other way add '-' to number")]
    [SerializeField] private float rotateSpeed;
    
    [SerializeField] protected Rigidbody2D rigidbody;
    private void Update()
    {
        rigidbody.MoveRotation(rotateSpeed * Time.deltaTime);
    }
}
