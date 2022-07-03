using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MainMode
{
    public class SignalTile : Device
    {
        public override TrapType DeviceType => TrapType.SignalTile;

        public delegate void Singnal(Collider2D collision);
        public event Singnal SingnalAction;

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (SingnalAction != null &&  collision.GetComponent<Player>())
                SingnalAction(collision);
        }
    }
}