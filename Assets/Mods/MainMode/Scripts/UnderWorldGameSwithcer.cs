using System.Collections;
using UnityEngine;
using MainMode;

namespace Underworld
{
    public class UnderWorldGameSwithcer : GameSwitcher
    {
        [SerializeField] private Ending _ending;
        [SerializeField] private TImerCount _timeCounter;
        [SerializeField] private TimerDisplay _bestRecords;
        [SerializeField] private UnderWorldGameBuilder _builder;

        private void Awake()
        {
            _bestRecords.Output(_timeCounter.GetRecords());
        }

        protected override void OnEnable()
        {
            base.OnEnable();
            _timeCounter.OnCompliteTimer += ShowWinScreen;
            _ending.HideAction += BackMainMenu;
        }

        protected override void OnDisable()
        {
            base.OnDisable();
            _timeCounter.OnCompliteTimer -= ShowWinScreen;
            _ending.HideAction -= BackMainMenu;
        }
        protected override void PlayMessange()
        {
            _builder.Play();
            _timeCounter.Play();
        }
        protected override void StopMessange()
        {
            _builder.Stop();
            _timeCounter.Stop();
        }
        private void ShowWinScreen()
        {
            Stop();
            hud.Hide();
            _ending.Win();
            _ending.Show();
        }
        protected override void BackMainMenu()
        {
            base.BackMainMenu();
            _bestRecords.Output(_timeCounter.GetRecords());
        }
    }
}