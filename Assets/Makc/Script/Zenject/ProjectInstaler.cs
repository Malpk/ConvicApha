using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Underworld;
using Zenject;

public class ProjectInstaler : MonoInstaller
{
    [SerializeField] private UnderWorldEvent _gameEvent;
    [SerializeField] private Player _player;
    public override void InstallBindings()
    {
        Container.Bind<UnderWorldEvent>().AsSingle();
    }
}
