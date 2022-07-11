using System.Collections;
using UnityEngine;

namespace MainMode.Items
{
    public class ExtraChocoArtifact : Artifact
    {
        [SerializeField] private ItemEffect _itemEffect;
        public override void Use()
        {
            _target.ApplyEffect(_itemEffect);
        }
    }
}



