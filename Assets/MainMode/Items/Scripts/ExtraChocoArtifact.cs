using UnityEngine;
using PlayerComponent;

namespace MainMode.Items
{
    public class ExtraChocoArtifact : Item
    {
        [Header("Time Active")]
        [Min(1)]
        [SerializeField] private float _timeActive;
        [Header("Reference")]
        [SerializeField] private MovementEffect _itemEffect;

        public override string Name => "Шоколадка";

        protected override void OnEnable()
        {
            base.OnEnable();
            UseAction += Actvate;
        }
        protected override void OnDisable()
        {
            base.OnDisable();
            UseAction -= Actvate;
        }
        private void Actvate()
        {
            user.AddEffects(_itemEffect, _timeActive);
        }
    }
}



