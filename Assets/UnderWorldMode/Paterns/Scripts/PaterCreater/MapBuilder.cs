using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Underworld
{
    [System.Serializable]
    public class MapBuilder : MonoBehaviour
    {
        [Header("Map Setting")]
        [SerializeField] protected bool playOnAwake;
        [Min(1)]
        [SerializeField] protected int mapSize = 1;
        [SerializeField] protected Vector2 unitSize;

        public Vector2 MapSize => unitSize * mapSize;
        public Point[,] Points { get; private set; } = null;
        public List<Point> PointsList { get; private set; } = new List<Point>();

        private void Awake()
        {
            FormattingData();
            if (playOnAwake)
                Intializate(transform);
        }

        public bool Intializate(Transform parent = null)
        {
            if (Points != null)
                return false;
            FormattingData();
            Points = CreateMap();
            foreach (var point in Points)
            {
                PointsList.Add(point);
            }
            return true;
        }
        public Transform GetHolder(string name)
        {
            var holder = new GameObject("PointHolder").transform;
            holder.parent = transform.parent;
            holder.localPosition = Vector2.zero;
            return holder;
        }
        public void Clear()
        {
            foreach (var points in Points)
            {
                points.RemoveObject();
            }
        }
        private Point[,] CreateMap()
        {
            var points = new Point[mapSize, mapSize];
            var sideSize = mapSize % 2 == 0 ? mapSize  : mapSize - 1;
            var start = new Vector2(unitSize.x / 2 - unitSize.x * sideSize/2 ,
                -unitSize.y / 2 + unitSize.y * sideSize / 2);
            for (int i = 0; i < sideSize; i++)
            {
                for (int j = 0; j < sideSize; j++)
                {
                    points[i, j] = new Point(start + new Vector2(unitSize.x * j, -unitSize.y * i));
                }
            }
            return points;
        }
        private void FormattingData()
        {
            var x = unitSize.x == 0 ? 1 : Mathf.Abs(unitSize.x);
            var y = unitSize.y == 0 ? 1 : Mathf.Abs(unitSize.y);
            unitSize = new Vector2(x, y);
        }

    }
}