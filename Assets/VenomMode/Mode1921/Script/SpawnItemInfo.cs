using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MainMode.Mode1921
{
    [System.Serializable]
    public class SpawnItemInfo 
    {
        [SerializeField] private float _spawnDistance;
        [SerializeField] private DeviceV2 _perfab;

        private List<GameObject> _items = new List<GameObject>();

        public float SpawnDistance => _spawnDistance;
        public IReadOnlyList<GameObject> Items => _items;


        public GameObject Create()
        {
            var item = MonoBehaviour.Instantiate(_perfab.gameObject);
            _items.Add(item);
            return item;
        }
    }
}