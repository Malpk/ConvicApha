using UnityEngine;
using TMPro;

public class TimerDisplay : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _text;

    private int _minute = 60;

    public void Output(int value)
    {
        _text.text = $"{GetValue(value / _minute) }:{GetValue(value % _minute)}";
    }
    private string GetValue(float value)
    {
        var result = ((int)value).ToString();
        while (result.Length < 2)
        {
            result = "0" + result;
        }
        return result;
    }
}
