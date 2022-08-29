using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MainMode
{
    public class VenomIzolator : Izolator
    {
        [SerializeField] private VenomCloud _cloud;

        public override TrapType DeviceType => TrapType.VenomIsolator;

        public override void Activate()
        {
            base.Activate();
            _cloud.Show();
        }
        public override void Deactivate()
        {
            base.Deactivate();
            _cloud.Hide();
        }
    }
}