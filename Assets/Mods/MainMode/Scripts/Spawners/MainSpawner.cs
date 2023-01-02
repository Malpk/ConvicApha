using System.Collections.Generic;
using UnityEngine;
using Zenject;

namespace MainMode
{
    [RequireComponent(typeof(MapGrid))]
    public class MainSpawner : MonoBehaviour
    {
        [Header("Spawn Setting")]
        [SerializeField] private bool _playOnStart;
        [Min(1)]
        [SerializeField] private float _spawnRadius = 1;
        [Range(0.1f, 2)]
        [SerializeField] private float _spawnDelay = 0.1f;
        [SerializeField] private PoolDevice _pool;
        [Header("Reference")]
        [SerializeField] private Player _player;
        [SerializeField] private MapGrid _mapGrid;

        private bool _isPlay = false;
        private float _progress = 0f;

        private List<Point> _points = new List<Point>();

        private void Start()
        {
            foreach (var point in _mapGrid.Points)
            {
                _points.Add(point);
            }
            if (_playOnStart)
            {
                Play();
            }
            else
            {
                Stop();
            }
        }
        private void FixedUpdate()
        {
            _progress += Time.deltaTime / _spawnDelay;
            if (_progress >= 1f)
            {
                _progress = 0f;
                SpawnItem();
            }
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
                enabled = true;
                _progress = 0f;
            }
        }
        public void Stop()
        {
            if (_isPlay)
            {
                _isPlay = false;
                enabled = false;
                _pool.ClearPool();
            }
        }

        private void SpawnItem()
        {
            if (_pool.GetPool(out PoolItem pool))
            {
                if (GetFreePoints(out List<Point> points, pool.SizePoint))
                {
                    Create(pool, points[Random.Range(0, points.Count)]);  
                }
            }
        }

        private void Create(PoolItem pool, Point point)
        {
            if (pool.Create(out UpPlatform device))
            {
                device.transform.parent = transform;
                point.SetItem(device, pool.SizePoint);
                if (device.TryGetComponent(out DeviceSpawner spawner))
                {
                    spawner.Intializate(_mapGrid, _player, point.ArrayPosition);
                }
                device.UpDevice();
            }
        }
        private bool GetFreePoints(out List<Point> points, int size)
        {
            points = new List<Point>();
            var busyPoints = GetBusyPoints();
            foreach (var point in _points)
            {
                if (!point.IsBusy)
                {
                    if (CheakDistance(point, busyPoints, size))
                    {
                        points.Add(point);
                    }
                }
            }
            return points.Count > 0;
        }
        private bool CheakDistance(Point point, List<Point> target, int size)
        {
            foreach (var busyPoint in target)
            {
                if (Vector2.Distance(busyPoint.Position, point.Position) < size + busyPoint.Size)
                {
                    return false;
                }
            }
            return true;
        }
        private List<Point> GetBusyPoints()
        {
            var list = new List<Point>();
            foreach (var point in _points)
            {
                if (point.IsBusy)
                    list.Add(point);
            }
            return list;
        }
    }
}