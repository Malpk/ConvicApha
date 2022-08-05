using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MainMode.LoadScene;

namespace MainMode
{
    public class MainLoader : BaseLoder
    {
        [SerializeField] private MapSpawner _spawner;

        public override void Load(PlayerType choose)
        {
            base.Load(choose);
            _spawner.Intializate(player);
            _spawner.Run();
        }
    }
}