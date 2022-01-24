using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapReader 
{

    public List<Vector2Int> ReadMap(Texture2D map)
    {
        var list = new List<Vector2Int>();
        var size = new Vector2Int(map.width, map.height);
        for (int y = 0; y < size.y; y++)
        {
            for (int x = 0; x < size.x; x++)
            {
                if (map.GetPixel(x, y) == Color.white)
                    list.Add(new Vector2Int(x, y));
            }
        }
        return list;
    }
}
