using UnityEngine;
using Zenject;

namespace MainMode
{
    public class MainGameSwitcher : GameSwitcher
    {
        [SerializeField] private MainSpawner _mapSpawner;
        [SerializeField] private EviilBotSpawner _botSpawner;
        [SerializeField] private MainModeGameRecord _saverRecord;
        [SerializeField] private TimerDisplay _bestRecords;
        [SerializeField] private MainModeGameRecord _saver;

        private void Awake()
        {
            _bestRecords.Output(_saver.GetBestRecord());
        }

        [Inject]
        public void Construct(MainSpawner mapSpawner, EviilBotSpawner eviilBotSpawner)
        {
            _mapSpawner = mapSpawner;
            _botSpawner = eviilBotSpawner;
        }

        protected override void PlayMessange()
        {
            _mapSpawner.Play();
            _botSpawner.Play();
            _saverRecord.Play();
        }

        protected override void StopMessange()
        {
            _mapSpawner.Stop();
            _botSpawner.Stop();
            _saverRecord.Stop();
        }
        protected override void BackMainMenu()
        {
            base.BackMainMenu();
            _bestRecords.Output(_saver.GetBestRecord());
        }
    }
}