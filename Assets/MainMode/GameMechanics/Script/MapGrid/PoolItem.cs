using System.Collections;
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
        [SerializeField] private bool _isInfinity;
        [SerializeField] private float _timeDestoroy = 1f;
        [Min(1f)]
        [SerializeField] private float _spawnProbility = 1f;
        [Header("Referemce")]
        [SerializeField] private SpawnItem _perfab;

        private List<SpawnItem> _poolActive = new List<SpawnItem>();
        private List<SpawnItem> _poolDeactive = new List<SpawnItem>();

        public int DistanceFromPlayer => _distanceFormPlayer;
        public bool IsAcces => _poolActive.Count < _maxCount || _isInfinity;
        public float SpawnProbility => GetProbility(); 

        public void ResetItem()
        {
            while (_poolActive.Count > 0)
            {
                var item = _poolActive[0];
                _poolActive.Remove(item);
                item.Deactivate();
            }
        }
        public bool Create(out SpawnItem item)
        {
            CheakState();
            if (IsAcces)
            {
                item = Instantiate();
                if(!_poolActive.Contains(item))
                    _poolActive.Add(item);
                return true;
            }
            else
            {
                item = null;
                return false;
            }
        }

        private SpawnItem Instantiate()
        {
            if (_poolDeactive.Count > 0)
            {
                var item = _poolDeactive[0];
                _poolDeactive.Remove(item);
                return item;
            }
            return MonoBehaviour.Instantiate(_perfab.gameObject).GetComponent<SpawnItem>();
        }
        private void CheakState()
        {
            for (int i = 0; i < _poolActive.Count; i++)
            {
                if (!_poolActive[i].IsShow)
                {
                    var item = _poolActive[i];
                    _poolActive.Remove(item);
                    if (_poolActive.Count > _poolDeactive.Count)
                        _poolDeactive.Add(item);
                    else
                        MonoBehaviour.Destroy(item.gameObject);
                }
            }
            while (_poolActive.Count < _poolDeactive.Count)
            {
                var item = _poolDeactive[0];
                _poolDeactive.Remove(item);
                MonoBehaviour.Destroy(item.gameObject);
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