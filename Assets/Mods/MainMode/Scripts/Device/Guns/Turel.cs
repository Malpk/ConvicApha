using UnityEngine;
using System.Collections.Generic;

namespace MainMode
{
    public class Turel : Gun
    {
        [Header("Attacks properties")]
        [Min(1)]
        [SerializeField] private int _countShoot = 1;
        [SerializeField] protected float _shootDelay = 1f;
        [SerializeField] protected Bullet _bulletPrefab;
        [SerializeField] protected Transform _spawnTransform;
        [Header("Move properties")]
        [SerializeField] protected Rigidbody2D _rigidbody;

        private int _curretShootCount;
        private float _progress = 0f;
        private float _angleSteep;
        private float _startAngle;
        private float _rotateSteep;
        private int[] _directions = new int[] { -1, 1 };
        private List<Bullet> _pool = new List<Bullet>();

        public override TrapType DeviceType => TrapType.Turel;

        private void OnValidate()
        {
            _angleSteep = 360 / _countShoot;
        }

        protected override void Launch()
        {
            DropProgress();
            _curretShootCount = 0;
            _rotateSteep = _angleSteep * _directions[Random.Range(0, _directions.Length)];
        }

        private void Update()
        {
            _progress = Mathf.Clamp01(_progress + Time.deltaTime / _shootDelay);
            _rigidbody.rotation = _startAngle + _rotateSteep * _progress;
            if (_progress >= 1)
            {
                var bullet = CreateProjectale();
                bullet.transform.position = _spawnTransform.position;
                bullet.transform.rotation = _spawnTransform.rotation;
                bullet.Shoot();
                DropProgress();
                _curretShootCount++;
                gunAnimator.SetTrigger("Shoot");
                if (_countShoot == _curretShootCount)
                {
                    Deactivate();
                }
            }
        }
        private void DropProgress()
        {
            _progress = 0f;
            _startAngle = _rigidbody.rotation;
        }
        private Bullet CreateProjectale()
        {
            foreach (var poolBullet in _pool)
            {
                if (poolBullet.IsDestroy)
                {
                    return poolBullet;
                }
            }
            var bullet = Instantiate(_bulletPrefab.gameObject, transform.parent).GetComponent<Bullet>();
            bullet.SetAttack(attackInfo);
            _pool.Add(bullet);
            return bullet;
        }
    }
}