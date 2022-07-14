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


     
        public bool DisplayConsumablesItem(Sprite itemIcon, int count = 0)
        {
            Debug.Log("shwo");
            return ShowObject(itemIcon, count, _item);
        }
        public bool DisplayArtifact(Sprite itemIcon, int count = 0)
        {
            return ShowObject(itemIcon, count, _artifact);
        }

        private bool ShowObject(Sprite itemIcon,int count, InventoryView view)
        {
            if (view != null)
            {
                view.Display(itemIcon, count);
                return true;
            }
            return false;
        }
        public void DisplayInfinity(Sprite itemIcon)
        {
            _artifact.DisplayInfinity(itemIcon);
        }

    }
}