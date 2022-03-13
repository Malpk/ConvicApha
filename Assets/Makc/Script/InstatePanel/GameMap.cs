using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using TileSpace;

namespace Underworld
{
    public class GameMap : MonoBehaviour
    {
        [SerializeField] private int _sizeMap;
        [SerializeField] private Tilemap _tilemap;

        private IVertex[,] _vertex;
        private Vector2Int[] _offsets = new Vector2Int[] { Vector2Int.up, Vector2Int.down, Vector2Int.left, Vector2Int.right };

        public int MapSize => _sizeMap;
        public IVertex[,] Map => _vertex;

        private void Awake()
        {
            Intializate();
        }

        private void Intializate()
        {
            _vertex = new IVertex[_sizeMap, _sizeMap];
            int index = _sizeMap / 2 - 1;
            for (int i = 0; i < _sizeMap; i++)
            {
                int x = index - i;
                for (int j = 0; j < _sizeMap; j++)
                {
                    int y = index - j;
                    Vector3 cellPosition = _tilemap.GetCellCenterWorld(new Vector3Int(x, y, 0));
                    _vertex[i, j] = new WayVertex(cellPosition, new Vector2Int(i, j));
                }
            }
            SetConectinoVertex();
        }
        private void SetConectinoVertex()
        {
            for (int i = 0; i < _sizeMap; i++)
            {
                for (int j = 0; j < _sizeMap; j++)
                {
                    for (int k = 0; k < _offsets.Length; k++)
                    {
                        int x = i + _offsets[k].x;
                        int y = j + _offsets[k].y;
                        if (CheakRangeOutArray(_sizeMap, x) && CheakRangeOutArray(_sizeMap, y))
                            _vertex[i, j].AddVertex(_vertex[x, y]);
                    }
                }
            }
        }
        public List<IVertex> GetVertexInRadius(Vector3 position, int radius)
        {
            List<IVertex> vertexs = new List<IVertex>();
            Vector2Int center = ConvertInIndex(Vector3Int.RoundToInt(position));
            for (int i = -radius + 1; i < radius; i++)
            {
                int x = center.x + i;
                for (int j = -radius + 1; j < radius && CheakRangeOutArray(_sizeMap, x); j++)
                {
                    int y = center.y + j;
                    if (CheakRangeOutArray(_sizeMap, y))
                    {
                        if (_vertex[x, y].State == TileState.Empty)
                            vertexs.Add(_vertex[x, y]);
                    }
                }
            }
            return vertexs;
        }
        private bool CheakRangeOutArray(int size, int value)
        {
            return value < size && value >= 0;
        }
        private Vector2Int ConvertInIndex(Vector3Int position)
        {
            position = _tilemap.WorldToCell(position);
            int x = _sizeMap / 2 - 1 - position.x;
            int y = _sizeMap / 2 - 1 - position.y;
            return new Vector2Int(x, y);
        }
    }
}