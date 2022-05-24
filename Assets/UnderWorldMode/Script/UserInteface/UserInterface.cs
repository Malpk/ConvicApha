using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;
using UnityEngine.SceneManagement;
using Underworld;

namespace UIInteface
{
    public class UserInterface : MonoBehaviour, BaseInteface
    {
        [SerializeField] private Canvas _curreCanvas;
        [SerializeField] private Canvas _deadMenu;
        [SerializeField] private Canvas _HUD;
        [SerializeField] private Canvas _exitGameMenu;
        [SerializeField] private Canvas _titleMenu;
        [SerializeField] private string _vkUrl = "https://vk.com/nestestate";
        [SerializeField] private string _youTubeUrl = "https://www.youtube.com/channel/UCkCJRNGvuwb8JmoF3vqvtcw";

        [Inject] private UnderWorldEvent _eventMap;

        
        private List<Canvas> _showHistor = new List<Canvas>();
        private List<Canvas> _closeHistor = new List<Canvas>();

        public event BaseInteface.Commands CommandsAction;

        private void OnEnable()
        {
            _eventMap.StartAction += () => OnShow(_HUD);
            _eventMap.DeadAction += ShowDeadMenu;
            _eventMap.WinAction += ShowTitle;
        }
        private void OnDisable()
        {
            _eventMap.StartAction -= () => OnShow(_HUD);
            _eventMap.DeadAction -= ShowDeadMenu;
            _eventMap.WinAction -= ShowTitle;
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                if (!_exitGameMenu.enabled)
                    ShowExitGameNenu();
                else
                    OnReturnInGame();
            }
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
            InvateEvent(TypeGameEvent.MainMenu);
            LoadScene();
        }
        public void OnPlay()
        {
            InvateEvent(TypeGameEvent.Start);
        }
        public void OnRestart()
        {
            InvateEvent(TypeGameEvent.Start);
            LoadScene();
        }
        private void InvateEvent(TypeGameEvent state)
        {
            if (CommandsAction != null)
                CommandsAction(state);
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
        private void ShowTitle()
        {
            _titleMenu.enabled = true;
        }
    }
}