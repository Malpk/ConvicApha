using System;
using UnityEngine;
using System.Collections.Generic;


namespace MainMode.Items
{
    public abstract class ConsumablesItem : Item
    {
        [SerializeField] protected float resistDuration;
        [SerializeField] protected AttackInfo attackResist;
    }
}
