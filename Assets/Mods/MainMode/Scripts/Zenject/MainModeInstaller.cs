using MainMode.GameInteface;
using UnityEngine;
using Zenject;

namespace MainMode
{
    public class MainModeInstaller : MonoInstaller
    {
        [SerializeField] private MapGrid _mapgrid;
        [SerializeField] private MainMenu _mainMenu;
        [SerializeField] private DeadMenu _deadMenu;
        [SerializeField] private MainSpawner _mapSpawner;
        [SerializeField] private EviilBotSpawner _eviilBotSpawner;

        public override void InstallBindings()
        {
            Container.Bind<MapGrid>().FromInstance(_mapgrid).AsSingle();
            Container.Bind<MainMenu>().FromInstance(_mainMenu).AsSingle();
            Container.Bind<DeadMenu>().FromInstance(_deadMenu).AsSingle();
            Container.Bind<MainSpawner>().FromInstance(_mapSpawner).AsSingle();
            Container.Bind<EviilBotSpawner>().FromInstance(_eviilBotSpawner).AsSingle();
        }
    }
}