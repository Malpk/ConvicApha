using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace Trident
{
    [System.Serializable]
    public class MarkerSetting

    {
        [SerializeField] private bool _verticalMode;
        
        public List<IVertex> VertexList;

        public int[] Angls 
        {
            get
            {
                if (VerticalMode)
                    return new int[] { 0, 180 };
                else
                    return new int[] { 90, -90 };
            }
        } 
        public int MarkerAngle
        {
            get
            {
                if (VerticalMode)
                    return 90;
                else
                    return 180;
            }
        }
        public bool VerticalMode => _verticalMode;
        public Vector3Int Direction 
        {
            get
            {
                if (VerticalMode)
                    return Vector3Int.right;
                else
                    return Vector3Int.up;
            }
        }

    }
}