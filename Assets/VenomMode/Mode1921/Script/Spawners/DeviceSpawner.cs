using System.Collections.Generic;
using UnityEngine;

namespace MainMode.Mode1921
{
    public class DeviceSpawner : GeneralSpawner
    {
        [Header("Spawner Setting")]
        [SerializeField] private SmartItem[] _perfab;
        [SerializeField] private Vector2Int _deviceCount;

        private List<SmartItem> _devices = new List<SmartItem>();

        private void OnEnable()
        {
            PlayAction += CreateMap;
        }
        private void OnDisable()
        {
            PlayAction -= CreateMap;
        }
        private void OnValidate()
        {
            var x = _deviceCount.x < 0 ? 0 : _deviceCount.x;
            var y = _deviceCount.y < 0 ? 0 : _deviceCount.y;
            _deviceCount = new Vector2Int(x, y);
        }
        public override void Replay()
        {
            int count = Random.Range(_deviceCount.x, _deviceCount.y) - _devices.Count;
            if (count < 0)
                DeleteDevice(Mathf.Abs(count));
            foreach (var device in _devices)
            {
                mapGrid.SetItemOnMap(device);
            }
            if(count > 0)
                SpawnDevice(count);
        }
        private void DeleteDevice(int count)
        {
            for (int i = 0; i < count; i++)
            {
                _devices.Remove(_devices[0]);
            }
        }
        private void CreateMap()
        {
            int count = Random.Range(_deviceCount.x, _deviceCount.y);
            SpawnDevice(count);
        }

        private void SpawnDevice(int count)
        {
            for (int i = 0; i < count; i++)
            {
                var index = Random.Range(0, _perfab.Length);
                var device = Instantiate(_perfab[index].gameObject).GetComponent<SmartItem>();
                device.transform.parent = transform;
                mapGrid.SetItemOnMap(device);
                _devices.Add(device);
            }
        }
    }
}