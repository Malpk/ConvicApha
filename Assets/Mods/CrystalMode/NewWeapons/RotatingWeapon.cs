using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotatingWeapon : MonoBehaviour
{
    [Header("If want rotate to other way add '-' to number")]
    [SerializeField] private float rotateSpeed;
    private void Update()
    {
        gameObject.transform.Rotate(transform.forward, rotateSpeed * Time.deltaTime);
    }
}
