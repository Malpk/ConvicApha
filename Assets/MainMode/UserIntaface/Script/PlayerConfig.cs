using UnityEngine;
using MainMode.Items;
using System.Threading.Tasks;
using UnityEngine.AddressableAssets;

namespace MainMode.LoadScene
{
    [System.Serializable]
    public class PlayerConfig
    {
        [SerializeField] private PlayerType _type;
        [SerializeField] private Item _artifact;
        [SerializeField] private ConsumablesItem _itemConsumable;

        private string _artifactLoad = "";
        private string _consumableItemLoad = "";

        public PlayerType Type => _type;
        public Item ItemArtifact => _artifact;
        public ConsumablesItem ItemConsumable => _itemConsumable;

        public void SetPlayerType(PlayerType player)
        {
            _type = player;
        }

        public async Task SetArtifactAsync (string itemArtifact)
        {
            if (itemArtifact != _artifactLoad)
            { 
                _artifactLoad = itemArtifact;
                var taskArtifact = Addressables.InstantiateAsync(itemArtifact).Task;
                await taskArtifact;
                if(_artifact)
                    MonoBehaviour.Destroy(_artifact.gameObject);
                _artifact = taskArtifact.Result.GetComponent<Item>();
            }
        }
        public async Task SetConsumableAsync(string itemConsumable)
        {
            if (itemConsumable != _consumableItemLoad)
            {
                _consumableItemLoad = itemConsumable;
                var taskConsumable = Addressables.InstantiateAsync(itemConsumable).Task;
                await taskConsumable;
                if(_itemConsumable)
                    MonoBehaviour.Destroy(_itemConsumable.gameObject);
                _itemConsumable = taskConsumable.Result.GetComponent<ConsumablesItem>();
            }
        }

        public void Restart()
        {
            if (_itemConsumable)
                _itemConsumable.SetDefoutValue();
            if (_artifact)
                _artifact.SetDefoutValue();
        }
    }
}

