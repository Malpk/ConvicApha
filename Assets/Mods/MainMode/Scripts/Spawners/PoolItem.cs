using System.Collections.Generic;
using UnityEngine;

namespace MainMode
{
    [System.Serializable]
    public class PoolItem
    {
        [Header("Spawn Setting")]
        [SerializeField] private int _maxCount;
        [Min(0)]
        [SerializeField] private int _distanceFormPlayer;
        [SerializeField] private int _requaredSize;
        [SerializeField] private bool _isInfinity;
        [Min(1f)]
        [SerializeField] private float _spawnProbility = 1f;
        [Header("Referemce")]
        [SerializeField] private UpPlatform _perfab;

        private List<UpPlatform> _poolActive = new List<UpPlatform>();
        private List<UpPlatform> _poolDeactive = new List<UpPlatform>();

        public int DistanceFromPlayer => _distanceFormPlayer;
        public int SizePoint => _requaredSize;
        public bool IsAcces => _poolActive.Count < _maxCount || _isInfinity;
        public float SpawnProbility => GetProbility();

        public void ClearPool(bool waitComplite)
        {
            if (!waitComplite)
            {
                Clear(_poolActive);
            }
            else
            {
                foreach (var device in _poolActive)
                {
                    device.DownDevice();
                }
            }
            Clear(_poolDeactive);
        }

        private void Clear(List<UpPlatform> pool)
        {
            while (pool.Count > 0)
            {
                var item = pool[0];
                pool.Remove(item);
                MonoBehaviour.Destroy(item.gameObject);
            }
        }
        public bool Create(out UpPlatform item)
        {
            Update();
            if (IsAcces)
            {
                item = Instantiate();
                return item;
            }
            else
            {
                item = null;
                return false;
            }
        }

        private UpPlatform Instantiate()
        {
            if (_poolDeactive.Count > 0)
            {
                var item = _poolDeactive[0];
                _poolDeactive.Remove(item);
                _poolActive.Add(item);
                return item;
            }
            var newItem = MonoBehaviour.Instantiate(_perfab.gameObject).GetComponent<UpPlatform>();
            _poolActive.Add(newItem);
            return newItem;
        }
        public void Update()
        {
            for (int i = 0; i < _poolActive.Count; i++)
            {
                if (!_poolActive[i].IsShow)
                {
                    var item = _poolActive[i];
                    _poolActive.Remove(item);
                    _poolDeactive.Add(item);
                }
            }
        }
        private float GetProbility()
        {
            if (IsAcces)
                return _poolActive.Count > 0 ? _spawnProbility / _poolActive.Count : _spawnProbility;
            else
                return 0f;
        }
    }
}