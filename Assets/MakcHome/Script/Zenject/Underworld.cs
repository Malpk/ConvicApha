using UnityEngine;
using Zenject;
using PlayerSpace;

public class Underworld : MonoInstaller
{
    [SerializeField] private GameMap _map;
    [SerializeField] private FireMap _fireMap;
    [SerializeField] private PCController _pcController;

    public override void InstallBindings()
    {
        Container.Bind<GameMap>().FromInstance(_map).AsSingle();
        Container.Bind<FireMap>().FromInstance(_fireMap).AsSingle();
        Container.Bind<IGameController>().FromInstance(_pcController).AsSingle();
    }
}