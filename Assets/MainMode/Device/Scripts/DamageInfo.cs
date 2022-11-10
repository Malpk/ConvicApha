using MainMode;
using UnityEngine;

[CreateAssetMenu(menuName = "PlayerComponent/DamageInfo")]
public class DamageInfo : ScriptableObject, IEffect
{
    [Min(0)]
    [SerializeField] private float _timeEffect = 0.5f;
    [SerializeField] private EffectType _effect;
    [SerializeField] private AttackType _attack;

    public float TimeEffect => _timeEffect;
    public EffectType Effect => _effect;
    public AttackType Attack => _attack;
}
