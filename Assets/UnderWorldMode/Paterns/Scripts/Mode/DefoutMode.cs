using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using System.Threading.Tasks;

namespace Underworld
{
    public sealed class DefoutMode : GeneralMode
    {
        [Header("Spawn Setting")]
        [Min(1)]
        [SerializeField] private int _spawnDistance = 1;
        [Range(0.005f, 1)]
        [SerializeField] private float _delay = 1f;
        [SerializeField] private AutoTerm _termPerfab;
        [Header("Reference")]
        [SerializeField] private Player _player;
        [SerializeField] private MapBuilder _builder;


        private Point[,] _map;
        private Coroutine _startMode;

        private List<AutoTerm> _poolActive = new List<AutoTerm>();
        private List<AutoTerm> _poolDeactive = new List<AutoTerm>();

        public bool IsActive => _startMode != null;

        public override bool IsReady => _builder && _player && _termPerfab;

        private void Start()
        {
            if (playOnStart)
                Activate();
        }

        public override void Intializate(MapBuilder builder, Player player)
        {
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
            yield return new WaitWhile(() => !IsReady);
            _map = _builder.Points;
            float progress = 0;
            while (progress < 1f && State != ModeState.Stop)
            {
                yield return new WaitWhile(() => State == ModeState.Pause);
                if (GetPoint(_player, _spawnDistance, out Point choosePoint))
                {
                    var term = GetTerm();
                    choosePoint.SetItem(term);
                    term.ShowItem();
                    term.StartAutoMode();
                }
                yield return WaitTime(_delay);
                progress += _delay / workDuration;
            }
            ClearPool(_poolDeactive);
            yield return ClearActivePool();
            _startMode = null;
            State = ModeState.Stop;
        }

        private IEnumerator ClearActivePool()
        {
            var list = new List<AutoTerm>();
            list.AddRange(_poolActive);
            while (list.Count > 0)
            {
                var listActive = new List<AutoTerm>();
                yield return WaitTime(0.2f);
                for (int i = 0; i < list.Count; i++)
                {
                    if (list[i].IsShow)
                        listActive.Add(list[i]);
                }
                list.Clear();
                list = listActive;
            }
            ClearPool(_poolActive);
        }
        private AutoTerm GetTerm()
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
                var term = Instantiate(_termPerfab.gameObject).GetComponent<AutoTerm>();
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
            return freePoints.Count > 0;
        }
        private void PoolUpdate()
        {
            var deactiveTerms = new List<AutoTerm>();
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
        private void ClearPool(List<AutoTerm> pool)
        {
            while (pool.Count > 0)
            {
                var term = pool[0];
                pool.Remove(term);
                Destroy(term.gameObject);
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
