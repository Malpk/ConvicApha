using System.Collections;
using UnityEngine;

namespace MainMode.Items
{
    public class ExtraChocoArtifact : Artifact
    {    
        public override void Pick(Player player)
        {
            _ownerPlayer = player;
            gameObject.SetActive(false);
        }

        public override void Use()
        {
            _ownerPlayer.ApplyEffect(_itemEffect);
        }
    }
}



