using System.Collections;
using UnityEngine;

namespace MainMode.Items
{
    public class ChocolateSpeed : Item
    {
        public override void Pick(Player player)
        {
            _ownerPlayer = player;  
            gameObject.SetActive(false);    
        }

        public override void Use()
        {
            _ownerPlayer.ApplyEffect(_itemEffect);
        }
    }
}