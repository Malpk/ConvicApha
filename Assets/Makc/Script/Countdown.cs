using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Countdown : Timer
{
    [SerializeField] private int _timeStart;

    public override int TimeValue => _timeStart;

    protected override IEnumerator RunTimer()
    {
        _timeStart *= 60;
        while (_timeStart > 0)
        {
            yield return new WaitForSeconds(1f);
            _timeStart -= 1;
            _text.text = GetTimeValue(_timeStart / 60) + "." + GetTimeValue(_timeStart % 60);
        }
    }
    private string GetTimeValue(int value)
    {
        if (value / 10 > 0)
            return value.ToString();
        else
            return "0" + value.ToString();
    }

}
