using UnityEngine;
using Zenject;
using PlayerSpace;
using Trident;
using UnityEngine.Tilemaps;

namespace Underworld
{
    public class UnderworldInstaller : MonoInstaller
    {
        [SerializeField] private Tilemap _tileMap;
        [SerializeField] private GameMap _map;
        [SerializeField] private PCController _pcController;
        [SerializeField] private TridentSetting _trident;
        [SerializeField] private Transform _player;

        public override void InstallBindings()
        {
            Container.Bind<GameMap>().FromInstance(_map).AsSingle();
            Container.Bind<Tilemap>().FromInstance(_tileMap).AsSingle();
            Container.Bind<Transform>().FromInstance(_player).AsSingle();
            Container.Bind<TridentSetting>().FromInstance(_trident).AsSingle();
            Container.Bind<IGameController>().FromInstance(_pcController).AsSingle();
        }
    }
}