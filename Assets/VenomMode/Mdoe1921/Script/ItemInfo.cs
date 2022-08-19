using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MainMode.Items;

namespace MainMode
{
    [System.Serializable]
    public class ItemInfo
    {
        [SerializeField] private string _name;
        [Min(0)]
        [SerializeField] private int _maxLimit;
        [SerializeField] private SpawnItem _item;

        private int coutnDeactive = 0;
        private List<SpawnItem> _pool = new List<SpawnItem>();

        public int Count => GetCount();
        public bool IsAccess => Count < _maxLimit || _pool.Count <=_maxLimit;

        public SpawnItem Instantiate(Transform transform)
        {
            GetCount();
            if (coutnDeactive > 0)
            {
                var item = GetDeactiveItem();
                item.transform.position = transform.position;
                item.transform.parent = transform;
                item.SetMode(true);
                return item;
            }
            else if (_pool.Count <= _maxLimit)
            {
                var item = MonoBehaviour.Instantiate(_item.gameObject,transform).GetComponent<SpawnItem>();
                _pool.Add(item);
                return item;
            }
            else
            {
                return null;
            }
        }
        private int GetCount()
        {
            int count = 0;
            foreach (var item in _pool)
            {
                if (item.IsShow)
                    count++;
            }
            coutnDeactive = _pool.Count - count;
            return count;
        }
        private SpawnItem GetDeactiveItem()
        {
            foreach (var item in _pool)
            {
                if (!item.IsShow)
                    return item;
            }
            return null;
        }
        public void DeSpawnItems()
        {
            foreach (var item in _pool)
            {
                item.SetMode(false);
            }
        }
    }
}