using UnityEngine;
using Underworld.Editors;

namespace Underworld
{
    [System.Serializable]
    public class TridetSetting : IModeSetting
    {
        [Header("General Setting")]
        [SerializeField] private float _speedMovement;
        [SerializeField] private float _moveDistance;
        [SerializeField] private bool _horizontalMode;
        [SerializeField] private bool _verticalMode;
        [Header("Time Setting")]
        [Min(0)]
        [SerializeField] private float _shootDelay;
        [Min(0)]
        [SerializeField] private float _groupShootDelay;
        [Min(0)]
        [SerializeField] private float _warningTime;
        [Min(0)]
        [SerializeField] private float _workDuration;

        public float SpeedMovement => _speedMovement;
        public float MoveDistance => _moveDistance;
        public bool Horizontal => _horizontalMode;
        public bool Vertical => _verticalMode;
        public float ShootDelay => _shootDelay;
        public float GroundShootDelay => _groupShootDelay;
        public float WarningTime => _warningTime;
        public float WorkDuration => _workDuration;

        public ModeTypeNew type => ModeTypeNew.TrindetMode;
    }
}
