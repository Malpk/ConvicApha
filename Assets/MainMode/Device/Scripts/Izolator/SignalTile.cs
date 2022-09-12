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

        [SerializeField] private Collider2D _collider;
        [SerializeField] private SpriteRenderer _body;

        public TrapType DeviceType => TrapType.SignalTile;

        public delegate void TrigerEnter(Transform target);
        public event System.Action SingnalAction;
        public event TrigerEnter EnterAction;

        private void Awake()
        {
            _body.enabled = false;
            _collider = GetComponent<Collider2D>();
            _collider.isTrigger = true;
            SetMode(false);
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.GetComponent<Player>())
            {
                if (EnterAction != null)
                    EnterAction(collision.transform);
                if (SingnalAction != null)
                    SingnalAction();
            }
        }

        public void SetMode(bool mode)
        {
            _body.enabled = IsShowTile ? mode : false;
        }
    }
}