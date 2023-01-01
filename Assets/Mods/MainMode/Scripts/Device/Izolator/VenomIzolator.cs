using UnityEngine;

namespace MainMode
{
    public class VenomIzolator : Izolator
    {
        [SerializeField] private VenomCloud _cloud;

        public override TrapType DeviceType => TrapType.VenomIsolator;

        public void ActivateVenomCloud()
        {
            _cloud.Show();
        }
        public void DeactivateVenomCloud()
        {
            _cloud.Hide();
        }
    }
}