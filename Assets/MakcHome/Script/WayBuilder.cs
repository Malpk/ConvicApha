using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TileSpace;

public class WayBuilder : IBuildWay
{
    private List<AStartVertex> _reachable = new List<AStartVertex>();
    private List<AStartVertex> _explore = new List<AStartVertex>();

    public List<IVertex> GetWay(IVertex startVertex, int requredlenght)
    {
        _reachable.Clear();
        _explore.Clear();
        var nextSteep = new AStartVertex(startVertex);
        AddReachable(nextSteep);
        while (_reachable.Count > 0)
        {
            AddReachable(nextSteep);
            nextSteep = ChooseNextVertex(_reachable);
            if (Vector3.Distance(nextSteep.Vertex.VertixPosition, startVertex.VertixPosition) > requredlenght)
            {
                return BuildWay(nextSteep,startVertex);
            }
        }
        return null;
    }
    private AStartVertex ChooseNextVertex(List<AStartVertex> reachable)
    {
        int index = Random.Range(0, reachable.Count);
        return reachable[index];
    }
    private List<IVertex> BuildWay(AStartVertex endVertex, IVertex srartVertex)
    {
        List<IVertex> way = new List<IVertex>();
        AStartVertex aSartVertex = endVertex;
        while (aSartVertex.Vertex != srartVertex)
        {
            way.Add(aSartVertex.Vertex);
            aSartVertex = aSartVertex.Previous;
        }
        return way;
    }

    private bool AddReachable(AStartVertex exploreVertex)
    {
        if (exploreVertex == null)
            return false;
        _explore.Add(exploreVertex);
        for (int i = 0; i < exploreVertex.CountEdge; i++)
        {
            if (exploreVertex.Edge[i].State == TileState.Empty)
            {
                _reachable.Add(new AStartVertex(exploreVertex.Edge[i], exploreVertex));
            }
        }
        return true;
    }
}
