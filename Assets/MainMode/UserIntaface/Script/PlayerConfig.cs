using UnityEngine;
using MainMode.Items;

namespace MainMode.LoadScene
{
    [System.Serializable]
    public class PlayerConfig
    {
        [SerializeField] private PlayerType _type;
        [SerializeField] private Artifact _artifact;
        [SerializeField] private ConsumablesItem _itemConsumable;

        public PlayerType Type => _type;
        public Artifact ItemArtifact => _artifact;
        public ConsumablesItem ItemConsumable => _itemConsumable;

        public void SetConfig (GameObject itemConsumable, GameObject itemArtifact, PlayerType player)
        {
            _itemConsumable = itemConsumable.GetComponent<ConsumablesItem>();
            _artifact = itemArtifact.GetComponent<Artifact>();
            _type = player;
        }
    }
}

