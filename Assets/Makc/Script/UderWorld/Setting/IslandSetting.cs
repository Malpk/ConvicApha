using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace Underworld
{
    [System.Serializable]
    public class IslandSetting
    {
        [Header("Mode Setting")]
        [Min(0)]
        [SerializeField] private float _warningTime = 0;
        [Min(0)]
        [SerializeField] private float _workTime = 1;
        [Header("Island Setting")]
        [Min(1)]
        [SerializeField] private int _minDistanceBothIsland = 1;
        [SerializeField] private int _minSizeIsland;
        [SerializeField] private int _maxSizeIsland;

        public ModeTypeNew type => ModeTypeNew.IslandMode;

        public int MinDistanceBothIsland => _minDistanceBothIsland;
        public int MinSizeIsland => _minSizeIsland;
        public int MaxSizeIsland => _maxSizeIsland;
        public float WarningTime => _warningTime;
        public float WorkTime => _workTime;
    }
}