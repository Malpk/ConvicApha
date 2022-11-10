using UnityEngine;

namespace Underworld
{
    [CreateAssetMenu(menuName = "UnderWorld/PaternConfigs/RunOrDeadConfig")]
    public class RunOrDeadConfig : PaternConfig
    {
        [Header("Movement Setting")]
        [SerializeField] private float _speedRotation;
        [Min(0)]
        [SerializeField] private float _warningTime;

        public override ModeType TypeMode => ModeType.RunOrDead;

        public float WarningTime => _warningTime;
        public float SpeedRotation => _speedRotation;
    }
}