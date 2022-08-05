using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MainMode
{
    public class DaimonIzolator : Izolator
    {
        [SerializeField] private SignalTile _signalArea;

        public override TrapType DeviceType => TrapType.DaimondIzolator;

    }
}