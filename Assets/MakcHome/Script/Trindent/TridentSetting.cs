using UnityEngine;

[System.Serializable]
public class TridentSetting 
{
    [SerializeField] private float _delay;
    [SerializeField] private Vector3 _offset;
    [SerializeField] private GameObject _trident;

    public float startDelay => _delay;
    public Vector3 OffSet => _offset;
    public GameObject InstateObject => _trident;
}
