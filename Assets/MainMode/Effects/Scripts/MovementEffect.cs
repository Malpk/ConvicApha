using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MainMode.Effects
{
    [CreateAssetMenu(menuName = "PlayerComponent/Effects/Movement")]
    public class MovementEffect : ScriptableObject
    {
        [Range(0,2f)]
        [SerializeField] private float _effectValue = 1f;
        [SerializeField] private EffectType _effectType;
        public float Value => _effectValue;
        public EffectType Effect => _effectType;
    }
}