using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MainMode
{
    public class StreamIzolator : Izolator
    {
        public override TrapType DeviceType => TrapType.SteamIsolator;


        protected  void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.GetComponent<Character>() != null)
                Activate();
        }

    }
}