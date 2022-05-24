using UnityEngine;
using Zenject;
using PlayerComponent;
using Underworld;
using UnityEngine.Tilemaps;
using UIInteface;

namespace Underworld
{
    public class UnderworldInstaller : MonoInstaller
    {
        [SerializeField] private Player _player;
        [SerializeField] private UnderWorldEvent _gameEvent;
        [SerializeField] private Transform _playerTransform;
        [SerializeField] private SwitchMode _switchMode;
        [SerializeField] private PCController _pcController;
        [SerializeField] private UserInterface _inteface;
        [SerializeField] private CameraAnimation _cameraAnimation;
        
        [SerializeField] private Win _win;
        [SerializeField] private LvlTimer _timer;
        public override void InstallBindings()
        {
            Container.Bind<Win>().FromInstance(_win).AsSingle();
            Container.Bind<Player>().FromInstance(_player).AsSingle();
            Container.Bind<LvlTimer>().FromInstance(_timer).AsSingle();
            Container.Bind<GameEvent>().FromInstance(_gameEvent).AsSingle();
            Container.Bind<UnderWorldEvent>().FromInstance(_gameEvent).AsSingle();
            Container.Bind<SwitchMode>().FromInstance(_switchMode).AsSingle();
            Container.Bind<UserInterface>().FromInstance(_inteface).AsSingle();
            Container.Bind<IGameController>().FromInstance(_pcController).AsSingle();
            Container.Bind<CameraAnimation>().FromInstance(_cameraAnimation).AsSingle();
     
        }
    }
}