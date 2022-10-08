using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MainMode.GameInteface
{
    public class InventroryUI : Receiver
    {
        [SerializeField] private InventoryView _item;
        [SerializeField] private InventoryView _artifact;

        public override TypeDisplay DisplayType => TypeDisplay.ItemInventory;

        public void DisplayConsumablesItem(Sprite itemIcon, int count = 0)
        {
            ShowObject(itemIcon, count, _item);
        }
        public void DisplayArtifact(Sprite itemIcon, int count = 0)
        {
            ShowObject(itemIcon, count, _artifact);
        }

        private void ShowObject(Sprite itemIcon,int count, InventoryView view)
        {
            view.Display(itemIcon, count);
        }
        public void DisplayInfinity(Sprite itemIcon)
        {
            _artifact.DisplayInfinity(itemIcon);
        }

    }
}