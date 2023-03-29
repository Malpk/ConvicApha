using System.Collections.Generic;
using UnityEngine;

namespace MainMode
{
    public class MapGrid : MonoBehaviour
    {
        [Header("General")]
        [SerializeField] private float _unitSize = 1f;
        [SerializeField] private Vector2Int _mapSize;
#if UNITY_EDITOR
        [Header("Debug")]
        [SerializeField] private bool _showMarker;
        [SerializeField] private GameObject _marker;

        private List<GameObject> _markers = new List<GameObject>();
#endif

        private Point[,] _points;

        public Point[,] PointsArray => _points;
        public List<Point> Points { get; private set; }
        public Vector2 MapSize => new Vector2(_mapSize.x, _mapSize.y) * _unitSize;

        private void Awake()
        {
            _points = CreateMap(_unitSize, _mapSize);
        }

        public void SetSize(Vector2Int size)
        {
            _mapSize = size;
        }

        public void Intilizate()
        {
            _points = CreateMap(_unitSize, _mapSize);
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

        public bool GetFreePoints(out List<Point> freePoints, float distanceFromPlayer = 0f, float minDistance = 0f, Transform target = null)
        {
            minDistance = minDistance < 0 ? 0 : minDistance;
            freePoints = new List<Point>();
            foreach (var point in _points)
            {
                var distance = Vector2.Distance(target ? target.transform.position : Vector3.zero, point.Position);
                var cheak = (distance <= distanceFromPlayer || distanceFromPlayer == 0) && distance > minDistance;
                if (!point.IsBusy && cheak)
                    freePoints.Add(point);
            }
            return freePoints.Count > 0;
        }

        public bool SetItemOnMap(DevicePlatform smartItem, float distanceFromPlayer = 0, float minDistance = 0, Transform target = null)
        {
            if (GetFreePoints(out List<Point> points, distanceFromPlayer, minDistance, target))
            {
                var index = Random.Range(0, points.Count);
                points[index].SetItem(smartItem);
                return true;
            }
            return false;
        }
        private Point[,] CreateMap(float unity, Vector2Int mapSize)
        {
            var map = new Point[mapSize.y, mapSize.x];
            for (int i = 0; i < mapSize.y; i++)
            {
                for (int j = 0; j < mapSize.x; j++)
                {
                    var x = transform.right * (-mapSize.x / 2 + j) * unity;
                    var y = transform.up * (-mapSize.y / 2 + i) * unity;
                    map[i, j] = new Point(transform.position + x + y);
#if UNITY_EDITOR
                    if (_marker && _showMarker)
                    {
                        _markers.Add(Instantiate(_marker, map[i, j].Position, Quaternion.identity));
                        _markers[_markers.Count-1].transform.parent = transform;
                    }
#endif
                }
            }
            return map;
        }
        public void ClearMap()
        {
            foreach (var point in _points)
            {
                point.Delete();
            }
        }
    }
}