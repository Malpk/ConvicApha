using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine.AddressableAssets;

namespace Underworld
{
    public class IslandModeD : GeneralMode
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
        [SerializeField] private MapBuilder _mapBuilder;
        [AssetReferenceUILabelRestriction("term")]
        [SerializeField] private AssetReferenceGameObject _handTermAsset;

        private int _mapSize;
        private bool _isReady = false;
        private Point[,] _map;
        private Coroutine _runMode = null;
        private HandTermTile _handTermPerfab;
        private List<HandTermTile> _activeTils = new List<HandTermTile>();
        private Vector2Int[] _bounds;

        private readonly Vector2Int[] offset = new Vector2Int[]
        {
            Vector2Int.right, Vector2Int.left,Vector2Int.up, Vector2Int.down
        };

        public override bool IsReady => _isReady;

        protected override void Awake()
        {
            base.Awake();
        }
        private void Start()
        {
            if (_mapBuilder)
                Intializate(_mapBuilder, null);
            if (playOnStart)
                Activate();
        }
        #region Intializate
        public override void Intializate(MapBuilder builder, Player player)
        {
            _mapBuilder = builder;
            _map = _mapBuilder.Points;
            _mapSize = _map.GetLength(0);
            _bounds = SetBounds(_mapSize);
        }
        protected async override Task<bool> LoadAsync()
        {
            if (_handTermAsset == null)
                throw new System.NullReferenceException();
            var task = _handTermAsset.LoadAssetAsync().Task;
            await task;
            if (task.Result.TryGetComponent(out HandTermTile term))
            {
                _isReady = true;
                _handTermPerfab = term;
                return true;
            }
            else
            {
                throw new System.NullReferenceException("Gameobject is not component HandTermTile");
            }
        }

        protected override void Unload()
        {
            _handTermAsset.ReleaseAsset();
            _isReady = false;
        }
        #endregion
        public override bool Activate()
        {
            if (_runMode == null)
            {
                State = ModeState.Play;
                _runMode = StartCoroutine(RunMode());
                return true;
            }
            return false;
        }

        #region Work Mode
        private IEnumerator RunMode()
        {
            yield return new WaitWhile(() => !IsReady);
            _activeTils = CreateMap();
            yield return WaitTime(_warningTime);
            MapActivate();
            yield return WaitTime(_workTime);
            Debug.Log("DeaActivate");
            var deactivateMap = MapDeactivate(_activeTils);
            yield return new WaitWhile(() => deactivateMap.IsCompleted);
            yield return new WaitWhile(() => deactivateMap.Result.IsActive);
            State = ModeState.Stop;
            _runMode = null;
            yield return null;
        }

        private void MapActivate()
        {
            int index = 0;
            int steep = (int)Mathf.Sqrt(_activeTils.Count);
            while (_activeTils.Count > index)
            {
                var progress = 0;
                while (_activeTils.Count > index && progress < steep)
                {
                    _activeTils[index].Activate(FireState.Start);
                    index++;
                }
            }
        }
        private async Task<HandTermTile> MapDeactivate(List<HandTermTile> activateMap)
        {
            await Task.Run(() =>
            {
                foreach (var point in activateMap)
                {
                    point.Deactivate();
                }
            });
            return activateMap[activateMap.Count - 1];
        }
        #endregion
        #region Get Seeds
        private List<Vector2Int> GetSeed(Vector2Int startPoint, Vector2Int[] bounds)
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
        #endregion
        #region Create Map
        private List<HandTermTile> CreateMap()
        {
            var result = new List<HandTermTile>();
            var seeds = GetSeed(GetStartPoint(_mapSize), _bounds);
            var islands = new List<Vector2Int>();
            foreach (var seed in seeds)
            {
                islands.AddRange(CreateIsland(seed, _mapSize));
            }
            var mapActive = GetMapActive(_map, islands).ToList();
            for (int i = 0; i < mapActive.Count; i++)
            {
                var term = Instantiate(_handTermPerfab, transform);
                mapActive[i].SetItem(term);
                result.Add(term);
            }
            return result;
        }
        private IEnumerable<Point> GetMapActive(Point[,] map, IReadOnlyList<Vector2Int> islands)
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
        private Vector2Int[] CreateIsland(Vector2Int seed, int mapSize)
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
        #endregion
        #region Pause
        public override void Pause()
        {
            base.Pause();
            foreach (var term in _activeTils)
            {
                term.Pause();
            }
        }
        public override void UnPause()
        {
            base.UnPause();
            foreach (var term in _activeTils)
            {
                term.UnPause();
            }
        }
        #endregion
    }
}