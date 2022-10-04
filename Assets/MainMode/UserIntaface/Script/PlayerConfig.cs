using UnityEngine;
using MainMode.Items;

namespace MainMode.LoadScene
{
    [System.Serializable]
    public class PlayerConfig
    {
        public readonly PlayerType type;
        public readonly Artifact itemArtifact;
        public readonly ConsumablesItem itemConsumable;

        public PlayerConfig(GameObject itemConsumable, GameObject itemArtifact, PlayerType player)
        {
            this.itemConsumable = itemConsumable.GetComponent<ConsumablesItem>();
            this.itemArtifact = itemArtifact.GetComponent<Artifact>();
            this.type = player;
        }
    }
}

