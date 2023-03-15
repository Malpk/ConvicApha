using System.Collections.Generic;
using UnityEngine;

namespace MainMode
{
    [System.Serializable]
    public class DevicePool
    {
        [SerializeField] private DevicePlatform _prefab;

        private List<DevicePlatform> _pool = new List<DevicePlatform>();

        private Transform _holder;

        public void SetHolder(Transform holder)
        {
            _holder = holder;
        }

        public DevicePlatform Create()
        {
            DevicePlatform platform;
            if (_pool.Count > 0)
            {
                platform = _pool[0];
                _pool.Remove(platform);
                platform.gameObject.SetActive(true);
            }
            else
            {
                platform = Object.Instantiate(_prefab.gameObject, _holder).
                    GetComponent<DevicePlatform>();
            }
            platform.OnDelete += AddDevceInPool;
            return platform;
        }

        private void AddDevceInPool(DevicePlatform platform)
        {
            platform.gameObject.SetActive(false);
            platform.OnDelete -= AddDevceInPool;
            _pool.Add(platform);
        }
    }
}
