using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MainMode;

[System.Serializable]
public class AttackInfo
{
    [SerializeField] private TrapType _device;
    [SerializeField] private EffectType _effect;
    [SerializeField] private AttackType _attack;

    public TrapType Device => _device;
    public EffectType Effect => _effect;
    public AttackType Attack => _attack;
}
