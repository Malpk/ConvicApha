using UnityEngine;
using UnityEngine.Video;
using MainMode.GameInteface;

namespace Underworld
{
    public class DeadMenu : UserInterface
    {
        [SerializeField] private bool _showOnStart;
        [Header("Reference")]
        [SerializeField] private Canvas _canvas;
        [SerializeField] private VideoPlayer _backGroundVideo;
        [SerializeField] private UnderWorldLoader _underWorldLoader;

        public override UserInterfaceType Type => UserInterfaceType.EndMenu;

        public void Intializate(UnderWorldLoader underWorldLoader)
        {
            _underWorldLoader = underWorldLoader;
        }

        private void OnEnable()
        {
           ShowAction += ShowMenu;
           HideAction += HideMenu;
        }
        private void OnDisable()
        {
            ShowAction -= ShowMenu;
            HideAction -= HideMenu;
        }
        private void Start()
        {
            if (_showOnStart)
                Show();
            else
                Hide();
        }

        public void OnRestart()
        {
            _underWorldLoader.Restart();
        }

        public void OnBackMainMenu()
        {
            _underWorldLoader.BackMainMenu();
        }

        private void ShowMenu()
        {
            _canvas.enabled = true;
            _backGroundVideo.Play();
        }

        private void HideMenu()
        {
            _canvas.enabled = false;
            _backGroundVideo.Stop();
        }
    }
}