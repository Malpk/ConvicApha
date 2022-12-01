using UnityEngine;
using TMPro;
using System.Collections;
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

    private void OnEnable()
    {
        _player.OnDead += Dead;
        ShowAction += ShowEnding;
        HideAction += () => _canvas.enabled = false;
    }
    private void OnDisable()
    {
        _player.OnDead -= Dead;
        ShowAction -= ShowEnding;
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

    private void ShowEnding()
    {
        if (!_canvas.enabled)
        {
            _canvas.enabled = true;
            StartCoroutine(WaitPressAnyKey());
        }
    }

    private IEnumerator WaitPressAnyKey()
    {
        yield return new WaitWhile(() => !Input.anyKeyDown);
        Hide();
    }

    public void Win()
    {
        _timer.Output((int)_generalTime);
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
