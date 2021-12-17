using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IVertex 
{
    public TileState State { get; }
    public Vector3 VertixPosition { get; }
    public Vector2Int ArryaPostion { get; }
    public List<IVertex> Edge { get; }

    public bool SetTile(GameObject tile);
    public bool AddVertex(IVertex vertex);
    public bool RemoveVertex(IVertex vertex);
}
