using UnityEngine;
using MainMode;

public class CrystalModeSwitcher : GameSwitcher
{
    [SerializeField] private Ending _ending;

    private Coroutine _corotine;

    protected override void OnEnable()
    {
        base.OnEnable();
        _ending.HideAction += BackMainMenu;
    }
    protected override void OnDisable()
    {
        base.OnDisable();
        _ending.HideAction -= BackMainMenu;
    }

    protected override void PlayMessange()
    {

    }

    protected override void StopMessange()
    {

    }

    public void ShowEnding()
    {
        if (_corotine == null)
        {
            _ending.Show();
            hud.Hide();
            Stop();
        }
    }
}

