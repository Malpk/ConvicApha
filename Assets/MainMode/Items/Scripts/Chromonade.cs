﻿using UnityEngine;

namespace MainMode.Items
{
    public class Chromonade : ConsumablesItem
    {
        [SerializeField] private ItemEffect _itemEffect;

        public override string Name => "Хромонаде";
    }
}