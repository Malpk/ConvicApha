using System.Collections.Generic;
using UnityEngine;

public class Graph : IGraph
{
    private int _index;
    private List<IGraph> _edge = new List<IGraph>();

    public Graph(int index)
    {
        _index = index;
    }

    public int index => _index;
    public List<IGraph> edges => _edge;

    public void AddVertex(IGraph vertex)
    {
        _edge.Add(vertex);
    }

    public void RemoveVertex(IGraph vertex)
    {
        _edge.Remove(vertex);
    }
}
