using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using Random = UnityEngine.Random;

public class LaserShotgun : Weapon
{
    [SerializeField] private int bulletsCount;
    [SerializeField] private GameObject bulletPref;
    [SerializeField] private float spreadStrength;
    [SerializeField] private float bulletsSpeed;
    [SerializeField] private int totalBulletsCount;
    [SerializeField] private int maxBulletsInHands;
    private int bulletsInHand;
    protected override void Shoot()
    {
        for (int i = 0; i < bulletsCount; i++)
        {
            var bullet = Instantiate(bulletPref, transform.position + transform.up / 3, quaternion.identity);
            Vector2 offset = transform.parent.transform.right * Random.Range(spreadStrength, - spreadStrength);
            Vector2 dir = transform.parent.transform.up +(Vector3) offset;
            bullet.GetComponent<Rigidbody2D>().AddForce(dir * bulletsSpeed);
        }
    }

    public override void Reload()
    {
        int bulletsToAdd = maxBulletsInHands - bulletsInHand;
        if (totalBulletsCount > bulletsToAdd)
        {
            bulletsInHand += bulletsToAdd;
            totalBulletsCount -= bulletsToAdd;
        }
        else
        {
            bulletsInHand += totalBulletsCount;
            totalBulletsCount = 0;
        }
    }
}
