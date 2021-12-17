using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[System.Serializable]
public class UnderworldTile : IGetTileMap
{
    [SerializeField] private GameObject _tile;

    public GameObject GetTypeTile()
    {
        return _tile;
    }
}
