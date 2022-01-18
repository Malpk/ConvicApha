using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Trident
{
    public interface IVertex 
    {
        public Vector2 position { get; }
        public VertexState state { get; }

        public bool InstateObject(GameObject instateObject,Transform parent = null);
    }
}