using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MainMode.GameInteface
{
    public class InventroryUI : MonoBehaviour
    {
        [SerializeField] private InventoryView _item;
        [SerializeField] private InventoryView _artifact;

        public void DisplayConsumablesItem(Sprite itemIcon, int count = 0)
        {
            _item.Display(itemIcon, count);
        }
        public void DisplayArtifact(Sprite itemIcon, int count = 0)
        {
            _artifact.Display(itemIcon, count);
        }

        public void DisplayInfinity(Sprite itemIcon)
        {
            _artifact.DisplayInfinity(itemIcon);
        }

    }
}