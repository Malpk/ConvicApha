using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MainMode
{
    [RequireComponent(typeof(Collider2D))]
    public class SignalTile : Device
    {
        [SerializeField] private SpriteRenderer _body;

        private Collider2D _collider;

        public override TrapType DeviceType => TrapType.SignalTile;

        public delegate void Singnal(Collider2D collision);
        public event Singnal SingnalAction;

        protected override void Intilizate()
        {
            _collider = GetComponent<Collider2D>();
            Deactivate();
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (SingnalAction != null &&  collision.GetComponent<Player>())
                SingnalAction(collision);
        }

        protected override void SetState(bool mode)
        {
            if(_body)
                _body.enabled = mode;
            _collider.enabled = mode;
        }
    }
}