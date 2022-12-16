using UnityEngine;
using PlayerComponent;

namespace MainMode.Items
{
    public class ExtraChocoArtifact : ConsumablesItem
    {
        [Header("Time Active")]
        [Min(1)]
        [SerializeField] private float _timeActive;
        [Header("Reference")]
        [SerializeField] private MovementEffect _itemEffect;

        public override string Name => "Шоколадка";

        protected override void UseConsumable()
        {
            user.AddEffect(_itemEffect, _timeActive);
        }
    }
}



