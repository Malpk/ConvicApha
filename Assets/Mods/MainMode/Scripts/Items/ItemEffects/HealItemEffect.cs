using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MainMode.Items;

public class HealItemEffect : ItemEffect
{
    [SerializeField]private int _healPoints;
  
    public override void UseEffect(Player player)
    {
        player.Heal(_healPoints);
    }
}
