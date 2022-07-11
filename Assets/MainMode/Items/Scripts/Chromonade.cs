using System.Collections;
using UnityEngine;

namespace MainMode.Items
{
    public class Chromonade : ConsumablesItem
    {
        [SerializeField] private ItemEffect _itemEffect;
        public override void Use()
        {
            if (_target != null && _itemEffect != null)
                _target.ApplyEffect(_itemEffect);
        }
    }
}