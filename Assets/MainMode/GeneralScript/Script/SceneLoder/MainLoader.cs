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

        private void LoadMainMode()
        {
            if (!_isLoad)
            {
                _isLoad = true;
                _mapGride.Intilizate();
            }
        }
        private void IntializateAsync(Player player)
        {
        }
        private void PlayMainMode()
        {
            _botSpawner.Play();
        }
        private void StopMainMode()
        {
            _botSpawner.Stop();
        }

    }
}