using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MainMode.LoadScene;
using UserIntaface.MainMenu;
using System.Threading.Tasks;

namespace MainMode
{
    public class MainLoader : BaseLoader
    {
        [SerializeField] private MapSpawner _spawner;

        public async override Task LoadAsync(PlayerConfig config)
        {
            await base.LoadAsync(config);
            await _spawner.Intializate(player);
            _spawner.Run();
        }
    }
}