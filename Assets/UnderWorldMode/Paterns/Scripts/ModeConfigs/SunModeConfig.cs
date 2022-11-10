using UnityEngine;

namespace Underworld
{
    [CreateAssetMenu(menuName = "UnderWorld/PaternConfigs/SunModeConfig")]
    public class SunModeConfig : PaternConfig
    {
        [Header("General Setting")]
        [Min(1)]
        [SerializeField] private int _countRay = 1;
        [SerializeField] private float _offset;
        [SerializeField] private float _speedOffset;
        [Header("Work Setting")]
        [Min(0)]
        [SerializeField] private float _warningTime;
        [Range(0,1f)]
        [SerializeField] private float _delay;

        public override ModeType TypeMode => ModeType.SunMode;
        public int CountRay => _countRay;
        public float Delay => _delay;
        public float AngleOffet => _offset;
        public float SpeedOffset => _speedOffset;
        public float WarningTime => _warningTime;

    }
}
