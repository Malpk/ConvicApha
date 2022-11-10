using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Underworld
{
    [CreateAssetMenu(menuName = "UnderWorld/TridentConfig")]
    public class TridentConfig : ScriptableObject
    {
        [Min(1)]
        [SerializeField] private float _distanceFly = 25;
        [Min(1)]
        [SerializeField] private float _speedMovement = 1;

        public float DistanceFly => _distanceFly;
        public float SpeedMovement => _speedMovement;
    }
}