using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class VenomEffect : DestructiveEffect
{
    [SerializeField]
    private int _damageValue = 1;

    private Health _health;

    public override void StartEffect(Health health, UnitMove unitMove)
    {
        ActivateTheEffectOnHealth(health);
        base.StartEffect(health, unitMove);
    }

    public void ActivateTheEffectOnHealth(Health health)
    {
        _health = health;
        health.TakeDamage(_damageValue);
    }

    public override void StopEffect()
    {
        _health.AddHealth(_damageValue);
        base.StopEffect();
    }
}
