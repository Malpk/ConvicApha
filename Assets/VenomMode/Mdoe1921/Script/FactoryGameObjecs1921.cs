using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MainMode.Map;

namespace MainMode.Mode1921
{
    public class FactoryGameObjecs1921
    {
        private readonly MapGrid map;

        private List<GameObject> _items;

        public FactoryGameObjecs1921(MapGrid map)
        {
            this.map = map;
            _items = new List<GameObject>();
        }

        public List<GameObject> Create(GameObject[] perfabs, float minDistance)
        {
            _items.Clear();
            minDistance = Mathf.Abs(minDistance);
            foreach (var perfab in perfabs)
            {
                if (Create(perfab, minDistance, out GameObject item))
                    _items.Add(item);
            }
            return _items;
        }
        public List<GameObject> Create(GameObject perfab, int count = 1, float minDistance = 0)
        {
            _items.Clear();
            count = Mathf.Abs(count);
            minDistance = Mathf.Abs(minDistance);
            for (int i = 0; i < count; i++)
            {
                if (Create(perfab, minDistance, out GameObject item))
                    _items.Add(item);
                else
                    break;
            }
            return _items;
        }

        private bool Create(GameObject perfab ,float distance,out GameObject item)
        {
            var freePoints = FilteDistance(map.GetFreePoints(), distance);
            if (freePoints.Count > 0)
            {
                item = MonoBehaviour.Instantiate(perfab);
                freePoints[Random.Range(0, freePoints.Count)].SetObject(item);
                return true;
            }
            else
            {
                item = null;
                return false;
            }
        }
        private List<Point> FilteDistance(List<Point> points, float distance)
        {
            var list = new List<Point>();
            for (int i = 0; i < points.Count; i++)
            {
                if (CheakDistance(points[i].Position, distance))
                    list.Add(points[i]);
            }
            return list;
        }
        private bool CheakDistance(Vector2 position, float distance)
        {
            foreach (var item in _items)
            {
                if (Vector2.Distance(item.transform.position, position) < distance)
                    return false;
            }
            return true;
        }
    }
}