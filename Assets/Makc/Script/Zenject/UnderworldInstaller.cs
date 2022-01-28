using UnityEngine;
using Zenject;
using PlayerSpace;
using GameMode;
using SwitchMode;
using UnityEngine.Tilemaps;
using UIInteface;

namespace Underworld
{
    public class UnderworldInstaller : MonoInstaller
    {
        [SerializeField] private Player _player;
        [SerializeField] private Tilemap _tileMap;
        [SerializeField] private GameMap _map;
        [SerializeField] private GameEvent _gameEvent;
        [SerializeField] private Transform _playerTransform;
        [SerializeField] private SwitchMods _switchMode;
        [SerializeField] private PCController _pcController;
        [SerializeField] private TridentSetting _trident;
        [SerializeField] private UserInterface _inteface;
        [SerializeField] private CameraAnimation _cameraAnimation;


        public override void InstallBindings()
        {
            Container.Bind<GameMap>().FromInstance(_map).AsSingle();
            Container.Bind<Player>().FromInstance(_player).AsSingle();
            Container.Bind<Tilemap>().FromInstance(_tileMap).AsSingle();
            Container.Bind<GameEvent>().FromInstance(_gameEvent).AsSingle();
            Container.Bind<SwitchMods>().FromInstance(_switchMode).AsSingle();
            Container.Bind<UserInterface>().FromInstance(_inteface).AsSingle();
            Container.Bind<TridentSetting>().FromInstance(_trident).AsSingle();
            Container.Bind<IGameController>().FromInstance(_pcController).AsSingle();
            Container.Bind<CameraAnimation>().FromInstance(_cameraAnimation).AsSingle();
        }
    }
}