using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;
public class LvlTimer : Timer
{
    [SerializeField] private int _timeStart;

    [Inject] private CameraAnimation _cameraAnimation;

    public delegate void WinGame();
    public event WinGame WinGameAction;

    public override int TimeValue => _timeStart;

    private void Awake()
    {
        _text.text = GetText();
    }
    private void OnEnable()
    {
        _cameraAnimation.CompliteAction += Run;
    }
    private void OnDisable()
    {
        _cameraAnimation.CompliteAction -= Run;
    }
    private void Run()
    {
        StartCoroutine(RunTimer());
    }
    protected override IEnumerator RunTimer()
    {
        while (_timeStart > 0)
        {
            yield return new WaitForSeconds(1f);
            _timeStart -= 1;
            _text.text = GetText();
        }
        WinEvent();
    }
    private string GetText()
    {
        return GetTimeValue(_timeStart / 60) + "." + GetTimeValue(_timeStart % 60);
    }    
    private string GetTimeValue(int value)
    {
        if (value / 10 > 0)
            return value.ToString();
        else
            return "0" + value.ToString();
    }
    private void WinEvent()
    {
        if (WinGameAction != null)
            WinGameAction();
    }
}
