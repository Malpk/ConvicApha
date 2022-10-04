using UnityEngine;
using MainMode.LoadScene;

namespace MainMode
{
    public sealed class MainLoader : BaseLoader
    {
        [SerializeField] private MapGrid _mapGride;
        [SerializeField] private MapSpawner _spawner;
        [SerializeField] private EviilBotSpawner _botSpawner;

        private bool _isLoad;

        protected override void OnEnable()
        {
            base.OnEnable();
            LoadAction += LoadMainMode;
            PlayAction += PlayMainMode;
            StopGameAction += StopMainMode;
            playerLoader.PlayerLoadAction += IntializateAsync;
        }

        protected override void OnDisable()
        {
            base.OnDisable();
            LoadAction -= LoadMainMode;
            PlayAction -= PlayMainMode;
            StopGameAction -= StopMainMode;
            playerLoader.PlayerLoadAction -= IntializateAsync;
        }

        private async void LoadMainMode()
        {
            if (!_isLoad)
            {
                _isLoad = true;
                await _spawner.Load();
                _mapGride.Intilizate();
                _botSpawner.SetMapGrid(_mapGride);
            }
        }
        private void IntializateAsync(Player player)
        {
            _spawner.Intializate(player);
            _botSpawner.SetPlayer(player);
        }
        private void PlayMainMode()
        {
            _spawner.Play();
            _botSpawner.Play();
        }
        private void StopMainMode()
        {
            _spawner.Stop();
            _botSpawner.Stop();
        }

    }
}