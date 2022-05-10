using UnityEngine;
using Underworld.Editors;

namespace Underworld
{
    [System.Serializable]
    public class RaySetting : IModeSetting
    {
        [Header("Count Setting")]
        [Min(1)]
        [SerializeField] private int _countRay = 1;
        [Header("Offset Setting")]
        [SerializeField] private float _offsetAngle;
        [SerializeField] private float _speedOffset;
        [Header("Time Setting")]
        [Min(0)]
        [SerializeField] private float _warningTime;
        [Min(0)]
        [SerializeField] private float _delay;
        [Min(0)]
        [SerializeField] private float _duration;

        public int CountRay => _countRay;
        public float OfssetAngle => _offsetAngle;
        public float SpeedOffset => _speedOffset;
        public float WarningTime => _warningTime;
        public float Dealy => _delay;
        public float Duration => _duration;

        public ModeTypeNew type => ModeTypeNew.RayMode;
    }
}