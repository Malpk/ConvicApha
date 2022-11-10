using UnityEngine;
using MainMode.Items;
using MainMode.GameInteface;

namespace MainMode.LoadScene
{
    [System.Serializable]
    public class PlayerConfig
    {
        [SerializeField] private PlayerType _type;
        [SerializeField] private Item _artifact;
        [SerializeField] private ConsumablesItem _itemConsumable;

        public PlayerType Type => _type;
        public Item ItemArtifact => _artifact;
        public ConsumablesItem ItemConsumable => _itemConsumable;

        public void SetPlayerType(PlayerType type)
        {
            _type = type;
        }

        public void SetArtifactAsync(ScrollItem itemArtifact)
        {
            _artifact = itemArtifact.Create<Item>();
            _artifact.Reset();
        }
        public void SetConsumableAsync(ScrollItem itemConsumable)
        {
            _itemConsumable = itemConsumable.Create<ConsumablesItem>();
            _itemConsumable.Reset();
        }

    }
}

