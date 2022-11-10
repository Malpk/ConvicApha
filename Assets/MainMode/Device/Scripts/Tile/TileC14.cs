using UnityEngine;
using PlayerComponent;

namespace MainMode
{
    public class TileC14 : Trap
    {
        [SerializeField] private MovementEffect _effect;

        public override TrapType DeviceType => TrapType.C14;

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.TryGetComponent(out IAddEffects target) && IsShow)
            {
                target.AddEffects(_effect, attackInfo.TimeEffect);
            }
        }
    }
}