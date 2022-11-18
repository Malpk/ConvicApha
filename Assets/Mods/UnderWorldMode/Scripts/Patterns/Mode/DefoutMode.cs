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

        private BasePatternState _curretState;

        private void Awake()
        {
            _pool = new PoolTerm(_termPerfab);
            _spawnState = new DeffoutSpawnState();
            _spawnState.Intializate(_delay, workDuration);
            _spawnState.SetNextState(new DefoutCompliteState(_pool, 0.2f));
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
        protected override void PlayMode()
        {
            _curretState = _spawnState;
            _curretState.Start();
            enabled = true;
        }
        protected override void StopMode()
        {
            enabled = false;
            _pool.ClearPool();
        }
        private void Update()
        {
            if (!_curretState.Update())
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
