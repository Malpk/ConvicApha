using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MainMode
{
    public class DeviceSpawner : MonoBehaviour
    {
        [SerializeField] private bool _playOnStart;
        [Header("Spawn Setting")]
        [Min(2)]
        [SerializeField] private int _spawnRadius = 2;
        [SerializeField] private float _spawnDelay;
        [Min(0)]
        [SerializeField] private float _deviceDensity;
        [SerializeField] private PoolDevice _pool;
        [SerializeField] private Vector2Int _position = Vector2Int.zero;
        [Header("Border Setting")]
        [Min(0)]
        [SerializeField] private float _borderDensity = 1;
        [SerializeField] private PoolItem _borderDevice;
        [Header("Reference")]
        [SerializeField] private Player _player;
        [SerializeField] private MapGrid _mapGrid;

        private float _progress = 0f;
        private List<Point> _borderPoint = new List<Point>();
        private List<Point> _spawnPoints = new List<Point>();

        private void Start()
        {
            if (_mapGrid)
            {
                _mapGrid.GetPointInRadius(out _spawnPoints, _position, _spawnRadius);
                _mapGrid.GetCirculBorder(out _borderPoint, _position, _spawnRadius);
            }
            if (_playOnStart)
                Play();
            else
                Stop();
        }

        private void FixedUpdate()
        {
            _progress += Time.fixedDeltaTime / _spawnDelay;
            if (_progress >= 1f)
            {
                _progress = 0f;
                Spawn();
            }
        }

        public void Intializate(MapGrid mapGrid, Player player, Vector2Int position)
        {
            _player = player;
            _mapGrid = mapGrid;
            _position = position;
            mapGrid.GetPointInRadius(out _spawnPoints, position, _spawnRadius);
            mapGrid.GetCirculBorder(out _borderPoint, position, _spawnRadius);
        }

        public void Play()
        {
            _progress = 0f;
            CreateBorder();
            enabled = true;
        }

        public void Stop()
        {
            enabled = false;
            _borderDevice.ClearPool(true);
        }

        private void Spawn()
        {
            if (GetFreePoints(out List<Point> points))
            {
                if (CreateDevice(out UpPlatform device))
                {
                    var point = points[Random.Range(0, points.Count)];
                    device.transform.parent = transform;
                    device.UpDevice();
                    point.SetItem(device);
                }
            }
        }

        private bool CreateDevice(out UpPlatform device)
        {
            if (_pool.GetPool(out PoolItem pool))
            {
                return pool.Create(out device);
            }
            else
            {
                device = null;
                return false;
            }
        }

        private bool GetFreePoints(out List<Point> points)
        {
            points = new List<Point>();
            foreach (var point in _spawnPoints)
            {
                if (!point.IsBusy)
                {
                    if(CheakPosition(point, _spawnPoints))
                        points.Add(point);
                }
            }
            return points.Count > 0;
        }

        private void CreateBorder()
        {
            var border = new List<Point>();
            border.AddRange(_borderPoint);
            while (border.Count > 0)
            {
                var point = border[Random.Range(0, border.Count)];
                _borderDevice.Create(out UpPlatform device);
                device.transform.parent = transform;
                point.SetItem(device);
                device.UpDevice();
                var newList = new List<Point>();
                for (int i = 0; i < border.Count; i++)
                {
                    if (Vector2.Distance(border[i].Position, point.Position) > _borderDensity)
                        newList.Add(border[i]);
                }
                border = newList;
            }
        }

        private bool CheakPosition(Point selectPoint, List<Point> points)
        {
            foreach (var point in points)
            {
                if (point.IsBusy)
                {
                    if (Vector2.Distance(selectPoint.Position, point.Position) <= _deviceDensity)
                        return false;
                }
            }
            return true;
        }

    }
}
