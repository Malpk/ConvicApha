using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElectroGun : Weapon
{
    private float timeSinceShoot;
    protected override void Shoot()
    {
        
    }

    public override void Reload()
    {
        throw new System.NotImplementedException();
    }

    private void LateUpdate()
    {
        timeSinceShoot += Time.deltaTime;
        if (Input.GetKeyUp(KeyCode.H))
        {
            
        }
    }
}
