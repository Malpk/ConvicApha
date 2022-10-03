using UnityEngine;
using MainMode.Items;

namespace MainMode.LoadScene
{
    [System.Serializable]
    public class PlayerConfig
    {
        public readonly PlayerType player;
        public readonly Artifact itemArtifact;
        public readonly ConsumablesItem itemConsumable;

        public PlayerConfig(GameObject itemConsumable, GameObject itemArtifact, PlayerType player)
        {
            Debug.Log(itemConsumable.name);
            this.itemConsumable = itemConsumable.GetComponent<ConsumablesItem>();
            this.itemArtifact = itemArtifact.GetComponent<Artifact>();
            this.player = player;
        }
    }
}

