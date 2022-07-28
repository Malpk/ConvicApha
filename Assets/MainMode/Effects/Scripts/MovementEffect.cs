using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MainMode.Effects
{
    [CreateAssetMenu(menuName = "Effects/Movement")]
    public class MovementEffect : ScriptableObject
    {
        [Range(0,2f)]
        [SerializeField] private float _effect = 1f;

        public float Effect => _effect;
    }
}