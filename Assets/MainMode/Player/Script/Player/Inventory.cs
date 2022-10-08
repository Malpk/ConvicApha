using MainMode.LoadScene;
using System.Collections.Generic;
using UnityEngine;
using MainMode.Items;
using MainMode.GameInteface;


namespace MainMode
{
    public class Inventory : MonoBehaviour, ISender, IReset
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

        public void Restart(PlayerConfig config)
        {
            if (config != null)
            {
                _artifact = config.ItemArtifact;
                _consumablesItem = config.ItemConsumable;
                _artifact.SetDefoutValue();
                _consumablesItem.SetDefoutValue();
                _artifact.Pick(_player);
                _consumablesItem.Pick(_player);
            }
            else
            {
                _artifact = null;
                _consumablesItem = null;
            }
            UpdateInventory();
        }
        public bool AddReceiver(Receiver receiver)
        {
            if (_display != null)
                return false;
            if (receiver is InventroryUI display)
                _display = display;
            UpdateInventory();
            return _display;
        }
        public void AddConsumablesItem(ConsumablesItem item)
        {
            if (item != null)
            {
                item.Pick(_player);
                _consumablesItem = item;
            }
            UpdateInventory();
        }

        public void AddArtifact(Item artifact)
        {
            if (artifact != null)
            {
                if (artifact.IsShow)
                {
                    _artifact = artifact;
                    _artifact.Pick(_player);
                }
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