using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

namespace MainMode
{
    [RequireComponent(typeof(MapGrid))]
    public class MapSpawner : MonoBehaviour
    {
        [Header("Spawn Setting")]
        [SerializeField] private bool _playOnStart;
        [Min(1)]
        [SerializeField] private float _spawnRadius = 1;
        [Range(0.001f, 2)]
        [SerializeField] private float _timeDelay = 0.001f;
        [SerializeField] private List<PoolItem> _pools;
        [Header("Reference")]
        [SerializeField] private Player _player;
        [SerializeField] private MapGrid _mapGrid;


        private float _progress = 0f;
        private void Awake()
        {
            if (!_playOnStart)
                Stop();
        }

        [Inject]
        public void Construct(Player player)
        {
            _player = player;
        }

        private void FixedUpdate()
        {
            _progress += Time.fixedDeltaTime / _timeDelay;
            if (_progress >= 1f)
            {
                _progress = 0f;
                SpawnItem();
            }
        }

        public void Play()
        {
            enabled = true;
            _progress = 0f;
        }
        public void Stop()
        {
            enabled = false;
        }

        public void Clear()
        {
            foreach (var pool in _pools)
            {
                pool.ClearPool();
            }
        }

        private void SpawnItem()
        {
            if (GetPool(out PoolItem pool))
            {
                var item = pool.Create();
                item.transform.parent = transform;
                item.Show();
                _mapGrid.SetItemOnMap(item, _spawnRadius, pool.DistanceFromPlayer, _player.transform);
            }
        }

        private bool GetPool(out PoolItem pool)
        {
            pool = _pools[Random.Range(0, _pools.Count)];
            return pool != null;
        }
    }
}