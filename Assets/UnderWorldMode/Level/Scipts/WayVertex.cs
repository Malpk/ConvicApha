using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TileSpace;

public class WayVertex : IVertex
{
    private GameObject _tile;
    private List<IVertex> _listEdges = new List<IVertex>();

    private readonly Vector3 vertexPosition;
    private readonly Vector2Int _tileArrayPosition;

    public WayVertex(Vector3 position, Vector2Int arrayPosition)
    {
        vertexPosition = position;
        _tileArrayPosition = arrayPosition;
    }

    public TileState State => _tile == null ? TileState.Safe : TileState.Dangerous;
    public Vector3 VertixPosition => vertexPosition;
    public Vector2Int ArryaPostion => _tileArrayPosition;
    public List<IVertex> Edge => _listEdges;

    public bool AddVertex(IVertex vertex)
    {
        if (vertex == this || vertex ==null)
            return false;
        _listEdges.Add(vertex);
        return true;
    }

    public bool RemoveVertex(IVertex vertex)
    {
        if (vertex == null)
            return false;
        return _listEdges.Remove(vertex);
    }

    public bool SetTile(GameObject tile)
    {
        if (_tile != null)
            return false;
        _tile = tile;
        return true;
    }
}
