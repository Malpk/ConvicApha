using System.Collections.Generic;

public interface IGraph 
{
    public List<IGraph> Vertexs { get; }

    public void AddVertex();
    public void RemoveVertex();
    public IGraph GetVertex();
}
