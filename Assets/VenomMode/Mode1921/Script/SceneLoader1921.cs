using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MainMode.LoadScene;
using MainMode.GameInteface;
using System.Threading.Tasks;
using UserIntaface.MainMenu;

namespace MainMode.Mode1921
{
    public sealed class SceneLoader1921 : BaseLoader
    {
        [SerializeField] private Mode1921 _mode;

        private EndMenu _endMenu;

        private void OnEnable()
        {
            SubcriteEvents();
        }
        private void OnDisable()
        {
            UnSubcriteEvents();
        }
        public override async Task LoadAsync(PlayerConfig config)
        {
            await base.LoadAsync(config);
            _endMenu = holder.GetComponentInChildren<EndMenu>();
            SubcriteEvents();
            if (autoRestart)
                StartCoroutine(TracingCompliteGame());
            _mode.Intializate(holder.GetComponentInChildren<ChangeTest>());
            _mode.Play();
        }
        public void ResetGame()
        {
            _mode.Restart();
            if (player.TryGetComponent(out IReset restart))
                restart.Restart();
            player.Respawn();
            holder.SetShow(holder.GetComponentInChildren<HUDInteface>());
        }
        private void SubcriteEvents()
        {
            if (player != null)
                player.DeadAction += ShowEndMenu;
        }
        private void UnSubcriteEvents()
        {
            if (_endMenu)
                _endMenu.Restart -= ResetGame;
            if (_mode)
                _mode.Win.RemoveListener(ShowEndMenu);
            if (player != null)
                player.DeadAction -= ShowEndMenu;
        }
        private void ShowEndMenu()
        {
            holder.SetShow(_endMenu);
        }
        private IEnumerator TracingCompliteGame()
        {
            while (true)
            {
                yield return new WaitWhile(() => !player.IsDead);
                ResetGame();
            }
        }
    }
}