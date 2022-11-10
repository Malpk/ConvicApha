using UnityEngine;
using Zenject;
using MainMode.GameInteface;

public class BaseInstaller : MonoInstaller
{
    [SerializeField] private Player _player;
    [SerializeField] private HUDInteface _hud;
    [SerializeField] private CameraFollowing _cameraFolowing;

    public override void InstallBindings()
    {
        Container.Bind<Player>().FromInstance(_player).AsSingle();
        Container.Bind<HUDInteface>().FromInstance(_hud).AsSingle();
        Container.Bind<CameraFollowing>().FromInstance(_cameraFolowing).AsSingle();
    }
}
