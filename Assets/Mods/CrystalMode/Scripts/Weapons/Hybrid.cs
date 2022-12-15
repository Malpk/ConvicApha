using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using Random = UnityEngine.Random;

public class Hybrid : Weapon
{
    [Header("Start shoot type")]
    [SerializeField] private ShootType shootType;
    
    
    [SerializeField] private float bulletsSpeed;

    [Header("Strong shoot")]
    [SerializeField] private GameObject strongBullet;

    [Header("Part shoot")]
    [SerializeField] private GameObject partBullet;
    [SerializeField] private int partBulletCount;
    [SerializeField] private float timeBetweenShoots;

    [Header("Random shoot")] 
    [SerializeField] private float spreadStrength;
    [SerializeField] private int randomBulletCount;

    private ShootType[] shootTypes = {ShootType.SingleShoot, ShootType.PartShoot, ShootType.RandomShoot};
    private int currentShootTypeIndex;
   
    protected override void Shoot()
    {
        switch (shootTypes[currentShootTypeIndex])
        {
            case ShootType.SingleShoot:
                StartCoroutine(SingleShoot());
                break;
            case ShootType.PartShoot:
                StartCoroutine(PartShoot());
                break;
            case ShootType.RandomShoot:
                RandomShoot();
                break;
        }
    }

    public override void Reload()
    {
        throw new NotImplementedException();
    }

    private IEnumerator SingleShoot()
    {
        yield return new WaitForSeconds(2);
        if (Input.GetKey(KeyCode.H))
        {
           InstAndAddForce(strongBullet, bulletsSpeed);
        }
    }

    private IEnumerator PartShoot()
    {
        for (int i = 0; i < partBulletCount; i++)
        {
            InstAndAddForce(partBullet, bulletsSpeed);
            yield return new WaitForSeconds(timeBetweenShoots);
        }
    }

    private void RandomShoot()
    {
        for (int i = 0; i < randomBulletCount; i++)
        {
            var bullet = Instantiate(partBullet, transform.position + transform.up / 3, quaternion.identity);
            Vector2 offset = transform.parent.transform.right * Random.Range(spreadStrength, - spreadStrength);
            Vector2 dir = transform.parent.transform.up +(Vector3) offset;
            bullet.GetComponent<Rigidbody2D>().AddForce(dir * bulletsSpeed);
        }
    }

    private void FixedUpdate()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            currentShootTypeIndex++;
            if (currentShootTypeIndex > shootTypes.Length - 1)
                currentShootTypeIndex = 0;
        }
    }
}
enum ShootType
{
    SingleShoot,
    PartShoot,
    RandomShoot
}
