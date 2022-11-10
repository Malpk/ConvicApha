using System;
using System.Collections.Generic;

using UnityEngine;

namespace MainMode.Items
{
    public class FireResistItemEffect:ItemEffect
    {

        public override void UseEffect(Player player)
        {
            Debug.Log($"use fire resist effect");
               
        }
    }
}
