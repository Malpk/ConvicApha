using UnityEngine;

namespace MainMode.Items
{
    public class ExtraChocoArtifact : Artifact
    {
        [SerializeField] private ItemEffect _itemEffect;
        public override void Use()
        {
            user.ApplyEffect(_itemEffect);
        }
    }
}



