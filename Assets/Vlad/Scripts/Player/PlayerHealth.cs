using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PlayerHealth : Health
{
    [SerializeField]
    private HealthUI _healthUI;

    public AudioSource AddHealthSound;
    

    public void Start()
    {
        _healthUI.Setup(_maxHealthPoint);
        _healthUI.DisplayHealth(_healthPoints);
    }

    public override void AddHealth(int healthValue)
    {
        base.AddHealth(healthValue);
        //AddHealthSound.Play();
        _healthUI.DisplayHealth(_healthPoints);
    }

    public override void TakeDamage(int damageValue)
    {

        base.TakeDamage(damageValue);

        Invoke("StopInvulnerable", 1);
        _healthUI.DisplayHealth(_healthPoints);

    }

}
