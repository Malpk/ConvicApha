using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MainMode.LoadScene;
using MainMode.GameInteface;

namespace MainMode.Mode1921
{
    public class SceneLoader1921 : MainLoader
    {
        [SerializeField] private Mode1921 _perfab;

        private Mode1921 _mode;
        private EndMenu _endMenu;

        private void OnEnable()
        {
            SubcriteEvents();
        }
        private void OnDisable()
        {
            UnSubcriteEvents();
        }

        public override void Load(PlayerType choose)
        {
            base.Load(choose);
            _mode = Instantiate(_perfab.gameObject, Vector3.zero, Quaternion.identity).GetComponent<Mode1921>();
            _mode.GetComponent<ItemSpwaner>().Run(player.transform);
            _endMenu = holder.GetComponentInChildren<EndMenu>();
            var test = holder.GetComponentInChildren<ChangeTest>();
            if (test)
                _mode.Intializate(test);
            SubcriteEvents();
        }
        public void ResetGame()
        {
            _mode.CreateMap();
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