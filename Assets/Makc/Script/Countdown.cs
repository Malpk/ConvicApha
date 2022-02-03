using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEngine.SceneManagement;

public class Countdown : Timer
{
    [SerializeField] private int _timeStart;

    private string _saveName = "SaveData.json";
    private SaveData _data;

    public override int TimeValue => _timeStart;
    private string Path => Application.streamingAssetsPath + _saveName;

    private void Awake()
    {

        if (!File.Exists(Path))
        {
            _data = new SaveData();
            _data.IsFirstEnter = true;
        }
        else
        {
            var saveData = File.ReadAllText(Path);
            _data = JsonUtility.FromJson<SaveData>(saveData);
        }
    }
    private void OnDestroy()
    {
        File.WriteAllText(Path, JsonUtility.ToJson(_data));
    }
    protected override IEnumerator RunTimer()
    {
        while (_timeStart > 0)
        {
            yield return new WaitForSeconds(1f);
            _timeStart -= 1;
            _text.text = GetTimeValue(_timeStart / 60) + "." + GetTimeValue(_timeStart % 60);
        }
        LoadTitleScene();
    }
    private string GetTimeValue(int value)
    {
        if (value / 10 > 0)
            return value.ToString();
        else
            return "0" + value.ToString();
    }
    private bool LoadTitleScene()
    {
        if (_data.IsFirstEnter)
        {
            _data.IsFirstEnter = false;
            SceneManager.LoadScene(1);
            return false;
        }
        else
        {
            return true;
        }
    }
}
