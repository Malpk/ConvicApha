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

        private PlayerBehaviour _behaviour;

        protected virtual void OnEnable()
        {
            _deadMenu.RestartAction += Play;
            _mainMenu.PlayGameAction += Play;
            _deadMenu.RestartAction += ShowHud;
            _mainMenu.PlayGameAction += ShowHud;
            _deadMenu.BackMainMenuAction += BackMainMenu;
        }
        protected virtual void OnDisable()
        {
            _deadMenu.RestartAction -= Play;
            _mainMenu.PlayGameAction -= Play;
            _deadMenu.RestartAction -= ShowHud;
            _mainMenu.PlayGameAction -= ShowHud;
            _deadMenu.BackMainMenuAction -= BackMainMenu;
        }

        public void SetPlayerBehaviour(PlayerBehaviour behaviour)
        {
            if (_behaviour)
            {
                _behaviour.OnDead.RemoveListener(Stop);
                _behaviour.OnDead.RemoveListener(ShowDeadMenu);
            }
            _behaviour = behaviour;
            _behaviour.OnDead.AddListener(Stop);
            _behaviour.OnDead.AddListener(ShowDeadMenu);
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
