using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingWeapon : MonoBehaviour
{
    [SerializeField] private float stopCoolDown;
    [SerializeField] private float moveSpeed;

    [SerializeField] private Vector2 secondPosOffset;
   
    private Vector2 initialPos;
    private Vector2 target;
    private float timeSinceLastMove;

    private void Start()
    {
        initialPos = transform.position;
    }

    private void Update()
    {
        timeSinceLastMove += Time.deltaTime;
        if (Vector2.Distance(transform.position, target) < 0.001)
        {
            target = Vector2.Distance(transform.position, initialPos) < 0.01 ? initialPos + secondPosOffset : initialPos;
            timeSinceLastMove = 0;
        }

        if (timeSinceLastMove > stopCoolDown)
        {
            var dir = (target - (Vector2)transform.position).normalized * (moveSpeed * Time.deltaTime);
            transform.position += (Vector3)dir;
        }
    }
}
