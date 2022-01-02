using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TileSpace;

public class AStartVertex
{
    private AStartVertex _previous;
    private IVertex _vertex;

    public AStartVertex(IVertex reachable, AStartVertex previous = null)
    {
        _vertex = reachable;
        _previous = previous;
    }

    public int CountEdge => _vertex.Edge.Count;
    public List<IVertex> Edge => _vertex.Edge;
    public IVertex Vertex => _vertex;
    public AStartVertex Previous => _previous;
}
