using System.Collections.Generic;
using UnityEngine;

namespace MainMode.Mode1921
{
    public class DeviceSpawner : GeneralSpawner
    {
        [Header("Spawner Setting")]
        [SerializeField] private string[] _keysLoad;
        [SerializeField] private Vector2Int _deviceCount;

        private bool _isRedy;
        private SmartItem[] _assets;
        private List<SmartItem> _devices = new List<SmartItem>();

        public override bool IsRedy => _isRedy;

        private async void Awake()
        {
            var task = LoadAssetsAsync<SmartItem>(_keysLoad);
            await task;
            _assets = task.Result.ToArray();
            _isRedy = true;
        }
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
                var index = Random.Range(0, _assets.Length);
                var device = Instantiate(_assets[index].gameObject).GetComponent<SmartItem>();
                device.transform.parent = transform;
                mapGrid.SetItemOnMap(device);
                _devices.Add(device);
            }
        }
    }
}