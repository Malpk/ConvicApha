using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MainMode.LoadScene;
using System.Threading.Tasks;

namespace Underworld
{
    public sealed class UnderWorldLoader : BaseLoader
    {
        [SerializeField] private MainMenu _mainMenu;
        [SerializeField] private UnderWorldGameBuilder _builder;

        private DeadMenu _deadMenu;

        public async override Task LoadAsync(PlayerConfig config)
        {
            await base.LoadAsync(config);
            _builder.Intilizate(player);
            _builder.Play();
            if (autoRestart)
                player.DeadAction += Restart;
            _deadMenu = holder.GetComponentInChildren<DeadMenu>();
            _deadMenu.Intializate(this);
            player.DeadAction += StopGame;
        }

        public void StopGame()
        {
            holder.SetShow(_deadMenu);
            _builder.Stop();
        }

        public void Restart()
        {
            _builder.Play();
            holder.SetHide();
            player.Respawn();
        }

        public void BackMainMenu()
        {
            _mainMenu.Show();
            _deadMenu.Hide();
        }
    }
}