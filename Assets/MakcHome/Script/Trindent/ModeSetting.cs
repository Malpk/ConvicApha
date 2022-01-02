using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ModeSetting
{
    [SerializeField] private bool _isDebug;
    [SerializeField] private float _duration;
    [SerializeField] private GameObject _point;

    public bool isDebug => _isDebug;
    public float duration => _duration;
    public GameObject point => _point;
}
