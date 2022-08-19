using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using System.Threading.Tasks;

namespace Underworld
{
    public sealed class DefoutMode : GeneralMode
    {
#if UNITY_EDITOR 
        [SerializeField] private bool _isDebug;
#endif
        [Header("Spawn Setting")]
        [Min(1)]
        [SerializeField] private int _spawnDistance = 1;
        [Range(0.005f, 1)]
        [SerializeField] private float _delay = 1f;
        [AssetReferenceUILabelRestriction("term")]
        [SerializeField] private AssetReferenceGameObject _termAsset;
        [Header("Reference")]
        [SerializeField] private Player _player;
        [SerializeField] private MapBuilder _builder;

        private bool _isReady;
        private Point[,] _map;
        private Coroutine _startMode;
        private DefautTermTile _termPerfab;
        private List<DefautTermTile> _poolActive = new List<DefautTermTile>();
        private List<DefautTermTile> _poolDeactive = new List<DefautTermTile>();

        public bool IsActive => _startMode != null;

        public override bool IsReady => _isReady;

        private void Start()
        {
            if (playOnStart)
                Activate();
        }

        protected override async Task<bool> LoadAsync()
        {
            if (!_isReady)
            {
                var load = _termAsset.LoadAssetAsync().Task;
                await load;
                if (load.Result.TryGetComponent(out DefautTermTile term))
                {
                    _isReady = true;
                    _termPerfab = term;
#if UNITY_EDITOR
                    if(_isDebug)
                        Debug.Log("Load Complite");
#endif
                    return true;
                }
                else
                {
                    throw new System.NullReferenceException("GameObject is not component DefautTermTile");
                }
            }
            return false;
        }

        protected override void Unload()
        {
            _isReady = false;
            _termAsset.ReleaseAsset();
            ClearPool(_poolActive);
            ClearPool(_poolDeactive);
        }
        public override void Intializate(MapBuilder builder, Player player)
        {
            _builder.Clear();
            _builder = builder;

            _player = player;
        }

        public override bool Activate()
        {
            if (_startMode == null)
            {
                State = ModeState.Play;
                _startMode = StartCoroutine(Spawn());
                return true;
            }
            else
            {
                return false;
            }
        }

        private IEnumerator Spawn()
        {
#if UNITY_EDITOR
            if (_isDebug)
                Debug.Log("Start Game");
#endif
            yield return new WaitWhile(() => !IsReady);
            _map = _builder.Points;
            float progress = 0;
            while (progress < 1f && State != ModeState.Stop)
            {
                yield return new WaitWhile(() => State == ModeState.Pause);
                if (GetPoint(_player, _spawnDistance, out Point choosePoint))
                {
                    var term = GetTerm();
                    if (!term.IsShow)
                        term.SetMode(true);
                    term.Activate();
                    choosePoint.SetItem(term);
                }
                yield return WaitTime(_delay);
                progress += _delay / workDuration;
            }
#if UNITY_EDITOR
            if (_isDebug)
                Debug.Log("End Game");
#endif
            _startMode = null;
        }
        private DefautTermTile GetTerm()
        {
            PoolUpdate();
            if (_poolDeactive.Count > 0)
            {
                var term = _poolDeactive[0];
                _poolDeactive.Remove(term);
                _poolActive.Add(term);
                return term;
            }
            else
            {
                var term = Instantiate(_termPerfab.gameObject).GetComponent<DefautTermTile>();
                term.transform.parent = transform;
                _poolActive.Add(term);
                return term;
            }
        }
        private bool GetPoint(Player target, float spawnDistance , out Point point)
        {
            point = null;
            var freePoints = new List<Point>();
            for (int i = 0; i < _map.GetLength(0); i++)
            {
                for (int j = 0; j < _map.GetLength(1); j++)
                {
                    if (!_map[i, j].IsBusy && Vector2.Distance(_map[i,j].Position, target.Position) <= spawnDistance)
                    {
                        freePoints.Add(_map[i, j]);
                    }
                }
            }
            if (freePoints.Count > 0)
                point = freePoints[Random.Range(0, freePoints.Count)];
#if UNITY_EDITOR
            if (_isDebug)
                Debug.Log($"freeTiles= {freePoints.Count}");
#endif

            return freePoints.Count > 0;
        }
        private void PoolUpdate()
        {
            var deactiveTerms = new List<DefautTermTile>();
            for (int i = 0; i < _poolActive.Count; i++)
            {
                if (!_poolActive[i].IsShow)
                {
                    _poolDeactive.Add(_poolActive[i]);
                    deactiveTerms.Add(_poolActive[i]);
                }
            }
            foreach (var term in deactiveTerms)
            {
                _poolActive.Remove(term);
            }
            if (_poolDeactive.Count > _poolActive.Count)
            {
                while (_poolDeactive.Count > _poolActive.Count)
                {
                    var term = _poolDeactive[0];
                    _poolDeactive.Remove(term);
                    Destroy(term.gameObject);
                }
            }
        }
        private void ClearPool(List<DefautTermTile> pool)
        {
            while (pool.Count > 0)
            {
                var term = pool[0];
                pool.Remove(term);
                Destroy(term);
            }
        }
        public override void Pause()
        {
            base.Pause();
            foreach (var term in _poolActive)
            {
                term.Pause();
            }
        }
        public override void UnPause()
        {
            base.UnPause();
            foreach (var term in _poolActive)
            {
                term.UnPause();
            }
        }
    }
}
