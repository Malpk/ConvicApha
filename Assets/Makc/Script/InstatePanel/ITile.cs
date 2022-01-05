using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ITile
{
    public TileState State { get; }
    public GameObject TileObject { get; }
}
