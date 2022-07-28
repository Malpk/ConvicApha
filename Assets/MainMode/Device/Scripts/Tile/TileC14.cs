using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MainMode.Effects;

namespace MainMode
{
    public class TileC14 : Trap
    {
        [SerializeField] private MovementEffect _effect;

        public override TrapType DeviceType => TrapType.C14;

        private void OnTriggerEnter2D(Collider2D collision)
        {
            SetScreen(collision, attackInfo);
            if (collision.TryGetComponent(out IAddEffects target))
            {
                target.AddEffects(_effect, attackInfo.TimeEffect);
            }
        }
    }
}