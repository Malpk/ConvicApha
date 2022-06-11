using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MainMode
{
    public class DaimonIzolator : Izolator
    {
        [SerializeField] private SignalTile _signalArea;

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