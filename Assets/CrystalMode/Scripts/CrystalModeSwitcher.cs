using System.Collections;
using UnityEngine;
using MainMode;

public class CrystalModeSwitcher : GameSwitcher
{
    [SerializeField] private Ending _ending;

    private Coroutine _corotine;

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
            _corotine = StartCoroutine(BackMenu());
        }
    }

    private IEnumerator BackMenu()
    {
        yield return new WaitWhile(() => !Input.anyKeyDown);
        BackMainMenu();
        _corotine = null;
    }
}

