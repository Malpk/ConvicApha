using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BaseMode;

[CreateAssetMenu] 
public class KIFactory : ObjectFactroy
{
    /*
     * When add new prefab — change random value in GetRandomKI
     */
    // Add prefabs here
    [Header("Prefabs")]
    [SerializeField] private Device[] _tilePrefabs;
    [SerializeField] private Device[] _gunPrefabs;
    [SerializeField] private Device[] _izolatorPerfabs;

    public KI GetIzolatorKI()
    {
        if (_izolatorPerfabs.Length > 0)
        {
            return Get(_izolatorPerfabs[Random.Range(0, _izolatorPerfabs.Length)]);
        }
        return null;
    }
    public KI GetGunKI()
    {
        if (_gunPrefabs.Length > 0)
        {
            return Get(_gunPrefabs[Random.Range(0, _gunPrefabs.Length)]);
        }
        return null;
    }
    public KI GetTileKI()
    {
        if (_tilePrefabs.Length > 0)
        {
            return Get(_tilePrefabs[Random.Range(0, _tilePrefabs.Length)]);
        }
        return null;
    }
    public KI GetRandomKI()
    {
        Debug.Log("Get");
        var typs = new DeviceEnum[] { DeviceEnum.Izolator, DeviceEnum.Tile, DeviceEnum.Gun };
        switch (typs[Random.Range(0,typs.Length)])
        {
            case DeviceEnum.Izolator:
                return GetIzolatorKI(); // use random after
            case DeviceEnum.Gun:
                return GetGunKI();
            case DeviceEnum.Tile:
                return GetTileKI();
        }
        return null;
    }

    private T Get<T>(T prefab) where T : MonoBehaviour
    {
        T instance = CreateGameObjectInstance(prefab);
        return instance;
    }
}
