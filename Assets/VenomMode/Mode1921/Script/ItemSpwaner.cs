using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MainMode.Items;

namespace MainMode.Mode1921
{
    public class ItemSpwaner : MonoBehaviour
    {
        [Header("General Setting")]
        [SerializeField] private bool _onStart = true;
        [Header("Spawn Setting")]
        [SerializeField] private float _maxDistanceOnTarget;
        [SerializeField] private Vector2 _rangeTimeSpawn;
        [SerializeField] private ItemInfo[] _items;
        [Header("Requred Reference")]
        [SerializeField] private MapGrid _mapGrid;
        [SerializeField] private Transform _target;

        private Coroutine _run;

        private void Start()
        {
            _rangeTimeSpawn = new Vector2(Mathf.Abs(_rangeTimeSpawn.x), Mathf.Abs(_rangeTimeSpawn.y));
            if (_onStart)
                _run = StartCoroutine(SpwanwItem());
        }
        public void Run(Transform target)
        {
            if (_run == null)
            {
                _target = target;
                StartCoroutine(SpwanwItem());
            }
        }
        public void Restart()
        {
            foreach (var item in _items)
            {
                item.DeSpawnItems();
            }
        }
        private IReadOnlyList<ItemInfo> GetItems()
        {
            var list = new List<ItemInfo>();
            foreach (var item in _items)
            {
                if (item.IsAccess)
                    list.Add(item);
            }
            return list;
        }
        private List<Point> GetPoints()
        {
            var freePoints = new List<Point>();
            foreach (var point in _mapGrid.GetFreePoints())
            {
                if (Vector2.Distance(point.Position, _target.position) <= _maxDistanceOnTarget)
                    freePoints.Add(point);
            }
            return freePoints;
        }
        private IEnumerator SpwanwItem()
        {
            while (true)
            {
                IReadOnlyList<ItemInfo> items = null;
                yield return new WaitUntil(() =>
                {
                    items = GetItems();
                    return items.Count > 0;
                });
                List<Point> freePoints = null;
                yield return new WaitUntil(() =>
                {
                    freePoints = GetPoints();
                    return freePoints.Count > 0;
                });
                var item = items[Random.Range(0, items.Count)].Instantiate(transform);
                if (item != null)
                    freePoints[Random.Range(0, freePoints.Count)].SetItem(item);
                yield return new WaitForSeconds(Random.Range(_rangeTimeSpawn.x, _rangeTimeSpawn.y));
            }
        }
    }
}