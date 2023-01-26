using UnityEngine;

namespace MainMode
{
    public class ShootDeviceSet : MonoBehaviour
    {
        [SerializeField] private int _countShootInSecond;
        [SerializeField] private float _spread;
        [SerializeField] private float _spreadDistance;
        [SerializeField] private DamageInfo _damage;
        [Header("Reference")]
        [SerializeField] private Pool _pool;
        [SerializeField] private Bullet _prefab;

        private float _progress = 0f;
        private float _shootDelay = 1f;

        private void OnValidate()
        {
            _shootDelay = 1f / _countShootInSecond;
        }

        private void FixedUpdate()
        {
            _progress += Time.fixedDeltaTime / _shootDelay;
            if (_progress >= 1f)
            {
                _progress = 0f;
                Shoot();
            }
        }

        public void Play()
        {
            _progress = 0f;
            enabled = true;
        }
        public void Stop()
        {
            enabled = false;
        }

        private void Shoot()
        {
            if (_pool.Create(_prefab).TryGetComponent(out Bullet bullet))
            {
                var spread = transform.right * Random.Range(-_spread, _spread);
                bullet.transform.position = transform.position;
                bullet.transform.up = transform.up * _spreadDistance + spread;
                bullet.SetAttack(_damage);
                bullet.Shoot();
            }
        }

    }
}