using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MainMode.Items;
using MainMode.GameInteface;


namespace MainMode
{
    public class Inventory : MonoBehaviour, ISender, IReset
    {
        [SerializeField] private Player _player;
        [SerializeField] private Artifact _artifact;
        [SerializeField] private InventroryUI _display;
        [SerializeField] private List<ConsumablesItem> _consumablesItem = new List<ConsumablesItem>();

        public TypeDisplay TypeDisplay => TypeDisplay.ItemInventory;

        private void Awake()
        {
            _player = GetComponent<Player>();
        }

        public void Restart()
        {
            _artifact = null;
            _consumablesItem.Clear();
            _display.Restart();
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
            if (item != null)
            {
                if (_consumablesItem.Count == 0)
                {
                    _consumablesItem.Add(item);

                    item.Pick(_player);
                }
                else
                    if (_consumablesItem[0].GetType() == item.GetType())
                {
                    _consumablesItem.Add(item);

                    item.Pick(_player);
                }

                UpdateInventory();
            }
        }

        public void AddArtifact(Artifact artifact)
        {
            if (artifact != null)
            {
                if (artifact.IsInfinity || artifact.Count > 0)
                {
                    _artifact = artifact;
                    _artifact.Pick(_player);

                }

                UpdateInventory();
            }
        }

        public bool TryGetConsumableItem(out Item item)
        {
            if (_consumablesItem.Count > 0)
            {
                item = _consumablesItem[_consumablesItem.Count - 1];
                _consumablesItem.Remove((ConsumablesItem)item);
                UpdateInventory();
                return true;
            }
            else
            {
                item = null;
                UpdateInventory();
                return false;
            }
        }

        public bool TryGetArtifact(out Artifact artifact)
        {
            if (_artifact != null)
            {
                if (_artifact.IsInfinity)
                {
                    artifact = _artifact;
                    return true;
                }

                if (_artifact.Count > 0)
                {
                    artifact = _artifact;
                    _artifact.Count--;
                    UpdateInventory();
                    return true;
                }
                else
                {
                    artifact = null;
                    UpdateInventory();
                    return false;
                }
            }
            else
            {
                artifact = null;
                return false;
            }
        }

        private void UpdateInventory()
        {
            if (_consumablesItem.Count > 0)
                _display.DisplayConsumablesItem(_consumablesItem[0].Sprite, _consumablesItem.Count);
            else
                _display.DisplayConsumablesItem(null);

            if (_artifact != null)
            {
                if (_artifact.IsInfinity)
                    _display.DisplayInfinity(_artifact.Sprite);
                else if (_artifact.Count > 0)
                    _display.DisplayArtifact(_artifact.Sprite, _artifact.Count);
                else
                    _display.DisplayArtifact(null);
            }
        }

    }
}