using System;
using System.Collections.Generic;
using UnityEngine;

namespace MainMode.Items
{
    public class ChangeSpeedItemEffect : ItemEffect
    {
        [SerializeField]private int _speed;            

        public override void UseEffect(Player player)
        {
            player.ChangeSpeed(Duration,_speed);
        }
    }
}
