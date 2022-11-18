using UnityEngine;

namespace Underworld
{
    public class DeffoutSpawnState : BasePatternState
    {
        private float _spawnDelay;
        private float _duration;

        private float _progress = 0f;
        private float _delay = 0f;

        public System.Action OnSpawn;

        public override bool IsComplite => _progress >= 1f;

        public void Intializate(float spawnDelay, float duration)
        {
            _spawnDelay = spawnDelay;
            _duration = duration >= 1 ? duration : 1;
        }

        public override void Start()
        {
            _progress = 0;
            _delay = 0;
        }

        public override bool Update()
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
    }
}
