using MainMode.GameInteface;
using UnityEngine;
using Zenject;

namespace MainMode
{
    public abstract class GameSwitcher : MonoBehaviour
    {
        [SerializeField] private Player _player;
        [SerializeField] private DeadMenu _deadMenu;
        [SerializeField] private MainMenu _mainMenu;

        [SerializeField] protected HUDInteface hud;

        private void OnEnable()
        {
            _player.DeadAction += Stop;
            _deadMenu.RestartAction += Play;
            _mainMenu.PlayGameAction += Play;
            _deadMenu.BackMainMenuAction += BackMainMenu;
        }
        private void OnDisable()
        {
            _player.DeadAction -= Play;
            _deadMenu.RestartAction -= Play;
            _mainMenu.PlayGameAction -= Play;
            _deadMenu.BackMainMenuAction -= BackMainMenu;
        }

        private void Play()
        {
            _player.Play();
            _mainMenu.Hide();
            _deadMenu.Hide();
            hud.Show();
            PlayMessange();
        }

        private void Stop()
        {
            _player.Stop();
            _deadMenu.Show();
            hud.Hide();
            StopMessange();
        }

        private void BackMainMenu()
        {
            _deadMenu.Hide();
            _mainMenu.Show();
        }
        protected abstract void PlayMessange();
        protected abstract void StopMessange();

    }
}
