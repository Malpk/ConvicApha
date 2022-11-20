using UnityEngine;
using PlayerComponent;

namespace MainMode
{
    public class TileC14 : DeviceV2
    {
        [Range(0.1f, 1f)]
        [SerializeField] private float _timeEffectActive = 1;
        [SerializeField] private MovementEffect _effect;

        public override TrapType DeviceType => TrapType.C14;

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.TryGetComponent(out IAddEffects target))
            {
                target.AddEffects(_effect, _timeEffectActive);
            }
        }
    }
}