using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TileSpace;

public interface IBuildWay
{
    public List<IVertex> GetWay(IVertex startVertex, int requredlenght);
}
