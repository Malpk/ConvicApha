using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Underworld
{
    [CreateAssetMenu(menuName = "UnderWorld/TridentHolderConfig")]
    public class TridentPointConfig : ScriptableObject
    {
        [Min(1)]
        [SerializeField] private int _countTrident = 1;
        [Min(0)]
        [SerializeField] private float _warningTime = 1;
        [Min(1)]
        [SerializeField] private float _widhtOneTrident = 1f;
        [Min(1)]
        [SerializeField] private float _distanceFromCenter = 1;
        [SerializeField] private TridentConfig _tridentConfig;

        public int CountTrident => _countTrident;
        public float WarningTime => _warningTime;
        public float WidthOneTrident => _widhtOneTrident;
        public float DistanceFromCenter => _distanceFromCenter;
        public TridentConfig TridentConfig => _tridentConfig;
    }
}
