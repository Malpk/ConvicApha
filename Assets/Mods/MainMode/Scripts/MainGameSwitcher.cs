using UnityEngine;
using Zenject;

namespace MainMode
{
    public class MainGameSwitcher : GameSwitcher
    {
        [SerializeField] private MapSpawner _mapSpawner;
        [SerializeField] private EviilBotSpawner _botSpawner;
        [SerializeField] private MainModeGameRecord _saverRecord;
        [SerializeField] private TimerDisplay _bestRecords;
        [SerializeField] private MainModeGameRecord _saver;
        [SerializeField] private ChankGroup _chanks;

        private void Awake()
        {
            _bestRecords.Output(_saver.GetBestRecord());
        }

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
            _saverRecord.Play();
            _chanks.SpawnGroup();
        }

        protected override void StopMessange()
        {
            _mapSpawner.Stop();
            _botSpawner.Stop();
            _saverRecord.Stop();
            _chanks.ClearDelete();
        }
        protected override void BackMainMenu()
        {
            base.BackMainMenu();
            _bestRecords.Output(_saver.GetBestRecord());
        }
    }
}