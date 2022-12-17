using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sword : Weapon
{
    [SerializeField] private DamageInfo damageInfo;
    [SerializeField] private int teleportsCount;
    
    [SerializeField] private int hitDamageValue;
    [SerializeField] private int teleportDamageValue;
    
    [SerializeField] private float hitDistance;
    [SerializeField] private float teleportDistance;
    protected override void Shoot()
    {
        if (teleportsCount > 0)
        {
            ShootWithTeleport();
        }
        else
        {
            Hit();
        }
    }

    private void Hit()
    {
        var hit2D = Physics2D.Raycast(transform.position, transform.up, hitDistance);
        if (hit2D)
        {
            hit2D.transform.gameObject.GetComponent<IDamage>()?.TakeDamage(hitDamageValue, damageInfo);
            Reload();
        }
    }
    private void ShootWithTeleport()
    {
        var raycastHits2D = Physics2D.RaycastAll(transform.position, transform.up, teleportDistance);
        foreach (var hit in raycastHits2D)
        {
            hit.transform.gameObject.GetComponent<IDamage>()?.TakeDamage(teleportDamageValue ,damageInfo);
            Reload();
        }
        transform.parent.position += transform.parent.up * teleportDistance;
        teleportsCount--;
    }
    public override void Reload()
    {
        teleportsCount++;
    }
}
