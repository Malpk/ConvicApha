using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Underworld
{
    [CreateAssetMenu(menuName = "UnderWorld/PaternConfigs/PaternCreaterConfig")]
    public class PaternCreaterConfig : PaternConfig
    {
        [SerializeField] private ModeType _modeType;
        [SerializeField] private bool _iversionMode;
        [SerializeField] private float _warningTime;

        public override ModeType TypeMode => _modeType;
        public bool InversMode => _iversionMode;
        public float WarningTime => _warningTime;
    }
}