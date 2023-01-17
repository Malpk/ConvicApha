using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElectroGun : Weapon
{
    private float timeSinceShoot;
    [SerializeField] private GameObject lightning;
    private bool hadShoot;
    protected override void Shoot()
    {
        hadShoot = true;
    }

    public override void Reload()
    {
        throw new System.NotImplementedException();
    }

    private void LateUpdate()
    {
        if (hadShoot)
        {
            timeSinceShoot += Time.deltaTime;
            if (Input.GetKeyUp(KeyCode.H))
            {
                var lighting = Instantiate(lightning, transform.position + transform.up, transform.rotation, transform);
                lighting.GetComponent<Lighting>().timeToLive = timeSinceShoot * 1.5f;
                timeSinceShoot = 0;
                hadShoot = false;
            }
        }
    }
}
