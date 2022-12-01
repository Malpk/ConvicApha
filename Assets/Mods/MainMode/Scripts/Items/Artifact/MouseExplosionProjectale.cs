using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MainMode.Items
{
    public class MouseExplosionProjectale : MonoBehaviour
    {
        [SerializeField] private float _speedMovement;
        [Header("Reference")]
        [SerializeField] private Rigidbody2D _rigidBody;

        private bool _isFly;
        private float _progress = 0f;
        private float _timeDestroy;

        public bool IsShoot => _isFly;

        public event System.Action DestroyAction;

        public void Shoot(float timeDestory)
        {
            SetMode(true);
            _timeDestroy = timeDestory;
        }

        private void Update()
        {
            _progress += Time.deltaTime/ _timeDestroy;
            _rigidBody.MovePosition(_rigidBody.position + (Vector2)transform.up * _speedMovement * Time.fixedDeltaTime);
            if(_progress >=1f)
                Destroy(gameObject);
        }
        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.TryGetComponent(out IExplosion target))
            {
                if (target.IsReadyExplosion)
                {
                    target.Explosion();
                    Destroy(gameObject);
                }
            }
        }

        private void SetMode(bool mode)
        {
            _isFly = mode;
            gameObject.SetActive(mode);
        }
    }
}