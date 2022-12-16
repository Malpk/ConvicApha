using UnityEngine;
using PlayerComponent;

namespace MainMode
{
    public class TileN7 : TileDevice
    {
        [Range(0.1f,1f)]
        [SerializeField] private float _timeEffectActive = 1;
        [SerializeField] private MovementEffect _effect;
        
        public override TrapType DeviceType => TrapType.N7;

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.TryGetComponent(out IAddEffects target))
            {
                target.AddEffect(_effect, _timeEffectActive);
            }
        }
    }
}