using UnityEngine;
using MainMode.Items;

namespace PlayerComponent
{
    public class PlayerInventory
    {
        [SerializeField] private Item _artifact;
        [SerializeField] private ConsumablesItem _consumablesItem;

        public void AddConsumablesItem(ConsumablesItem item)
        {
            if (item)
            {        
                _consumablesItem = item;
            }
        }

        public void AddArtifact(Item artifact)
        {
            if (artifact)
            {
                _artifact = artifact;
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
            return artifact;
        }
    }
}