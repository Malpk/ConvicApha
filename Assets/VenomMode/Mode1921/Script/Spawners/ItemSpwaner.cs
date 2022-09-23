using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MainMode.Mode1921
{
    public class ItemSpwaner : GeneralSpawner
    {
        [Header("Spawner Setting")]
        [SerializeField] private float _maxDistanceOnTarget;
        [SerializeField] private Vector2 _rangeTimeSpawn;
        [SerializeField] private ItemInfo[] _items;

        private Coroutine _run;

        public override bool IsRedy => true;

        private void OnEnable()
        {
            PlayAction += Run;
        }

        private void OnDisable()
        {
            PlayAction += Run;
        }
        public override void Replay()
        {
            foreach (var item in _items)
            {
                item.DeSpawnItems();
            }
        }
        private void Run()
        {
            if (_run == null)
            {
                StartCoroutine(SpwanwItem());
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
        private IEnumerator SpwanwItem()
        {
            while (IsPaly)
            {
                IReadOnlyList<ItemInfo> items = null;
                yield return new WaitUntil(() =>
                {
                    items = GetItems();
                    return items.Count > 0;
                });
                List<Point> freePoints = null;
                yield return new WaitUntil(() => mapGrid.GetFreePoints(out freePoints, _maxDistanceOnTarget));
                var item = items[Random.Range(0, items.Count)].Instantiate(transform);
                if (item != null)
                    freePoints[Random.Range(0, freePoints.Count)].SetItem(item);
                yield return new WaitForSeconds(Random.Range(_rangeTimeSpawn.x, _rangeTimeSpawn.y));
            }
        }


    }
}