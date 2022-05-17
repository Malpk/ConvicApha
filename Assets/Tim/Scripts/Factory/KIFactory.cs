using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu] 
public class KIFactory : ObjectFactroy
{
    /*
     * When add new prefab — change random value in GetRandomKI
     */
    // Add prefabs here
    [Header("Prefabs")]
    //[SerializeField] private FireGun fireGunPrefab;
    [SerializeField] private Gun gunPrefab;
    //[SerializeField] private LaserGun laserGunPrefab;
    [SerializeField] private RocketLauncher rocketLauncherPrefab;
    [SerializeField] private Izolator izolatorPrefab;
    //[SerializeField] private Stream streamPrefab;

    public KI GetKI(DeviceEnum type)
    {
        switch (type)
        {
            case DeviceEnum.Izolator:
                return Get(izolatorPrefab);
            //case DeviceEnum.Stream:
            //    return Get(streamPrefab);
        }
        return null;
    }
    public KI GetKI(GunsEnum type)
    {
        switch (type)
        {
            //case GunsEnum.FireGun:
            //    return Get(fireGunPrefab);
            //case GunsEnum.LaserGun:
            //    return Get(laserGunPrefab);
            case GunsEnum.RocketLauncher:
                return Get(rocketLauncherPrefab);
            case GunsEnum.Gun:
                return Get(gunPrefab); 
        }
        return null;
    }

    public KI GetRandomKI()
    {
        int a = Random.Range(1, 3);
        switch (a)
        {
            case 1:
                return GetKI(DeviceEnum.Izolator); // use random after
            case 2:
                int i = Random.Range(1, 3);
                switch (i)
                {
                    case 1:
                        return GetKI(GunsEnum.Gun);
                    case 2:
                        //return GetKI(GunsEnum.RocketLauncher);
                        break;
                }
                break;
        }
        return GetKI(DeviceEnum.Izolator);
    }

    private T Get<T>(T prefab) where T : MonoBehaviour
    {
        T instance = CreateGameObjectInstance(prefab);
        return instance;
    }
}
