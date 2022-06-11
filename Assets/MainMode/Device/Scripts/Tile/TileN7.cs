using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MainMode
{
    public class TileN7 : Trap
    {
        [SerializeField] private float _duration;

        private void OnTriggerEnter2D(Collider2D collision)
        {
            SetScreen(collision, _duration);
            if (collision.TryGetComponent<IMoveEffect>(out IMoveEffect target))
            {
                target.StopMove(_duration,attackInfo.Effect);
            }
        }
    }
}