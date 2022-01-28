using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UIInteface;
using Zenject;


namespace GameMode
{
    public class GameEvent : MonoBehaviour
    {
        [SerializeField] private int _id;

        private static Dictionary<int, GameEvent> _objetToScene = new Dictionary<int, GameEvent>();

        private Player _player;
        private UserInterface _inteface;
        private GameState _state = GameState.Pause;
        
        public delegate void State(GameState State);
        public event State StatusUpdate;

        public delegate void Dead();
        public event Dead DeadAction;

        public GameState state => _state;

        private void Start()
        {
            if (_objetToScene.ContainsKey(_id))
            {
                InvateStateUpdate(Choose(_objetToScene[_id].state));
                _objetToScene.Remove(_id);
            }
            _objetToScene.Add(_id, this);
        }

        [Inject] 
        private void Constructor(Player player,UserInterface inteface)
        {
            _player = player;
            _inteface = inteface;
        }
        private void OnEnable()
        {
            _player.DeadAction += MessangePlayerDead;
            _inteface.CommandsAction += InvateStateUpdate;
        }
        private void OnDisable()
        {
            _player.DeadAction -= MessangePlayerDead;
            _inteface.CommandsAction -= InvateStateUpdate;
        }
        private GameState Choose(GameState state)
        {
            switch (state)
            {
                case GameState.MainMenu:
                    return GameState.Pause;
                case GameState.Restart:
                    return GameState.Play;
                default:
                    return state;
            }
        }

        public void MessangePlayerDead()
        {
            if (DeadAction != null)
                DeadAction();
            InvateStateUpdate(GameState.Dead);
        }

        private void InvateStateUpdate(GameState state)
        {
            _state = state;
            if (StatusUpdate != null)
                StatusUpdate(state);
        }
    }
}