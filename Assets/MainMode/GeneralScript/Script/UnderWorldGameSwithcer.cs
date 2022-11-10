using System.Collections;
using UnityEngine;
using MainMode;

namespace Underworld
{
    public class UnderWorldGameSwithcer : GameSwitcher
    {
        [SerializeField] private Ending _ending;
        [SerializeField] private TImerCount _timeCounter;
        [SerializeField] private UnderWorldGameBuilder _builder;


        protected override void OnEnable()
        {
            base.OnEnable();
            _timeCounter.OnCompliteTimer += ShowWinScreen;
        }

        protected override void OnDisable()
        {
            base.OnDisable();
            _timeCounter.OnCompliteTimer -= ShowWinScreen;
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
            StartCoroutine(WaitPreesAnyKey());
        }

        private IEnumerator WaitPreesAnyKey()
        {
            yield return new WaitForSeconds(1f);
            yield return new WaitWhile(() => !Input.anyKeyDown);
            _ending.Hide();
            BackMainMenu();
        }
    }
}