using System;
using UnityEngine;
using UnityEngine.Video;
using MainMode.GameInteface;

public class DeadMenu : UserInterface
{
    [SerializeField] private bool _showOnStart;
    [Header("Reference")]
    [SerializeField] private Canvas _canvas;
    [SerializeField] private VideoPlayer _backGroundVideo;

    public event Action RestartAction;
    public event Action BackMainMenuAction;

    public override UserInterfaceType Type => UserInterfaceType.EndMenu;

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
        if (RestartAction != null)
            RestartAction();
    }

    public void OnBackMainMenu()
    {
        if (BackMainMenuAction != null)
            BackMainMenuAction();
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