using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MainMode.Items
{
    public class InterdimensionalSphere : Item
    {
        public override void Pick(Player player)
        {
            _ownerPlayer = player;  
            

        }

        public override void Use()
        {
            throw new System.NotImplementedException();
        }
    }
}