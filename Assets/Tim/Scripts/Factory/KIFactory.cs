using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu] 
public class KIFactory : ObjectFactroy
{
    [Header("Prefabs")]
    [SerializeField] private FireGun fireGunPrefab;
    [SerializeField] private Gun gunPrefab;
    [SerializeField] private LaserGun laserGunPrefab;
    [SerializeField] private RocketLauncher rocketLauncherPrefab;
    [SerializeField] private Izolator izolatorPrefab;
    [SerializeField] private Stream streamPrefab;

    public void DebugCreateDevice()
    {
        var instance = GetGun(GunsEnum.Gun);
    }

    public Device GetDevice(DeviceEnum type)
    {
        switch (type)
        {
            case DeviceEnum.Izolator:
                return Get(izolatorPrefab);
            case DeviceEnum.Stream:
                return Get(streamPrefab);
        }
        return null;
    }
    public Gun GetGun(GunsEnum type)
    {
        switch (type)
        {
            case GunsEnum.FireGun:
                return Get(fireGunPrefab);
            case GunsEnum.LaserGun:
                return Get(laserGunPrefab);
            case GunsEnum.RocketLauncher:
                return Get(rocketLauncherPrefab);
            case GunsEnum.Gun:
                return Get(gunPrefab); 
        }
        return null;
    }

    private T Get<T>(T prefab) where T : MonoBehaviour
    {
        T instance = CreateGameObjectInstance(prefab);
        return instance;
    }
}
