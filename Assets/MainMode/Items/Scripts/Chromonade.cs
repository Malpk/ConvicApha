using System.Collections;
using UnityEngine;

namespace MainMode.Items
{
    public class Chromonade : ConsumablesItem
    {
        public override void Pick(Player player)
        {
            _ownerPlayer = player;
            gameObject.SetActive(false);
        }

        public override void Use()
        {
            if (_ownerPlayer != null && _itemEffect != null)
                _ownerPlayer.ApplyEffect(_itemEffect);
        }
    }
}