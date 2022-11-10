using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Underworld
{
    [CreateAssetMenu(menuName = "UnderWorld/PaternConfigs/BoxModeConfig")]
    public class BoxModeConfig : PaternConfig
    {
        [Min(1)]
        [SerializeField] private int _countMove = 1;
        [Min(1)]
        [SerializeField] private float _speedMovement = 1;
        [Range(1,2)]
        [SerializeField] private float _delayMove;
        [Min(1)]
        [SerializeField] private float _scaleTime = 1;

        public override ModeType TypeMode => ModeType.BoxMode;
        public float SpeedMovement => _speedMovement;
        public float ScaleTime => _scaleTime;
        public float DealyMove => _delayMove;
        public int CountMove => _countMove;
    }
}
