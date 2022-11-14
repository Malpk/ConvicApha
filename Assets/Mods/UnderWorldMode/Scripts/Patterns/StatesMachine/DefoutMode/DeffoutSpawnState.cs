using UnityEngine;

namespace Underworld
{
    public class DeffoutSpawnState : IPatternState
    {
        private readonly IStateSwitcher switcher;

        private float _spawnDelay;
        private float _duration;

        private float _progress = 0f;
        private float _delay = 0f;

        public DeffoutSpawnState(IStateSwitcher switcher)
        {
            this.switcher = switcher;
        }

        public System.Action OnSpawn;

        public bool IsComplite => _progress >= 1f;

        public void Intializate(float spawnDelay, float duration)
        {
            _spawnDelay = spawnDelay;
            _duration = duration >= 1 ? duration : 1;
        }

        public void Start()
        {
            _progress = 0;
            _delay = 0;
        }

        public bool Update()
        {
            _delay += Time.deltaTime / _spawnDelay;
            if (_delay > 1f)
            {
                _delay = 0f;
                OnSpawn?.Invoke();
            }
            _progress += Time.deltaTime / _duration;
            return !IsComplite;
        }

        public bool SwitchState(out IPatternState nextState)
        {
            return switcher.SwitchState<DefoutCompliteState>(out nextState);
        }
    }
}
