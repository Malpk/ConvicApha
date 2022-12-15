using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBullet : MonoBehaviour
{
    [SerializeField] private DamageInfo damageInfo;
    [SerializeField] private int damageValue;
    [SerializeField] private float maxLifeTime;
    private float lifeTime;
    private void OnCollisionEnter2D(Collision2D col)
    {
        if (col.gameObject.GetComponent<IDamage>() != null)
        {
            col.gameObject.GetComponent<IDamage>().TakeDamage(damageValue, damageInfo);
        }

        if (!col.gameObject.GetComponent<PlayerBullet>())
        {
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.GetComponent<IDamage>() != null)
        {
            col.gameObject.GetComponent<IDamage>().TakeDamage(damageValue, damageInfo);
        }

        if (!col.gameObject.GetComponent<PlayerBullet>())
        {
            Destroy(gameObject);
        }
    }

    private void Update()
    {
        lifeTime += Time.deltaTime;
        if (lifeTime > maxLifeTime)
        {
            Destroy(gameObject);
        }
    }
}
