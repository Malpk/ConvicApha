using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lighting : MonoBehaviour
{
    [SerializeField] private int damageValue;
    [SerializeField] private DamageInfo damageInfo;
    [SerializeField] private float maxLiveTime;
    [HideInInspector] public float timeToLive;
    private float liveTime;

    private void Update()
    {
        liveTime += Time.deltaTime;
        if (liveTime > timeToLive || liveTime > maxLiveTime)
        {
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        col.gameObject.GetComponent<IDamage>()?.TakeDamage(damageValue, damageInfo);
    }
}
