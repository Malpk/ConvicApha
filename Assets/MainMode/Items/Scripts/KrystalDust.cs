using System;
using UnityEngine;

namespace MainMode.Items
{
    public class KrystalDust : Item
    {
        [SerializeField] private GameObject _wallCrystalPrefab;
        public override void Pick(Player player)
        {
            throw new NotImplementedException();
        }

        public override void Use()
        {
            throw new NotImplementedException();
        }
    }
}
