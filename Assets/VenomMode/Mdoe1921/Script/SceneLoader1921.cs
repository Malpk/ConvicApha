using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MainMode.LoadScene;
using MainMode.GameInteface;

namespace MainMode.Mode1921
{
    public sealed class SceneLoader1921 : BaseLoader
    {
        [SerializeField] private Mode1921 _perfab;

        private Mode1921 _mode;
        private EndMenu _endMenu;
        private ItemSpwaner _itemSpawner;

        private void OnEnable()
        {
            SubcriteEvents();
        }
        private void OnDisable()
        {
            UnSubcriteEvents();
        }

        public void ResetGame()
        {
            _mode.CreateMap();
            if (player.TryGetComponent(out IReset restart))
                restart.Restart();
            _itemSpawner.Restart();
            player.Respawn();
            holder.SetShow(holder.GetComponentInChildren<HUDInteface>());
        }
        private void SubcriteEvents()
        {
            if (_endMenu)
                _endMenu.Restart += ResetGame;
            if (_mode)
                _mode.Win.AddListener(ShowEndMenu);
            if (player != null)
                player.OnDead += ShowEndMenu;
        }
        private void UnSubcriteEvents()
        {
            if (_endMenu)
                _endMenu.Restart -= ResetGame;
            if (_mode)
                _mode.Win.RemoveListener(ShowEndMenu);
            if (player != null)
                player.OnDead -= ShowEndMenu;
        }
        private void ShowEndMenu()
        {
            holder.SetShow(_endMenu);
        }
       
    }
}