using UnityEngine;
using Underworld.Editors;

namespace Underworld
{
    [System.Serializable]
    public class BoxSetting : IModeSetting
    {
        [Header("Movement Setting")]
        [Min(1)]
        [SerializeField] private int _countMove;
        [Min(0)]
        [SerializeField] private float _speedMovement;
        [Header("Time Setting")]
        [Min(1)]
        [SerializeField] private float _scaleDuration;
        [Min(0)]
        [SerializeField] private float _delay;
        [Header("Scale Setting")]
        [SerializeField] private Vector2 _minSize;
        [SerializeField] private Vector3 _maxOffset;

        public int CountMove => _countMove;
        public float SpeedMove => _speedMovement;
        public float ScaleDuration => _scaleDuration;
        public float Delay => _delay;
        public Vector2 MinSize => _minSize;
        public Vector2 MaxOffset => _maxOffset;
        public ModeTypeNew type => ModeTypeNew.BoxMode;
    }
}