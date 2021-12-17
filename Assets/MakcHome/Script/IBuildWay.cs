using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IBuildWay
{
    public List<IVertex> GetWay(IVertex startVertex, int requredlenght);
}
