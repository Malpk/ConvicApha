using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Underworld
{
    public class TridentSetting : ScriptableObject
    {
        [SerializeField] private TridentState state;
        [SerializeField] private float _warningTime;
        [SerializeField] private float _speedMovement;

        public float WarningTime => _warningTime;
        public float SpeedMovement => _speedMovement;
        public TridentState Angel => state;
    }
}
