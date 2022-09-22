using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MainMode
{
    public class MapGrid : MonoBehaviour
    {
        [Header("General")]
        [SerializeField] private Vector2 _unitSize;
        [SerializeField] private Vector2Int _mapSize;
        [Header("Reference")]
        [SerializeField]private Player _player;
#if UNITY_EDITOR
        [Header("Debug")]
        [SerializeField] private bool _showMarker;
        [SerializeField] private GameObject _marker;

        private List<GameObject> _markers = new List<GameObject>();
#endif

        private Point[,] _points;

        public Point[,] PointsArray => _points;
        public List<Point> Points { get; private set; }
        public Vector2 MapSize => new Vector2(_mapSize.x * _unitSize.x, _mapSize.y * _unitSize.y);
        private void Awake()
        {
            _points = CreateMap(_unitSize, _mapSize);
        }
        public void Intilizate(Player player)
        {
            _player = player;
            GetFreePoints(out List<Point> Points);
        }
        #region SearchPoint
        public Vector2 SearchPoint(Vector2 position)
        {
            var i = GetVerticalAxis(position.y);
            var j = GetHorizontalAxis(position.x);
            return _points[i, j].Position;
        }
        private int GetHorizontalAxis(float x)
        {
            int index = 0;
            var min = Mathf.Abs(_points[0, 0].Position.x - x);
            for (int i = 1; i < _points.GetLength(1); i++)
            {
                var newValue = Mathf.Abs(_points[0, i].Position.x - x);
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
            var min = Mathf.Abs(_points[0, 0].Position.y - y);
            for (int i = 1; i < _points.GetLength(0); i++)
            {
                var newValue = Mathf.Abs(_points[i, 0].Position.y - y);
                if (min > newValue)
                {
                    index = i;
                    min = newValue;
                }
            }
            return index;
        }
        #endregion

        public bool GetFreePoints(out List<Point> freePoints, float distanceFromPlayer = 0f, float minDistance = 0f)
        {
            minDistance = minDistance < 0 ? 0 : minDistance;
            freePoints = new List<Point>();
            foreach (var point in _points)
            {
                var distance = Vector2.Distance(_player.Position, point.Position);
                var cheak = (distance <= distanceFromPlayer || distanceFromPlayer == 0) && distance > minDistance;
                if (!point.IsBusy && cheak)
                    freePoints.Add(point);
            }
            return freePoints.Count > 0;
        }

        public bool SpawnItem(SmartItem smartItem, float distanceFromPlayer = 0, float minDistance = 0)
        {
            if (GetFreePoints(out List<Point> points, distanceFromPlayer, minDistance))
            {
                var index = Random.Range(0, points.Count);
                if(!smartItem.IsShow)
                    smartItem.ShowItem();
                points[index].SetItem(smartItem);
                return true;
            }
            return false;
        }
        private Point[,] CreateMap(Vector2 unity, Vector2Int mapSize)
        {
            mapSize = (mapSize / 2) * 2;
            var map = new Point[mapSize.y, mapSize.x];
            var y = mapSize.y / 2;
            var x = mapSize.x / 2;
            for (int i = -y; i < y; i++)
            {
                for (int j = -x; j < x; j++)
                {
                    map[y + i, x + j] = new Point(transform.position + 
                        new Vector3(j * unity.x, i * unity.y) + (Vector3)unity/2);
#if UNITY_EDITOR
                    if (_marker && _showMarker)
                    {
                        _markers.Add(Instantiate(_marker, map[y + i, x + j].Position, Quaternion.identity));
                        _markers[_markers.Count-1].transform.parent = transform;
                    }
#endif
                }
            }
            return map;
        }
    }
}