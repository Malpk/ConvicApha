using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MainMode
{
    public class MapGrid : MonoBehaviour,ISpawnItem
    {
        [SerializeField] private Vector2 _unitSize;
        [SerializeField] private Vector2Int _mapSize;
#if UNITY_EDITOR
        [SerializeField] private bool _showMarker;
        [SerializeField] private GameObject _marker;

        private List<GameObject> _markers = new List<GameObject>();
#endif
        private Vector2[,] _map;

        private void Awake()
        {
            _map = CreateMap(_unitSize, _mapSize);
        }
        public Vector2 SearchPoint(Vector2 position)
        {
            var i = GetVerticalAxis(position.y);
            var j = GetHorizontalAxis(position.x);
            return _map[i, j];
        }
        private int GetHorizontalAxis(float x)
        {
            int index = 0;
            var min = Mathf.Abs(_map[0, 0].x - x);
            for (int i = 1; i < _map.GetLength(1); i++)
            {
                var newValue = Mathf.Abs(_map[0, i].x - x);
                if (min > newValue)
                {
                    index = i;
                    min = newValue;
                }
            }
            return index;
        }
        private int GetVerticalAxis(float y)
        {
            int index = 0;
            var min = Mathf.Abs(_map[0, 0].y - y);
            for (int i = 1; i < _map.GetLength(0); i++)
            {
                var newValue = Mathf.Abs(_map[i, 0].y - y);
                if (min > newValue)
                {
                    index = i;
                    min = newValue;
                }
            }
            return index;
        }
        private Vector2[,] CreateMap(Vector2 unity, Vector2Int mapSize)
        {
            var map = new Vector2[mapSize.y, mapSize.x];
            var y = mapSize.y / 2;
            var x = mapSize.x / 2;
            for (int i = -y; i < y; i++)
            {
                for (int j = -x; j < x; j++)
                {
                    map[y + i, x + j] = transform.position + new Vector3(j * unity.x, i * unity.y);
#if UNITY_EDITOR
                    if (_marker && _showMarker)
                    {
                        _markers.Add(Instantiate(_marker, map[y + i, x + j], Quaternion.identity));
                        _markers[_markers.Count-1].transform.parent = transform;
                    }
#endif
                }
            }
            return map;
        }
        public GameObject InstateItem(Vector3 position, GameObject item)
        {
            var point = SearchPoint(position);
            return Instantiate(item, point, Quaternion.identity);
        }
    }
}