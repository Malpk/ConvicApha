using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace MainMode
{
    [RequireComponent(typeof(CircleCollider2D))]
    public class DetectDeviceSet : MonoBehaviour
    {
        [SerializeField] private UnityEvent _detect;

        private CircleCollider2D _collider;

        private void Awake()
        {
            _collider = GetComponent<CircleCollider2D>();
            _collider.isTrigger = true;
        }

        public virtual void SetMode(bool mode)
        {
            _collider.enabled = mode;
        }
        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.TryGetComponent(out Player player))
            {
                SendMessange(player);
            }
        }
        protected virtual void SendMessange(Player player)
        {
            _detect.Invoke();
        }
    }
}
