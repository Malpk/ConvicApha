using System.Collections.Generic;
using UnityEngine;

namespace MainMode
{
    public class MapGrid : MonoBehaviour
    {
        [Header("General")]
        [SerializeField] private Vector2 _unitSize;
        [SerializeField] private Vector2Int _mapSize;
#if UNITY_EDITOR
        [Header("Debug")]
        [SerializeField] private bool _showMarker;
        [SerializeField] private GameObject _marker;

        private List<GameObject> _markers = new List<GameObject>();
#endif

        private Point[,] _points;

        public Point[,] Points => _points;
        public Vector2 MapSize => new Vector2(_mapSize.x * _unitSize.x, _mapSize.y * _unitSize.y);
        private void Awake()
        {
            _points = CreateMap(_unitSize, _mapSize);
        }
        public void Intilizate()
        {
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

        public bool GetPointInRadius(out List<Point> points, Vector2Int parentPosition, int radius)
        {
            points = new List<Point>();
            var x = Mathf.Clamp(parentPosition.x - radius, 0, _points.GetLength(0));
            var y = Mathf.Clamp(parentPosition.y - radius, 0, _points.GetLength(1));
            var xEnd = Mathf.Clamp(parentPosition.x + radius, parentPosition.x, _points.GetLength(0));
            var yEnd = Mathf.Clamp(parentPosition.y + radius, parentPosition.y, _points.GetLength(1));
            for (int i = x; i < xEnd; i++)
            {
                for (int j = y; j < yEnd; j++)
                {
                    var childPosition = new Vector2Int(i, j);
                    if (childPosition != parentPosition && Vector2Int.Distance(childPosition, parentPosition) <= radius)
                        points.Add(_points[i, j]);
                }
            }
            return points.Count > 0;
        }
        public bool GetCirculBorder(out List<Point> points, Vector2Int parentPosition, int radius)
        {
            points = new List<Point>();
            var x = Mathf.Clamp(parentPosition.x - radius, 0, _points.GetLength(0));
            var y = Mathf.Clamp(parentPosition.y - radius, 0, _points.GetLength(1));
            var xEnd = Mathf.Clamp(parentPosition.x + radius, parentPosition.x, _points.GetLength(0));
            var yEnd = Mathf.Clamp(parentPosition.y + radius, parentPosition.y, _points.GetLength(1));
            for (int i = x; i < xEnd; i++)
            {
                for (int j = y; j < yEnd; j++)
                {
                    var childPosition = new Vector2Int(i, j);
                    var rate = Mathf.Clamp(radius - Vector2Int.Distance(childPosition, parentPosition), 0, radius);
                    if (rate < 1.1f && rate > 0)
                    {
                        points.Add(_points[i, j]);
                    }
                }
            }
            return points.Count > 0;
        }
        public bool SetItemOnMap(UpPlatform smartItem, float distanceFromPlayer = 0, float minDistance = 0, Transform target = null)
        {
            if (GetFreePoints(out List<Point> points, distanceFromPlayer, minDistance, target))
            {
                var index = Random.Range(0, points.Count);
                if(!smartItem.IsShow)
                    smartItem.UpDevice();
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
                    var position = transform.position +
                        new Vector3(j * unity.x, i * unity.y) + (Vector3)unity / 2;
                    map[y + i, x + j] = new Point(position, new Vector2Int(y + i, x + j));
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
        public void ClearMap()
        {
            foreach (var point in _points)
            {
                point.Delete();
            }
        }
    }
}