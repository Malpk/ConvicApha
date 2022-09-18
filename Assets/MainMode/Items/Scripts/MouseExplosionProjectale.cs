using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MainMode.Items
{
    public class MouseExplosionProjectale : MonoBehaviour
    {
        [SerializeField] private float _speedMovement;
        [Header("Reference")]
        [SerializeField] private Collider2D _collider;
        [SerializeField] private Rigidbody2D _rigidBody;
        [SerializeField] private SpriteRenderer _spriteBody;

        private bool _isFly;

        public bool IsShoot => _isFly;

        public event System.Action DestroyAction;

        public void Shoot(float timeDestory)
        {
            SetMode(true);
            StartCoroutine(Fly(timeDestory));
        }

        private IEnumerator Fly(float timeDestroy)
        {
            var progress = 0f;
            while (progress < 1f && _isFly)
            {
                _rigidBody.MovePosition(_rigidBody.position + (Vector2)transform.up * _speedMovement * Time.fixedDeltaTime);
                yield return new WaitForFixedUpdate();
                progress += Time.fixedDeltaTime / timeDestroy;
            }
            Destroy(gameObject);
        }
        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.TryGetComponent(out DeviceV2 device))
            {
                if (device.IsShow)
                {
                    device.Explosion();
                    SetMode(false);
                }
            }
        }

        private void SetMode(bool mode)
        {
            _isFly = mode;
            _collider.enabled = mode;
            _spriteBody.enabled = mode;
        }
    }
}