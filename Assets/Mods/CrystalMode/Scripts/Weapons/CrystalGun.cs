using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class CrystalGun : Weapon
{
    [SerializeField] private GameObject smallCrystal;
    [SerializeField] private float bulletSpeed;

    protected override void Shoot()
    {
        InstAndAddForce(smallCrystal, bulletSpeed);
    }

    public override void Reload()
    {
        throw new System.NotImplementedException();
    }
}
