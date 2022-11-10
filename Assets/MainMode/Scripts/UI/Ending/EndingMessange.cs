using UnityEngine;

[System.Serializable]
public class EndingMessange
{
    [SerializeField] private int _condition;
    [SerializeField] private string _messange;

    public int Condition => _condition;
    public string Messange => _messange;
}
