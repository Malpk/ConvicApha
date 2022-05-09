using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageEffect : DestructiveEffect
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
        _health.TakeDamage(_damageValue);
    }

    private void OnDestroy()
    {
        StopEffect();
    }
}
