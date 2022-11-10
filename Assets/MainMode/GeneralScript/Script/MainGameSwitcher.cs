using UnityEngine;
using Zenject;
using MainMode.GameInteface;

namespace MainMode
{
    public class MainGameSwitcher : GameSwitcher
    {
        [SerializeField] private MapSpawner _mapSpawner;
        [SerializeField] private EviilBotSpawner _botSpawner;
        [SerializeField] private TimerCountRecord _timer;

        [Inject]
        public void Construct(MapSpawner mapSpawner, EviilBotSpawner eviilBotSpawner)
        {
            _mapSpawner = mapSpawner;
            _botSpawner = eviilBotSpawner;
        }

        protected override void PlayMessange()
        {
            _mapSpawner.Play();
            _botSpawner.Play();
            _timer.Play();
        }

        protected override void StopMessange()
        {
            _mapSpawner.Stop();
            _botSpawner.Stop();
            _timer.Stop();
        }
    }
}