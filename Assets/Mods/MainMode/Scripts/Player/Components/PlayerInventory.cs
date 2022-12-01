using UnityEngine;
using MainMode.Items;
using MainMode.GameInteface;

namespace PlayerComponent
{
    public class PlayerInventory : MonoBehaviour
    {
        [SerializeField] private Artifact _artifact;
        [SerializeField] private ConsumablesItem _consumablesItem;
        [Header("Reference")]
        [SerializeField] private Player _user;
        [SerializeField] private ShootMarkerView _shootMarker;

        public System.Func<bool> UpdateState;
        public event System.Action<Artifact> OnUpdateArtefact;
        public event System.Action<ConsumablesItem> OnUpdateConsumableItem;
        public event System.Action<float> OnUpdateReloadArtefact;
        public event System.Action<bool> OnUpdateStateArtefact;

        private void Start()
        {
            enabled = false;
        }
        private void Update()
        {
            OnUpdateReloadArtefact?.Invoke(_artifact.Reloading());
            if (_artifact.IsUse)
            {
                OnUpdateStateArtefact?.Invoke(true);
                enabled = false;
            }
        }
        public void UseItem()
        {
            if (_consumablesItem)
            {
                if (Use(_consumablesItem))
                    OnUpdateConsumableItem?.Invoke(null);
            }
        }
        public void UseArtifact()
        {
            if (_artifact)
            {
                if (!Use(_artifact))
                {
                    OnUpdateStateArtefact?.Invoke(false);
                    enabled = true;
                }
            }
        }
        private bool Use(Item item)
        {
            if (item.IsUse)
            {
                item.Use();
                if(item.IsShoot)
                    _user.transform.rotation = Quaternion.Euler(Vector3.forward * _shootMarker.Angel);
            }
            return item.IsUse;
        }
        public void PickItem(Item itemUse)
        {
            if (itemUse is ConsumablesItem consumablesItem)
            {
                AddConsumablesItem(consumablesItem);
            }
            else if(itemUse is Artifact artifact)
            {
                AddArtifact(artifact);
                OnUpdateStateArtefact?.Invoke(true);
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

        public void AddArtifact(Artifact artifact)
        {
            if (artifact)
            {
                artifact.Pick(_user);
                _artifact = artifact;
                OnUpdateArtefact?.Invoke(artifact);
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