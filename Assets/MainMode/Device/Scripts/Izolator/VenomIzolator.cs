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
            _cloud.SetMode(mode);
            base.SetState(mode);
        }
        protected override void SetDeviceMode(bool mode)
        {
            base.SetDeviceMode(mode);
            if (mode)
                _cloud.Show();
            else
                _cloud.Hide();
        }
    }
}