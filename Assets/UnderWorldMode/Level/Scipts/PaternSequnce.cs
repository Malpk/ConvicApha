using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Underworld
{
    [CreateAssetMenu(menuName ="UnderWorld/PaternSequnce")]
    public class PaternSequnce : ScriptableObject
    {
        [Range(0,1f)]
        [SerializeField] private float _probility = 1f;
        [SerializeField] private PaternConfig[] _sequencs;

        public float Probility => _probility;
        public PaternConfig[] Configs => _sequencs;
    }
}