using UnityEngine;
using Zenject;
using PlayerSpace;
using Trident;

public class Underworld : MonoInstaller
{
    [SerializeField] private GameMap _map;
    [SerializeField] private FireMap _fireMap;
    [SerializeField] private PCController _pcController;
    [SerializeField] private TridentSetting _trident;

    public override void InstallBindings()
    {
        Container.Bind<GameMap>().FromInstance(_map).AsSingle();
        Container.Bind<FireMap>().FromInstance(_fireMap).AsSingle();
        Container.Bind<TridentSetting>().FromInstance(_trident).AsSingle();
        Container.Bind<IGameController>().FromInstance(_pcController).AsSingle();
    }
}