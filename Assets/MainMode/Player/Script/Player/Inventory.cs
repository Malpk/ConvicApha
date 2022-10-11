using MainMode.LoadScene;
using System.Collections.Generic;
using UnityEngine;
using MainMode.Items;
using MainMode.GameInteface;


namespace MainMode
{
    public class Inventory : MonoBehaviour, ISender
    {
        [SerializeField] private Player _player;
        [SerializeField] private Item _artifact;
        [SerializeField] private InventroryUI _display;
        [SerializeField] private ConsumablesItem _consumablesItem;

        public TypeDisplay TypeDisplay => TypeDisplay.ItemInventory;

        private void Awake()
        {
            _player = GetComponent<Player>();
        }


        public bool AddReceiver(Receiver receiver)
        {
            if (_display != null)
                return false;
            if (receiver is InventroryUI display)
                _display = display;
            return _display;
        }
        public void AddConsumablesItem(ConsumablesItem item)
        {
            if (item)
            {        
                _consumablesItem = item;
                UpdateInventory();
            }
        }

        public void AddArtifact(Item artifact)
        {
            if (artifact)
            {
                _artifact = artifact;
                UpdateInventory();
            }
        }

        public bool TryGetConsumableItem(out ConsumablesItem item)
        {
            item = null;
            if (_consumablesItem)
            {
                item = _consumablesItem;
                _consumablesItem = null;
            }
            UpdateInventory();
            return item;
        }

        public bool TryGetArtifact(out Item artifact)
        {
            artifact = null;
            if (_artifact)
            {
                if (_artifact.Count > 0)
                    artifact = _artifact;
                else
                    _artifact = null;
            }
            UpdateInventory();
            return artifact;
        }

        private void UpdateInventory()
        {
            if (_consumablesItem)
                _display.DisplayConsumablesItem(_consumablesItem.Sprite, _consumablesItem.Count);
            else
                _display.DisplayConsumablesItem(null);
            if (_artifact)
            {
                if (_artifact.IsInfinity)
                    _display.DisplayInfinity(_artifact.Sprite);
                else if (_artifact.Count > 0)
                    _display.DisplayArtifact(_artifact.Sprite, _artifact.Count);
            }
            else
            {
                _display.DisplayArtifact(null);
            }
        }

    }
}