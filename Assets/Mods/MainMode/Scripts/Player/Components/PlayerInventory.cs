using UnityEngine;
using MainMode.Items;

namespace PlayerComponent
{
    public class PlayerInventory : MonoBehaviour
    {
        [SerializeField] private Player _user;
        [SerializeField] private Item _artifact;
        [SerializeField] private ConsumablesItem _consumablesItem;

        public event System.Action<Item> OnUpdateArtefacct;
        public event System.Action<ConsumablesItem> OnUpdateConsumableItem;

        public void UseItem()
        {
            if (_consumablesItem)
            {
                if (_consumablesItem.Count > 0)
                {
                    _consumablesItem.Use();
                }
                else
                {
                    OnUpdateConsumableItem?.Invoke(null);
                }
            }
        }
        public void UseArtifact()
        {
            if (_artifact)
            {
                if (_artifact.Count > 0)
                {
                    _artifact.Use();
                }
                else
                {
                    OnUpdateArtefacct?.Invoke(null);
                }
            }
        }
        public void PickItem(Item itemUse)
        {
            if (itemUse is ConsumablesItem consumablesItem)
            {
                AddConsumablesItem(consumablesItem);
            }
            else
            {
                AddArtifact(itemUse);
            }

        }
        public void AddConsumablesItem(ConsumablesItem item)
        {
            if (item)
            {
                item.Pick(_user);
                _consumablesItem = item;
                OnUpdateConsumableItem?.Invoke(item);
            }
        }

        public void AddArtifact(Item artifact)
        {
            if (artifact)
            {
                artifact.Pick(_user);
                _artifact = artifact;
                OnUpdateArtefacct?.Invoke(artifact);
            }
        }
        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.TryGetComponent(out Item itemUse))
            {
                PickItem(itemUse);
            }
        }
    }
}