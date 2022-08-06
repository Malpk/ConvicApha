using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MainMode.LoadScene;
using UserIntaface.MainMenu;

namespace MainMode
{
    public class MainLoader : BaseLoader
    {
        [SerializeField] private MapSpawner _spawner;

        public override void Load(PlayerType choose)
        {
            base.Load(choose);
            _spawner.Intializate(player);
            _spawner.Run();
        }
        public override void Load(PlayerConfig config)
        {
            base.Load(config);
            _spawner.Intializate(player);
            _spawner.Run();
        }
    }
}