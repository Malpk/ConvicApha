using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PlayerHealth : Health
{
    public HealthUI HealthUI;
    public bool Invulnerable = false;  

    public AudioSource AddHealthSound;
    

    public void Start()
    {
        HealthUI.Setup(_maxHealthPoint);
        HealthUI.DisplayHealth(_healthPoints);
    }

    public override void AddHealth(int healthValue)
    {
        base.AddHealth(healthValue);
        //AddHealthSound.Play();
        HealthUI.DisplayHealth(_healthPoints);
    }

    public override void TakeDamage(int damageValue)
    {
        if (!Invulnerable)
        {
            base.TakeDamage(damageValue);
            Invulnerable = true;
            Invoke("StopInvulnerable", 1);
            HealthUI.DisplayHealth(_healthPoints);
        }
    }

    void StopInvulnerable()
    {
        Invulnerable = false;
    }

}
