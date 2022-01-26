using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameMode;
using Zenject;
using UnityEngine.SceneManagement;

namespace UIInteface
{
    public class UserInterface : MonoBehaviour, BaseInteface
    {
        [SerializeField] private Canvas _curreCanvas;
        [SerializeField] private Canvas _deadMenu;
        [SerializeField] private Canvas _HUD;

        [Inject] private GameEvent _eventMap;

        private List<Canvas> _showHistor = new List<Canvas>();
        private List<Canvas> _closeHistor = new List<Canvas>();

        public event BaseInteface.Commands CommandsAction;

        private void OnEnable()
        {
            _eventMap.StatusUpdate += ShowMenu;
        }
        private void OnDisable()
        {
            _eventMap.StatusUpdate -= ShowMenu;
        }
        public void OnShow(Canvas canvas)
        {
            _showHistor.Add(_curreCanvas);
            _curreCanvas.enabled = false;
            canvas.enabled = true;
            _curreCanvas = canvas;
        }
        public void OnShow()
        {
            var count = _closeHistor.Count;
            if (count > 0)
                _closeHistor[count - 1].enabled = false;
        }
        private void ShowDeadMenu()
        {
            OnShow(_deadMenu);
        }
        public void OnPause()
        {
            InvateEvent(GameState.MainMenu);
            LoadScene();
        }
        public void OnPlay()
        {
            InvateEvent(GameState.Play);
        }
        public void OnRestart()
        {
            InvateEvent(GameState.Play);
            LoadScene();
        }
        private void InvateEvent(GameState state)
        {
            if (CommandsAction != null)
                CommandsAction(state);
        }
        private void ShowMenu(GameState state)
        {
            switch (state)
            {
                case GameState.Pause:
                    return;
                case GameState.Play:
                    OnShow(_HUD);
                    return;
                case GameState.Dead:
                    ShowDeadMenu();
                    return;
            }
        }
        private void LoadScene()
        {
            Time.timeScale = 1f;
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
    }
}