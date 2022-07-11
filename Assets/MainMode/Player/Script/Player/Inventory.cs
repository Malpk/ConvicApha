using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MainMode.Items;

namespace MainMode
{
    public class Inventory : MonoBehaviour
    {
        [SerializeField] private InventoryView ConsumablesItemView;
        [SerializeField] private InventoryView ArtifactItemView;

        [SerializeField] private List<ConsumablesItem> _consumablesItem = new List<ConsumablesItem>();
        [SerializeField] private Player _player;
        [SerializeField] private Artifact _artifact;

        private void OnEnable()
        {
            if (Application.platform == RuntimePlatform.Android || Application.platform == RuntimePlatform.IPhonePlayer)
            {
                ConsumablesItemView.ClickView += UseConsumableItem;
                ArtifactItemView.ClickView += UseArtifactItem;

            }
        }

        private void OnDisable()
        {
            if (Application.platform == RuntimePlatform.Android || Application.platform == RuntimePlatform.IPhonePlayer)
            {
                ConsumablesItemView.ClickView -= UseConsumableItem;
                ArtifactItemView.ClickView -= UseArtifactItem;
            }
        }

        private void UseArtifactItem()
        {
            if (TryGetArtifact(out Artifact artifact))
            {
                artifact.Use();
            }

        }

        private void UseConsumableItem()
        {
            if (TryGetConsumableItem(out Item item))
            {
                item.Use();
            }
        }

        private void Start()
        {
            _player = GetComponent<Player>();
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
                ConsumablesItemView.Display(_consumablesItem[0].Sprite, _consumablesItem.Count);
            else
                ConsumablesItemView.DisplayEmpty();

            if (_artifact != null)
            {
                if (_artifact.Count > 0)
                    ArtifactItemView.Display(_artifact.Sprite, _artifact.Count);
                else
                    ArtifactItemView.DisplayEmpty();

                if (_artifact.IsInfinity)
                    ArtifactItemView.DisplayInfinity(_artifact.Sprite);
            }
        }
    }
}