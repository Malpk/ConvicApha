using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MainMode
{
    public class DaimonIzolator : Izolator
    {
        [SerializeField] private SignalTile _signalArea;
        public override EffectType TypeEffect => EffectType.None;

        public override TrapType DeviceType => TrapType.DaimondIzolator;

        private void OnEnable()
        {
            _signalArea.SingnalAction +=(Collider2D collison) => ActivateDevice();
        }
        private void OnDisable()
        {
            _signalArea.SingnalAction -= (Collider2D collison) => ActivateDevice();
        }
        protected override void SetMode(bool mode)
        {
        }
    }
}