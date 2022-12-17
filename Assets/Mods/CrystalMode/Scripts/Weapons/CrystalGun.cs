using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class CrystalGun : Weapon
{
    [SerializeField] private GameObject smallCrystal;
    [SerializeField] private float bulletSpeed;
    [SerializeField] private int bulletsCount;
    [SerializeField] private int maxBulletCount;

    protected override void Shoot()
    {
        InstAndAddForce(smallCrystal, bulletSpeed);
    }

    public override void Reload()
    {
        int bulletsToAdd = maxBulletCount - bulletsCount;
        bulletsCount += bulletsToAdd;
    }
}
