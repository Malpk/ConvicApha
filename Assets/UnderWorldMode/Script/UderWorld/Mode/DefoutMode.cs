using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

namespace Underworld
{
    public class DefoutMode : MonoBehaviour, IModeForSwitch
    {
        [Header("Spawn Setting")]
        [Min(0)]
        [SerializeField] private int _spawnDistance;
        [SerializeField] private Player _player;
        [Header("Time Setting")]
        [Min(0)]
        [SerializeField] private float _delay;
        [Min(0)]
        [SerializeField] private float _timeFireActive;
        [Min(0)]
        [SerializeField] private float _workDuration;
        [Header("Map Setting")]
        [SerializeField] private Vector2 _unitSize;

        private Point[,] _map;
        private Vector2Int _centerPosition;
        private Vector2Int _mapLimit;
        private Coroutine _startMode;

        public bool IsAttackMode => _startMode != null;

        public void Constructor(SwitchMode swictMode)
        {
            if (_startMode != null)
                return;
            _map = swictMode.builder.Map;
            _player = swictMode.Player;
            _unitSize = swictMode.UnitSize;
            _centerPosition = new Vector2Int(_map.GetLength(0) / 2, _map.GetLength(1) / 2);
            _mapLimit = new Vector2Int( _map.GetLength(0), _map.GetLength(1)) - Vector2Int.one;
            _startMode = StartCoroutine(Spawn());
        }

        private IEnumerator Spawn()
        {
            float progress = 0;
            while (progress < 1f)
            {
                Vector2Int index = Vector2Int.RoundToInt(_player.Position / _unitSize);
                index = new Vector2Int(_centerPosition.x + index.x, _centerPosition.y - index.y);
                progress += _delay / _workDuration;
                index.Clamp(Vector2Int.zero, _mapLimit);
                StartCoroutine(ActivateTile(GetSpawnZone(index).ToList()));
                yield return new WaitForSeconds(_delay);
            }
            _startMode = null;
        }
        private IEnumerator ActivateTile(List<Point> points)
        {
            if (points.Count == 0)
                yield break;
            var index = Random.Range(0, points.Count);
            points[index].SetAtiveObject(true);
            points[index].Animation.StartTile();
            yield return new WaitForSeconds(_timeFireActive);
            points[index].Animation.Stop();
        }
        private IEnumerable<Point> GetSpawnZone(Vector2Int index)
        {
            var limitX =  GetLimit(index.y, _spawnDistance, _mapLimit.x);
            var limitY = GetLimit(index.y, _spawnDistance, _mapLimit.y);
            for (int i = limitY.x; i < limitY.y; i++)
            {
                for (int j = limitX.x; j < limitX.y; j++)
                {
                    if (!_map[i, j].IsActive)
                    {
                        yield return _map[i, j];
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

        public void SetSetting(string jsonSetting)
        {
            var setting = JsonUtility.FromJson<BaseSetting>(jsonSetting);
            _delay = setting.Delay;
            _spawnDistance = setting.SpawnDistance;
            _timeFireActive = setting.TimeFireActive;
            _workDuration = setting.WorkDuration;
        }
    }
}
