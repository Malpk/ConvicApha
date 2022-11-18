using System.Collections.Generic;
using UnityEngine;
using System.Linq;

namespace Underworld
{
    public class IslandModeD : TotalMapMode
    {
        [Header("Mode Setting")]
        [Min(0)]
        [SerializeField] private float _warningTime = 0;
        [Header("Island Setting")]
        [Min(1)]
        [SerializeField] private float _minDistanceBothIsland = 1;
        [SerializeField] private int _minSizeIsland;
        [SerializeField] private int _maxSizeIsland;
        [Header("Map Setting")]
        [SerializeField] private int _mapSize;

        private BasePatternState _curretState;
        private List<Term> _activeTils = new List<Term>();
        private PatternIdleState _warningState;
        private PatternIdleState _activeState;
        private Vector2Int[] _bounds;

        private readonly Vector2Int[] offset = new Vector2Int[]
        {
            Vector2Int.right, Vector2Int.left,Vector2Int.up, Vector2Int.down
        };

        protected override void Awake()
        {
            base.Awake();
            _warningState = new PatternIdleState(_warningTime);
            _activeState = new PatternIdleState(workDuration);
            _warningState.SetNextState(_activeState);
            _activeState.SetNextState(compliteState);
            _bounds = SetBounds(_mapSize);
            enabled = false;
        }
        public override void SetConfig(PaternConfig config)
        {
            if (config is IslandModeConfig islandModeConfig) 
            {
                workDuration = islandModeConfig.WorkDuration;
                _warningTime = islandModeConfig.WarningTime;
                _minDistanceBothIsland = islandModeConfig.MinDistanceBothIsland;
                _minSizeIsland = islandModeConfig.IslandSize.x;
                _maxSizeIsland = islandModeConfig.IslandSize.y;
            }
            else
            {
                throw new System.NullReferenceException("IslandModeConfig is null");
            }
        }
        protected override void OnEnable()
        {
            base.OnEnable();
            _warningState.OnComplite += () => ActivateTerms(_activeTils);
            _activeState.OnComplite += DeactivateTerms;
        }
        protected override void OnDisable()
        {
            base.OnDisable();
            _warningState.OnComplite -= () => ActivateTerms(_activeTils);
            _activeState.OnComplite -= DeactivateTerms;
        }
                public override bool Play()
        {
            if (!enabled)
            {
                enabled = true;
                State = ModeState.Play;
                _activeTils = CreateMap();
                _curretState = _warningState;
                _curretState.Start();
                return true;
            }
            return false;
        }

        public void Stop()
        {
            enabled = false;
        }
        private void Update()
        {
            if (_curretState.IsComplite)
            {
                if (_curretState.GetNextState(out BasePatternState nextState))
                {
                    _curretState = nextState;
                    _curretState.Start();
                }
                else
                {
                    Stop();
                }
            }
            else
            {
                _curretState.Update();
            }
        }


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
        private List<Term> CreateMap()
        {
            var seeds = GetSeed(GetStartPoint(_mapSize), _bounds);
            var islands = new List<Vector2Int>();
            foreach (var seed in seeds)
            {
                islands.AddRange(CreateIsland(seed, _mapSize));
            }
            var mapActive = GetMapActive(terms, islands).ToList();
            for (int i = 0; i < mapActive.Count; i++)
            {
                mapActive[i].Show();
            }
            return mapActive;
        }
        private IEnumerable<Term> GetMapActive(Term[,] terms, IReadOnlyList<Vector2Int> islands)
        {
            for (int i = 0; i < terms.GetLength(0); i++)
            {
                for (int j = 0; j < terms.GetLength(1); j++)
                {
                    if (!islands.Contains<Vector2Int>(new Vector2Int(i, j)))
                        yield return terms[i,j];
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
    }
}