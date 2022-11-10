using Zenject;
using UnityEngine;

namespace MainMode.Mode1921
{
    public class VenomGameSwitceher : GameSwitcher
    {
        [SerializeField] private Mode1921 _mode;
        [SerializeField] private ChangeTest _chageTest;

        protected override void PlayMessange()
        {
            _mode.Play();
        }

        protected override void StopMessange()
        {
            _mode.Stop();
        }

        public void ShowRepairTest()
        {
            _chageTest.Show();
            if (hud.IsShow)
                hud.Hide();
        }

        public void HideRepairTest()
        {
            _chageTest.Hide();
            if (hud.IsShow)
                hud.Show();
        }
    }
}
