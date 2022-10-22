using UnityEngine;
using Zenject;

namespace MainMode
{
    public abstract class GameSwitcher : MonoBehaviour
    {
        private Player _player;
        private DeadMenu _deadMenu;
        private MainMenu _mainMenu;

        [Inject]
        public void ConstructGameSwitcher(Player player, DeadMenu deadMenu, MainMenu mainMenu)
        {
            _player = player;
            _deadMenu = deadMenu;
            _mainMenu = mainMenu;
        }

        private void OnEnable()
        {
            _player.DeadAction += Stop;
            _deadMenu.RestartAction += Play;
            _mainMenu.PlayGameAction += Play;
        }
        private void OnDisable()
        {
            _player.DeadAction -= Play;
            _deadMenu.RestartAction -= Play;
            _mainMenu.PlayGameAction -= Play;
        }

        private void Play()
        {
            _player.Play();
        }

        protected abstract void PlayMessange();
        protected abstract void Stop();

    }
}
