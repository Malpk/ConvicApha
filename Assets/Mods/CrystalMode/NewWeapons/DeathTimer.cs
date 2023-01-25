using System;
using System.Collections;
using System.Collections.Generic;
using MainMode.Mode1921;
using UnityEngine;

public class DeathTimer : MonoBehaviour
{
    public event Action OnWorkFinished;
    [SerializeField] private float maxLifeTime;
    private float lifeTime;
    public bool workFinished => lifeTime > maxLifeTime;

    private void Update()
    {
        lifeTime += Time.deltaTime;
        if (workFinished)
        {
            OnWorkFinished?.Invoke();
        }
    }
}
