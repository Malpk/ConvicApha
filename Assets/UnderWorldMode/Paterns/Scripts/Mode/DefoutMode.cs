using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

namespace Underworld
{
    public class DefoutMode : PaternMode
    {
        [Header("Spawn Setting")]
        [Min(0)]
        [SerializeField] private int _spawnDistance;
        [Header("Time Setting")]
        [Min(0)]
        [SerializeField] private float _delay;
        [SerializeField] private TileInfo _timeFireActive;
        [Min(0)]
        [SerializeField] private float _workDuration;
        [Header("Map Setting")]
        [SerializeField] private Vector2 _unitSize;

        private Coroutine _startMode;

        public bool IsActive => _startMode != null;


        private void Start()
        {
            if (playOnStart)
                Run();
        }
        public override void Run()
        {
            if (_startMode == null)
                _startMode = StartCoroutine(Spawn());
        }

        private IEnumerator Spawn()
        {
            float progress = 0;
            while (progress < 1f)
            {
                Vector2Int index = Vector2Int.RoundToInt(player.Position / _unitSize);
                index = new Vector2Int(centerPosition.x + index.x, centerPosition.y - index.y);
                progress += _delay / _workDuration;
                index.Clamp(Vector2Int.zero, mapSize);
                ChoosePoint(GetSpawnZone(index).ToList());
                yield return new WaitForSeconds(_delay);
            }
            _startMode = null;
        }
        private void ChoosePoint(List<Point> points)
        {
            if (points.Count == 0)
                return;
            var index = Random.Range(0, points.Count);
            points[index].SetAtiveObject(true);
            points[index].Animation.Activate(FireState.Start, _timeFireActive);
        }
        private IEnumerable<Point> GetSpawnZone(Vector2Int index)
        {
            var limitX =  GetLimit(index.y, _spawnDistance, mapSize.x);
            var limitY = GetLimit(index.y, _spawnDistance, mapSize.y);
            for (int i = limitY.x; i < limitY.y; i++)
            {
                for (int j = limitX.x; j < limitX.y; j++)
                {
                    if (!map[i, j].IsActive)
                    {
                        yield return map[i, j];
                    }
                }
            }
        }
        private Vector2Int GetLimit(int value, int offset, int mapLimit)
        {
            Vector2Int limit  = new Vector2Int(value - offset, value + offset);
            var x = limit.x > 0 ? limit.x : 0;
            var y = limit.y < mapLimit ? limit.y : mapLimit;
            return new Vector2Int(x, y);
        }

    }
}
