using System.Collections.Generic;
using UnityEngine;

public class Pool : MonoBehaviour
{
    private List<GameObject> _pool = new List<GameObject>();

    public GameObject Create(IPoolItem prefab)
    {
        GameObject item;
        if (_pool.Count > 0)
        {
            _pool[0].SetActive(true);
            _pool[0].transform.parent = null;
            item = _pool[0];
            _pool.Remove(_pool[0]);
        }
        else
        {
            item = Instantiate(prefab.PoolItem);
        }
        item.GetComponent<IPoolItem>().OnDelete += AddItem;
        return item;
    }

    private void AddItem(IPoolItem item)
    {
        item.OnDelete -= AddItem;
        item.PoolItem.transform.parent = transform;
        item.PoolItem.transform.localPosition = Vector3.zero;
        _pool.Add(item.PoolItem);
    }
}
