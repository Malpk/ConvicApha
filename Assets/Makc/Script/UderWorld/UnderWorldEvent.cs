using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UIInteface;
using Zenject;

namespace Underworld
{
    public class UnderWorldEvent : GameEvent
    {
        private Win _win;
        private Player _player;
        private LvlTimer _timer;

        private UserInterface _inteface;

        public override event Commands DeadAction;
        public override event Commands WinAction;
        public override event Commands StartAction;
        public override event Commands StopAction;

        [Inject] 
        private void Constructor(Player player,UserInterface inteface, LvlTimer timer,Win win)
        {
            _win = win;
            _player = player;
            _inteface = inteface;
            _timer = timer;
        }
        private void OnEnable()
        {
            _win.BlackScreenAction += StopGame;
            _player.DeadAction += MessangePlayerDead;
            _inteface.CommandsAction += InvateStateUpdate;
            _timer.WinGameAction += WinEvent;
        }
        private void OnDisable()
        {
            _win.BlackScreenAction -= StopGame;
            _player.DeadAction -= MessangePlayerDead;
            _inteface.CommandsAction -= InvateStateUpdate;
            _timer.WinGameAction -= WinEvent;
        }
        private void Start()
        {
            if (typeEvent == TypeGameEvent.Start)
                StartGame();
        }
        public void MessangePlayerDead()
        {
            typeEvent = TypeGameEvent.Dead;
            state = LvlSate.Pause;
            if (DeadAction != null)
                DeadAction();
        }

        private void InvateStateUpdate(TypeGameEvent state)
        {
            switch (state)
            {
                case TypeGameEvent.MainMenu:
                    StopAction();
                    return;
                case TypeGameEvent.Start:
                    StartGame();
                    return;
                case TypeGameEvent.Dead:
                    MessangePlayerDead();
                    return;
                case TypeGameEvent.Win:
                    WinEvent();
                    return;
            }
        }
        private void WinEvent()
        {
            if (WinAction != null)
                WinAction();
            state = LvlSate.Pause;
            typeEvent = TypeGameEvent.Win;
        }
        private void StartGame()
        {
            if (StartAction != null)
                StartAction();
            state = LvlSate.Play;
            typeEvent = TypeGameEvent.Start;
        }
        private void StopGame()
        {
            if (StopAction != null)
                StopAction();
            state = LvlSate.Pause;
        }
    }
}