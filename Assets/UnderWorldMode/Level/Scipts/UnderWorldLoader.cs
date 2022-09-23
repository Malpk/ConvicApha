using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using MainMode.LoadScene;
using System.Threading.Tasks;
using UserIntaface.MainMenu;

namespace Underworld
{
    public sealed class UnderWorldLoader : BaseLoader
    {
        [SerializeField] private UnderWorldGameBuilder _builder;

        public async override Task LoadAsync(PlayerConfig config)
        {
            await base.LoadAsync(config);
            _builder.Intilizate(player);
            _builder.Play();
            if (autoRestart)
                player.DeadAction += Restart;
        }

        public void Restart()
        {
            _builder.Restart();
        }
    }
}