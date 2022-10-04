using UnityEngine;
using UnityEngine.Video;
using MainMode.LoadScene;
using MainMode.GameInteface;

public class DeadMenu : UserInterface
{
    [SerializeField] private bool _showOnStart;
    [Header("Reference")]
    [SerializeField] private Canvas _canvas;
    [SerializeField] private BaseLoader _sceneLoader;
    [SerializeField] private VideoPlayer _backGroundVideo;

    public override UserInterfaceType Type => UserInterfaceType.EndMenu;

    public void Intializate(BaseLoader loader)
    {
        _sceneLoader = loader;
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
        _sceneLoader.Play();
    }

    public void OnBackMainMenu()
    {
        _sceneLoader.BackMainMenu();
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