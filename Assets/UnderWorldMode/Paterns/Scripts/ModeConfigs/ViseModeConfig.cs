using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Underworld
{
    [CreateAssetMenu(menuName = "UnderWorld/PaternConfigs/ViseModeConfig")]
    public class ViseModeConfig : PaternConfig
    {
        [Header("General")]
        [SerializeField] private Vector2 _activeTime;
        [SerializeField] private Vector2 _warningTime;
        [SerializeField] private ViseState _mode = ViseState.HorizontalMode;

        public override ModeType TypeMode => ModeType.ViseMode;

        public Vector2 ActiveTime => _activeTime;
        public Vector2 WarningTime => _warningTime;
        public ViseState Mode => _mode;
    }
}