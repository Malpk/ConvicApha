using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;
using TileSpace;
using SwitchModeComponent;

namespace Underworld
{
    public class FireMapMode : MonoBehaviour, IModeForSwitch
    {
        [Header("Time Setting")] [Min(0)]
        [SerializeField] private float _warningTime;
        [Min(0)]
        [SerializeField] private float _attackTime;
        [Header("Mode Setting")]
        [Min(0)]
        [SerializeField] private int _countIsland;
        [Min(0)]
        [SerializeField] private int _maxCountTile;

        private GameMap _map = null;
        private MapBuilder _mapBuilder;
        private Point[,] _poolMap;

        private List<Vector2Int> _allMap;
        private IVertex[,] _vertexs;
        private Coroutine _workMode;

        public bool IsAttackMode => true;
        public void Constructor(SwitchMode swictMode)
        {
            if (_workMode == null)
            {
                _map = swictMode.map;
                _mapBuilder = swictMode.builder;
                _workMode = StartCoroutine(ModeRun());
            }
        }
        private IEnumerator ModeRun()
        {
            var mapSpawn = GetMapSpawn();
            _poolMap = _mapBuilder.Map;
            List<Point> activeMap = new List<Point>();
            foreach (var vertex in mapSpawn)
            {
                var index = vertex;
                _poolMap[index.x, index.y].SetAtiveObject(true);
                activeMap.Add(_poolMap[index.x, index.y]);
            }
            yield return StartCoroutine(AttackMode(activeMap));
            _workMode = null;
            Destroy(gameObject);
        }
        private IEnumerator AttackMode(List<Point> activeMap)
        {
            yield return new WaitForSeconds(_warningTime);
            foreach (var point in activeMap)
            {
                point.Animation.StartTile();
            }
            yield return StartCoroutine(StopMode(activeMap));
        }
        private IEnumerator StopMode(List<Point> activeMap)
        {
            yield return new WaitForSeconds(_attackTime);
            if (activeMap.Count > 0)
            {
                Point lost = activeMap[0];
                foreach (var point in activeMap)
                {
                    point.Animation.Stop();
                    lost = point;
                }
                yield return new WaitWhile(() => lost.IsActive);
            }
            else
            {
                yield return null;
            }
        }
        private List<Vector2Int> GetMapSpawn()
        {
            _vertexs = _map.Map;
            _allMap = GetVertexList(_vertexs);
            var map = GetVertexList(_vertexs);
            var seed = GetSeeds(map);
            seed = SetIslandSize(seed);
            foreach (var item in seed)
            {
                _allMap.Remove(item);
            }
            return _allMap;
        }
        private List<Vector2Int> GetSeeds(List<Vector2Int> list)
        {
            List<Vector2Int> seeds = new List<Vector2Int>();
            int interval = Mathf.RoundToInt(_map.MapSize / Mathf.Sqrt(_countIsland));
            for (int i = 0; i < _countIsland && list.Count > 0; i++)
            {
                int index = Random.Range(0, list.Count);
                int curretInterval = Random.Range(interval - 1, interval);
                seeds.Add(list[index]);
                list = CutRadius(list[index], list, _vertexs, curretInterval);
            }
            return seeds;
        }
        private List<Vector2Int> CutRadius(Vector2Int chooseVertex, List<Vector2Int> mapList, IVertex[,] vertexs, int interval)
        {
            interval /= 2;
            for (int i = -interval; i <= interval; i++)
            {
                int x = Mathf.Clamp(chooseVertex.x + i, 0, _map.MapSize - 1);
                for (int j = -interval; j <= interval; j++)
                {
                    int y = Mathf.Clamp(chooseVertex.y + j, 0, _map.MapSize - 1);
                    mapList.Remove(vertexs[x, y].ArryaPostion);
                }
            }
            return mapList;
        }
        private List<Vector2Int> GetVertexList(IVertex[,] vertexs)
        {
            List<Vector2Int> map = new List<Vector2Int>();
            for (int i = 0; i < _map.MapSize; i++)
            {
                for (int j = 0; j < _map.MapSize; j++)
                {
                    if (vertexs[i, j].State == TileState.Empty)
                        map.Add(vertexs[i, j].ArryaPostion);
                }
            }
            return map;
        }
        private List<Vector2Int> SetIslandSize(List<Vector2Int> seeds)
        {
            List<Vector2Int> islandsOnMap = new List<Vector2Int>();
            int count = seeds.Count;
            for (int i = 0; i < count; i++)
            {
                List<IVertex> island = new List<IVertex>();
                List<IVertex> accessForExtension = new List<IVertex>();
                island.Add(_vertexs[seeds[i].x,seeds[i].y]);
                int countTile = Random.Range(1, _maxCountTile);
                for (int j = 0; j < countTile; j++)
                {
                    int seedsID = Random.Range(0, island.Count);
                    foreach (var vertex in island[seedsID].Edge)
                    {
                        accessForExtension.Add(vertex);
                    }
                    int index = Random.Range(0, accessForExtension.Count);
                    island.Add(accessForExtension[index]);
                    accessForExtension.Remove(accessForExtension[index]);
                }
                foreach (var vertex in island)
                {
                    islandsOnMap.Add(vertex.ArryaPostion);
                }
            }
            return islandsOnMap;
        }
    }
}