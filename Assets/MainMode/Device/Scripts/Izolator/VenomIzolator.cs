using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MainMode
{
    public class VenomIzolator : Izolator
    {
        [SerializeField] private VenomCloud _cloud;

        public override TrapType DeviceType => TrapType.VenomIsolator;

        protected override void SetState(bool mode)
        {
            base.SetState(mode);
            _cloud.SetMode(mode);
        }
    }
}