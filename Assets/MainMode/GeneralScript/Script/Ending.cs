using UnityEngine;
using TMPro;
using MainMode.GameInteface;

public class Ending : UserInterface
{
    [SerializeField] private int _numberSize;
    [SerializeField] private Canvas _canvas;
    [SerializeField] private EndingMessange[] _messanges;
    [Header("Reference")]
    [SerializeField] private Player _player;
    [SerializeField] private TimerDisplay _timer;
    [SerializeField] private TextMeshProUGUI _playerName;
    [SerializeField] private TextMeshProUGUI _atifactName;
    [SerializeField] private TextMeshProUGUI _countDeathText;
    [SerializeField] private TextMeshProUGUI _consumableName;
    [SerializeField] private TextMeshProUGUI _result;

    private int _countDeadth;
    private float _generalTime;

    public override UserInterfaceType Type => throw new System.NotImplementedException();

    private void OnEnable()
    {
        _player.DeadAction += Dead;
        ShowAction += () => _canvas.enabled = true;
        HideAction += () => _canvas.enabled = false;
    }
    private void OnDisable()
    {
        _player.DeadAction -= Dead;
        ShowAction -= () => _canvas.enabled = true;
        HideAction -= () => _canvas.enabled = false;
    }
    public void SetUseItems(string artifact, string cosumable, string player)
    {
        _playerName.text = player;
        _atifactName.text = artifact;
        _consumableName.text = cosumable;
    }

    private void Update()
    {
        _generalTime += Time.deltaTime;
    }
    public void Win()
    {
        _timer.Output(_generalTime);
        _countDeathText.text = ToStringNumber(_countDeadth.ToString());
        _result.text = GetMessange();
    }
    private void Dead()
    {
        _countDeadth++;
    }
    private string ToStringNumber(string text)
    {
        while (_numberSize > text.Length)
        {
            text = "0" + text;
        }
        return text;
    }
    private string GetMessange()
    {
        foreach (var messange in _messanges)
        {
            if (_countDeadth <= messange.Condition)
            {
                return messange.Messange;
            }
        }
        return _messanges[_messanges.Length].Messange;
    }
}
