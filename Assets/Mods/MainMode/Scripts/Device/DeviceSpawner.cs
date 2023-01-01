using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MainMode
{
    public class DeviceSpawner : MonoBehaviour
    {
        [SerializeField] private bool _playOnStart;
        [SerializeField] private int _spawnRadius;
        [SerializeField] private float _spawnDelay;
        [SerializeField] private float _distanceBothDevice;
        [SerializeField] private PoolDevice _pool;
        [SerializeField] private Vector2Int _position = Vector2Int.zero;
        [Header("Reference")]
        [SerializeField] private Player _player;
        [SerializeField] private MapGrid _mapGrid;

        private float _progress = 0f;
        private List<Point> _spawnPoints = new List<Point>();

        private void Start()
        {
            if (_mapGrid)
                _mapGrid.GetPointInRadius(out _spawnPoints, _position, _spawnRadius);
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

        public void Intializate(MapGrid mapGrid, Player player)
        {
            _player = player;
            _mapGrid = mapGrid;
            mapGrid.GetPointInRadius(out _spawnPoints, _position, _spawnRadius);
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
                    if(CheakPosition(point))
                        points.Add(point);
                }
            }
            return points.Count > 0;
        }

        private bool CheakPosition(Point selectPoint)
        {
            foreach (var point in _spawnPoints)
            {
                if (point.IsBusy)
                {
                    if (Vector2.Distance(selectPoint.Position, point.Position) <= _distanceBothDevice)
                        return false;
                }
            }
            return true;
        }
    }
}
