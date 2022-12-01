using MainMode.GameInteface;
using UnityEngine;


namespace MainMode
{
    public abstract class GameSwitcher : MonoBehaviour
    {
        [SerializeField] private Player _player;
        [SerializeField] private DeadMenu _deadMenu;
        [SerializeField] private MainMenu _mainMenu;

        [SerializeField] protected HUDUI hud;

        protected virtual void OnEnable()
        {
            _player.OnDead += Stop;
            _deadMenu.RestartAction += Play;
            _mainMenu.PlayGameAction += Play;
            _player.OnDead += ShowDeadMenu;
            _deadMenu.RestartAction += ShowHud;
            _mainMenu.PlayGameAction += ShowHud;
            _deadMenu.BackMainMenuAction += BackMainMenu;
        }
        protected virtual void OnDisable()
        {
            _player.OnDead -= Stop;
            _deadMenu.RestartAction -= Play;
            _mainMenu.PlayGameAction -= Play;
            _player.OnDead -= ShowDeadMenu;
            _deadMenu.RestartAction -= ShowHud;
            _mainMenu.PlayGameAction -= ShowHud;
            _deadMenu.BackMainMenuAction -= BackMainMenu;
        }

        protected void Play()
        {
            _player.Play();
            PlayMessange();
        }

        protected void Stop()
        {
            _player.Stop();
            StopMessange();
        }
        private void ShowHud()
        {
            _mainMenu.Hide();
            _deadMenu.Hide();
            hud.Show();
        }

        private void ShowDeadMenu()
        {
            _deadMenu.Show();
            hud.Hide();
        }
        protected virtual void BackMainMenu()
        {
            _deadMenu.Hide();
            _mainMenu.Show();
        }
        protected abstract void PlayMessange();
        protected abstract void StopMessange();
    }
}
