using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MainMode
{
    public class TileC14 : Trap
    {
        [Range(0,1f)]
        [SerializeField] private float _speedReduce = 1f;
        [SerializeField] private float _durationEffect;

        public override TrapType DeviceType => TrapType.C14;

        private void OnTriggerEnter2D(Collider2D collision)
        {
            SetScreen(collision, _durationEffect);
            if (collision.TryGetComponent<IMoveEffect>(out IMoveEffect target))
            {
                target.ChangeSpeed(_durationEffect,EffectType.Stone,_speedReduce);
            }
        }
    }
}