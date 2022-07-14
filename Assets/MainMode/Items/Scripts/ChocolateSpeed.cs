﻿using System.Collections;
using UnityEngine;

namespace MainMode.Items
{
    public class ChocolateSpeed : Item
    {
       [SerializeField] private ItemEffect _itemEffect;

        public override void Use()
        {
            user.ApplyEffect(_itemEffect);
        }
    }
}