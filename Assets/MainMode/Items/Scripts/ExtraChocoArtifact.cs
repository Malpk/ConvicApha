using UnityEngine;
using MainMode.Effects;

namespace MainMode.Items
{
    public class ExtraChocoArtifact : Item
    {
        [Header("Time Active")]
        [Min(1)]
        [SerializeField] private float _timeActive;
        [Header("Reference")]
        [SerializeField] private MovementEffect _itemEffect;
        private void OnEnable()
        {
            UseAction += Actvate;
        }
        private void OnDisable()
        {
            UseAction -= Actvate;
        }
        private void Actvate()
        {
            user.AddEffects(_itemEffect, _timeActive);
        }
    }
}



