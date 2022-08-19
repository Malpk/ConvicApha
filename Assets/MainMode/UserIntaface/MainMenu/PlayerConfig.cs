using MainMode;
using System.Collections.Generic;
using UnityEngine;

namespace UserIntaface.MainMenu
{
    [System.Serializable]
    public class PlayerConfig
    {      
        public GameObject itemConsumable;
        public GameObject itemArtifact;
        public PlayerType characterType;
        public PlayerConfig(ItemScroller consumableScroller,ItemScroller artifactScroller, CharacterScroller characterScroller) 
        {
            itemConsumable = GameObject.Instantiate(consumableScroller.SelectedItem.itemPrefab);
            itemArtifact = GameObject.Instantiate(artifactScroller.SelectedItem.itemPrefab);
            characterType = characterScroller.SelectedPlayer.PlayerType;
        }     
    }
}
