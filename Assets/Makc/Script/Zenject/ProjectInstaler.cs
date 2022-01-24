using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameMode;
using Zenject;

public class ProjectInstaler : MonoInstaller
{
    [SerializeField] private GameEvent _gameEvent;
    [SerializeField] private Player _player;
    public override void InstallBindings()
    {
        Container.Bind<GameEvent>().AsSingle();
    }
}
