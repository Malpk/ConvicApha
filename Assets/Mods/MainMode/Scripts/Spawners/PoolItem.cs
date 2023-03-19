using System.Collections.Generic;
using UnityEngine;

namespace MainMode
{
    [System.Serializable]
    public class PoolItem
    {
        [Header("Spawn Setting")]
        [Min(0)]
        [SerializeField] private int _distanceFormPlayer;
        [Header("Referemce")]
        [SerializeField] private DevicePlatform _perfab;

        private List<DevicePlatform> _createItems = new List<DevicePlatform>();
        private List<DevicePlatform> _pool = new List<DevicePlatform>();

        public int DistanceFromPlayer => _distanceFormPlayer;

        public void ClearPool()
        {
            while (_createItems.Count > 0)
            {
                ReturnInPool(_createItems[0]);
            }
        }
        public DevicePlatform Create()
        {
            var device = Instantiate();
            _createItems.Add(device);
            device.OnDelete += ReturnInPool;
            return device;
        }

        private DevicePlatform Instantiate()
        {
            if (_pool.Count > 0)
            {
                var item = _pool[0];
                item.gameObject.SetActive(true);
                _pool.Remove(item);
                return item;
            }
            else
            {
               return Object.Instantiate(_perfab.gameObject).
                    GetComponent<DevicePlatform>();
            }
        }

        private void ReturnInPool(DevicePlatform device)
        {
            device.OnDelete -= ReturnInPool;
            device.gameObject.SetActive(false);
            _pool.Add(device);
            _createItems.Remove(device);
        }
    }
}