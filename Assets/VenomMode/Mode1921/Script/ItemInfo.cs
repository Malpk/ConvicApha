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
        [SerializeField] private SmartItem _item;

        private int coutnDeactive = 0;
        private List<SmartItem> _pool = new List<SmartItem>();

        public int Count => GetCount();
        public bool IsAccess => Count < _maxLimit || _pool.Count <=_maxLimit;

        public SmartItem Instantiate(Transform transform)
        {
            GetCount();
            if (coutnDeactive > 0)
            {
                var item = GetDeactiveItem();
                item.transform.position = transform.position;
                item.transform.parent = transform;
                item.ShowItem();
                return item;
            }
            else if (_pool.Count <= _maxLimit)
            {
                var item = MonoBehaviour.Instantiate(_item.gameObject,transform).GetComponent<SmartItem>();
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
        private SmartItem GetDeactiveItem()
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
                item.HideItem();
            }
        }
    }
}