using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MainMode.LoadScene;
using UserIntaface.MainMenu;
using System.Threading.Tasks;

namespace MainMode
{
    public sealed class MainLoader : BaseLoader
    {
        [SerializeField] private MapGrid _mapGride;
        [SerializeField] private MapSpawner _spawner;
        [SerializeField] private EviilBotSpawner _botSpawner;

        public async override Task LoadAsync(PlayerConfig config)
        {
            await base.LoadAsync(config);
            await _spawner.Intializate(player);
            _mapGride.Intilizate(player);
            _spawner.Run();
            _botSpawner.Intitlizate(player, _mapGride);
            _botSpawner.Play();
        }
    }
}