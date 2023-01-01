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
        [Min(1)]
        [SerializeField] private float _spawnRadius = 1;
        [Range(0.001f, 2)]
        [SerializeField] private float _timeDelay = 0.001f;
        [SerializeField] private PoolDevice _pool;
        [Header("Game Setting")]
        [Min(0)]
        [SerializeField] private int _difficleGame = 0;
        [Min(1)]
        [SerializeField] private float _difficleTimeLimite = 1;
        [SerializeField] private AnimationCurve _curveDificle;
        [Header("Reference")]
        [SerializeField] private Player _player;

        private bool _isPlay = false;
        private MapGrid _mapGrid;

        private void Awake()
        {
            _mapGrid = GetComponent<MapGrid>();
        }

        [Inject]
        public void Construct(Player player)
        {
            _player = player;
        }

        public void Play()
        {
            if (!_isPlay)
            {
                _isPlay = true;
                StartCoroutine(Spawning());
            }
        }
        public void Stop()
        {
            if (_isPlay)
            {
                _isPlay = false;
                _pool.ClearPool();
            }
        }

        private IEnumerator Spawning()
        {
            var duration = 0f;
            while (_isPlay)
            {
                var delay = _timeDelay / (1f +  _difficleGame / 10f *
                    _curveDificle.Evaluate(duration / _difficleTimeLimite));
                duration += delay;
                yield return WaitDelay(delay);
                if (_isPlay)
                    SpawnItem();
            }
        }

        private void SpawnItem()
        {
            if (_pool.GetPool(out PoolItem pool))
            {
                if (pool.Create(out UpPlatform item))
                {
                    if (_mapGrid.SetItemOnMap(item, _spawnRadius, pool.DistanceFromPlayer, _player.transform))
                    {
                        item.transform.parent = transform;
                    }
                }
            }
        }

        private IEnumerator WaitDelay(float delay)
        {
            var progress = 0f;
            while (progress <= 1f && _isPlay)
            {
                progress += Time.deltaTime / delay;
                yield return null;
            }
        }
     
    }
}