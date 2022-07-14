using System.Collections;
using UnityEngine;

namespace MainMode.Items
{
    public class Chromonade : ConsumablesItem
    {
        [SerializeField] private ItemEffect _itemEffect;
        public override void Use()
        {
            if (user != null && _itemEffect != null)
                user.ApplyEffect(_itemEffect);
        }
    }
}