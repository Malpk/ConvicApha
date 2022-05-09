using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FreezEffect : DestructiveEffect
{
    UnitMove _unitMove;

    public override void StartEffect(Health health, UnitMove unitMove)
    {
        ActivateTheEffectOnMove(unitMove);
        base.StartEffect(health, unitMove);
    }

    public void ActivateTheEffectOnMove(UnitMove unitMove)
    {
        _unitMove = unitMove;
        unitMove.SetMultiplierSpeed(0);
    }

    public override void StopEffect()
    {
        _unitMove.SetMultiplierSpeed(1);
        base.StopEffect();
    }
}
