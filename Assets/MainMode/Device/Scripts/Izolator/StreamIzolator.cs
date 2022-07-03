using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MainMode
{
    public class StreamIzolator : Izolator
    {
        public override TrapType DeviceType => TrapType.SteamIsolator;

        protected override void Intilizate()
        {
            base.Intilizate();
            var jets = GetComponentsInChildren<ISetAttack>();
            foreach (var jet in jets)
            {
                jet.SetAttack(attackInfo);
            }
        }

        protected override void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.GetComponent<IMoveEffect>() != null)
                ActivateDevice();
        }

        protected override void SetMode(bool mode)
        {
        }
    }
}