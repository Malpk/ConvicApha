using UnityEngine;

[System.Serializable]
public class SettingSequence 
{
    [SerializeField] private string _name;
    [SerializeField] private GameObject _sequence;

    public GameObject sequence => _sequence;
}
