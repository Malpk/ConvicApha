using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MainMode.LoadScene;

namespace MainMode.GameInteface
{
    public class PlayerSelectMenu : MonoBehaviour
    {
        [SerializeField] private ScrolMenuBase _selecter;
        [SerializeField] private List<PlayerInfo> _players;

        public PlayerInfo PlayerConfig => GetPlayerConfig();

        private void Start()
        {
            var items = new ScrollItem[_players.Count];
            for (int i = 0; i < items.Length; i++)
            {
                items[i] = _players[i];
            }
            _selecter.Intializate(items);
        }

        private PlayerInfo GetPlayerConfig()
        {
            if (_selecter.GetCenterPoint().Item is PlayerInfo config)
                return config;
            return null;
        }
    }
}