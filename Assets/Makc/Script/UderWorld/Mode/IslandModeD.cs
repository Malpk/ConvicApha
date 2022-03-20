using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SwitchModeComponent;
using System.Linq;

namespace Underworld
{
    public class IslandModeD : MonoBehaviour,IModeForSwitch
    {
        [Header("Mode Setting")]
        [Min(0)]
        [SerializeField] private float _warningTime = 0;
        [Min(0)]
        [SerializeField] private float _workTime = 1;
        [Header("Island Setting")]
        [Min(1)]
        [SerializeField] private int _minDistanceBothIsland = 1;
        [SerializeField] private int _minSizeIsland;
        [SerializeField] private int _maxSizeIsland;
        [Header("Map Setting")]
        [SerializeField] private bool _playToAwake;
        [SerializeField] private MapBuilder _mapBuilder;

        private int _mapSize;
        private Point[,] _map;
        private Coroutine _runMode = null;
        private Vector2Int[] _bounds;
        
        private readonly Vector2Int[] offset = new Vector2Int[]
        {
            Vector2Int.right, Vector2Int.left,Vector2Int.up, Vector2Int.down 
        };

        public bool IsAttackMode => _runMode != null;

        private void Awake()
        {
            if (_playToAwake)
            {
                _mapBuilder.Intializate(transform);
                _map = _mapBuilder.Map;
                _mapSize = _map.GetLength(0);
                _bounds = SetBounds(_mapSize);
                StartMode();
            }
        }
        public void Constructor(SwitchMode swictMode)
        {
            if (_playToAwake)
                throw new System.Exception("Turn off playToAwake ");
            if (_map == null || _playToAwake)
            {
                _map = swictMode.builder.Map;
                _mapSize = _map.GetLength(0);
                _bounds = SetBounds(_mapSize);
            }
            StartMode();
        }
        public bool StartMode()
        {
            if (_runMode == null)
            {
                _runMode = StartCoroutine(
                    RunMode(
                        GetSeed(
                            GetStartPoint(_mapSize), _bounds)));
                return true;
            }
            return false;
        }
        private IEnumerator RunMode(List<Vector2Int> seeds)
        {
            CheakValue();
            var islands = new List<Vector2Int>();
            foreach (var seed in seeds)
            {
                islands.AddRange(CreateIsland(seed,_mapSize));
            }
            var mapActive = GetMapActive(_map, islands).ToList();
            foreach (var point in mapActive)
            {
                point.SetAtiveObject(true);
            }
            yield return new WaitForSeconds(_warningTime);
            foreach (var point in mapActive)
            {
                point.Animation.StartTile();
            }
            yield return new WaitForSeconds(_workTime);
            foreach (var point in mapActive)
            {
                point.Animation.Stop();
            }
            yield return new WaitWhile(() => mapActive[mapActive.Count - 1].IsActive);
            _runMode = null;
        }
        private IEnumerable<Point> GetMapActive(Point[,] map,IReadOnlyList<Vector2Int> islands)
        {
            for (int i = 0; i < map.GetLength(0); i++)
            {
                for (int j = 0; j < map.GetLength(1); j++)
                {
                    if (!islands.Contains<Vector2Int>(new Vector2Int(i, j)))
                        yield return map[i, j];
                }
            }
        }
        private void CheakValue()
        {
            if (_minSizeIsland <= 0)
                throw new System.Exception("MinSize is can't be <= 0");
            if (_maxSizeIsland <= 0)
                throw new System.Exception("MaxSize is can't be <= 0");
        }
        private Vector2Int[] CreateIsland(Vector2Int seed,int mapSize)
        {
            List<Vector2Int> island = new List<Vector2Int>();
            List<Vector2Int> extensionPoint = new List<Vector2Int>();
            extensionPoint.Add(seed);
            int count = Random.Range(_minSizeIsland, _maxSizeIsland);
            while (island.Count < count && extensionPoint.Count > 0)
            {
                var vertex = extensionPoint[Random.Range(0, extensionPoint.Count)];
                for (int i = 0; i < offset.Length; i++)
                {
                    var newVertex = vertex - offset[i];
                    if (!island.Contains(newVertex) && !extensionPoint.Contains(newVertex))
                    {
                        if (newVertex.x >= 0 && newVertex.x < mapSize
                            && newVertex.y >= 0 && newVertex.y < mapSize)
                        {
                            extensionPoint.Add(newVertex);
                        }
                    }
                }
                extensionPoint.Remove(vertex);
                island.Add(vertex);
            }
            return island.ToArray();
        }
        private List<Vector2Int> GetSeed(Vector2Int startPoint,Vector2Int[] bounds)
        {
            List<Vector2Int> seeds = new List<Vector2Int>();
            List<Vector2Int> expansionPoints = new List<Vector2Int>();
            seeds.Add(startPoint);
            expansionPoints.Add(startPoint);
            while (expansionPoints.Count > 0)
            {
                var start = expansionPoints[Random.Range(0, expansionPoints.Count)];
                var edgs = GetEdge(expansionPoints, bounds).ToList();
                Vector2Int newPoint = GetNewVertex(start, edgs, seeds);
                if (newPoint != Vector2Int.zero)
                {
                    expansionPoints.Add(newPoint);
                    seeds.Add(newPoint);
                }
                else
                {
                    expansionPoints.Remove(start);
                }
            }
            return seeds;
        }
        private Vector2Int GetNewVertex(Vector2Int start, List<Vector2Int> edgs, IReadOnlyList<Vector2Int> seeds)
        {
            Vector2Int edge;
            Vector2Int newPoint = Vector2Int.zero;
            if (_minDistanceBothIsland <= 0)
                throw new System.Exception(" Distance can't be <= 0");
            while (newPoint == Vector2Int.zero && edgs.Count > 0)
            {
                edge = edgs[Random.Range(0, edgs.Count)];
                newPoint = (edge + start) / 2;
                foreach (var seed in seeds)
                {
                    var distance = newPoint - seed;
                    if (Mathf.Abs(distance.x) < _minDistanceBothIsland &&
                             Mathf.Abs(distance.y) < _minDistanceBothIsland)
                    {
                        edgs.Remove(edge);
                        newPoint = Vector2Int.zero;
                    }
                }
            }
            return newPoint;
        }
        private IEnumerable<Vector2Int> GetEdge(IReadOnlyList<Vector2Int> expansion, Vector2Int[] bounds)
        {
            foreach (var vertex in expansion)
            {
                yield return vertex;
            }
            foreach (var vertex in bounds)
            {
                yield return vertex;
            }
        }
        private Vector2Int GetStartPoint(int mapSize)
        {
            if (mapSize <= 0)
                throw new System.Exception(" MapSize can't be <= 0");
            var x = Random.Range(0, mapSize);
            var y = Random.Range(0, mapSize);
            return new Vector2Int(x, y);
        }
        private Vector2Int[] SetBounds(int mapSize)
        {
            if (mapSize <= 0)
                throw new System.Exception(" MapSize can't be <= 0");
            Vector2Int[] boundsNormalize = new Vector2Int[]
                { Vector2Int.zero, Vector2Int.up, Vector2Int.one, Vector2Int.right };
            Vector2Int[] bounds = new Vector2Int[boundsNormalize.Length];
            for (int i = 0; i < bounds.Length; i++)
            {
                bounds[i] = boundsNormalize[i] * mapSize;
            }
            return bounds;
        }
    }
}