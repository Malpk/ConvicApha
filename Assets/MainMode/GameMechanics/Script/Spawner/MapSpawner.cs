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
        
        private MapGrid _mapGrid;
        private Coroutine _corotine;

        private void Awake()
        {
            _mapGrid = GetComponent<MapGrid>();
        }

        public async Task Intializate(Player player)
        {
            _player = player;
            var list = new List<Task>();
            foreach (var pool in _pools)
            {
                list.Add(pool.LoadAsset());
            }
            await Task.WhenAll(list);
        }
        private void OnDestroy()
        {
            foreach (var pool in _pools)
            {
                pool.UnLaodAsset();
            }
        }
        public void Run()
        {
            if (_corotine == null)
            {
                Restart();
                _corotine = StartCoroutine(Spawning());
            }
        }
        private void Restart()
        {
            foreach (var pool in _pools)
            {
                pool.ResetItem();
            }
        }
        private IEnumerator Spawning()
        {
            var duration = 0f;
            while (!_player.IsDead)
            {
                var delay = _timeDelay / (1f +  _difficleGame / 10f *
                    _curveDificle.Evaluate(duration / _difficleTimeLimite));
                duration += delay;
                yield return WaitDelay(delay);
                if (GetPool(out PoolItem pool))
                {
                    if (GetFreePoints(_spawnRadius, pool, out List<Point> freePoints))
                    {
                        var point = freePoints[Random.Range(0, freePoints.Count)];
                        if (pool.Create(out SpawnItem item))
                        {
                            item.SetMode(true);
                            point.SetItem(item);
                            item.transform.parent = transform;
                        }
                    }
                }
            }
        }

        private IEnumerator WaitDelay(float delay)
        {
            var progress = 0f;
            while (progress <= 1f && !_player.IsDead)
            {
                progress += Time.deltaTime / delay;
                yield return null;
            }
        }
        private bool GetFreePoints(float filtrDistance, PoolItem perfab ,out List<Point> freePoints)
        {
            freePoints = new List<Point>();
            foreach (var point in _mapGrid.Points)
            {
                var distance = Vector2.Distance(_player.Position, point.Position);
                if (distance <= filtrDistance && distance > perfab.DistanceFromPlayer && !point.IsBusy)
                {
                    freePoints.Add(point);
                }
            }
            return freePoints.Count > 0;
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