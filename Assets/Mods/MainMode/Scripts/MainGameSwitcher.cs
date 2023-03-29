using UnityEngine;
using Zenject;

namespace MainMode
{
    public class MainGameSwitcher : GameSwitcher
    {
        [SerializeField] private MainModeGameRecord _saverRecord;
        [SerializeField] private TimerDisplay _bestRecords;
        [SerializeField] private MainModeGameRecord _saver;
        [SerializeField] private ChankGroup _chanks;

        private void Awake()
        {
            _bestRecords.Output(_saver.GetBestRecord());
        }

        protected override void PlayMessange()
        {
            _saverRecord.Play();
            _chanks.SpawnGroup();
        }

        protected override void StopMessange()
        {
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