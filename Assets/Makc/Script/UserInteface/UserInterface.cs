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
        [SerializeField] private Canvas _exitGameMenu;
        [SerializeField] private string _vkUrl = "https://vk.com/nestestate";
        [SerializeField] private string _youTubeUrl = "https://www.youtube.com/channel/UCkCJRNGvuwb8JmoF3vqvtcw";

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

        private void Update()
        {
            if (Input.GetKey(KeyCode.Escape))
                ShowExitGameNenu();
        }
        private void ShowExitGameNenu()
        {
            Time.timeScale = 0;
            _exitGameMenu.enabled = true;
        }
        public void OnExitGame()
        {
            Application.Quit();
        }
        public void OnReturnInGame()
        {
            Time.timeScale = 1;
            _exitGameMenu.enabled = false;
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
        public void OpenYouTubeCannel()
        {
            Application.OpenURL(_youTubeUrl);
        }
        public void OpenVkGroup()
        {
            Application.OpenURL(_vkUrl);
        }
    }
}