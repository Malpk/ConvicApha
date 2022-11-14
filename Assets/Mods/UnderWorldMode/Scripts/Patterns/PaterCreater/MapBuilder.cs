using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Underworld
{
    [System.Serializable]
    public class MapBuilder : MonoBehaviour
    {
        [Header("Map Setting")]
        [Min(1)]
        [SerializeField] protected int mapSize = 1;
        [SerializeField] protected Vector2 unitSize;
        [Header("Reference")]
        [SerializeField] private Term _perfab;

        public Vector2 MapSize => unitSize * mapSize;
        public Term[,] Terms { get; private set; }
        public List<Point> Points { get; private set; } = new List<Point>();

        private void Awake()
        {
            CheakOutRange();
            Intializate();
        }

        private bool Intializate()
        {
            if (Terms == null)
            {
                CheakOutRange();
                Terms = CreateMatrixMap();
                return true;
            }
            return false;
        }
        public void ClearMap()
        {
            foreach (var point in Points)
            {
                point.Delete();
            }
        }
        public bool GetPoint(Player target, float spawnDistance, out Point point)
        {
            point = null;
            var freePoints = new List<Point>();
            foreach (var map in Points)
            {
                if (!map.IsBusy && Vector2.Distance(map.Position, target.Position) <= spawnDistance)
                {
                    freePoints.Add(map);
                }
            }
            if (freePoints.Count > 0)
                point = freePoints[Random.Range(0, freePoints.Count)];
            return freePoints.Count > 0;
        }
        private Term[,] CreateMatrixMap()
        {
            var holder = new GameObject("Terms").transform;
            holder.transform.parent = transform;
            var terms = new Term[mapSize, mapSize];
            var sideSize = mapSize % 2 == 0 ? mapSize : mapSize - 1;
            var start = new Vector2(unitSize.x / 2 - unitSize.x * sideSize / 2,
                -unitSize.y / 2 + unitSize.y * sideSize / 2);
            for (int i = 0; i < sideSize; i++)
            {
                for (int j = 0; j < sideSize; j++)
                {
                    var position = start + new Vector2(unitSize.x * j, -unitSize.y * i);
                    Points.Add(new Point(position));
                    terms[i, j] = Instantiate(_perfab.gameObject).GetComponent<Term>();
                    terms[i, j].transform.parent = holder;
                    terms[i, j].transform.localPosition = position;
                }
            }
            return terms;
        }
        private void CheakOutRange()
        {
            var x = unitSize.x == 0 ? 1 : Mathf.Abs(unitSize.x);
            var y = unitSize.y == 0 ? 1 : Mathf.Abs(unitSize.y);
            unitSize = new Vector2(x, y);
        }
    }
}