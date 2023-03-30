using System;
using System.Collections.Generic;
using UnityEngine;

public class SmallCrystal : MonoBehaviour
{
    [SerializeField] private int damageValue;
    [SerializeField] private float maxLifeTime;
    [SerializeField] private DamageInfo damageInfo;
    private float lifeTime;
    private Transform target;

    private void OnTriggerEnter2D(Collider2D col)
    {
        GetComponent<Rigidbody2D>().simulated = false;
        if (col.gameObject.GetComponent<IDamage>() != null)
        {
            target = col.gameObject.transform;
            transform.SetParent(col.transform);
            target = col.transform;
        }
    }

    private void OnCollisionEnter2D(Collision2D col)
    {
        if (col.gameObject.GetComponent<Player>() == null)
        {
            GetComponent<Rigidbody2D>().simulated = false;
        }
    }

    private void Update()
    {
        lifeTime += Time.deltaTime;
       
        if (target != null && Input.GetKeyDown(KeyCode.X) || lifeTime > maxLifeTime && target == null)
        {
            Explode();
        }
    }

    private void Explode()
    {
        if (target != null)
            target.GetComponent<IDamage>()?.TakeDamage(damageValue, damageInfo);
        
        Destroy(gameObject);
    }
}
