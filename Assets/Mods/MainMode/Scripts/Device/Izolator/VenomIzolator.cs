using UnityEngine;

namespace MainMode
{
    public class VenomIzolator : Izolator
    {
        [SerializeField] private VenomCloud _cloud;

        public override TrapType DeviceType => TrapType.VenomIsolator;

        protected override void OnEnable()
        {
            base.OnEnable();
            OnActivate += ActivateVenomCloud;
            OnDeactivate += DeactivateVenomCloud;
        }
        protected override void OnDisable()
        {
            base.OnDisable();
            OnActivate -= ActivateVenomCloud;
            OnDeactivate -= DeactivateVenomCloud;
        }

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