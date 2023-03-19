using System.Collections.Generic;
using UnityEngine;

namespace MainMode
{
    public class DeviceSpwaner : MonoBehaviour
    {
        [SerializeField] private bool _playOnStart;
        [SerializeField] private float _spawnDelay;
        [SerializeField] private DevicePool[] _pools;
        [Header("Reference")]
        [SerializeField] private MapGrid _grid;

        private float _progress = 0f;

        public void Play()
        {
            enabled = true;
        }

        public void Stop()
        {
            enabled = false;
        }
        private void OnValidate()
        {
            enabled = _playOnStart;
            _spawnDelay = _spawnDelay <= Time.fixedDeltaTime ? Time.fixedDeltaTime : _spawnDelay;
            foreach (var pool in _pools)
            {
                pool.SetHolder(transform);
            }
        }
        private void FixedUpdate()
        {
            _progress += Time.deltaTime / _spawnDelay;
            if (_progress > 1f)
            {
                if (_grid.GetFreePoints(out List<Point> points))
                {
                    var point = points[Random.Range(0, points.Count)];
                    var pool = _pools[Random.Range(0, _pools.Length)];
                    var device = pool.Create();
                    device.Show();
                    point.SetItem(device);
                }
                _progress = 0f;
            }
        }

    }
}