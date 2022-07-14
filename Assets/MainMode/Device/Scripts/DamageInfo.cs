using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MainMode;

[System.Serializable]
public class DamageInfo
{
    [Min(0)]
    [SerializeField] private float _timeEffect = 0.5f;
    [SerializeField] private EffectType _effect;
    [SerializeField] private AttackType _attack;

    public float TimeEffect => _timeEffect;
    public EffectType Effect => _effect;
    public AttackType Attack => _attack;
}
