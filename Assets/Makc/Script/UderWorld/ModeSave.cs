using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

[System.Serializable]
public class ModeSave : MonoBehaviour
{
    [SerializeField] private float _delay;
    [SerializeField] private float _radius;
    [SerializeField] private float _duration;

    private void Start()
    {
        Save();
    }
    private void Save()
    {
       File.WriteAllText($"{Application.streamingAssetsPath}/{ToString()}.json" ,JsonUtility.ToJson(this));
    }
}
