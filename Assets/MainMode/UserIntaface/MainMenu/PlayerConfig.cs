using MainMode;
using MainMode.Items;
using MainMode.LoadScene;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace UserIntaface.MainMenu
{
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
