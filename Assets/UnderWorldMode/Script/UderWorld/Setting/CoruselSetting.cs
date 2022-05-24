using UnityEngine;
using Underworld.Editors;

namespace Underworld
{
    [System.Serializable]
    public class CoruselSetting : IModeSetting
    {
        [Header("Movement Setting")]
        [SerializeField] private float _speedRotation;
        [Header("Time Setting")]
        [Min(0)]
        [SerializeField] private float _duration;
        [Min(0)]
        [SerializeField] private float _warningTime;

        public float SpeedRotation => _speedRotation;
        public float Duration => _duration;
        public float WarningTime => _warningTime;

        public ModeTypeNew type => ModeTypeNew.CoruselMode;
    }
}