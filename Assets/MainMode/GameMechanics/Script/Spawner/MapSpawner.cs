using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading.Tasks;

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
        [SerializeField] private List<PoolItem> _pools;
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
        public void Intializate(Player player)
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
                foreach (var pool in _pools)
                {
                    pool.ClearPool();
                }
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
            if (GetPool(out PoolItem pool))
            {
                if (pool.Create(out SmartItem item))
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
            while (progress <= 1f && _player.IsPlay)
            {
                progress += Time.deltaTime / delay;
                yield return null;
            }
        }
        private bool GetPool(out PoolItem pool)
        {
            pool = null;
            var pools = GeneralProbility(out float amount);
            var choosee = Random.Range(0, 1.000f);
            if (pools.Count > 1)
            {
                var curretProbillity = 0f;
                for (int i = 0; i < pools.Count; i++)
                {
                    curretProbillity += pools[i].SpawnProbility / amount;
                    if (choosee <= curretProbillity)
                    {
                        pool = pools[i];
                        return true;
                    }
                }
            }
            else if (pools.Count > 0)
            {
                pool = pools[0];
                return true;
            }
            return false;
        }

        public List<PoolItem> GeneralProbility(out float amount)
        {
            amount = 0f;
            var list = new List<PoolItem>();
            for (int i = 0; i < _pools.Count; i++)
            {
                if (_pools[i].SpawnProbility > 0)
                {
                    amount += _pools[i].SpawnProbility;
                    list.Add(_pools[i]);
                }
            }
            return list;
        }
    }
}