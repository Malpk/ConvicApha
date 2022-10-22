using UnityEngine;
using MainMode;

namespace PlayerComponent
{
    [CreateAssetMenu(menuName = "PlayerComponent/Effects/Movement")]
    public class MovementEffect : ScriptableObject, IEffect
    {
        [Range(0,2f)]
        [SerializeField] private float _effectValue = 1f;
        [SerializeField] private EffectType _effectType;

        public float Value => _effectValue;
        public EffectType Effect => _effectType;
    }
}