using System.Collections.Generic;

public interface IGraph
{
    public List<IGraph> edges { get;}
    public void AddVertex(IGraph vertex);
    public void RemoveVertex(IGraph vertex);

}
