using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MainMode
{
    [RequireComponent(typeof(Collider2D))]
    public class SignalTile : MonoBehaviour
    {
        [Header("General Setting")]
        public bool IsShowTile = false;

        [SerializeField] private SpriteRenderer _body;

        private Collider2D _collider;

        public TrapType DeviceType => TrapType.SignalTile;

        public delegate void Singnal(Collider2D collision);
        public event Singnal SingnalAction;

        private void Awake()
        {
            _body.enabled = false;
            _collider = GetComponent<Collider2D>();
            _collider.isTrigger = true;
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (SingnalAction != null &&  collision.GetComponent<Player>())
                SingnalAction(collision);
        }

        public void SetMode(bool mode)
        {
            _body.enabled =IsShowTile ? mode : false;
            _collider.enabled = mode;
        }
    }
}