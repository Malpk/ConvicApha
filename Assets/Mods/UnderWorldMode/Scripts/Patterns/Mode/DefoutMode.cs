using System.Collections.Generic;
using UnityEngine;

namespace Underworld
{
    public sealed class DefoutMode : GeneralMode
    {
        [Header("Spawn Setting")]
        [Min(1)]
        [SerializeField] private float _spawnDistance = 1;
        [Range(0.005f, 1)]
        [SerializeField] private float _delay = 1f;
        [SerializeField] private AutoTerm _termPerfab;
        [Header("Reference")]
        [SerializeField] private Player _player;
        [SerializeField] private MapBuilder _builder;

        private PoolTerm _pool;
        private DeffoutSpawnState _spawnState;

        private IPatternState _curretState;

        public bool IsActive => enabled;

        private void Awake()
        {
            _pool = new PoolTerm(_termPerfab);
            _spawnState = new DeffoutSpawnState(switcher);
            switcher.AddState(_spawnState);
            switcher.AddState(new DefoutCompliteState(_pool, 0.2f));
            _spawnState.Intializate(_delay, workDuration);
            enabled = false;
        }
        private void OnEnable()
        {
            _spawnState.OnSpawn += SpawnTerm;
        }
        private void OnDisable()
        {
            _spawnState.OnSpawn -= SpawnTerm;
        }
        public override void Intializate(MapBuilder builder, Player player)
        {
            _builder = builder;
            _player = player;
        }

        public override void SetConfig(PaternConfig config)
        {
            if (config is DefoutModeConfig defoutModeConfig)
            {
                workDuration = defoutModeConfig.WorkDuration;
                _spawnDistance = defoutModeConfig.SpawnDistance;
                _delay = defoutModeConfig.SpawnDelay;
                _spawnState.Intializate(_delay, workDuration);
            }
            else
            {
                throw new System.NullReferenceException("DefoutModeConfig is null");
            }
        }

        private void Start()
        {
            if (playOnStart)
                Play();
        }
        public override bool Play()
        {
            State = ModeState.Play;
            _curretState = _spawnState;
            _curretState.Start();
            enabled = true;
            return true;
        }
        public void Stop()
        {
            enabled = false;
            _pool.ClearPool();
        }
        private void Update()
        {
            if (!_curretState.Update())
            {
                if (_curretState.SwitchState(out IPatternState nextState))
                {
                    _curretState = nextState;
                }
                else
                {
                    Stop();
                }
            }
        }
        private void SpawnTerm()
        {
            if (_builder.GetPoint(_player, _spawnDistance, out Point choosePoint))
            {
                var term = _pool.GetTerm(transform);
                choosePoint.SetItem(term);
                term.StartAutoMode();
            }
        }
    }
}
