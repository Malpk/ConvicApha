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
        [SerializeField] private Item _item;

        private int coutnDeactive = 0;
        private List<Item> _pool = new List<Item>();

        public int Count => GetCount();
        public bool IsAccess => Count <= _maxLimit || _pool.Count <=_maxLimit;

        public Item Instantiate(Transform transform)
        {
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
                var item = MonoBehaviour.Instantiate(_item.gameObject,transform).GetComponent<Item>();
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
                if (item.Active)
                    count++; ;
            }
            coutnDeactive = _pool.Count - count;
            return count;
        }
        private Item GetDeactiveItem()
        {
            foreach (var item in _pool)
            {
                if (!item.Active)
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