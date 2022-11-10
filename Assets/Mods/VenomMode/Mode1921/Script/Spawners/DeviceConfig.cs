using UnityEngine;

public class DeviceConfig : ScriptableObject
{
    [SerializeField] private string _loadKey;

    public string LoadKey => _loadKey;
}
